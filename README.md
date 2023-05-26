# Online-Jewellery-Shop

Online Jewellery Shop is a web application built using ASP.NET Core. It provides functionality for managing jewellery products and allows users to browse and purchase items from the shop.

## Prerequisites
.NET 5.0 SDK or later
Microsoft SQL Server (Express edition or higher)
Installation
Clone the repository or download the source code files.
Open the solution in Visual Studio or your preferred code editor.
Build the solution to restore the dependencies.
Run the database migrations to create the necessary tables. Open the Package Manager Console and run the following command:
Update-Database
Start the application using the debugging tools of your code editor or by running the following command in the terminal:
dotnet run

## Configuration
The application uses the appsettings.json file to store configuration settings. You can modify the file to change the default configuration, such as the database connection string and other application-specific settings.

## Authentication and Authorization
The application utilizes cookie-based authentication for user authentication and authorization. Users can log in, log out, and access protected resources based on their roles and permissions.

## Usage
Once the application is running, you can access the different features and pages of the Online Jewellery Shop through a web browser. The application provides the following main functionalities:

Browsing jewellery products
Adding products to the shopping cart
Completing the checkout process
Managing user accounts and roles (admin functionality)

## Contributing
Contributions to this project are welcome. If you find any issues or have suggestions for improvements, feel free to create a pull request or submit an issue in the repository.
