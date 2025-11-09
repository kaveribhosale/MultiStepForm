# MultiStepForm
MultiStepForm
This project is a Multi-Step Form Application built using ASP.NET Core MVC and SQL Server, designed to collect and manage user account applications with features like auto-save, file uploads, and final submission.

Features

✅ Multi-step wizard form with progress indicator
✅ Dynamic fields based on account type (Individual / Company)
✅ File upload with preview and progress bar
✅ Auto-save draft every 10 seconds
✅ Final review and submit functionality
✅ Responsive UI using Bootstrap 5
✅ Stores all data in SQL Server database

Frontend: HTML, CSS, jQuery, Bootstrap 5
Backend: ASP.NET Core MVC (.NET 8.0)
Database: SQL Server

Setup Instructions
Clone this repository
git clone https://github.com/yourusername/MultiStepAccountApp.git

Create Database
CREATE DATABASE AccountApplicationDB;


 Use That Database
USE AccountApplicationDB;


Create Table
CREATE TABLE AccountApplications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountType NVARCHAR(100) NULL,
    FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL,
    Email NVARCHAR(150) NULL,
    Phone NVARCHAR(20) NULL,
    Address NVARCHAR(MAX) NULL,
    DocumentPath NVARCHAR(255) NULL,
    Status NVARCHAR(50) NULL DEFAULT 'Draft',
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);


