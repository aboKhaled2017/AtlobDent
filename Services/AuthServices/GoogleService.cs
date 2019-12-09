using Atlob_Dent.Models.ExternalAuthModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlob_Dent.Services.AuthServices
{
    public class GoogleService
    {
        private readonly HttpClient _httpClient;
        public GoogleService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress=new Uri("https://oauth2.googleapis.com/tokeninfo")
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<GoogleUserModel>GetUserFromGoogleAsync(string googleToken)
        {
            var result = await GetAsync<dynamic>(googleToken);
            if (result == null)
            {
                throw new Exception("user from this token not exists");
            }
            return new GoogleUserModel
            {
                id=result.sub,
                email=result.email,
                name=result.name,
                pictureSrc=result.picture,
                IsEmailConfirmed=result.email_verified
            };
        }
        private async Task<T> GetAsync<T>(string accessToken)
        {
            var response =await _httpClient.GetAsync($"?id_token={accessToken}");
            if (!response.IsSuccessStatusCode)
                return default(T);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
