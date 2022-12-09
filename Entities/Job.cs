using MyCarearApi.Entities.Enums;
using MyCareerApi.Entities;

namespace MyCarearApi.Entities;
public class Job
{
    public int Id { get; set; }

    public  string? Name { get; set; }

    public string? Description { get; set; }

    public string? FilePath { get; set; }

    public decimal Price { get; set; }

    public int CurrencyId { get; set; }
    public Currency? Currency { get; set; }

    public PriceRate PriceRate { get; set; }

    public JobState State { get; set; }

    public int DeadLine { get; set; }

    public DeadlineRate DeadlineRate { get; set; }

    public Level RequiredLevel { get; set; }

    public bool IsSaved { get; set; }

    //Category
    public int PositionsId { get; set; }

    public Position? Position { get; set; }

    public int CompanyId { get; set; }
    public Company? Company { get; set; }

    public IEnumerable<JobSkill>? JobSkills { get; set; }
    public IEnumerable<JobLanguage>? JobLanguages { get; set; }
}


