using GiveOrTake.FrontEnd.Shared.Data;
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

		public DataStore()
		{
			context = new ApplicationDbContext(App.LocalFolderPath);
			context.Database.EnsureCreated();
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

		public async Task InitializeAsync()
		{
			if (context.Users.Count() != 0) return;
			await CreateUserAsync(null);
		}

		public async Task<User> GetUserDetails()
		{
			await this.InitializeAsync();

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
				var device = (await context.AddAsync(new Database.Device
				{
					UserId = newUser.UserId,
					Name = "This Device",
					DeviceId = Guid.NewGuid()
				})).Entity;
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
			Database.Transaction transaction;

			var user = await context.Users.Include(p => p.Device).FirstOrDefaultAsync();

			item = await context.Items.Where(p => p.Name.ToUpper() == itemName.ToUpper()).FirstOrDefaultAsync();
			if (item is null)
				item = (await context.AddAsync(new Database.Item
				{
					Name = itemName,
					UserId = user.UserId,
					ItemId = Guid.NewGuid()
				})).Entity;

			if (t.TransactionId == new Guid())
			{
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
			}
			else
			{
				transaction = context.Transactions.Find(t.TransactionId);

				transaction.ItemId = item.ItemId;
				transaction.Name = t.Name;
				transaction.Description = t.Description;
				transaction.ExpectedCompletionDate = t.ExpectedCompletionDate;
				transaction.OccurrenceDate = t.OccurrenceDate;
				transaction.TransactionType = t.TransactionType;

			}
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
