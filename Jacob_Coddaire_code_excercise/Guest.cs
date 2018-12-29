using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Models
{
    public class Guest
    {
        public int GuestID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        //This will track the number of rooms that need to be handicap accessible.
        public int RequiredHandicapRooms { get; set; } = 0;

        //This will track the number of pets per guest.
        public int PetCount { get; set; } = 0;

        public Reservation Reservation { get; set; } = new Reservation();
    }
}
