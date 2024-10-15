namespace RoomBookingConflicts;

using System;

public class CSVReader {

    // This method reads a CSV file line by line and adds the booking requests to the RoomBookingStore object.
    public static void CSVToBookingStore(string filename, RoomBookingStore store) {

        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-GB");

        using (StreamReader reader = new StreamReader(filename)) {
            reader.ReadLine();
            string? line;
            while ((line = reader.ReadLine()) != null) {
                string[] values = line.Split(',');
                store.AddBooking(new RoomBooking(
                    int.Parse(values[0]),
                    DateTime.Parse(values[1], cultureInfo),
                    DateTime.Parse(values[2], cultureInfo),
                    values[3],
                    values.Length > 4 ? values[4] : ""
                ));
            }
        }
    }
}