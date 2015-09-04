namespace HelloMVC.Models
{
    public class PlayersViewModel
    {
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public string TeamName { get; set; }
        public bool IsDelete { get; set; }
    }
}