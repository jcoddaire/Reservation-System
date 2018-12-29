using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Booking.Helpers;
using Booking.Models;

namespace Booking.Business
{
    public class BookingSystem : IBookingSystem
    {
        //The number of dollars to charge per pet.
        private const double PET_COST = 20;

        /// <summary>
        /// Calculates the reservation cost.
        /// </summary>
        /// <param name="guest">The guest.</param>
        /// <param name="reservation">The reservation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Guest is required! Please create a guest.
        /// or
        /// Reservation is required! Please create a reservation.
        /// </exception>
        /// <exception cref="InvalidOperationException">Error! No rooms are selected! Please select a room.</exception>
        public double CalcualteReservationCost(Guest guest, Reservation reservation)
        {
            if (guest == null)
            {
                throw new ArgumentNullException("Guest is required! Please create a guest.");
            }
            if (reservation == null)
            {
                throw new ArgumentNullException("Reservation is required! Please create a reservation.");
            }
            if (reservation.Rooms == null || reservation.Rooms.Count <= 0)
            {
                throw new InvalidOperationException("Error! No rooms are selected! Please select a room.");
            }

            //TODO: other sanity checks : guests have names, rooms have numbers, etc. Make sure the Start and End dates are not the same, past, present future, etc.
                        

            double durationOfStay = (reservation.EndDate - reservation.StartDate).TotalDays;

            double result = 0;
            foreach (var room in reservation.Rooms)
            {
                result += (room.CostPerNight * durationOfStay);
            }

            //TODO: figure out if the pet charge should be once per stay or once per night. 

            /*
             * Consider this: you stay for one night, with one pet. The pet fee should be 20.00.
             * 
             * What if it is for a five day stay? Should the total pet fee be re-charged each night, for a total  of 100.00? Or is the one time 20.00 fee enough?
             */

            //For now assume it's once per stay / reservation, ie 20.00.
            if(guest.PetCount > 0)
            {
                result += (guest.PetCount * PET_COST);
            }

            return result;
        }

        /// <summary>
        /// Cancels the reservation.
        /// </summary>
        /// <param name="guest">The guest.</param>
        /// <param name="reservation">The reservation.</param>
        /// <returns></returns>
        public string CancelReservation(Guest guest, Reservation reservation)
        {
            var reservations = GetReservations();
            if(reservations == null || reservations.Count <= 0)
            {
                // there are no reservations in the system. Say it's fine.
                return "No reservations were found.";
            }

            //The theory: you don't want anyone just calling up and cancelling someone's reservation.
            //So we need to validate that the name of the caller matches the name of the reservation.
            //For now assume the client (web browser) already has a List<Guest> and the correct information is provided in the Guest object.
            //Used in this way, finding reservations based onthe GuestID should be sufficient.
            //I could also create a search based on First Name / Last name, but that has too many issues.
            //See this list: https://www.kalzumeus.com/2010/06/17/falsehoods-programmers-believe-about-names/

            var targetReservations = reservations.Where(x => x.GuestID == guest.GuestID).ToList();

            if (targetReservations.Count() <= 0)
            {
                return "No reservations were found.";
            }

            if (targetReservations.Any(x => x.ReservationID == reservation.ReservationID))
            {
                //the reservation exists, now delete it.
                //TODO: delete the reservation from the database.
            }
            return "The reservation has been deleted successfully.";
        }

        /// <summary>
        /// Reserves the room.
        /// </summary>
        /// <param name="guest">The guest.</param>
        /// <param name="reservation">The reservation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Guest is required! Please create a guest.
        /// or
        /// Reservation is required! Please create a reservation.
        /// </exception>
        /// <exception cref="InvalidOperationException">Error! No rooms are selected! Please select a room.</exception>
        public string CreateReservation(Guest guest, Reservation reservation)
        {
            if (guest == null)
            {
                throw new ArgumentNullException("Guest is required! Please create a guest.");
            }
            if (reservation == null)
            {
                throw new ArgumentNullException("Reservation is required! Please create a reservation.");
            }
            if(reservation.Rooms == null || reservation.Rooms.Count <= 0)
            {
                throw new InvalidOperationException("Error! No rooms are selected! Please select a room.");
            }

            //TODO: other sanity checks : guests have names, rooms have numbers, etc. Make sure the Start and End dates are not the same, past, present future, etc.

            //check to ensure there are enough Handicap accessible rooms.
            if (guest.RequiredHandicapRooms > 0)
            {
                var handicapRoomCount = 0;
                foreach (var room in reservation.Rooms)
                {
                    if (room.IsHandicapAccessible)
                    {
                        handicapRoomCount++;
                    }
                    if(handicapRoomCount != guest.RequiredHandicapRooms)
                    {
                        return $"Error! {guest.FirstName} requires {guest.RequiredHandicapRooms} handicap-accessible rooms, but {handicapRoomCount} rooms were selected!";
                    }
                }
            }

            //check to ensure there are not too many pets.
            if(guest.PetCount > 2)
            {
                return "Sorry! According to the Motel Pet Policy, only a maximum of two pets are allowed. The reservation was not saved.";
            }

            //if pets are required, make sure the rooms they selected are pet friendly.
            if(guest.PetCount > 0)
            {
                var petFriendlyRoomCount = reservation.Rooms.Where(x => x.IsPetFriendly).Count();

                if(petFriendlyRoomCount <= 0)
                {
                    return $"Error! {guest.FirstName} requires at least one pet-friendly room! Please select a room on the ground floor. The reservation was not saved.";
                }
            }

            
            //Check if all rooms will be full
            var availableRooms = GetAvailableRooms(reservation.StartDate, reservation.EndDate, guest.RequiredHandicapRooms > 0, guest.PetCount > 0);
            if(availableRooms.Count <= 0)
            {
                return "Error! All rooms are booked! Please select another time frame.";
            }

            //check if the rooms are already in use by another reservation during the specified time period.
            //people can wait five minutes before clicking the submit button, so what was available five minutes ago may not be right now.
            foreach (var roomCantidate in reservation.Rooms)
            {
                if(!availableRooms.Any(x => x.RoomNumber == roomCantidate.RoomNumber))
                {
                    return $"Error! Room {roomCantidate.RoomNumber} is already reserved. Please select another room.";
                }
            }
            

            //calculate the cost.
            var totalCost = CalcualteReservationCost(guest, reservation);

            //TODO: add database logic:
            // 1. Save guest info.
            // 2. Save reservation info.
            // 3. Verify reservation now has a ReservationID > 0 - it should be stored as an Identity column in the DB.

            //ASSUMING THE DB LOGIC WORKS:

            return $"Thanks! The reservation({reservation.ReservationID}) was placed successfully! Your total cost was ${totalCost.ToString("#.##")}.";
        }

