using AutoservisSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoservisSystem.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<OrdersPart> OrdersParts { get; set; }
        public DbSet<CalendarDay> CalendarDays { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    CarId = 1,
                    YearOfProduction = 0,
                    VIN = "Default",
                    ModelType = "Default",
                    LastSTK = DateTime.Parse("1.1.0001"),
                    LastOilChange = DateTime.Parse("1.1.0001"),
                    EngineVolume = 0,
                    EngineNumber = "Default",
                    EngigePowerInKW = 0,
                    CustomerId = 1,
                    CarEvidenceNumber = "Default",
                    CalendarEvents = new List<CalendarEvent>(),
                    Brand = "Default",
                    AutomaticTransmission = false
                });

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    Email = "Default",
                    Surname = "Default",
                    Name = "Default",
                    MobileNumber = "Default"
                });

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    Hours = 0,
                    Name = "Nepriradené"
                });
        }

    }
}
