using GiveOrTake.FrontEnd.Shared.Models;
using GiveOrTake.FrontEnd.Shared.Data;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;
using GiveOrTake.Database;

[assembly: Dependency(typeof(GiveOrTake.FrontEnd.Shared.Services.DataStore))]
namespace GiveOrTake.FrontEnd.Shared.Services
{
	public class DataStore
	{
		ApplicationDbContext context;
		bool isInitialized;
		List<Models.Item> items;

		public DataStore()
		{
			context = new ApplicationDbContext(App.DatabasePath);
			//context.Database.EnsureCreated();
		}

		~DataStore() { context.Dispose(); }

		public async Task<bool> AddItemAsync(Models.Item item)
		{
			await InitializeAsync();

			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(Models.Item item)
		{
			await InitializeAsync();

			var _item = items.Where((Models.Item arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(_item);
			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(Models.Item item)
		{
			await InitializeAsync();

			var _item = items.Where((Models.Item arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(_item);

			return await Task.FromResult(true);
		}

		public async Task<Models.Item> GetItemAsync(string id)
		{
			await InitializeAsync();

			return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<Models.Item>> GetItemsAsync(bool forceRefresh = false)
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

					items = new List<Models.Item>();
					isInitialized = true;
				}
			});
		}
		public async Task<Database.User> GetUserDetails()
		{
			return await context.Users
				.Include(p => p.Device)
				.Include(p => p.Transaction)
				.Include(p => p.Item)
				.FirstOrDefaultAsync();
		}

		public bool IsLoggedIn()
		{ return (context.Users.Count() != 0); }

		public async Task CreateUserAsync(Database.User user = null)
		{
			Database.User newUser;
			if (user is null)
			{
				newUser = (await context.AddAsync(new Database.User
				{
					UserId = Guid.NewGuid().ToString(),
					FirstName = "",
					MiddleName = "",
					LastName = "",
					Email = ""
				})).Entity;
				await context.AddAsync(new Database.Device
				{
					UserId = newUser.UserId,
					Name = "This Device",
					DeviceId = Guid.NewGuid(),
					//TODO
				});
			}
			else
			{
				var id = user.UserId;
				await context.AddAsync(new Database.User
				{
					FirstName = user.FirstName,
					MiddleName = user.MiddleName,
					LastName = user.LastName,
					Email = user.Email,
					UserId = id
				});

				await context.AddRangeAsync(
					from item in user.Item
					select new Database.Item
					{
						Name = item.Name,
						ItemId = item.ItemId,
						UserId = id,
					});

				await context.AddRangeAsync(
					from device in user.Device
					select new Database.Device
					{
						Name = device.Name,
						DeviceId = device.DeviceId,
						UserId = id
					});

				await context.AddRangeAsync(
					from t in user.Transaction
					select new Database.Transaction
					{
						TransactionId = t.TransactionId,
						Name = t.Name,
						OccurrenceDate = t.OccurrenceDate,
						ExpectedCompletionDate = t.ExpectedCompletionDate,
						CompletionDate = t.CompletionDate,
						Description = t.Description,
						TransactionType = t.TransactionType,
						UserId = id,
						DeviceId = t.DeviceId,
						ItemId = t.ItemId
					});
				await context.AddAsync(new Database.Device
				{
					UserId = user.UserId,
					Name = "This Device",
					DeviceId = Guid.NewGuid()
					//TODO
				});
			}

			await context.SaveChangesAsync();
		}

		public async Task AddTransactionAsync(Transaction t, string itemName)
		{
			Database.Item item;

			var user = await context.Users.Include(p => p.Device).FirstOrDefaultAsync();

			item = await context.Items.Where(p => p.Name.ToUpper() == itemName.ToUpper()).FirstOrDefaultAsync();
			if (item is null)
				item = (await context.AddAsync(new Database.Item
				{
					Name = itemName,
					UserId = user.UserId,
					ItemId = Guid.NewGuid()
				})).Entity;

			await context.AddAsync(new Transaction
			{
				TransactionId = Guid.NewGuid(),
				Name = t.Name,
				Description = t.Description,
				ItemId = item.ItemId,
				UserId = user.UserId,
				OccurrenceDate = t.OccurrenceDate,
				ExpectedCompletionDate = t.ExpectedCompletionDate,
				CompletionDate = null,
				TransactionType = t.TransactionType,
				DeviceId = user.Device.FirstOrDefault().DeviceId
			});

			await context.SaveChangesAsync();
		}

		public async Task<List<Database.Item>> GetItemsAsync()
		{
			return (await context.Users
				.Include(p => p.Item)
				.FirstOrDefaultAsync())?.Item?.ToList();
		}
		public async Task<List<Database.Transaction>> GetTransactionsAsync()
		{
			return (await context.Users
				.Include(p => p.Transaction)
				.FirstOrDefaultAsync())?.Transaction?.ToList();
		}
	}
}
