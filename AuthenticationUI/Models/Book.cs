namespace AuthenticationUI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public double Price { get; set; }
        public string Author { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishDate { get; set; }
        public int NbrOfCopy { get; set; }
    }
}
