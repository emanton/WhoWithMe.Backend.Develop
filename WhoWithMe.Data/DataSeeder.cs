using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WhoWithMe.Core.Entities.dictionaries;
using WhoWithMe.Core.Entities;
using System;

namespace WhoWithMe.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(EFDbContext context)
        {
            // Ensure database created/migrated done by caller

            // Seed MeetingType
            var defaultMeetingTypes = new List<MeetingType>
            {
                new MeetingType { Name = "Casual" },
                new MeetingType { Name = "Study" },
                new MeetingType { Name = "Sport" },
                new MeetingType { Name = "Work" },
                new MeetingType { Name = "Party" }
            };

            foreach (var mt in defaultMeetingTypes)
            {
                if (!await context.MeetingType.AnyAsync(x => x.Name == mt.Name))
                {
                    context.MeetingType.Add(mt);
                }
            }

            // Seed City
            var defaultCities = new List<City>
            {
                new City { Name = "New York" },
                new City { Name = "London" },
                new City { Name = "Moscow" },
                new City { Name = "San Francisco" },
                new City { Name = "Berlin" }
            };

            foreach (var city in defaultCities)
            {
                if (!await context.City.AnyAsync(x => x.Name == city.Name))
                {
                    context.City.Add(city);
                }
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // -----------------------------------------------------------------
            // Seed demo users
            // -----------------------------------------------------------------

            // Only add users if none of the demo emails exist
            var demoUsers = new List<User>
            {
                new User
                {
                    Nickname = "alice",
                    Firstname = "Alice",
                    Lastname = "Anderson",
                    Email = "alice@example.com",
                    Password = "password", // use hashing in real scenarios
                    Phone = "+1-555-0100",
                },
                new User
                {
                    Nickname = "bob",
                    Firstname = "Bob",
                    Lastname = "Brown",
                    Email = "bob@example.com",
                    Password = "password",
                    Phone = "+1-555-0101",
                }
            };

            foreach (var u in demoUsers)
            {
                if (!await context.User.AnyAsync(x => x.Email == u.Email))
                {
                    // assign a default city (first in DB)
                    var city = await context.City.FirstOrDefaultAsync();
                    if (city != null)
                    {
                        u.City = city;
                    }

                    context.User.Add(u);
                }
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }

            // -----------------------------------------------------------------
            // Seed demo meetings
            // -----------------------------------------------------------------

            // Select an existing meeting type and city
            var casualType = await context.MeetingType.FirstOrDefaultAsync(x => x.Name == "Casual");
            var defaultCity = await context.City.FirstOrDefaultAsync();
            var aliceUser = await context.User.FirstOrDefaultAsync(x => x.Email == "alice@example.com");
            var bobUser = await context.User.FirstOrDefaultAsync(x => x.Email == "bob@example.com");

            if (casualType != null && defaultCity != null && aliceUser != null)
            {
                // Add a meeting created by Alice
                if (!await context.Meeting.AnyAsync(m => m.Title == "Morning Run" && m.CreatorId == aliceUser.Id))
                {
                    var meeting = new Meeting
                    {
                        Title = "Morning Run",
                        Description = "Casual 5km run in the park",
                        Requirements = "Running shoes",
                        Address = "Central Park",
                        CreatedDate = DateTime.UtcNow,
                        StartDate = DateTime.UtcNow.AddDays(3),
                        Latitude = 40.785091,
                        Longitude = -73.968285,
                        CreatorId = aliceUser.Id,
                        CityId = defaultCity.Id,
                        MeetingTypeId = casualType.Id
                    };

                    context.Meeting.Add(meeting);
                }
            }

            if (casualType != null && defaultCity != null && bobUser != null)
            {
                // Add a meeting created by Bob
                if (!await context.Meeting.AnyAsync(m => m.Title == "Evening Study" && m.CreatorId == bobUser.Id))
                {
                    var meeting = new Meeting
                    {
                        Title = "Evening Study",
                        Description = "Group study session: algorithms",
                        Requirements = "Laptop",
                        Address = "Local Library",
                        CreatedDate = DateTime.UtcNow,
                        StartDate = DateTime.UtcNow.AddDays(5),
                        Latitude = 51.507351,
                        Longitude = -0.127758,
                        CreatorId = bobUser.Id,
                        CityId = defaultCity.Id,
                        MeetingTypeId = casualType.Id
                    };

                    context.Meeting.Add(meeting);
                }
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
