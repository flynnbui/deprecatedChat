Create new migration
dotnet ef migrations add MigrationName -s ./ChatApp.API/ -p ./ChatApp.Infrastructure

Update database
dotnet ef database update -s ./ChatApp.API/ -p ./ChatApp.Infrastructure

Drop database
dotnet ef database drop -s ./ChatApp.API/ -p ./ChatApp.Infrastructure

Remove latest migration
ef migrations remove -s ./ChatApp.API/ -p ./ChatApp.Infrastructure
