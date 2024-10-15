using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RoomBookingConflicts.Tests;

[TestClass]
public class CSVReaderTests {
    
    [TestMethod]
    public void ReadCSV() {
        RoomBookingStore store = new RoomBookingStore();

        int initial_storeCount = store.ReadBookings().Count;

        Assert.AreEqual(0, initial_storeCount);
        CSVReader.CSVToBookingStore("../../../../../RoomBookings.csv", store);
        Assert.IsTrue(store.ReadBookings().Count > initial_storeCount);
    }
}