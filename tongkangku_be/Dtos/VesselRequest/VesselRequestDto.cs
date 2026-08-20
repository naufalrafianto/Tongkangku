namespace tongkangku_be.Dtos.VesselRequest
{
    public class VesselRequestDto
    {
        public string name { get; set; }
       
        public Guid categoryId { get; set; }
        public Guid portId {get; set; }

        public int capacityFeed {  get; set; }
        public int dwtCapacity {  get; set; }
        public int year {  get; set; }
        public decimal ratePerDay {  get; set; }
        public int status { get; set; }


    }
}
