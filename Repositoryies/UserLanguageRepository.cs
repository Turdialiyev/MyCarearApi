using MyCarearApi.Data;
using MyCareerApi.Entities;

namespace MyCarearApi.Repositories;

public class UserLanguageRepository : GenericRepository<UserLanguage>, IUserLanguageRepository
{
    public UserLanguageRepository(AppDbContext context) : base(context)
    {
    }
}