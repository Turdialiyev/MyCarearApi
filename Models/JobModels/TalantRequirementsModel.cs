# pragma warning disable
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models
{
    public class TalantRequirementsModel
    {
        public int JobId { get; set; }
        public Level RequiredCandidateLevel { get; set; }
        public IEnumerable<int> RequiredSkillIds { get; set; }
        public IEnumerable<int> RequiredLanguageIds { get; set; }

    }
}
