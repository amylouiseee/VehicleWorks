# VehicleWorks 🚗
This application is a simple app to manage vehicles and their weight categories. 

## Tech stack
Frontend:
- .NET 10
- Blazor

Backend:
- C#
- MS SQL Server 2022 through Docker

## Initial setup 🛠️
### Prerequisities
- .NET 10.0.xx
- Docker 29.xx

### Set up steps
1. Clone the repository to your local storage.
2. Copy the .env.example file and save as a file called .env. 
3. Then populate it with your own values. (Note: the SQL password entered must follow SQL Server requirements: 8 characters or more, at least one uppercase character, lowercase character and a symbol.)
4. Run command "docker compose up" in the root folder
5. Run command "dotnet run" in the VehicleWorks.Web folder
6. Server will now be live on http://localhost:5058