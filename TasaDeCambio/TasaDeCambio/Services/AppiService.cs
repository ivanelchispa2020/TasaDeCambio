using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TasaDeCambio.Models;

namespace TasaDeCambio.Services
{
    public class AppiService
    {
        public async Task<Response> GetList<T>(string UrlBase, string Controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(UrlBase);
                var controller = Controller;
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response { IsSuccess = false, Message = result };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response { IsSuccess = true, Result = list };
            }
            catch (Exception ex)
            {
                return new Response { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Revisa la configuracion del internet",
                };
            }

            //var reachable = await CrossConnectivity.Current.IsReachable(
            //	"google.com");
            //if (!reachable)
            //{
            //	return new Response
            //	{
            //		IsSuccess = false,
            //		Message = "Check your internet connection.",
            //	};
            //}

            return new Response
            {
                IsSuccess = true,
            };
        }




    }
}
