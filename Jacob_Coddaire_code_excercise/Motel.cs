using Booking.Models;
using System;
using System.Collections.Generic;

namespace Booking.Models
{
    public class Motel
    {
        public int MaxLevelCount
        {
            get
            {
                return 2;
            }
        }

        public int MaxRoomCountPerLevel
        {
            get
            {
                return 20; //not specified in the spec. This is an assumption we can discuss.
            }
        }

        public int TotalRoomCount
        {
            get
            {
                return MaxLevelCount * MaxRoomCountPerLevel;
            }
        }

        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<Guest> Guests { get; set; } = new List<Guest>();
    }
}
