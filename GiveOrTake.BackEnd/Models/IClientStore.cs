using GiveOrTake.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Models
{
    public interface IClientStore<T>
    {
        IList<T> Clients { get; set; }
        Task<T> FindClient(string bearer);
        Task AddClient(T user);
    }
}
