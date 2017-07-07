using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GiveOrTake.FrontEnd.Shared.Services
{
	class DataRetrieval
	{
		public DataRetrieval()
		{

		}
	}
	public class NetworkCall
	{
		public static NetworkCall Instance = new NetworkCall();

		private HttpClient httpClient;

		private NetworkCall()
		{ httpClient = new HttpClient(); }

		~NetworkCall() { this.httpClient.Dispose(); }

		public void SetBearer(string token)
		{ httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}"); }

		public async Task<HttpResponseMessage> GetAsync(string uri)
		{ return await httpClient.GetAsync(new Uri(uri)); }

		public async Task<HttpResponseMessage> PostAsync(string uri, object data)
		{
			var dataString = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(data, Formatting.None));
			var requestBody = new StringContent(dataString, Encoding.UTF8);
			return await httpClient.PostAsync(new Uri(uri), requestBody);
		}

		public async Task<HttpResponseMessage> PostAsync(string uri)
		{
			return await httpClient.PostAsync(new Uri(uri), new StringContent(string.Empty));
		}
	}
}
