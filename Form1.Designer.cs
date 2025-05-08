namespace Web_Browser
{
    
         
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxurl = new TextBox();
            btngo = new Button();
            refresh = new Button();
            btnhome = new Button();
            btnforward = new Button();
            htmlTextBox = new TextBox();
            btnback = new Button();
            Favourites = new ListBox();
            History = new ListBox();
            addToFavt = new Button();
            statusLabel = new Label();
            deletefavt = new Button();
            editfavt = new Button();
            bulkdownloadbtn = new Button();
            DarkMode = new CheckBox();
            SuspendLayout();
            // 
            // textBoxurl
            // 
            textBoxurl.Location = new Point(3, 10);
            textBoxurl.Multiline = true;
            textBoxurl.Name = "textBoxurl";
            textBoxurl.Size = new Size(417, 20);
            textBoxurl.TabIndex = 0;
            textBoxurl.TextChanged += textBoxurl_TextChanged;
            // 
            // btngo
            // 
            btngo.Location = new Point(426, 10);
            btngo.Name = "btngo";
            btngo.Size = new Size(50, 23);
            btngo.TabIndex = 1;
            btngo.Text = "Go";
            btngo.UseVisualStyleBackColor = true;
            btngo.Click += btngo_Click;
            // 
            // refresh
            // 
            refresh.Location = new Point(482, 10);
            refresh.Name = "refresh";
            refresh.Size = new Size(55, 23);
            refresh.TabIndex = 2;
            refresh.Text = "Refresh";
            refresh.UseVisualStyleBackColor = true;
            refresh.Click += refresh_Click;
            // 
            // btnhome
            // 
            btnhome.Location = new Point(679, 10);
            btnhome.Name = "btnhome";
            btnhome.Size = new Size(50, 23);
            btnhome.TabIndex = 3;
            btnhome.Text = "Home";
            btnhome.UseVisualStyleBackColor = true;
            btnhome.Click += home_Click;
            // 
            // btnforward
            // 
            btnforward.Location = new Point(543, 10);
            btnforward.Name = "btnforward";
            btnforward.Size = new Size(74, 23);
            btnforward.TabIndex = 4;
            btnforward.Text = "Forward";
            btnforward.UseVisualStyleBackColor = true;
            btnforward.Click += btnforward_Click;
            // 
            // htmlTextBox
            // 
            htmlTextBox.ForeColor = Color.Black;
            htmlTextBox.Location = new Point(3, 39);
            htmlTextBox.MaximumSize = new Size(800, 500);
            htmlTextBox.MinimumSize = new Size(700, 400);
            htmlTextBox.Multiline = true;
            htmlTextBox.Name = "htmlTextBox";
            htmlTextBox.ReadOnly = true;
            htmlTextBox.ScrollBars = ScrollBars.Both;
            htmlTextBox.Size = new Size(800, 410);
            htmlTextBox.TabIndex = 5;
            htmlTextBox.Text = "WEB BROWSER";
            htmlTextBox.TextAlign = HorizontalAlignment.Center;
            htmlTextBox.TextChanged += textBox1_TextChanged_1;
            // 
            // btnback
            // 
            btnback.Location = new Point(623, 10);
            btnback.Name = "btnback";
            btnback.Size = new Size(50, 23);
            btnback.TabIndex = 6;
            btnback.Text = "Back";
            btnback.UseVisualStyleBackColor = true;
            btnback.Click += btnback_Click;
            // 
            // Favourites
            // 
            Favourites.FormattingEnabled = true;
            Favourites.ItemHeight = 15;
            Favourites.Location = new Point(12, 280);
            Favourites.Name = "Favourites";
            Favourites.Size = new Size(200, 94);
            Favourites.TabIndex = 7;
            Favourites.SelectedIndexChanged += Favourites_SelectedIndexChanged;
            // 
            // History
            // 
            History.FormattingEnabled = true;
            History.ItemHeight = 15;
            History.Location = new Point(220, 280);
            History.Name = "History";
            History.RightToLeft = RightToLeft.Yes;
            History.Size = new Size(200, 94);
            History.TabIndex = 8;
            History.SelectedIndexChanged += History_SelectedIndexChanged;
            // 
            // addToFavt
            // 
            addToFavt.Location = new Point(12, 390);
            addToFavt.Name = "addToFavt";
            addToFavt.Size = new Size(150, 23);
            addToFavt.TabIndex = 9;
            addToFavt.Text = "AddToFavourites";
            addToFavt.UseVisualStyleBackColor = true;
            addToFavt.Click += addToFavt_Click;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(12, 250);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 15);
            statusLabel.TabIndex = 10;
            statusLabel.Text = "Status";
            // 
            // deletefavt
            // 
            deletefavt.Location = new Point(12, 415);
            deletefavt.Name = "deletefavt";
            deletefavt.Size = new Size(75, 23);
            deletefavt.TabIndex = 11;
            deletefavt.Text = "DeleteFavt";
            deletefavt.UseVisualStyleBackColor = true;
            deletefavt.Click += deletefavt_Click;
            // 
            // editfavt
            // 
            editfavt.Location = new Point(93, 415);
            editfavt.Name = "editfavt";
            editfavt.Size = new Size(75, 23);
            editfavt.TabIndex = 12;
            editfavt.Text = "EditFavt";
            editfavt.UseVisualStyleBackColor = true;
            editfavt.Click += editfavt_Click;
            // 
            // bulkdownloadbtn
            // 
            bulkdownloadbtn.Location = new Point(320, 390);
            bulkdownloadbtn.Name = "bulkdownloadbtn";
            bulkdownloadbtn.Size = new Size(150, 23);
            bulkdownloadbtn.TabIndex = 13;
            bulkdownloadbtn.Text = "BulkDownload";
            bulkdownloadbtn.UseVisualStyleBackColor = true;
            bulkdownloadbtn.Click += bulkdownloadbtn_Click;
            // 
            // DarkMode
            // 
            DarkMode.AutoSize = true;
            DarkMode.Location = new Point(735, 14);
            DarkMode.Name = "DarkMode";
            DarkMode.Size = new Size(93, 19);
            DarkMode.TabIndex = 14;
            DarkMode.Text = "ApplyTheme";
            DarkMode.UseVisualStyleBackColor = true;
            DarkMode.CheckedChanged += DarkMode_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(840, 443);
            Controls.Add(bulkdownloadbtn);
            Controls.Add(editfavt);
            Controls.Add(deletefavt);
            Controls.Add(statusLabel);
            Controls.Add(addToFavt);
            Controls.Add(History);
            Controls.Add(Favourites);
            Controls.Add(btnback);
            Controls.Add(htmlTextBox);
            Controls.Add(btnforward);
            Controls.Add(btnhome);
            Controls.Add(refresh);
            Controls.Add(btngo);
            Controls.Add(textBoxurl);
            Controls.Add(DarkMode);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxurl;
        private Button btngo;
        private Button refresh;
        private Button btnhome;
        private Button btnforward;
        private TextBox htmlTextBox;
        private Button btnback;
        private ListBox Favourites;
        private ListBox History;
        private Button addToFavt;
        private Label statusLabel;
        private Button deletefavt;
        private Button editfavt;
        private Button bulkdownloadbtn;
        private CheckBox DarkMode;
    }
}
