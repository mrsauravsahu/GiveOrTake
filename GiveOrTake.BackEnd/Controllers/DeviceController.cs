using GiveOrTake.BackEnd.Data;
using GiveOrTake.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Controllers
{
	[Route("api/[controller]")]
	public class DeviceController : Controller
	{
		private readonly GiveOrTakeContext dbContext;

		public DeviceController(GiveOrTakeContext dbContext)
		{ this.dbContext = dbContext; }

		[Route("{deviceId}")]
		[AllowAnonymous]
		public async Task<IActionResult> SynchronizeDataAsync([FromBody] User user, string deviceId)
		{
			var userid = new JwtSecurityTokenHandler().ReadJwtToken(Request.Headers["Authorization"].First().Split(' ')[1]).Id;

			var device = dbContext.Devices.Find(new Guid(deviceId));

			if (device is null)
			{
				device = (await dbContext.Devices.AddAsync(new Device
				{
					DeviceId = new Guid(deviceId),
					Name = string.Empty,
					UserId = userid
				})).Entity;
			}

			device.Transaction.Clear();

			user.Item.ToList().ForEach(async (i) =>
			{
				var item = dbContext.Items.Find(i.ItemId);
				if (item is null) await dbContext.Items.AddAsync(i);
			});

			user.Transaction.ToList().ForEach(async (t) =>
			{
				var transaction = dbContext.Transactions.Find(t.TransactionId);
				if (transaction is null) await dbContext.Transactions.AddAsync(t);
			});

			return new OkObjectResult(new
			{
				Message = "Saved data succuessfully"
			});
		}
	}
}
