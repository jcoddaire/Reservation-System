using System;
using System.Collections.Generic;
using System.Text;
using Booking.Models;

namespace Booking.Business
{
    public interface IBookingSystem
    {
        /// <summary>
        /// Gets all reservations in the system.
        /// </summary>
        /// <returns></returns>
        List<Reservation> GetReservations();

        /// <summary>
        /// Gets all reservations between the two dates.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        List<Reservation> GetReservations(DateTime startDate, DateTime endDate);

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
        List<Room> GetAvailableRooms(DateTime startDate, DateTime endDate, bool willRequireHandicap, bool willRequirePets);

        /// <summary>
        /// Gets all rooms in the Motel.
        /// </summary>
        /// <returns></returns>
        List<Room> GetAllRooms();

        /// <summary>
        /// Calculates the reservation cost.
        /// </summary>
        /// <param name="guest">The guest.</param>
        /// <param name="reservation">The reservation.</param>
        /// <returns></returns>
        double CalcualteReservationCost(Guest guest, Reservation reservation);

        /// <summary>
        /// Reserves the reservation.
        /// </summary>
        /// <param name="guest">The guest.</param>
        /// <param name="reservation">The reservation.</param>
        /// <returns></returns>
        string CreateReservation(Guest guest, Reservation reservation);

        /// <summary>
        /// Cancels the reservation.
        /// </summary>
        /// <param name="guest">The guest.</param>
        /// <param name="reservation">The reservation.</param>
        /// <returns></returns>
        string CancelReservation(Guest guest, Reservation reservation);
    }
}
