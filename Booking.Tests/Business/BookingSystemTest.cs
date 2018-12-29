using System;
using System.Linq;
using Booking.Models;
using Booking.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Booking.Tests.Business
{
    [TestClass]
    public class BookingSystemTest : DataCreators
    {
        [TestMethod]
        public void MotelLevelCount()
        {
            var motel = new Models.Motel();
            Assert.IsNotNull(motel);
            Assert.AreEqual(2, motel.MaxLevelCount);
        }

        [TestMethod]
        public void Room_IsHandicapAccessible_TRUE()
        {
            var room = new Room() { Level = 1 };
            Assert.IsNotNull(room);
            Assert.IsTrue(room.IsHandicapAccessible);
        }

        [TestMethod]
        public void Room_IsHandicapAccessible_FALSE()
        {
            var room = new Room() { Level = 2 };
            Assert.IsNotNull(room);
            Assert.IsFalse(room.IsHandicapAccessible);
        }

        #region Cost Checks
        [TestMethod]
        public void Room_CostCheck_1_Bed()
        {
            const double EXPECTED_COST = 50;
            const int BED_COUNT = 1;

            var room = new Room() { BedCount = BED_COUNT };
            Assert.IsNotNull(room);
            Assert.AreEqual(EXPECTED_COST, room.CostPerNight);
        }

        [TestMethod]
        public void Room_CostCheck_2_Bed()
        {
            const double EXPECTED_COST = 75;
            const int BED_COUNT = 2;

            var room = new Room() { BedCount = BED_COUNT };
            Assert.IsNotNull(room);
            Assert.AreEqual(EXPECTED_COST, room.CostPerNight);
        }

        [TestMethod]
        public void Room_CostCheck_3_Bed()
        {
            const double EXPECTED_COST = 90;
            const int BED_COUNT = 3;

            var room = new Room() { BedCount = BED_COUNT };
            Assert.IsNotNull(room);
            Assert.AreEqual(EXPECTED_COST, room.CostPerNight);
        }

        //Check errors too:

        [TestMethod]
        public void Room_CostCheck_4_Bed()
        {
            const double EXPECTED_COST = 0;
            const int BED_COUNT = 4;

            var room = new Room() { BedCount = BED_COUNT };
            Assert.IsNotNull(room);
            Assert.AreEqual(EXPECTED_COST, room.CostPerNight);
        }

        [TestMethod]
        public void Room_CostCheck_0_Bed()
        {
            const double EXPECTED_COST = 0;
            const int BED_COUNT = 0;

            var room = new Room() { BedCount = BED_COUNT };
            Assert.IsNotNull(room);
            Assert.AreEqual(EXPECTED_COST, room.CostPerNight);
        }
        #endregion

        #region Pet Friendly Checks
        [TestMethod]
        public void Room_Is_Pet_Friendly_TRUE()
        {
            const int ROOM_LEVEL = 1;

            var room = new Room() { Level = ROOM_LEVEL };
            Assert.IsNotNull(room);
            Assert.IsTrue(room.IsPetFriendly);
        }

        [TestMethod]
        public void Room_Is_Pet_Friendly_FALSE()
        {
            const int ROOM_LEVEL = 2;

            var room = new Room() { Level = ROOM_LEVEL };
            Assert.IsNotNull(room);
            Assert.IsFalse(room.IsPetFriendly);
        }

        [TestMethod]
        public void Room_Is_Pet_Friendly_TRUE_VERBOSE()
        {
            var motel = GetMotel();
            const int TARGET_ROOM_LEVEL = 1;

            foreach (var room in motel.Rooms)
            {
                Assert.IsNotNull(room);

                if (room.Level == TARGET_ROOM_LEVEL)
                {
                    Assert.IsTrue(room.IsPetFriendly);
                }
            }
        }

        [TestMethod]
        public void Room_Is_Pet_Friendly_FALSE_VERBOSE()
        {
            var motel = GetMotel();
            const int TARGET_ROOM_LEVEL = 2;

            foreach (var room in motel.Rooms)
            {
                Assert.IsNotNull(room);

                if(room.Level == TARGET_ROOM_LEVEL)
                {
                    Assert.IsFalse(room.IsPetFriendly);
                }                
            }
        }
        #endregion

        #region Reservation Creation

        [TestMethod]
        public void Reservation_Create_no_handicap_no_pets_single_room_Pass()
        {
            const int PET_COUNT = 0;
            const int HANDICAP_COUNT = 0;
            const double EXPECTED_COST = 450; //Assume 10 day stay.
            const int RESERVATION_ID = 1;

            //prepare the test data.

            var room = new Room();
            room.BedCount = 1;
            room.Level = 1;
            room.RoomNumber = 111;

            //prepare the test data.
            var g = new Guest();
            g.GuestID = 1;
            g.FirstName = "Alice";
            g.LastName = "Smith";
            g.PetCount = PET_COUNT;
            g.RequiredHandicapRooms = HANDICAP_COUNT;

            var r = new Reservation();
            r.ReservationID = RESERVATION_ID;
            r.StartDate = new DateTime(2019, 1, 1);
            r.EndDate = new DateTime(2019, 1, 10);
            r.Rooms.Add(room);

            //test the implementation.
            IBookingSystem b = new BookingSystem();
            var result = b.CreateReservation(g, r);
            Assert.AreEqual($"Thanks! The reservation({r.ReservationID}) was placed successfully! Your total cost was ${EXPECTED_COST.ToString("#.##")}.", result);
        }

        [TestMethod]
        public void Reservation_Create_handicap_no_pets_single_room_Pass()
        {
            const int PET_COUNT = 0;
            const int HANDICAP_COUNT = 1;
            const double EXPECTED_COST = 450; //Assume 10 day stay.
            const int RESERVATION_ID = 1;

            //prepare the test data.

            var room = new Room();
            room.BedCount = 1;
            room.Level = 1;
            room.RoomNumber = 111;

            //prepare the test data.
            var g = new Guest();
            g.GuestID = 1;
            g.FirstName = "Alice";
            g.LastName = "Smith";
            g.PetCount = PET_COUNT;
            g.RequiredHandicapRooms = HANDICAP_COUNT;

            var r = new Reservation();
            r.ReservationID = RESERVATION_ID;
            r.StartDate = new DateTime(2019, 1, 1);
            r.EndDate = new DateTime(2019, 1, 10);
            r.Rooms.Add(room);

            //test the implementation.
            IBookingSystem b = new BookingSystem();
            var result = b.CreateReservation(g, r);
            Assert.AreEqual($"Thanks! The reservation({r.ReservationID}) was placed successfully! Your total cost was ${EXPECTED_COST.ToString("#.##")}.", result);
        }
        
        [TestMethod]
        public void Reservation_Create_no_handicap_pets_single_room_Pass()
        {
            const int PET_COUNT = 1;
            const int HANDICAP_COUNT = 0;
            const double EXPECTED_COST = 470; //Assume 10 day stay.
            const int RESERVATION_ID = 1;

            //prepare the test data.

            var room = new Room();
            room.BedCount = 1;
            room.Level = 1;
            room.RoomNumber = 111;

            //prepare the test data.
            var g = new Guest();
            g.GuestID = 1;
            g.FirstName = "Alice";
            g.LastName = "Smith";
            g.PetCount = PET_COUNT;
            g.RequiredHandicapRooms = HANDICAP_COUNT;

            var r = new Reservation();
            r.ReservationID = RESERVATION_ID;
            r.StartDate = new DateTime(2019, 1, 1);
            r.EndDate = new DateTime(2019, 1, 10);
            r.Rooms.Add(room);

            //test the implementation.
            IBookingSystem b = new BookingSystem();
            var result = b.CreateReservation(g, r);
            Assert.AreEqual($"Thanks! The reservation({r.ReservationID}) was placed successfully! Your total cost was ${EXPECTED_COST.ToString("#.##")}.", result);
        }
        
        [TestMethod]
        public void Reservation_Create_no_handicap_two_pets_single_room_Pass()
        {
            const int PET_COUNT = 2;
            const int HANDICAP_COUNT = 0;
            const double EXPECTED_COST = 490; //Assume 10 day stay.
            const int RESERVATION_ID = 1;

            //prepare the test data.

            var room = new Room();
            room.BedCount = 1;
            room.Level = 1;
            room.RoomNumber = 111;

            //prepare the test data.
            var g = new Guest();
            g.GuestID = 1;
            g.FirstName = "Alice";
            g.LastName = "Smith";
            g.PetCount = PET_COUNT;
            g.RequiredHandicapRooms = HANDICAP_COUNT;

            var r = new Reservation();
            r.ReservationID = RESERVATION_ID;
            r.StartDate = new DateTime(2019, 1, 1);
            r.EndDate = new DateTime(2019, 1, 10);
            r.Rooms.Add(room);

            //test the implementation.
            IBookingSystem b = new BookingSystem();
            var result = b.CreateReservation(g, r);
            Assert.AreEqual($"Thanks! The reservation({r.ReservationID}) was placed successfully! Your total cost was ${EXPECTED_COST.ToString("#.##")}.", result);
        }

        [TestMethod]
        public void Reservation_Create_three_pets_FAIL()
        {
            const int PET_COUNT = 3;
            const int HANDICAP_COUNT = 0;            
            const int RESERVATION_ID = 1;

            //prepare the test data.

            var room = new Room();
            room.BedCount = 1;
            room.Level = 1;
            room.RoomNumber = 111;

            //prepare the test data.
            var g = new Guest();
            g.GuestID = 1;
            g.FirstName = "Alice";
            g.LastName = "Smith";
            g.PetCount = PET_COUNT;
            g.RequiredHandicapRooms = HANDICAP_COUNT;

            var r = new Reservation();
            r.ReservationID = RESERVATION_ID;
            r.StartDate = new DateTime(2019, 1, 1);
            r.EndDate = new DateTime(2019, 1, 10);
            r.Rooms.Add(room);

            //test the implementation.
            IBookingSystem b = new BookingSystem();
            var result = b.CreateReservation(g, r);
            Assert.AreEqual("Sorry! According to the Motel Pet Policy, only a maximum of two pets are allowed. The reservation was not saved.", result);
        }

        [TestMethod]
        public void Reservation_Create_one_pet_wrong_level_FAIL()
        {
            const int PET_COUNT = 1;
            const int HANDICAP_COUNT = 0;
            const int RESERVATION_ID = 1;

            //prepare the test data.

            var room = new Room();
            room.BedCount = 1;
            room.Level = 2;
            room.RoomNumber = 211;

            //prepare the test data.
            var g = new Guest();
            g.GuestID = 1;
            g.FirstName = "Alice";
            g.LastName = "Smith";
            g.PetCount = PET_COUNT;
            g.RequiredHandicapRooms = HANDICAP_COUNT;

            var r = new Reservation();
            r.ReservationID = RESERVATION_ID;
            r.StartDate = new DateTime(2019, 1, 1);
            r.EndDate = new DateTime(2019, 1, 10);
            r.Rooms.Add(room);

            //test the implementation.
            IBookingSystem b = new BookingSystem();
            var result = b.CreateReservation(g, r);
            Assert.AreEqual($"Error! {g.FirstName} requires at least one pet-friendly room! Please select a room on the ground floor. The reservation was not saved.", result);
        }


        [TestMethod]
        public void Reservation_Create_handicap_pets_single_room_Pass()
        {
            const int PET_COUNT = 1;
            const int HANDICAP_COUNT = 1;
            const double EXPECTED_COST = 470; //Assume 10 day stay.
            const int RESERVATION_ID = 1;

            //prepare the test data.

            var room = new Room();
            room.BedCount = 1;
            room.Level = 1;
            room.RoomNumber = 111;

            var g = new Guest();
            g.GuestID = 1;
            g.FirstName = "Alice";
            g.LastName = "Smith";
            g.PetCount = PET_COUNT;
            g.RequiredHandicapRooms = HANDICAP_COUNT;

            var r = new Reservation();
            r.ReservationID = RESERVATION_ID;
            r.StartDate = new DateTime(2019, 1, 1);
            r.EndDate = new DateTime(2019, 1, 10);
            r.Rooms.Add(room);
            
            //test the implementation.
            IBookingSystem b = new BookingSystem();
            var result = b.CreateReservation(g, r);
            Assert.AreEqual($"Thanks! The reservation({r.ReservationID}) was placed successfully! Your total cost was ${EXPECTED_COST.ToString("#.##")}.", result);
        }

        #endregion

        #region Get Available Rooms
        [TestMethod]
        public void GetAvailableRooms_invalid_start_Date()
        {
            DateTime START_DATE = DateTime.MaxValue;
            DateTime END_DATE = DateTime.MinValue;
            const bool WILL_REQUIRE_HANDICAP = false;
            const bool WILL_REQUIRE_PETS = false;


            IBookingSystem b = new BookingSystem();
            
            try
            {
                var result = b.GetAvailableRooms(START_DATE, END_DATE, WILL_REQUIRE_HANDICAP, WILL_REQUIRE_PETS);
            }
            catch(ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Start date must precede the end date."));
            }
        }

        [TestMethod]
        public void GetAvailableRooms_valid_no_handicap_no_pets()
        {
            DateTime START_DATE = DateTime.MinValue;
            DateTime END_DATE = DateTime.MaxValue;
            const bool WILL_REQUIRE_HANDICAP = false;
            const bool WILL_REQUIRE_PETS = false;

            IBookingSystem b = new BookingSystem();

            var result = b.GetAvailableRooms(START_DATE, END_DATE, WILL_REQUIRE_HANDICAP, WILL_REQUIRE_PETS);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetAvailableRooms_valid_handicap_no_pets()
        {
            DateTime START_DATE = DateTime.MinValue;
            DateTime END_DATE = DateTime.MaxValue;
            const bool WILL_REQUIRE_HANDICAP = true;
            const bool WILL_REQUIRE_PETS = false;

            IBookingSystem b = new BookingSystem();

            var result = b.GetAvailableRooms(START_DATE, END_DATE, WILL_REQUIRE_HANDICAP, WILL_REQUIRE_PETS);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }


        [TestMethod]
        public void GetAvailableRooms_valid_no_handicap_pets()
        {
            DateTime START_DATE = DateTime.MinValue;
            DateTime END_DATE = DateTime.MaxValue;
            const bool WILL_REQUIRE_HANDICAP = false;
            const bool WILL_REQUIRE_PETS = true;

            IBookingSystem b = new BookingSystem();

            var result = b.GetAvailableRooms(START_DATE, END_DATE, WILL_REQUIRE_HANDICAP, WILL_REQUIRE_PETS);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }


        [TestMethod]
        public void GetAvailableRooms_valid_handicap_pets()
        {
            DateTime START_DATE = DateTime.MinValue;
            DateTime END_DATE = DateTime.MaxValue;
            const bool WILL_REQUIRE_HANDICAP = true;
            const bool WILL_REQUIRE_PETS = true;

            IBookingSystem b = new BookingSystem();

            var result = b.GetAvailableRooms(START_DATE, END_DATE, WILL_REQUIRE_HANDICAP, WILL_REQUIRE_PETS);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        #endregion
    }
}
