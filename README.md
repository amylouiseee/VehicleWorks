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
4. In the VehicleWorks.Web folder, run the command "dotnet user-secrets init"
5. In the same folder, run the command following command, making sure to replace the password with the same one in your .env: "dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=VehicleWorks;User Id=sa;Password=REPLACE_PASSWORD;TrustServerCertificate=True""
6. Run command "dotnet tool restore" in the same folder
7. Run command "docker compose up" in the root folder
8. Run command "dotnet ef database update --project VehicleWorks.Web" in the root folder
9. Run command "dotnet run --project VehicleWorks.Web" in the root folder
6. Server will now be live on http://localhost:5058