        /// <summary>
        /// Gets all rooms in the Motel.
        /// </summary>
        /// <returns></returns>
        public List<Room> GetAllRooms()
        {
            //READER's NOTE:
            //Normally this would be fetched from the Database but since I do not have that, I'll just hard-code it.

            var result = new Motel();

            //generate all rooms.
            for (int currentLevel = 1; currentLevel <= result.MaxLevelCount; currentLevel++)
            {
                for (int currentRoom = 0; currentRoom < result.MaxRoomCountPerLevel; currentRoom++)
                {
                    var r = new Room();
                    r.Level = currentLevel;
                    r.RoomNumber = (currentLevel * 100) + currentRoom;

                    //Get a random number between 1 and 3. 
                    //Used a helper method because this is being created in a
                    //tight (ie fast) loop and the Math.Random class generates random numbers based on millisecond ticks.
                    r.BedCount = StaticRandom.Instance.Next(1, 4);

                    result.Rooms.Add(r);
                }
            }

            return result.Rooms;
        }

        /// <summary>
        /// Determines whether the specified room is available during the specified time frame.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="willRequireHandicap">if set to <c>true</c> [will require handicap rooms].</param>
        /// <param name="willRequirePets">if set to <c>true</c> [will require a room that works with pets].</param>
        /// <returns>
        ///   <c>true</c> if the specified room is available; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// startDate
        /// or
        /// endDate
        /// </exception>
        /// <exception cref="ArgumentException">Start date must precede the end date.</exception>
        public List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate, bool willRequireHandicap = false, bool willRequirePets = false)
        {
            //this should never happen.
            if (startDate == null)
            {
                throw new ArgumentNullException("startDate");
            }
            //this should never happen.
            if (endDate == null)
            {
                throw new ArgumentNullException("endDate");
            }
            if(startDate >= endDate)
            {
                throw new ArgumentException("Start date must precede the end date.");
            }
            
            //Get all rooms in the system
            var availableRooms = GetAllRooms();

            //Get all reservations between the specified period of time.
            var reservations = GetReservations(startDate, endDate);

            //Remove rooms that are already reserved.
            foreach (var r in reservations)
            {
                foreach(var room in r.Rooms)
                {
                    var target = availableRooms.Where(a => a.RoomNumber == room.RoomNumber).FirstOrDefault();
                    if(target != null)
                    {
                        availableRooms.Remove(target);
                    }
                }
            }

            //if the Guest requires a handicap-capable room, remove all non-handicap rooms.
            if (willRequireHandicap)
            {
                var removalTargets = availableRooms.Where(x => x.IsHandicapAccessible == false).ToList();
                foreach(var rt in removalTargets)
                {
                    if (rt != null)
                    {
                        availableRooms.Remove(rt);
                    }
                }
            }

            //if the guest requires a pet-friendly room, remove all non-pet-friendly rooms.
            if (willRequirePets)
            {
                var removalTargets = availableRooms.Where(x => x.IsPetFriendly == false).ToList();
                foreach (var rt in removalTargets)
                {
                    if (rt != null)
                    {
                        availableRooms.Remove(rt);
                    }
                }
            }

            return availableRooms;
        }

        public List<Reservation> GetReservations()
        {
            var result = new List<Reservation>();

            //TODO:  wire this up to the database. But that is out of scope.

            return result;
        }

        /// <summary>
        /// Gets all reservations between the two dates.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// startDate
        /// or
        /// endDate
        /// </exception>
        /// <exception cref="ArgumentException">Start date must precede the end date.</exception>
        public List<Reservation> GetReservations(DateTime startDate, DateTime endDate)
        {
            //this should never happen.
            if (startDate == null)
            {
                throw new ArgumentNullException("startDate");
            }
            //this should never happen.
            if (endDate == null)
            {
                throw new ArgumentNullException("endDate");
            }
            if (startDate >= endDate)
            {
                throw new ArgumentException("Start date must precede the end date.");
            }

            var reservations = GetReservations().Where(x => x.StartDate >= startDate && x.EndDate <= endDate).ToList();            

            return reservations;
        }
    }
}
