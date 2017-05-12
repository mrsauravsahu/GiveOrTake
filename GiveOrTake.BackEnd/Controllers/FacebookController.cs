using GiveOrTake.BackEnd.Helpers;
using GiveOrTake.BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/login/[controller]")]
    public class FacebookController : Controller
    {
        private readonly FacebookOptions facebookOptions;

        public FacebookController(IOptions<FacebookOptions> facebookOptions)
        {
            this.facebookOptions = facebookOptions.Value;
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

                return new ObjectResult(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
