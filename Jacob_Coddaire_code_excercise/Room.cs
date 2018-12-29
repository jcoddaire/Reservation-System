using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Models
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public int BedCount { get; set; } = 1;
        public int Level { get; set; } = 1;

        public bool IsHandicapAccessible {
            get
            {
                if(Level == 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsPetFriendly
        {
            get
            {
                if (Level == 1)
                {
                    return true;
                }
                return false;
            }
        }


        public double CostPerNight {

            get
            {
                switch (BedCount)
                {
                    case 1:
                        return 50;
                    case 2:
                        return 75;
                    case 3:
                        return 90;
                    default:
                        return 0;
                }
            }

        }
    }
}
