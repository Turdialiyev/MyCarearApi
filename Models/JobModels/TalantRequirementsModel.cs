using MyCarearApi.Entities.Enums;

namespace MyCarearApi.Models
{
    public class TalantRequirementsModel
    {
        public int id;
        public Level reuiredCandidateLevel;
        public IEnumerable<int> requiredSkillIds;
        public IEnumerable<int> requiredLanguageIds;
    }
}
