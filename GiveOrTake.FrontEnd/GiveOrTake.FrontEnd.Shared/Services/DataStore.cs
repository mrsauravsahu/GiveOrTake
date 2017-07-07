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
using GiveOrTake.FrontEnd.Shared.Helpers;

[assembly: Dependency(typeof(GiveOrTake.FrontEnd.Shared.Services.DataStore))]
namespace GiveOrTake.FrontEnd.Shared.Services
{
	public class DataStore
	{
		ApplicationDbContext context;
		bool isInitialized;
		List<Models.Item> items;

		internal async Task<Database.Item> GetAssociatedItemAsync(Transaction t)
		{
			return await context.Items.Where(p => p.ItemId == t.ItemId).FirstAsync();
		}

		public DataStore()
		{
			context = new ApplicationDbContext(App.DatabasePath);
			context.Database.EnsureCreated();
		}

		public Task<IEnumerable<Guid>> GetTransactionsIdsAsync()
		{
			throw new NotImplementedException();
		}

		~DataStore() { context.Dispose(); }

		internal async Task DeleteTransactionAsync(Guid transactionId)
		{
			var transaction = await context.Transactions
				.Where(p => p.TransactionId.ToString() == transactionId.ToString())
				.FirstOrDefaultAsync();
			if (!(transaction is null))
			{
				context.Transactions.Remove(transaction);
				await context.SaveChangesAsync();
			}
		}

		public async Task<bool> AddItemAsync(Models.Item item)
		{
			await InitializeAsync();

			items.Add(item);

			return await Task.FromResult(true);
		}

		internal Task ResolveConflictsAndSaveTransactionsAsync(Task<List<Transaction>> serverData)
		{
			return Task.CompletedTask;
		}

		internal async Task SetTransactionCompleteAsync(Guid transactionId)
		{
			var id = transactionId.ToString();
			var transaction = await context.Transactions
				.Where(p => p.TransactionId.ToString() == id)
				.FirstOrDefaultAsync();

			if (!(transaction is null))
			{
				transaction.CompletionDate = DateTime.Now;
				await context.SaveChangesAsync();
			}
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
			var usersCount = await context.Users.CountAsync();

			if (usersCount == 0) await this.CreateUserAsync();
		}

		public async Task<Database.User> GetUserDetails()
		{
			await InitializeAsync();

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
				});
			}
			await context.SaveChangesAsync();

			var thisDeviceId = context.Devices.First()?.DeviceId.ToString();
			Settings.DeviceId = thisDeviceId;
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
			return await Task.Run(() => context.Items.ToList());
		}

		public async Task<List<Database.Transaction>> GetTransactionsAsync()
		{
			return await Task.Run(() => context.Transactions.ToList());
		}

		public async Task<dynamic> GetDataToSynchronizeForThisDeviceIdAsync(Guid deviceId)
		{
			return await Task.Run(() =>
			{
				var transactions = context.Transactions
				.Where(t => t.DeviceId == deviceId)
				.ToList();

				var items = context.Items.ToList();

				return new
				{
					Transaction = transactions,
					Item = items
				};
			});
		}
	}
}