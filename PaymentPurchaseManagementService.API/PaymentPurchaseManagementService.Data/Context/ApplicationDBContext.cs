//using PaymentPurchaseManagementService.Domain.Entity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Reflection.Metadata.BlobBuilder;

//namespace PaymentPurchaseManagementService.Data.Context;
//public class ApplicationDBContext : DbContext
//{
//    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
//    public DbSet<Book> books { get; set; }
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Book>()
//            .HasIndex(c => new { c.id, c.ISBN})
//            .HasDatabaseName("is,ISBN");

//    }

//}
