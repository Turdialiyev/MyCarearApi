using MyCarearApi.Entities;
using MyCareerApi.Entities;
using System.Linq.Expressions;

namespace MyCarearApi.Repositoryies.Interfaces
{
    public interface IAddressRepository
    {
        Address? GetById(int id);
        IQueryable<Address> GetAll();
        IEnumerable<Address> Find(Expression<Func<Address, bool>> expression);
        ValueTask<Address> AddAsync(Address entity);
        ValueTask AddRange(IEnumerable<Address> entities);
        ValueTask<Address> Remove(Address entity);
        ValueTask RemoveRange(IEnumerable<Address> entities);
        ValueTask<Address> Update(Address entity);
    }
}
