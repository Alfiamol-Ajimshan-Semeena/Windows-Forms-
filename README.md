# Windows-Forms-
## Introduction 
The purpose of this project is designing a simple web browser application with core 
functionalities such as web navigation, favorites, history tracking, homepage customization, and 
downloading URLs in bulk. The application will be designed to offer minimal browsing with ease 
of use in managing favorite pages and browsing history. Below is a project where one has to 
develop a browser based on a GUI, which can manipulate and store persistent data by utilizing 
an SQLite database for favorites without any additional set-up of the database. The assumptions 
that would be taken into consideration in this project are: There is an active internet connection. 
The operating system should be Windows. Hard and software must be compatible with.NET.
## Requirements 
• Web Navigation: Users can enter a URL, navigate to different web pages, and     user 
the back and forward navigation buttons. 

• History Tracking: Browsing history is maintained for the current session and displayed 
to user. 

• Favorites Management: Users can add, edit, and delete favorite URLs. Favorites are 
saved persistently using an SQLite database. 

• Bulk Download: The application includes a bulk download feature, allowing users to 
load and display multiple URLs from a text file. 

• Dark Mode: A toggle option allows users to switch between light and dark themes. 
# Design Considerations
The two primary classes that concern the core functionality of this application are Form1, 
controlling the user interface and core operations, and the Favourite class, modeling each 
bookmarked page with attributes such as Name and Url. The application has used a Stack to 
handle navigation back and forward, and a List in handling favorites. The GUI design was realized 
with the help of Windows Forms and contained all basic controls: text boxes for URL input, 
buttons to perform navigation, list boxes for favorites, and a rich text box displaying the content 
of the webpage. It also utilized advanced language constructs such as async and await keywords 
that made HTTP requests asynchronously, keeping the UI thread responsive. SQLite was chosen 
as a database to persist favorites and settings. This will definitely allow working with this 
application independently, without using an external database server. Storing history data in text 
files and using asynchronous operations enhanced the performance and let the content load 
smoothly.
# User guide
The application is designed using a pretty standard user experience to make it easier for users to 
accomplish their tasks using the browser. Users can go to a web page by writing the URL text in 
the URL text box and then pressing either the “GO” button or Enter. Adding a favorite is pretty 
simple; after entering a URL, users can click on “Add to Favorites” to save it. The saved favorites 
will appear in a listbox where any clicked item will load the associated webpage. History is tracked 
automatically and shown in another list; users can go back to the previously opened pages by just 
clicking on them. Application supports bulk download, accessible through “Bulk Download” 
button, where user can select the text file containing the multiple URLs. The Dark Mode checkbox 
should toggle light and dark themes upon one click of the user so that he may adjust the interface 
accordingly.  
# Developer guide
The code is structured in this application such that Form1 serves as the main class, incorporating 
the user interface as well as the core functionality, while the Favourites class fills in the role of a 
data structure for each favorite item. HTTP requests and communication are mediated through 
an instance of the HTTP Client class, which is an asynchronous web request handler that ensures 
the application remains responsive while loading content. For the purpose of persistence, SQLite 
was utilized to manage favorites and homepage settings. SQLite was utilized to manage favorites 
and homepage settings. SQLite provides a lightweight and embedded database facility. History is 
persisted in a text file for ease of ease of use and retrieval. The application’s GUI was developed 
in Windows Forms, making use of varied controls: text boxes, list boxes, and buttons to compose 
an interface that would be intuitive and thus easily accessible. This structure assures the code is 
readable, accessible, and extensible for any developer who could want to add improvements 
or modifications in the future.   
