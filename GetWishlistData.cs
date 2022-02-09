using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace SteamSalesApp
{

	public class SteamWishlist
	{



	}

	public static class GetWishlistData
	{

		static readonly HttpClient client = new HttpClient();

		[FunctionName("GetWishlistData")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			// https://store.steampowered.com/wishlist/profiles/76561198033429344/wishlistdata/
			string name = req.Query["name"];
			string steamId = req.Query["steamId"];
			steamId = steamId ?? "76561198033429344";

			// Call asynchronous network methods in a try/catch block to handle exceptions.
			try
			{
				string uri = "https://store.steampowered.com/wishlist/profiles/" + steamId + "/wishlistdata/";
				// HttpResponseMessage response = await client.GetAsync("https://store.steampowered.com/wishlist/profiles/" + steamId + "/wishlistdata/");
				// response.EnsureSuccessStatusCode();
				// string responseBody = await response.Content.ReadAsStringAsync();
				// Above three lines can be replaced with new helper method below
				string responseBody = await client.GetStringAsync(uri);
				dynamic json = JsonConvert.DeserializeObject<SteamWishlist>(responseBody);
				return new OkObjectResult(json);
			}
			catch (HttpRequestException e)
			{
				log.LogCritical("\nException Caught!");
				log.LogCritical("Message :{0} ", e.Message);
				return new NotFoundObjectResult("Steam problems");
			}

			// string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			// dynamic data = JsonConvert.DeserializeObject(requestBody);
			// name = name ?? data?.name;

			// string responseMessage = string.IsNullOrEmpty(name)
			// 	? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
			// 	: $"Hello, {name}. This HTTP triggered function executed successfully.";

			// return new OkObjectResult(responseMessage);
		}
	}
}
