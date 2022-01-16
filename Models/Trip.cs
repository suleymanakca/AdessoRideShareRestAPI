namespace AdessoRideShareRestAPI.Models
{
    public class Trip
    {
        public int Id { get; set; } 
        public string Nereden { get; set; }
        public string Nereye { get; set; }
        public DateTime Tarih { get; set; }
        public string Aciklama { get; set; }
        public int KoltukSayisi { get; set; }
        public bool Yayinda { get; set; }
        public int UserID { get; set; }

    }
}
