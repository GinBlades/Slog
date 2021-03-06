# ASP.NET Core MVC Template

This will be a general purpose template for starting ASP.NET Core MVC projects.

## Packages

### User Secrets

User secrets will be used to store database configuration.

    ConnectionStrings:DefaultConnection
    User ID=username;Password=secretpassword;Host=localhost;Port=5432;Database=dbname;Pooling=true;

Also save Admin information:

    Admin:Username "UserName"
    Admin:Email "test@example.com"
    Admin:Password "SecretPassword"

For production, use an `appsettings.Production.json` file:

    {
        "ConnectionStrings": {
            "DefaultConnection": "connection"
        }, "Admin": {
            "Username": "adminUser",
            "Password": "adminPassword",
            "Email": "admin@example.com"
        }
    }

A proper connection string for SQL Server, if you are connecting to a local database, in the `.json` file might look like:

	"DefaultConnection": "Server=.\\MSSQLSERVER,1433;Initial Catalog=Slog;User ID=[username];Password=[password];Connection Timeout=30;"

## Renaming Solution

Some files and namespaces within files will need to be renamed.

* From the root, `Slog.sln` should be renamed and any usage of `Slog` within the file should be replaced.
* There are file paths to be renamed in `.vs/restore.dg`, but I'm not sure if this is necessary since it is not checked in to git.
* In the project files, anywhere the `SlogWeb` namespace is used, rename it.
* Rename the `.xproj` file and the `RootNamespace` element in that file.
* References for the test project will need to be renamed.
* Rename the repository in `.git/config` so changes don't get accidentally pushed to the main template.
