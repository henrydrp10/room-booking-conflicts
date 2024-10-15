using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RoomBookingConflicts.Tests;

[TestClass]
public class ConflictDetectorServiceTests {

    [TestMethod]
    public void NoConflict_DifferentRooms() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 10, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(2, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 10, 0, 0), "Organiser 2", "Notes 2");

        ConflictDetectorService cds = new ConflictDetectorService();

        Assert.IsFalse(cds.ExistsConflict(bookingA, bookingB));
        Assert.IsFalse(cds.ExistsConflict(bookingB, bookingA));
    }

    [TestMethod]
    public void NoConflict_ABeforeB() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 10, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 11, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        ConflictDetectorService cds = new ConflictDetectorService();

        Assert.IsFalse(cds.ExistsConflict(bookingA, bookingB));
        Assert.IsFalse(cds.ExistsConflict(bookingB, bookingA));
    }

    [TestMethod]
    public void NoConflict_EndAEqualsStartB() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 10, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        ConflictDetectorService cds = new ConflictDetectorService();

        Assert.IsFalse(cds.ExistsConflict(bookingA, bookingB));
        Assert.IsFalse(cds.ExistsConflict(bookingB, bookingA));
    }

    [TestMethod]
    public void Conflict_EndAAfterStartB() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        ConflictDetectorService cds = new ConflictDetectorService();

        Assert.IsTrue(cds.ExistsConflict(bookingA, bookingB));
        Assert.IsTrue(cds.ExistsConflict(bookingB, bookingA));
    }

    [TestMethod]
    public void Conflict_EndAEqualsEndB() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        ConflictDetectorService cds = new ConflictDetectorService();

        Assert.IsTrue(cds.ExistsConflict(bookingA, bookingB));
        Assert.IsTrue(cds.ExistsConflict(bookingB, bookingA));
    }

    [TestMethod]
    public void Conflict_EndAAfterEndB() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 13, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        ConflictDetectorService cds = new ConflictDetectorService();

        Assert.IsTrue(cds.ExistsConflict(bookingA, bookingB));
        Assert.IsTrue(cds.ExistsConflict(bookingB, bookingA));
    }

    [TestMethod]
    public void Conflict_StartAEqualsStartB() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 13, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");

        ConflictDetectorService cds = new ConflictDetectorService();

        Assert.IsTrue(cds.ExistsConflict(bookingA, bookingB));
        Assert.IsTrue(cds.ExistsConflict(bookingB, bookingA));
    }

    [TestMethod]
    public void PrintAllConflicts() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");
        RoomBooking bookingC = new RoomBooking(1, new DateTime(2023, 1, 1, 8, 0, 0), new DateTime(2023, 1, 1, 17, 0, 0), "Organiser 3", "Notes 3");
        RoomBooking bookingD = new RoomBooking(1, new DateTime(2023, 1, 1, 13, 0, 0), new DateTime(2023, 1, 1, 15, 0, 0), "Organiser 4", "Notes 4");
        RoomBooking bookingE = new RoomBooking(1, new DateTime(2023, 1, 1, 14, 0, 0), new DateTime(2023, 1, 1, 16, 0, 0), "Organiser 5", "Notes 5");

        RoomBookingStore normalStore = new RoomBookingStore();
        ConflictDetectorService cds = new ConflictDetectorService();
        
        using (StringWriter sw = new StringWriter()) {
            Console.SetOut(sw);

            normalStore.AddBooking(bookingA);
            normalStore.AddBooking(bookingB);
            normalStore.AddBooking(bookingC);
            normalStore.AddBooking(bookingD);
            normalStore.AddBooking(bookingE);

            cds.PrintConflictsForAllRooms(normalStore);

            string actualOutput = sw.ToString().Trim();

            List<ValueTuple<RoomBooking, RoomBooking>> expectedConflicts = new List<ValueTuple<RoomBooking, RoomBooking>>();
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingC, bookingA));    
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingC, bookingB));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingA, bookingB));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingC, bookingD));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingC, bookingE));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingD, bookingE));

            string conflictOutput = "";

            foreach (ValueTuple<RoomBooking, RoomBooking> conflict in expectedConflicts) {
                conflictOutput += "Conflict detected\n" +
                    "Conflict Booking 1 : " + conflict.Item1.ToString() + "\n" +
                    "Conflict Booking 2 : " + conflict.Item2.ToString() + "\n";
            }

            Assert.AreEqual(actualOutput, conflictOutput.Trim());
        }
    }

    [TestMethod]
    public void PrintConflictsAsBeingAdded() {
        RoomBooking bookingA = new RoomBooking(1, new DateTime(2023, 1, 1, 9, 0, 0), new DateTime(2023, 1, 1, 11, 0, 0), "Organiser 1", "Notes 1");
        RoomBooking bookingB = new RoomBooking(1, new DateTime(2023, 1, 1, 10, 0, 0), new DateTime(2023, 1, 1, 12, 0, 0), "Organiser 2", "Notes 2");
        RoomBooking bookingC = new RoomBooking(1, new DateTime(2023, 1, 1, 8, 0, 0), new DateTime(2023, 1, 1, 17, 0, 0), "Organiser 3", "Notes 3");
        RoomBooking bookingD = new RoomBooking(1, new DateTime(2023, 1, 1, 13, 0, 0), new DateTime(2023, 1, 1, 15, 0, 0), "Organiser 4", "Notes 4");
        RoomBooking bookingE = new RoomBooking(1, new DateTime(2023, 1, 1, 14, 0, 0), new DateTime(2023, 1, 1, 16, 0, 0), "Organiser 5", "Notes 5");

        RoomBookingStoreWithConflictDetection conflictStore = new RoomBookingStoreWithConflictDetection();

        using (StringWriter sw = new StringWriter()) {
            Console.SetOut(sw);

            conflictStore.AddBooking(bookingA);
            conflictStore.AddBooking(bookingB);
            conflictStore.AddBooking(bookingD);
            conflictStore.AddBooking(bookingE);
            conflictStore.AddBooking(bookingC);

            string actualOutput = sw.ToString().Trim();

            List<ValueTuple<RoomBooking, RoomBooking>> expectedConflicts = new List<ValueTuple<RoomBooking, RoomBooking>>();
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingA, bookingB));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingD, bookingE));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingA, bookingC));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingB, bookingC));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingD, bookingC));
            expectedConflicts.Add(new ValueTuple<RoomBooking, RoomBooking>(bookingE, bookingC));

            string conflictOutput = "";

            foreach (ValueTuple<RoomBooking, RoomBooking> conflict in expectedConflicts) {
                conflictOutput += "Conflict detected\n" +
                    "Conflict Booking 1 : " + conflict.Item1.ToString() + "\n" +
                    "Conflict Booking 2 : " + conflict.Item2.ToString() + "\n";
            }

            Assert.AreEqual(actualOutput, conflictOutput.Trim());
        } 
    }
}