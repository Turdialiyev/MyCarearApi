# pragma warning disable
namespace MyCarearApi.Entities
{
    public class Chat
    {
        public int Id { get; set; }

        public string Member1 { get; set; }

        public string Member2 { get; set; }

        public DateTime DateTime { get; set; }

        public List<Message> Messages { get; set; }
    }
}
