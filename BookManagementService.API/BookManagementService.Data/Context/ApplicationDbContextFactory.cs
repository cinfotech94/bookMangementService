//using BookManagementService.Data.Context;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
//{
//    public ApplicationDBContext CreateDbContext(string[] args)
//    {
//        var configuration = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile("appsettings.json")
//        .Build();

//        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
//        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

//        return new ApplicationDBContext(optionsBuilder.Options);
//    }
//}

