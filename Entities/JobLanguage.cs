namespace MyCarearApi.Entities
{
    public class JobLanguage
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
