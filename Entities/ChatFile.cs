# pragma warning disable
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Entities
{
    public class ChatFile
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public MediaType MediType { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}
