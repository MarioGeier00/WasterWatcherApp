using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WasteWatcherApp.Waste;

namespace WasteWatcherApp.Firebase
{

    /// <summary>
    /// Class used to connect to and query the Firestore Database
    /// </summary>
    class Firestore : IWasteStore
    {
        public string authToken;
        string apiKey;
        string jsonStr;
        string loginUrl;
        string projectID;
        string collection;


        /// <summary>
        /// Initilizes the Firestore Object and requests an ID Token
        /// </summary>
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

        /// <summary>
        /// Get Firestore ID token
        /// </summary>
        private async Task<String> GetIdToken()
        {
            HttpClient client = new HttpClient();
            StringContent extraData = new StringContent(jsonStr, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(loginUrl, extraData);

            string jsonText = await response.Content.ReadAsStringAsync();
            FirebaseAuthentificationReply reply = JsonConvert.DeserializeObject<FirebaseAuthentificationReply>(jsonText);
            return reply.idToken;
        }


        /// <summary>
        /// Stores the given <see cref="WasteCollection"/> in the Firestore database.
        /// </summary>
        /// <param name="productId">The barcode of the product</param>
        /// <param name="wasteCollection">The waste collection to store</param>
        public async Task SaveData(string productId, WasteCollection wasteCollection)
        {
            HttpClient client = new HttpClient();
            string url = $"https://firestore.googleapis.com/v1/projects/{projectID}/databases/(default)/documents/{collection}/{productId}";

            string jsonData = string.Empty;

            bool addComma = false;
            foreach (var waste in wasteCollection)
            {
                if (addComma)
                    jsonData += ",";
                else
                    addComma = true;

                jsonData += waste.WasteType.ToFriendlyName().ToLower() + "_g:{";
                jsonData += "integerValue: \"" + waste.Amount + "\"";
                jsonData += "}";
            }

            StringContent extraData = new StringContent("{fields:{" + jsonData + "}}", Encoding.UTF8, "application/json");

            await client.PatchAsync(url, extraData);
        }

        /// <summary>
        /// Request data from given Object from Firestore
        /// </summary>
        public async Task<WasteCollection> GetData(string productId)
        {
            HttpClient client = new HttpClient();

            string url = $"https://firestore.googleapis.com/v1/projects/{projectID}/databases/(default)/documents/{collection}/{productId}";
            //string url = $"https://firestore.googleapis.com/v1/projects/test-aabf0/databases/(default)/documents/prod/5411188130765";

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

                return new WasteCollection(new(WasteType.Glas, glas_g),
                                           new(WasteType.Plastic, plastik_g),
                                           new(WasteType.Paper, papier_g),
                                           new(WasteType.Metal, metall_g));

            }
            catch
            {
                return null;
            }
        }

    }
}
