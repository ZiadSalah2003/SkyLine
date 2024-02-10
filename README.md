# MVC ITI Project

This is an ASP.NET Core 7.0 MVC project.

## Project Structure

- `Controllers/`: Contains all the controllers for the application.
- `Data/`: Contains the application's database context.
- `Models/`: Contains the models used in the application.
- `Views/`: Contains the views for the application.
- `wwwroot/`: Contains static files like CSS, JavaScript, and images.
- `Migrations/`: Contains the database migrations.
- `appsettings.json`: Contains the application's configuration settings.

## Getting Started

1. Ensure you have [.NET 7.0 SDK](https://dotnet.microsoft.com/download) installed.
2. Clone the repository.
3. Navigate to the project directory and run `dotnet restore` to restore the project dependencies.
4. Run `dotnet run` to start the application.

## Running the Tests

To run the tests, use the `dotnet test` command.

## Deployment

To publish the application for deployment, use the `dotnet publish -c Release -o ./publish` command. This will create a `publish` directory with the compiled application.

## Contributing
