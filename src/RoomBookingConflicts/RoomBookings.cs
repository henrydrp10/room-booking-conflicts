namespace RoomBookingConflicts;

using System;

public class RoomBooking : IComparable<RoomBooking> {

    public Guid Id { get; init; }
    public int RoomNumber { get; init; }
    public DateTime StartDT { get; init; }
    public DateTime EndDT { get; init; }
    public string Organiser { get; init; }
    public string? Notes { get; init; }

    // Constructor for a RoomBooking object.
    public RoomBooking(int roomNumber, DateTime startDT, DateTime endDT, string organiser, string? notes) {
        Id = Guid.NewGuid();
        RoomNumber = roomNumber;
        StartDT = startDT;
        EndDT = endDT;
        Organiser = organiser;
        Notes = notes;
    }

    // ToString
    public override string ToString() {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-GB");
        return $"Booking ID: {Id} - Room {RoomNumber}, from {StartDT.ToString(cultureInfo)} to {EndDT.ToString(cultureInfo)}. Booked by: {Organiser} ({Notes})";
    }

    // Method for comparing two RoomBooking objects in order to sort them.
    public int CompareTo(RoomBooking? other) {
        if (other == null) return 1;
        if (StartDT.CompareTo(other.StartDT) != 0) return StartDT.CompareTo(other.StartDT);
        if (EndDT.CompareTo(other.EndDT) != 0) return EndDT.CompareTo(other.EndDT);
        return Id.CompareTo(other.Id);
    }
}

// Interface for a RoomBookingStore, which forces the implementation of an AddBooking method.
public interface IRoomBookingStore {
    void AddBooking(RoomBooking booking);
}

public class RoomBookingStore : IRoomBookingStore {

    protected Dictionary<int, SortedSet<RoomBooking>> _bookings;

    // Constructor for a RoomBookingStore object.
    public RoomBookingStore() {
        _bookings = new Dictionary<int, SortedSet<RoomBooking>>();
    }

    // This method adds a booking to the RoomBookingStore.
    public virtual void AddBooking(RoomBooking booking) {
        if (!_bookings.ContainsKey(booking.RoomNumber)) {
            _bookings.Add(booking.RoomNumber, new SortedSet<RoomBooking>());
        }
        _bookings[booking.RoomNumber].Add(booking);
    }

    // This method returns a copy of the RoomBookingStore object. The ideal way of
    // approaching this would be to create a ReadOnlySortedSet structure, in order
    // to return a ReadOnlyDictionary<int, ReadOnlySortedSet<RoomBooking>>. However,
    // creating a ReadOnlySortedSet structure for this case would go out of the scope
    // of this exercise, in my opinion, and this solution also works fine, since it's
    // only executed once, when printing all the conflicts (due to the Service not being
    // a part of the RoomBookingStore class). Due to this, using the derived class
    // RoomBookingStoreWithConflictDetection and printing conflicts as they're being
    // added seems like a better choice on that front too. 
    public Dictionary<int, SortedSet<RoomBooking>> ReadBookings() {
        Dictionary<int, SortedSet<RoomBooking>> copy = new Dictionary<int, SortedSet<RoomBooking>>();
        foreach (KeyValuePair<int, SortedSet<RoomBooking>> pair in this._bookings) {
            copy[pair.Key] = new SortedSet<RoomBooking>(pair.Value);
        }
        return copy;
    }
}

public class RoomBookingStoreWithConflictDetection : RoomBookingStore {

    private IConflictDetector _conflictDetector;

    // Constructor for a RoomBookingStoreWithConflictDetection object, which also creates
    // a ConflictDetectorService object.
    public RoomBookingStoreWithConflictDetection() : base() {
        _conflictDetector = new ConflictDetectorService();
    }

    // Overridden AddBooking method, which checks for conflicts before the addition to the store.
    public override void AddBooking(RoomBooking booking) {
        if (_bookings.ContainsKey(booking.RoomNumber)) {
            _conflictDetector.PrintConflictBeforeBeingAdded(_bookings[booking.RoomNumber], booking);
        }
        base.AddBooking(booking);
    }
}