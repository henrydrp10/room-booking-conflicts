namespace RoomBookingConflicts;

using System;

// Interface for a Booking Conflict Detector, which forces the implementation
// of a logic for detecting conflicts and printing them.
public interface IConflictDetector {
    void PrintConflictBeforeBeingAdded(SortedSet<RoomBooking> room, RoomBooking booking);
    bool ExistsConflict(RoomBooking booking1, RoomBooking booking2);
}

public class ConflictDetectorService : IConflictDetector {

    // This method prints all the conflicts in all rooms of the RoomBookingStore object.
    public void PrintConflictsForAllRooms(RoomBookingStore store) {
        foreach (SortedSet<RoomBooking> room in store.ReadBookings().Values) {
            PrintAllBookingConflictsInARoom(room);
        }
    }

    // This method prints all the conflicts in a specific room.
    public void PrintAllBookingConflictsInARoom(SortedSet<RoomBooking> room) {
        foreach (RoomBooking booking in room) {
            PrintConflictsWithPreviousBookings(room, booking);
        }
    }

    // This method prints all the conflicts between a booking and bookings with a previous start or end time in a room.
    public void PrintConflictsWithPreviousBookings(SortedSet<RoomBooking> room, RoomBooking booking) {
        foreach (RoomBooking existingBooking in room) {
            if (existingBooking.Id == booking.Id) break;
            if (ExistsConflict(existingBooking, booking)) {
                Console.WriteLine("Conflict detected");
                Console.WriteLine($"Conflict Booking 1 : {existingBooking.ToString()}");
                Console.WriteLine($"Conflict Booking 2 : {booking.ToString()}");
            }
        }
    }

    // This method prints all the conflicts between a booking and bookings in a room.
    public void PrintConflictBeforeBeingAdded(SortedSet<RoomBooking> room, RoomBooking booking) {
        foreach (RoomBooking existingBooking in room) {
            if (existingBooking.StartDT > booking.EndDT) break;
            if (ExistsConflict(existingBooking, booking)) {
                Console.WriteLine("Conflict detected");
                Console.WriteLine($"Conflict Booking 1 : {existingBooking.ToString()}");
                Console.WriteLine($"Conflict Booking 2 : {booking.ToString()}");
            }
        }
    }

    // This method checks if there is a conflict between two bookings.
    public bool ExistsConflict(RoomBooking booking1, RoomBooking booking2) {
        if (booking1.RoomNumber != booking2.RoomNumber) return false;
        if (booking1.StartDT > booking2.StartDT) return ExistsConflict(booking2, booking1);
        return booking2.StartDT < booking1.EndDT;
    }
}