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
    class Firestore
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
        public async void GetProductData(string barcode)
        {
            HttpClient client = new HttpClient();

            // string url = $"https://firestore.googleapis.com/v1/projects/{projectID}/databases/(default)/documents/{collection}/{barcode}";
            string url = $"https://firestore.googleapis.com/v1/projects/test-aabf0/databases/(default)/documents/prod/5411188130765";


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            try
            {


                string res = await client.GetStringAsync(url);

                JObject root = JObject.Parse(res);
                var fields = root.Value<JObject>("fields");


                prod.glas_g = fields["glas_g"]["integerValue"].ToObject<int>();
                prod.plastik_g = fields["plastik_g"]["integerValue"].ToObject<int>();
                prod.papier_g = fields["papier_g"]["integerValue"].ToObject<int>();
                prod.metall_g = fields["metall_g"]["integerValue"].ToObject<int>();
            }
            catch (Exception e)
            {
                MessageService.ShowToastLong($"Fehler {e.ToString()}");

            }
        }


    }
}
