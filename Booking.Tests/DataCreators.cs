using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Helpers;
using Booking.Models;

namespace Booking.Tests
{
    public abstract class DataCreators
    {
        /// <summary>
        /// Gets a test motel for automated tests.
        /// </summary>
        /// <returns></returns>
        public Models.Motel GetMotel()
        {
            var result = new Motel();

            //generate all rooms.
            for(int currentLevel = 1; currentLevel <= result.MaxLevelCount; currentLevel++)
            {
                for (int currentRoom = 0; currentRoom < result.MaxRoomCountPerLevel; currentRoom++)
                {
                    var r = new Room();
                    r.Level = currentLevel;
                    r.RoomNumber = (currentLevel * 100) + currentRoom;

                    //Get a random number between 1 and 3. used a helper method because this is being created in a
                    //tight (ie fast) loop and the Math.Random class doesn't like that.
                    r.BedCount = StaticRandom.Instance.Next(1, 4);
                    
                    result.Rooms.Add(r);                    
                }
            }

            //TODO: generate Guests

            //TODO: generate Reservations

            return result;
        }
    }
}
