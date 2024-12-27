using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sun_Flower_Hotel.Data;
using Sun_Flower_Hotel.Menu;
using Sun_Flower_Hotel.Menu.Booking_Menu;
using Sun_Flower_Hotel.Menus;


namespace Sun_Flower_Hotel
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Configure the database context
            var builder = new ConfigurationBuilder()
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<HotelDbContext>();
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            using (var dbContext = new HotelDbContext(options.Options))
            {
                var initializer = new DataInitializer();
                initializer.MigrateAndSeed(dbContext); // Apply migrations and seed data
            }

            // Pass the same options to InvoiceMenu
            using (var dbContext = new HotelDbContext(options.Options))
            {
                var mainMenu = new MainMenu();
                mainMenu.DisplayMenu();


            }
        }
    }
}
