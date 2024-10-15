namespace RoomBookingConflicts;

using System;
using System.IO;

class Program {

    // This method creates a list of 50 RoomBookings. 

    // static List<RoomBooking> GenerateBookingsNextWeek(int count) {
    //     List<RoomBooking> bookings = new List<RoomBooking>();
    //     Random random = new Random();
    //     DateTime possibleDatesStart = DateTime.Today.AddDays(1).AddHours(9);
    //     for (int i = 0; i < count; i++) {

    //         DateTime startDT = possibleDatesStart.AddDays(random.Next(0, 6)).AddHours(random.Next(0, 6));
    //         DateTime endDT = startDT.AddHours(random.Next(1, 4));

    //         bookings.Add(new RoomBooking(
    //             random.Next(1, 10),
    //             startDT,
    //             endDT,
    //             $"Organiser {i}",
    //             $"Notes {i}"
    //         ));
    //     }
    //     return bookings;
    // }

    static void Main(string[] args) {

        // This part of the code is in charge of generating a CSV with the 50 booking requests generated.

        // List<RoomBooking> bookings = GenerateBookingsNextWeek(50);
        // System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-GB");
        // using (StreamWriter writer = new StreamWriter("../../RoomBookings.csv")) {
        //     writer.WriteLine("RoomNumber,StartDT,EndDT,Organiser,Notes");
        //     foreach (var booking in bookings)
        //     {
        //         writer.WriteLine(
        //             $"{booking.RoomNumber}," +
        //             $"{booking.StartDT.ToString(cultureInfo)}," +
        //             $"{booking.EndDT.ToString(cultureInfo)}," +
        //             $"{booking.Organiser}," +
        //             $"{booking.Notes}"
        //         );
        //     }
        // }
        // Console.WriteLine("CSV file created successfully: RoomBookings.csv");

        // In this part of the code, I decided to first add all the bookings to the RoomBookingStore,
        // and then check for conflicts by checking every single room.

        RoomBookingStore store = new RoomBookingStore();
        CSVReader.CSVToBookingStore("../../RoomBookings.csv", store);
        ConflictDetectorService conflictDetector = new ConflictDetectorService();
        conflictDetector.PrintConflictsForAllRooms(store);

        Console.WriteLine("");
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("");

        // I decided to create a class which would extend the RoomBookingStore,
        // and add the functionality to check for conflicts every time a new RoomBooking is added. In order
        // to do this, I created the IConflictDetector interface, which the derived RoomBookingStoreWithConflictDetection
        // class would implement, in order to add the functionality.

        RoomBookingStoreWithConflictDetection storeWithConflictDetection = new RoomBookingStoreWithConflictDetection();
        CSVReader.CSVToBookingStore("../../RoomBookings.csv", storeWithConflictDetection);
    }
}
