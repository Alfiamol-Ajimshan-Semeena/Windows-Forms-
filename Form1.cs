using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Net;
using System.Data.Entity;
namespace Web_Browser
{
    public partial class Form1 : Form
    {
        //HttpClient used to send Http requests
        private HttpClient httpClient;
        //Home page URL and other related file paths
        private string homePage = "https://www.hw.ac.uk/";
        private string homePageFile = "homepage.txt";

        private string historyFile = "history.txt";
        private string connectionString = "Data Source=MyDatabase.db"; //SQLite connection string
        private string BulkdowloadFile = "bulk.txt";

        // Stacks to store back and forward browsing history
        private Stack<string> backHistory = new Stack<string>();
        private Stack<string> forwardHistory = new Stack<string>();


        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true; // Allows from to capture key presses
            this.KeyDown += Form1_KeyDown; // Event handler for key down event
            textBoxurl.KeyPress += TextBoxUrl_KeyPress; //Event handler for Enter key press in the URL box
            httpClient = new HttpClient();
            CreateDatabase(); //Create database and table if not exists
            LoadHomePage();
            LoadFavourites();
            LoadHistory();
            LoadPage();

        }

        //Create the SQLite database and favourites table
        private void CreateDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"CREATE TABLE IF NOT EXISTS favourites (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, url TEXT)";
                command.ExecuteNonQuery();
            }
        }

        // Class to represent a Favourite(name and URL)
        public class Favourite
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public Favourite(string name, string url)
            {
                Name = name;
                Url = url;
            }
            public override string ToString()
            {
                return Name; 
            }

        }
    

        //Event handler for pressing Enter in the URL textbox
        private void TextBoxUrl_KeyPress(object? sender, KeyPressEventArgs? e)
        {
            if (e!.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btngo.PerformClick();
            }
        }

        // Key press event handler for form shortcuts
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            

            if (e.Control && e.KeyCode == Keys.B && !e.Shift)
            {
                btnback.PerformClick();  // Ctrl+B for Back
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                btnforward.PerformClick();  // Ctrl+F for Forward
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                refresh.PerformClick();  // Ctrl+R for Refresh
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                addToFavt.PerformClick();  // Ctrl+D for Add to Favorites
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.B)
            {
                bulkdownloadbtn.PerformClick();  // Ctrl+Shift+B for Bulk Download
                e.SuppressKeyPress = true;
            }
        }

        //Load the home page
        private async void LoadHomePage()
        {
            var response = await httpClient.GetAsync(homePage);
            if (response.IsSuccessStatusCode)
            {
                string htmlContent = await response.Content.ReadAsStringAsync();
                DisplayContent(htmlContent, response.StatusCode.ToString());
                string title = ExtractTitleFromHtml(htmlContent);
                this.Text = $"Simple Web Browser -{title}";
            }
            else
            {
                MessageBox.Show("Failed to load page. Status code:" + response.StatusCode);
            }
        }

        //Load a page based on the given URL
        private async void LoadPage(string url)
        {
            url = url.Trim(); //Remove extra spaces
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                MessageBox.Show("Invalid URL format. Please enter a valid URL.");
                return;
            }
            try
            {

                var response = await httpClient.GetAsync(uriResult); // Send HTTP request
                if (response.IsSuccessStatusCode)
                {
                    string html = await response.Content.ReadAsStringAsync(); // Get page content
                    textBoxurl.Text = url;

                    statusLabel.Text = $"Status: {response.StatusCode}";
                    DisplayContent(html, response.StatusCode.ToString());
                    string title = ExtractTitleFromHtml(html); // Get page title
                    this.Text = $"Simple Web Browser -{title}"; // Set window title

                }
                else
                {
                    MessageBox.Show($"Failed to load page. Status code : {response.StatusCode}");
                }
            }

            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error loading page: {ex.Message}");
            }
        }

        //Extract the title from the Html content
        private string ExtractTitleFromHtml(string htmlContent)
        {
            string title = "No Title Found";
            var match = Regex.Match(htmlContent, @"<title>\s*(.+?)\s*</title>", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                title = match.Groups[1].Value;
            }
            return title;
        }

        //Event handler for Go button click
        private async void go_Click(object sender, EventArgs e)
        {
            string currenturl = textBoxurl.Text.Trim();
            if (!string.IsNullOrEmpty(currenturl))
            {
                await LoadUrl(currenturl);
            }

        }

        //Load URL and add to history if successful
        private async Task LoadUrl(string url, bool addToFavourites = false)
        {
            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "http://" + url;   //Add http prefix if missing
                }

                if (Uri.TryCreate(url, UriKind.Absolute, out Uri? validUri))  //Validate URL
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await httpClient.GetAsync(validUri);
                        if (response.IsSuccessStatusCode)
                        {
                            string htmlContent = await response.Content.ReadAsStringAsync();  //Get page content


                            DisplayContent(htmlContent, response.StatusCode.ToString());
                            AddToHistory(url);

                            string title = ExtractTitleFromHtml(htmlContent);
                            this.Text = $"Simple Web Browser -{title}";
                        }
                        else
                        {
                            HandleHttpErrors(response.StatusCode);  //Handle HTTP errors
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Ivalid URL: " + url);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error loading page:{ex.Message}");
            }
        }

        //Handle specific HTTP error codes
        private void HandleHttpErrors(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    MessageBox.Show("400 Bad Request:The server could not understand the request due to invalid syntax.");
                    DisplayContent("400 Bad Request: Invalid syntax.", statusCode.ToString());
                    break;
                case HttpStatusCode.Forbidden:
                    MessageBox.Show("403 Forbidden: You do not have permission to access this resource.");
                    DisplayContent("403 Forbidden: Access denied.", statusCode.ToString());
                    break;
                case HttpStatusCode.NotFound:
                    MessageBox.Show("404 Not Found:The resource you are looking for could not be found.");
                    DisplayContent("404 Not Found:Resource not found.", statusCode.ToString());
                    break;
                default:
                    MessageBox.Show($"Error:{statusCode}");
                    DisplayContent($"Error:{statusCode}", statusCode.ToString());
                    break;
            }
        }

        //Load the home page on initialization
        private void LoadPage()
        {
            textBoxurl.Text = homePage;
            LoadPage(homePage);
        }

        //Display the content and updates the status
        private void DisplayContent(string htmlContent, string statusCode)
        {
            htmlTextBox.Text = htmlContent;
            statusLabel.Text = $"Status : + {statusCode}";
        }

        //Add a URL to the history list
        private void AddToHistory(string url)
        {
            History.Items.Add(url);
            System.IO.File.AppendAllLines(historyFile, new[] { url });  //Append to history file
        }

        private void LoadFavourites()
        {
            Favourites.Items.Clear(); //Clear existing items in the Favourtes list
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT name, url FROM favourites";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            string url = reader.GetString(1);
                            Favourites.Items.Add(new Favourite(name, url)); // Add to the Favourites list
                        }

                    }

                }
            }
         }

        //Method to add a new favourites
        private void AddToFavourites(string url, string name)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using(var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO favourites(name,url) VALUES($name,$url)";
                    command.Parameters.AddWithValue("$name", name);
                    command.Parameters.AddWithValue("$url", url);
                    command.ExecuteNonQuery();
                }
                Favourites.Items.Add(new Favourite(name,url));
                MessageBox.Show("URL added to favourites");
            }
            else
            {
                MessageBox.Show("Name cannot be empty.");
            }
        }

        //Event handler for adding a favourite when button clicked
        private void addToFavt_Click(object? sender, EventArgs e)
        {
            string url = textBoxurl.Text.Trim();  //Get the current URL from the textbox
            if (!string.IsNullOrEmpty(url))
            {

                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "http://" + url;
                }
                //Validate the URL format
                if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? validUri) ||
                    (validUri.Scheme != Uri.UriSchemeHttp && validUri.Scheme != Uri.UriSchemeHttps))
                {
                    MessageBox.Show("Please provide a valid URL");
                    return;
                }
                //Prompt user for a name for the favourite
                string name = Microsoft.VisualBasic.Interaction.InputBox("Enter a name for this favourite:", "Add Favourite");
                if (!string.IsNullOrEmpty(name))
                {
                    foreach (var item in Favourites.Items)
                    {
                        if (item is Favourite fav && fav.Url.Equals(url, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("This URL is already in your favourites");
                            return;
                        }
                    }
                    AddToFavourites(url, name);
                }

            }
            else
            {
                MessageBox.Show("Please provide a valid URL");
            }
        }

        //Load history from the history file
        private void LoadHistory()
        {
            if (System.IO.File.Exists(historyFile))
            {
                var history = System.IO.File.ReadAllLines(historyFile);
                foreach (var url in history)
                {
                    History.Items.Add(url);
                }
            }
        }

        //Event handler for when a favourite is selected
        private async void Favourites_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Favourites.SelectedIndex != -1 && Favourites.SelectedItem is Favourite selectedFavourite)
            {
                string selectedUrl = selectedFavourite.Url;

                textBoxurl.Text = selectedUrl;
                await LoadUrl(selectedUrl, false);
            }

        }

        //Event handler for when a history entry is selected
        private async void History_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (History.SelectedItem != null)
            {
                string? selectedUrl = History.SelectedItem.ToString();
                textBoxurl.Text = selectedUrl;
                if (!string.IsNullOrEmpty(selectedUrl))
                {
                    await LoadUrl(selectedUrl, false);

                }
                else
                {
                    MessageBox.Show("Selected Url is invalid.");
                }
            }

        }
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Event handler for home button click
        private async void home_Click(object sender, EventArgs e)
        {
            string homeUrl = "https://www.hw.ac.uk/";

            if (!string.IsNullOrEmpty(homeUrl))


            {
                string currenturl = homeUrl;
                textBoxurl.Text = currenturl;
                await LoadUrl(currenturl);
                MessageBox.Show($"Navigating to Home :{currenturl}");
            }
            else
            {
                MessageBox.Show("Home URL is invalid. Please check the URL");
            }

        }

        //Event handler for "Go" button
        private async void btngo_Click(object sender, EventArgs e)
        {
            string currenturl = textBoxurl.Text.Trim();
            if (!currenturl.StartsWith("http://") && !currenturl.StartsWith("https://"))
            {
                currenturl = "http://" + currenturl;
            }
            if (!string.IsNullOrEmpty(currenturl))
            {
                await LoadUrl(currenturl, true);
            }
            backHistory.Push(currenturl);
            LoadPage(currenturl);
        }

        private void textBoxurl_TextChanged(object sender, EventArgs e)
        {

        }

        //Event handler for the back button
        private void btnback_Click(object sender, EventArgs e)
        {
            forwardHistory.Push(textBoxurl.Text);
            if (backHistory.Count > 0)
            {
                string previousUrl = backHistory.Pop();
                textBoxurl.Text = previousUrl;
                LoadPage(previousUrl);
            }
            else
            {
                MessageBox.Show("No previous URLs in history");
            }
        }

        //Event handler for the forward button 
        private void btnforward_Click(object sender, EventArgs e)
        {
            if (forwardHistory.Count > 0)
            {
                string forwardUrl = forwardHistory.Pop();
                textBoxurl.Text = forwardUrl;
                LoadPage(forwardUrl);
            }
        }

        //Event handler for refresh button
        private async void refresh_Click(object sender, EventArgs e)
        {
            string currenturl = textBoxurl.Text;
            await LoadUrl(currenturl);
        }

        //Event handler for deleting a favourite
        private void deletefavt_Click(object sender, EventArgs e)
        {
            if (Favourites.SelectedItem != null)
            {
                Favourite selectedFavourite = (Favourite)Favourites.SelectedItem;
                MessageBox.Show($"Attempting to delete:selectedUrl");
                var confirmResult = MessageBox.Show($"Are you sure you want to delete 'selectedUrl'? ", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    Favourites.Items.Remove(selectedFavourite);
                    SaveFavouritesToFile();
                    MessageBox.Show("Deleted Successfully");

                }
            }
            else
            {
                MessageBox.Show("Please select a favourite to delete");
            }

        }

        //Event handler for editing a favourite
        private void editfavt_Click(object sender, EventArgs e)
        {
            if (Favourites.SelectedItem != null && Favourites.SelectedItem is Favourite favourite)
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Edit Favourite Name:", "Edit Favourite", Name);
                string newUrl = Microsoft.VisualBasic.Interaction.InputBox("Edit URL:", "Edit Favourite", favourite.Url);
                if (!string.IsNullOrEmpty(newName) && !string.IsNullOrEmpty(newUrl))
                {
                    if (!Uri.TryCreate(newUrl, UriKind.Absolute, out Uri? uriResult) ||
                        (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    {
                        MessageBox.Show("Invalid URL format.Please enter a valid URL.");
                        return;
                    }

                    favourite.Name = newName;
                    favourite.Url = newUrl;
                    int selectedIndex = Favourites.SelectedIndex;
                    Favourites.Items.RemoveAt(selectedIndex);
                    Favourites.Items.Insert(selectedIndex, favourite); 
                    Favourites.SelectedIndex = selectedIndex;
                    SaveFavouritesToFile();
                    MessageBox.Show("Favourite update successfully.");
                }
                else
                {
                    MessageBox.Show("Name and URL cannot be empty");
                }
            }
            else
            {
                MessageBox.Show("Please select a favourite to edit");
            }
        }

        //Method to save the list of favourites to a file
        private void SaveFavouritesToFile()
        {
            List<string> favourites = new List<string>();  // Initialize a list to hold the favourite entries
            foreach (var item in Favourites.Items)
            {
                if (item is Favourite favourite)
                {
                    favourites.Add($"{favourite.Name}|{favourite.Url}");
                }
            }
            

        }

        //Event handler for the bulk download button
        private async void bulkdownloadbtn_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Select  Bulk Files";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    await StartBulkDownload(selectedFilePath);
                }

            }
        }

        //Method to start the bulk download from a specifies file
        private async Task StartBulkDownload(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                MessageBox.Show("The specified file does not exist.");
                return;
            }


            htmlTextBox.Clear();
            htmlTextBox.AppendText("Starting download...\r\n\n");
            string[] urls = await System.IO.File.ReadAllLinesAsync(filePath);

            List<string> results = new List<string>();


            foreach (var url in urls)
            {
                var (statusCode, contentLength, validUrl) = await FetchWebContent(url.Trim());


                results.Add($"{statusCode} {contentLength} bytes - {validUrl}");
            }


            htmlTextBox.Text = string.Join(Environment.NewLine, results);
            htmlTextBox.AppendText("\r\n Bulk Download Completed...!");
        }

        //Method to fetch web content and return status code,content lenght ,and URL
        private async Task<(string statusCode, long contentLength, string url)> FetchWebContent(string url)
        {

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return ("Invalid URL", 0, url);
            }

            try
            {
                var response = await httpClient.GetAsync(uriResult);

                if (response.IsSuccessStatusCode)
                {
                    //Get content lenght if not available, read the content as byte array to get lenght
                    long contentLength = response.Content.Headers.ContentLength ?? (await response.Content.ReadAsByteArrayAsync()).LongLength;

                    return (response.StatusCode.ToString(), contentLength, url);
                }
                else
                {
                    return (response.StatusCode.ToString(), 0, url);
                }
            }
            catch (HttpRequestException)
            {

                return ("Error", 0, url);
            }
        }

        //Flag for dark mode
        private bool isDarKMode = false;

        //Method to add a dark mode checkbox
        private void AddDarkMode()
        {
            DarkMode = new CheckBox();
            DarkMode.Text = "Dark Mode";

        }

        //Method to apply the current theme(light or dark mode
        private void DarkMode_CheckedChanged(object sender, EventArgs e)
        {
            isDarKMode = DarkMode.Checked;
            ApplyTheme();
        }
        private void ApplyTheme()
        {
            if (isDarKMode)
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
                htmlTextBox.BackColor = Color.Black;
                htmlTextBox.ForeColor = Color.White;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
                htmlTextBox.BackColor = Color.White;
                htmlTextBox.ForeColor = Color.Black;
            }
        }

      
    }
}


            

 

