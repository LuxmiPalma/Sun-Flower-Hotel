using Microsoft.EntityFrameworkCore;
using Sun_Flower_Hotel.Data;
using Sun_Flower_Hotel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sun_Flower_Hotel.Helper;

namespace Sun_Flower_Hotel.Helper
{
    public class RoomDatabaseHelper
    {
        private readonly DbContextOptions<HotelDbContext> _options;

        // Constructor to inject DbContext options
        public RoomDatabaseHelper(DbContextOptions<HotelDbContext> options)
        {
            _options = options;
        }

        /// <summary>
        /// Adds a new room to the database.
        /// </summary>
        public void AddRoom(Room room)
        {

            using (var context = new HotelDbContext(_options))
            {
                try
                {
                    Console.WriteLine("Attempting to add room:");
                    Console.WriteLine($"Type: {room.Type}, Price: {room.Price}, ExtraBeds: {room.ExtraBeds}, Size: {room.Size}, MaxGuests: {room.MaxGuests}, IsAvailable: {room.IsAvailable}");

                    context.Rooms.Add(room);
                    int result = context.SaveChanges();
                    Console.WriteLine($"SaveChanges Result: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Retrieves all rooms from the database.
        /// </summary>
        public List<Room> GetAllRooms()
        {
            using (var context = new HotelDbContext(_options))
            {
                try
                {
                    return context.Rooms.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Failed to retrieve rooms. {ex.Message}");
                    return new List<Room>();
                }
            }
        }

        /// <summary>
        /// Deletes a room from the database by room number.
        /// </summary>
        public void DeleteRoom(int roomNumber)
        {
            using (var context = new HotelDbContext(_options))
            {
                try
                {
                    var room = context.Rooms.Find(roomNumber);
                    if (room != null)
                    {
                        context.Rooms.Remove(room);
                        context.SaveChanges();
                        Console.WriteLine($"Room {roomNumber} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Room {roomNumber} not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Failed to delete room. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Updates an existing room in the database.
        /// </summary>
        public void UpdateRoom(Room room)
        {
            using (var context = new HotelDbContext(_options))
            {
                try
                {
                    context.Rooms.Update(room);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Failed to update room. {ex.Message}");
                    throw; // Rethrow the exception for higher-level handling

                }
            }
        }

        /// <summary>
        /// Finds a room in the database by room number.
        /// </summary>
        public Room FindRoom(int roomNumber)
        {
            using (var context = new HotelDbContext(_options))
            {
                try
                {
                    return context.Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Failed to find room. {ex.Message}");
                    return null;
                }
            }
        }
        public List<Room> SearchRooms(string type = null, decimal? minPrice = null, decimal? maxPrice = null, bool? isAvailable = null)
        {
            using (var context = new HotelDbContext(_options))
            {
                try
                {
                    var query = context.Rooms.AsQueryable();

                    if (!string.IsNullOrEmpty(type))
                        query = query.Where(r => r.Type == type);

                    if (minPrice.HasValue)
                        query = query.Where(r => r.Price >= minPrice.Value);

                    if (maxPrice.HasValue)
                        query = query.Where(r => r.Price <= maxPrice.Value);

                    if (isAvailable.HasValue)
                        query = query.Where(r => r.IsAvailable == isAvailable.Value);

                    return query.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Failed to search rooms. {ex.Message}");
                    return new List<Room>();
                }
            }


        }





    }
}


