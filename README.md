# Movie Library

## Description
Movie Library is a web application built with .NET Core that allows users to manage a collection of movies. Users can add, edit, and rate movies. The application provides a user-friendly interface for managing movie details, including title, genre, year, rating, and poster images.

## Features
- Add new movies with details such as title, genre, year, rating, and poster.
- Edit existing movie details.
- Rate movies on a scale of 1 to 10.
- Display movie posters.
- Responsive design for a seamless experience on different devices.

## Technologies Used
- **.NET Core**: Backend framework for building the web application.
- **C#**: Programming language used for backend development.
- **Entity Framework Core**: ORM for database operations.
- **SQL Server**: Database for storing movie data.
- **ASP.NET Core MVC**: Framework for building the web application.
- **HTML5**: Markup language for structuring the web pages.
- **CSS**: Styling the web pages.
- **Bootstrap**: CSS framework for responsive design.
- **JavaScript**: Client-side scripting.
- **jQuery**: JavaScript library for DOM manipulation and AJAX requests.
- **Razor**: Templating engine for generating dynamic web pages.
- **Visual Studio**: Integrated development environment (IDE) for development.

## Getting Started

```bash
# 1. Clone the repository:
git clone https://github.com/yourusername/movielibrary.git

# 2. Navigate to the project directory:
cd movielibrary

# 3. Open the project in Visual Studio.
# (This step is done in the Visual Studio interface, not the terminal)

# 4. Update the database connection string in `appsettings.json`.
# (This step is done in the Visual Studio interface, not the terminal)

# 5. Apply migrations and create the database:
dotnet ef database update

# 6. Build and run the application:
dotnet run
