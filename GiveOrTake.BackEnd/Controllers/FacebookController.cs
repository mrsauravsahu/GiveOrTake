using GiveOrTake.BackEnd.Helpers;
using GiveOrTake.BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GiveOrTake.BackEnd.Data;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/login/[controller]")]
    public class FacebookController : Controller
    {
        private readonly FacebookOptions facebookOptions;
        private readonly LoginHelper loginHelper;
        private readonly GiveOrTakeContext dbContext;

        public FacebookController(GiveOrTakeContext dbContext,
            IOptions<FacebookOptions> facebookOptions,
            LoginHelper loginHelper)
        {
            this.facebookOptions = facebookOptions.Value;
            this.loginHelper = loginHelper;
            this.dbContext = dbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ObjectResult> Post([FromBody] Token auth)
        {
            using (var client = new HttpClient())
            {
                var url = String.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials",
                    facebookOptions.AppId,
                    facebookOptions.AppSecret);

                var accessToken = await client.GetAsync(url);

                var responseData = await accessToken.Content.ReadAsStringAsync();
                var data = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<Dictionary<string, string>>(responseData));

                url = String.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}",
                auth.AccessToken,
                data["access_token"]);

                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                //TODO: Check token authenticity.

                var newUser = await loginHelper.FacebookLogin(auth.AccessToken);
                var isRegistering = (from u in dbContext.Users
                                     where u.UserId == newUser.UserId
                                     select u).FirstOrDefault() == null;

                //Register
                if (isRegistering)
                {
                    await dbContext.Users.AddAsync(newUser);
                    await dbContext.SaveChangesAsync();
                }

                return new ObjectResult(await loginHelper.GenerateAuthToken(newUser));
            }
        }
    }
}