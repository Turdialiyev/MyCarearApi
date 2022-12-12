namespace MyCarearApi.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string FromId { get; set; }

        public string ToId { get; set; }

        public string Text { get; set; }
    }
}
