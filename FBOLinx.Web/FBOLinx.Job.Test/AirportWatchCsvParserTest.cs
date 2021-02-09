using FBOLinx.Job.AirportWatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FBOLinx.Job.Test
{
    [TestClass]
    public class AirportWatchCsvParserTest
    {
        [TestMethod]
        public void Parse_All_Csv_File_Lines()
        {
            AirportWatchCsvParser parser = new AirportWatchCsvParser("..\\..\\..\\TestFiles\\AirportWatchCsvParser.csv");
            var records = parser.GetRecords();

            Assert.AreEqual(3, records.Count);
        }

        [TestMethod]
        public void Skip_Previous_Lines_And_Parse()
        {
            AirportWatchCsvParser parser = new AirportWatchCsvParser("..\\..\\..\\TestFiles\\AirportWatchCsvParser.csv");
            var records = parser.GetRecords(2);

            // 1612313054,AC44F2,WCC89,5175,215,359,33.6131,-117.738,-832,4605,kvny_a01,1612313053,A1,5250,0

            Assert.AreEqual(1, records.Count);
            Assert.AreEqual("1612313054", records[0].BoxTransmissionDateTimeUtc);
            Assert.AreEqual("AC44F2", records[0].AircraftHexCode);
        }
    }
}
