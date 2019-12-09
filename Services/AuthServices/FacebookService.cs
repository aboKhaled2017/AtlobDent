using Atlob_Dent.Models.ExternalAuthModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlob_Dent.Services.AuthServices
{
    public class FacebookService
    {
        private readonly HttpClient _httpClient;
        public FacebookService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress=new Uri("https://graph.facebook.com/v2.8/")
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<FacebookUserModel>GetUserFromFacebookAsync(string facebookToken)
        {
            var result = await GetAsync<dynamic>(facebookToken, "me", "fields=id,first_name,last_name,email,picture.width(100).height(100)");
            if (result == null)
            {
                throw new Exception("user from this token not exists");
            }
            return new FacebookUserModel
            {
                id=result.id,
                email=result.email,
                fname=result.first_name,
                lname=result.last_name,
                pictureSrc=result.picture.data.url
            };
        }
        private async Task<T> GetAsync<T>(string accessToken,string endpoint,string args = null)
        {
            var response =await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
