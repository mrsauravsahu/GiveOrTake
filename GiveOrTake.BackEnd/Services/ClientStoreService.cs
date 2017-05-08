using GiveOrTake.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Services
{
    public class ClientStoreService : IClientStore<Client>
    {
        private List<Client> _clients = new List<Client>();
        public IList<Client> Clients
        {
            get { return _clients; }
            set { }
        }

        public ClientStoreService() { Clients = new List<Client>(); }

        public async Task AddClient(Client client)
        {
            await Task.Run(() =>
            {
                this.Clients.Add(client);
            });
        }

        public Task<Client> FindClient(string bearer)
        {
            try
            {
                var jwtToken = bearer.Split(' ')[1];
                var client = from u in Clients
                             where u.JwtToken == jwtToken
                             select u;
                return Task.FromResult(Clients.FirstOrDefault());
            }
            catch (Exception e)
            {
                //TODO: Catch when user is not found.
                throw e;
            }
        }
    }
}
