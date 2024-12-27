using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sun_Flower_Hotel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Data
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BookingService> BookingServices { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<PaymentInvoice> PaymentInvoices { get; set; }

        public HotelDbContext() { }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                //Console.WriteLine($"Using Connection String: {connectionString}");

                optionsBuilder.UseSqlServer(connectionString);

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Room>()
        .HasKey(r => r.RoomNumber);

            // Room Configuration
            modelBuilder.Entity<Room>()
                .Property(r => r.Type)
                .IsRequired();

            modelBuilder.Entity<Room>()
                .Property(r => r.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Room>()
                .Property(r => r.ExtraBeds)
                .IsRequired();

            modelBuilder.Entity<Room>()
                .Property(r => r.Size)
                .IsRequired();

            modelBuilder.Entity<Room>()
                .Property(r => r.MaxGuests)
                .IsRequired();

            // Employee Configuration
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Role)
                .HasMaxLength(50);

            // Service Configuration
            modelBuilder.Entity<Service>()
                .HasKey(s => s.ServiceId);

            modelBuilder.Entity<Service>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            // Feedback Configuration
            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.FeedbackId);

            modelBuilder.Entity<Feedback>()
                .Property(f => f.Comments)
                .HasMaxLength(1000);

            modelBuilder.Entity<Feedback>()
                .Property(f => f.Rating)
                .IsRequired();

            // PaymentInvoice Configuration
            modelBuilder.Entity<PaymentInvoice>()
                .HasKey(p => p.PaymentInvoiceId);

            modelBuilder.Entity<PaymentInvoice>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PaymentInvoice>()
                .Property(p => p.PaymentMethod)
                .HasMaxLength(50);

            modelBuilder.Entity<PaymentInvoice>()
                .Property(p => p.PaymentStatus)
                .HasMaxLength(20);

            modelBuilder.Entity<PaymentInvoice>()
                .Property(p => p.InvoiceNotes)
                .HasMaxLength(500);

            // Define one-to-many relationship
            modelBuilder.Entity<PaymentInvoice>()
                .HasOne(pi => pi.Booking)
                .WithMany(b => b.PaymentInvoices)
                .HasForeignKey(pi => pi.BookingId);


            // BookingService Many-to-Many Relationship

            modelBuilder.Entity<BookingService>()
        .HasKey(bs => bs.BookingServiceId); // Composite key for the junction table

            modelBuilder.Entity<BookingService>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingServices)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingService>()
                .HasOne(bs => bs.Service)
                .WithMany(s => s.BookingServices)
                .HasForeignKey(bs => bs.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure BookingId as an auto-generated identity column
            modelBuilder.Entity<Booking>()
                .Property(b => b.BookingId)
                    .ValueGeneratedOnAdd(); // This ensures BookingId is auto-incremented

            // Configure Booking ↔ Employee Relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Employee)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee ↔ Feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Employee)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }


}

