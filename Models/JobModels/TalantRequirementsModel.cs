# pragma warning disable
using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models
{
    public class TalantRequirementsModel
    {
        public int JobId;
        public Level requiredCandidateLevel;
        public IEnumerable<int> requiredSkillIds;
        public IEnumerable<int> requiredLanguageIds;

    }
}
