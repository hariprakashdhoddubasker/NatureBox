# NatureBox – Nutrition Club Management System

## Overview
NatureBox is a desktop application designed for Nutrition Clubs to streamline their daily operations and member management. Built with WPF in C# .NET, it provides an intuitive interface for club administrators to manage member profiles, track attendance, monitor nutritional plans, and handle product sales. The application centralizes the data and processes of a nutrition/fitness club, improving efficiency and record-keeping.

## Features

- **Member Profile Management:** Create and maintain detailed member profiles including contact information, health goals, and progress records.
- **Attendance & Session Tracking:** Log daily attendance of members for fitness or nutrition sessions and analyze participation trends over time.
- **Nutrition Plans & Progress Monitoring:** Assign customized diet or wellness plans to members and track their progress (e.g. weight changes, body metrics) through the application.
- **Product Sales & Inventory:** Manage the sale of nutritional products (supplements, health drinks) with an integrated inventory system. The app supports recording purchases, generating receipts, and adjusting stock levels accordingly.
- **Reporting & Analytics:** Generate summary reports on membership growth, attendance rates, sales figures, and member progress. Visual charts and printable reports help in making informed decisions for the club’s operations.

## Technical Highlights

- **MVVM Architecture:** NatureBox is implemented following the MVVM (Model-View-ViewModel) design pattern, which ensures a clear separation between the user interface and business logic. This results in a more maintainable and testable codebase.
- **XAML UI:** The UI is built with XAML, featuring a modern and user-friendly design for club staff.
- **Local Database Support:** The application likely uses a local SQL database (via Entity Framework or ADO.NET) to store club data securely on-premises.
- **Modular Structure:** A modular architecture with separate components for membership management, sales, and reporting.
- **Shared Utility Library:** Common functionalities such as database access and validation are encapsulated in a shared library (**CommonAppBase**) to promote reuse across projects.

## Setup & Installation

### 1. Prerequisites
- Install the appropriate .NET runtime (e.g., .NET Framework 4.x or .NET Core with WPF support) on Windows.
- A local database system (SQL Server Express or SQLite) may be needed if the app stores data locally.

### 2. Source Code
- Clone or download the [NatureBox](https://github.com/hariprakashdhoddubasker/NatureBox) repository from GitHub.
- Locate the `NatureBox` folder which contains the Visual Studio solution (`.sln`) and related project files.

### 3. Database Configuration
- Update the connection string in the configuration file (e.g., `app.config`) to point to your SQL instance.
- Run any provided SQL scripts or Entity Framework migrations to initialize the database schema (tables for members, attendance, sales, etc.).

### 4. Build and Run
- Open the solution in Visual Studio.
- Restore any NuGet packages if prompted.
- Build the project to ensure all dependencies are resolved.
- Run the application. The main dashboard or login window should launch.

### 5. Usage
- Use the GUI to add new members and products.
- Navigate to the **Members** section to register a new member.
- Navigate to the **Products** section to add and manage nutritional products.
- Record attendance or create a sales entry for a member.
- Explore the **Reports** section to generate club performance reports.

---
