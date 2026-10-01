# VehicleWorks 🚗
VehicleWorks is a simple web application for managing vehicles and their weight categories.

## Tech stack
### Frontend
* Blazor
* HTML
* CSS
* C#

### Backend
* ASP.NET Core
* C#
* Entity Framework Core
* Microsoft SQL Server 2022
* Docker
* EF Core Migrations

## Initial setup 🛠️
### Prerequisites

* .NET 10 SDK
* Docker Desktop

### Setup steps
1. Clone the repository to your local machine.
2. Copy `.env.example` and rename the copy to `.env`.
3. Open `.env` and set the SQL Server password. The password must meet SQL Server's password requirements:
   * At least 8 characters
   * At least one uppercase letter
   * At least one lowercase letter
   * At least one symbol
4. Navigate to the `VehicleWorks.Web` folder:

   ```bash
   cd VehicleWorks.Web
   ```

5. Add the database connection string, replacing `REPLACE_PASSWORD` with the same password used in `.env`:

   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=VehicleWorks;User Id=sa;Password=REPLACE_PASSWORD;TrustServerCertificate=True"
   ```

6. Restore the local .NET tool:

   ```bash
   dotnet tool restore
   ```

7. Return to the repository root and start SQL Server:
   ```bash
   cd ..
   docker compose up -d
   ```
8. Apply the Entity Framework database migrations:

   ```bash
   dotnet ef database update --project VehicleWorks.Web
   ```
9. Start the application:

```bash
dotnet run --project VehicleWorks.Web
```

10. Open the application at:
`http://localhost:5058`

## Running the tests 🧪
From the repository root, run:

```bash
dotnet test
```

The automated tests cover vehicle validation, category assignment, category range validation, and category add/edit/delete behaviour.

## Design decisions
### Category boundary rule

Category ranges use shared boundaries, but a shared boundary belongs to the category below it.

For example:

* `500.00 kg` → Light
* `500.01 kg` → Medium
* `2500.00 kg` → Medium
* `2500.01 kg` → Heavy

### Dynamic boundary calculation
Category ranges can be modified through the application. The neighbouring category boundaries are automatically adjusted to maintain complete weight coverage without gaps or overlaps.

For example, if the categories are initially:

* Light: 0–500 kg
* Medium: 500–2500 kg
* Heavy: 2500 kg–No maximum

If Medium is changed to have a maximum of 2000 kg, the Heavy category automatically updates to start at 2000 kg:

* Light: 0–500 kg
* Medium: 500–2000 kg
* Heavy: 2000 kg–No maximum

This dynamic category editing also occurs when adding or deleting categories.

### What I would add next
* A service to edit the list of manufacturers available to select so this can be done by the end user rather than via a code/production database change.
* More fields to the vehicle cateogry, such as registration number, WOF expiry, registration expiry, last service date.
* A more modern user interface, for the purposes of this assignment I've kept it as the default Blazor template.

## Application features

* Add, edit and delete vehicles
* Validate vehicle details
* Automatically assign vehicle categories based on weight
* Add, edit and delete vehicle categories
* Configure category weight ranges
* Assign category icons
* Sort vehicles by owner, manufacturer, year or weight
* Sort in ascending or descending order
* Persist data using SQL Server and Entity Framework Core
* Automated tests for key business rules
