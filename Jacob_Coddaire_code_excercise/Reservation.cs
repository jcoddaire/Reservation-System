using System;
using System.Collections.Generic;

namespace Booking.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int GuestID { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);//default to tomorrow.
    }
}