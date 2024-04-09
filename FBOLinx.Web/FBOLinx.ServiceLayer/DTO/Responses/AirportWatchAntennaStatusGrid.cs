namespace FBOLinx.ServiceLayer.Dto.Responses
{
    public class AirportWatchAntennaStatusGrid
    {
        public string BoxName { get; set; }
        public string Status { get; set; }
        public string LastUpdateRaw { get; set; }
        public string LastUpdateCurated { get; set; }
        public string FbolinxAccount { get; set; }
    }
}

