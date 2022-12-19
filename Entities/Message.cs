namespace MyCarearApi.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string FromId { get; set; }

        public string ToId { get; set; }

        public DateTime DateTime { get; set; }

        public string Text { get; set; }

        public bool FileMessage { get; set; }

        public string? FileName { get; set; }

        public bool IsRead { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
