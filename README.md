# Notes
 Webservice for PolSource recrutment assignment.

The project was made with Visual Studio 2019 and it is required for running it, although older version (like 2017) will probably suffice. The database is SQL Server Express, so that will be required as well. For visualization of the database, MS SQL Management Studio was used.

Steps to reproduce database (empty):
1. Open Command Prompt and navigate to Notes.API directory.
2. Run the "dotnet ef database update" command.

How to build and run the project:
1. Open the project in Visual Studio.
2. Press CTRL + F5 (for running without debug) or F5 (running with debug).

Example usages:
Using HTTP requests simulating tool of choice, send an HTTP request, e.g.
Type: GET, URL: http://localhost:53978/1
Type: POST, URL: http://localhost:53978/1, 
      Content: (application/json):
      {
         "Title": "title X",
         "Content": "content X
      }
The same content should be used for PUT. DELETE is the same as GET (only difference is type of request of course).

P.S. This was my first time creating integration tests, so they might be not ideal. While I was trying to make them so they will pass when run together, I couldn't find a working solution in time. They use the same resources, that's why some of them won't pass when run together, but they all pass when done alone.
