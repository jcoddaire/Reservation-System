# Reservation-System

This is a simple coding excercise. It is not perfect - the requirements said to "limit yourself to a few hours". This is the product of 4 hours of work while listening to Bob Dylan on a Sunday afternoon.

## Problem:
The owner of a motel wants to hire you to build a booking system for them. The motel owner
tells you the following information:

- The motel has two levels.
- Because the second level is only accessible via an outdoor staircase, rooms on this level are not handicap accessible.
- Rooms can have 1, 2, or 3 beds, at the following nightly costs:

-- 1 bed - $50 per night
-- 2 beds - $75 per night
-- 3 beds - $90 per night

- The motel allows guests to bring pets, but they charge $20 per pet and limit it to 2 pets
maximum. Also, because some pets are messy, pets are only allowed on the ground
floor so that cleaning is easier to perform if necessary.

## Exercise:
Implement a simple booking system based on these requirements.

## Technical Notes:
You may completely ignore the database for this exercise.


#Known Issues

- Per the requirements, this does not include database assets. Some methods return hard-coded data. It can be assumed that these methods have no logic - they only pull data directly from the DB and send it on.
- I assumed each floor of the motel had 20 rooms.
- I was not sure how you wanted to calculate the pet fee. The requirements did not specify if the 20.00 fee was per night or per reservation. I have coded it to charge 20.00 per reservation. That can be changed if necessary, though.
- This solution was designed with the intent of being consumed by a WebAPI or similar client.
- This solution requires .NET Standard 2.0.