using GiveOrTake.FrontEnd.Shared.Models;
using GiveOrTake.FrontEnd.Shared.Data;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(GiveOrTake.FrontEnd.Shared.Services.DataStore))]
namespace GiveOrTake.FrontEnd.Shared.Services
{
	public class DataStore
	{
		ApplicationDbContext context;
		bool isInitialized;
		List<Item> items;

		public DataStore()
		{ context = new ApplicationDbContext(App.DatabasePath); }

		~DataStore() { context.Dispose(); }

		public async Task<bool> AddItemAsync(Item item)
		{
			await InitializeAsync();

			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(Item item)
		{
			await InitializeAsync();

			var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(_item);
			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(Item item)
		{
			await InitializeAsync();

			var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(_item);

			return await Task.FromResult(true);
		}

		public async Task<Item> GetItemAsync(string id)
		{
			await InitializeAsync();

			return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
		{
			await InitializeAsync();

			return await Task.FromResult(items);
		}

		public Task<bool> PullLatestAsync()
		{
			return Task.FromResult(true);
		}


		public Task<bool> SyncAsync()
		{
			return Task.FromResult(true);
		}

		public async Task InitializeAsync()
		{
			if (isInitialized) return;

			await Task.Run(() =>
			{
				if (CrossConnectivity.Current.IsConnected == true)
				{
					//Send current device transactions.
					//Send all items.

					//Get all other devices transactions.
					//Get all items.

					items = new List<Item>();
					isInitialized = true;
				}
			});
		}

	}
}
