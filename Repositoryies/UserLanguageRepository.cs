using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class UserLanguageRepository : GenericRepository<UserLanguage>, IUserLanguageRepository
{
    public UserLanguageRepository(AppDbContext context) : base(context)
    {
    }
}