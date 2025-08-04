#joanna_news 


## Code First Approach

## Install packages required
-- MSSQL
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

--AutoMapper
``` dotnet add package automapper```


### To create new migration file
-- Crete new migraiton file "initialMigrations_TIMESTAMP" with sql script
dotnet ef migrations add initialMigrations
dotnet ef migrations add initialMigrations -o ./Data/Migrations


### To apply migration against DataBase
dotnet ef database update


### To View Migrations
dotnet ef migrations list


## To add db context
```
// NewsWebisteDbContext.cs
using Microsoft.EntityFrameworkCore;

public class NewswebsiteContext : DbContext
{
    public NewsWebsiteContext(DbContextOptions<NewsWebsiteContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } // Example entity
}

```


### Make Edits in Program.cs
```
// ✅ Add DbContext and SQL Server connection string
builder.Services.AddDbContext<NewsWebsiteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

```

Set connectionStirng in appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=WandaTech;Database=NewsWebsite_Db;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
