using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WasteWatcherApp.helper
{
    class Firestore : IWasteStore
    {
        string apiKey;
        string jsonStr;
        string loginUrl;
        public string authToken;
        string projectID;
        string collection;
        public class FirebaseAuthentificationReply
        {
            public string idToken { get; set; }
            string email { get; set; }
            string refreshToken { get; set; }
            int expiresIn { get; set; }
            string localId { get; set; }
        }

        public Firestore()
        {
            apiKey = "AIzaSyCrPIipyWAi2YSZC34HZRp2NQ2MU10APds";
            jsonStr = "{\"returnSecureToken\":true}";
            loginUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}";
            projectID = "test-aabf0";
            collection = "prod";
            Initialize();
        }
        public async void Initialize()
        {
            authToken = await GetIdToken();

        }
        private async Task<String> GetIdToken()
        {
            HttpClient client = new HttpClient();
            StringContent extraData = new StringContent(jsonStr, Encoding.UTF8, "application/json");


            var response = await client.PostAsync(loginUrl, extraData);


            string jsonText = await response.Content.ReadAsStringAsync();
            FirebaseAuthentificationReply reply = JsonConvert.DeserializeObject<FirebaseAuthentificationReply>(jsonText);
            await Task.Delay(1000);
            return reply.idToken;

        }


        public async Task SaveData(string productId, string plasticWaste, string paperWaste, string glasWaste)
        {
            HttpClient client = new HttpClient();

            string url = $"https://firestore.googleapis.com/v1/projects/{projectID}/databases/(default)/documents/{collection}/{productId}";
            string json_data = "{ \"fields\": {";
            if(!string.IsNullOrEmpty(plasticWaste) )
                json_data +=  "\"plastik_g\":" + "{ \"integerValue\": \"" +  plasticWaste +"\"},";
            if (!string.IsNullOrEmpty(paperWaste))
                json_data += "\"papier_g\":" + "{ \"integerValue\": \"" + paperWaste + "\" },";
            if (!string.IsNullOrEmpty(glasWaste))
                json_data += "\"glas_g\":" + "{ \"integerValue\": \"" + plasticWaste + "\" },";
            if (json_data.EndsWith(","))
                json_data = json_data.Remove(json_data.Length - 1);

            json_data += "} }";


            StringContent extraData = new StringContent(json_data, Encoding.UTF8, "application/json");

            await client.PatchAsync(url, extraData);
            
        }

        public async Task<WasteData<int>> GetData(string productId)
        {
            HttpClient client = new HttpClient();

            string url = $"https://firestore.googleapis.com/v1/projects/{projectID}/databases/(default)/documents/{collection}/{productId}";
            //string url = $"https://firestore.googleapis.com/v1/projects/test-aabf0/databases/(default)/documents/prod/5411188130765";

            WasteData<int> wasteData = null;
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            try
            {


                string res = await client.GetStringAsync(url);

                JObject root = JObject.Parse(res);
                var fields = root.Value<JObject>("fields");

                int glas_g = fields["glas_g"]?["integerValue"]?.ToObject<int>() ?? 0;
                int plastik_g = fields["plastik_g"]?["integerValue"]?.ToObject<int>() ?? 0;
                int papier_g = fields["papier_g"]?["integerValue"]?.ToObject<int>() ?? 0;
                int metall_g = fields["metall_g"]?["integerValue"]?.ToObject<int>() ?? 0;

                 wasteData = new WasteData<int>(plastik_g, papier_g, glas_g, metall_g);
                
            }
            catch (Exception e)
            {
            }
            return wasteData;
        }
    }
}
