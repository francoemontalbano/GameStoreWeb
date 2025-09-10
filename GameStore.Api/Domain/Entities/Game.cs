namespace GameStore.Api.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public bool IsDigital { get; set; }
    }
}
