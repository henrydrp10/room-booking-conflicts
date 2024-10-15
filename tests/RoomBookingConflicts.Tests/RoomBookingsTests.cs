using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RoomBookingConflicts.Tests;

[TestClass]
public class RoomBookingStoreTests {

    [TestMethod]
    public void AddBooking_NewRoom() {
        RoomBookingStore store = new RoomBookingStore();
        RoomBooking booking = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 10, 0, 0), "Organiser 1", "Notes 1");

        Assert.AreEqual(0, store.ReadBookings().Count);
        store.AddBooking(booking);
        Assert.AreEqual(1, store.ReadBookings().Count);
        Assert.IsTrue(store.ReadBookings().ContainsKey(1));
        Assert.AreEqual(1, store.ReadBookings()[1].Count);
        Assert.IsTrue(store.ReadBookings()[1].Contains(booking));
    }

    [TestMethod]
    public void AddBooking_ExistingRoom_DifferentTime() {
        RoomBookingStore store = new RoomBookingStore();
        RoomBooking booking1 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 10, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking booking2 = new RoomBooking(1, new DateTime(2023, 1, 1, 11, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        store.AddBooking(booking1);

        Assert.AreEqual(1, store.ReadBookings().Count);
        Assert.IsTrue(store.ReadBookings().ContainsKey(1));
        Assert.AreEqual(1, store.ReadBookings()[1].Count);
        Assert.IsTrue(store.ReadBookings()[1].Contains(booking1));

        store.AddBooking(booking2);

        Assert.AreEqual(1, store.ReadBookings().Count);
        Assert.IsTrue(store.ReadBookings().ContainsKey(1));
        Assert.AreEqual(2, store.ReadBookings()[1].Count);
        Assert.IsTrue(store.ReadBookings()[1].Contains(booking2));
    }

    [TestMethod]
    public void AddBooking_ExistingRoom_SameTime() {
        RoomBookingStore store = new RoomBookingStore();
        RoomBooking booking1 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking booking2 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 2", "Notes 2");

        store.AddBooking(booking1);

        Assert.AreEqual(1, store.ReadBookings().Count);
        Assert.IsTrue(store.ReadBookings().ContainsKey(1));
        Assert.AreEqual(1, store.ReadBookings()[1].Count);
        Assert.IsTrue(store.ReadBookings()[1].Contains(booking1));

        store.AddBooking(booking2);

        Assert.AreEqual(1, store.ReadBookings().Count);
        Assert.IsTrue(store.ReadBookings().ContainsKey(1));
        Assert.AreEqual(2, store.ReadBookings()[1].Count);
        Assert.IsTrue(store.ReadBookings()[1].Contains(booking2));
    }

    [TestMethod]
    public void ReadBookingsDoesNotModifyOriginalStore() {
        RoomBookingStore store = new RoomBookingStore();
        RoomBooking booking1 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking booking2 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 2", "Notes 2");

        store.AddBooking(booking1);
        Assert.AreEqual(1, store.ReadBookings().Count);
        Assert.AreEqual(1, store.ReadBookings()[1].Count);

        store.ReadBookings().Add(2, new SortedSet<RoomBooking>());
        store.ReadBookings()[1].Add(booking2);

        Assert.AreEqual(1, store.ReadBookings().Count);
        Assert.AreEqual(1, store.ReadBookings()[1].Count);
    }
}

[TestClass]
public class RoomBookingStoreWithConflictDetectionTests {

    [TestMethod]
    public void AddBooking_NoConflict() {

        RoomBookingStoreWithConflictDetection store = new RoomBookingStoreWithConflictDetection();
        RoomBooking booking1 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 10, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking booking2 = new RoomBooking(1, new DateTime(2023, 1, 1, 11, 0, 0), new DateTime(2023, 1, 1, 13, 0, 0), "Organiser 2", "Notes 2");

        using (StringWriter sw = new StringWriter()) {
            Console.SetOut(sw);

            store.AddBooking(booking1);
            store.AddBooking(booking2);

            string actualOutput = sw.ToString().Trim();
            Assert.IsTrue(string.IsNullOrEmpty(actualOutput));
        }
    }

    [TestMethod]
    public void AddLaterBooking_Conflict() {

        RoomBookingStoreWithConflictDetection store = new RoomBookingStoreWithConflictDetection();
        RoomBooking booking1 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking booking2 = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        using (StringWriter sw = new StringWriter()) {
            Console.SetOut(sw);

            store.AddBooking(booking1);
            store.AddBooking(booking2);

            string actualOutput = sw.ToString().Trim();
            string expectedOutput = "Conflict detected\n" +
                "Conflict Booking 1 : " + booking1.ToString() + "\n" +
                "Conflict Booking 2 : " + booking2.ToString();
            Assert.AreEqual(actualOutput, expectedOutput);
        }
    }

    [TestMethod]
    public void AddEarlierBooking_Conflict() {

        RoomBookingStoreWithConflictDetection store = new RoomBookingStoreWithConflictDetection();
        RoomBooking booking1 = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking booking2 = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 2", "Notes 2");

        using (StringWriter sw = new StringWriter()) {
            Console.SetOut(sw);

            store.AddBooking(booking1);
            store.AddBooking(booking2);

            string actualOutput = sw.ToString().Trim();
            string expectedOutput = "Conflict detected\n" +
                "Conflict Booking 1 : " + booking1.ToString() + "\n" +
                "Conflict Booking 2 : " + booking2.ToString();
            Assert.AreEqual(actualOutput, expectedOutput);
        }
    }
}
