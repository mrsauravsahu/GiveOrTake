using GiveOrTake.FrontEnd.Shared.Models;
using GiveOrTake.FrontEnd.Shared.Data;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;

[assembly: Dependency(typeof(GiveOrTake.FrontEnd.Shared.Services.DataStore))]
namespace GiveOrTake.FrontEnd.Shared.Services
{
	public class DataStore
	{
		ApplicationDbContext context;
		bool isInitialized;
		List<Item> items;

		public DataStore()
		{
			context = new ApplicationDbContext(App.DatabasePath);
			context.Database.EnsureCreated();
		}

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
					FirstName = "",
					MiddleName = "",
					LastName = "",
					Email = ""
				})).Entity;
				await context.AddAsync(new Database.Device
				{
					UserId = newUser.UserId,
					Name = "This Device"
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
						UserId = id
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
					Name = "This Device"
					//TODO
				});
			}

			await context.SaveChangesAsync();
		}
	}
}
