using System.Threading.Tasks;

namespace WasteWatcherApp
{
    public class LocalWasteStorage : IWasteStore
    {
        public async Task<WasteData> GetData(string productId)
        {
            return new WasteData (
                    plasticWaste: GetWasteValue(GetWasteKey(productId, WasteType.Plastic)),
                    glasWaste:  GetWasteValue(GetWasteKey(productId, WasteType.Glas)),
                    paperWaste: GetWasteValue(GetWasteKey(productId, WasteType.Paper))
                );
        }

        public async Task SaveData(string productId, string plasticWaste, string paperWaste, string glasWaste)
        {
            if (plasticWaste != null)
            {
                SetWasteValue(GetWasteKey(productId, WasteType.Plastic), plasticWaste);
            }
            if (paperWaste != null)
            {
                SetWasteValue(GetWasteKey(productId, WasteType.Paper), paperWaste);
            }
            if (glasWaste != null)
            {
                SetWasteValue(GetWasteKey(productId, WasteType.Glas), glasWaste);
            }

            await App.Current.SavePropertiesAsync();
            return;
        }

        private void SetWasteValue(string wasteKey, string value)
        {
            if (App.Current.Properties.ContainsKey(wasteKey))
            {
                App.Current.Properties[wasteKey] = value;
            }
            else
            {
                App.Current.Properties.Add(wasteKey, value);
            }
        }

        private string GetWasteValue(string wasteKey)
        {
            if (App.Current.Properties.TryGetValue(wasteKey, out object savedObject))
            {
                if (savedObject.GetType() == typeof(string))
                {
                    return (string)savedObject;
                }
            }
            return null;
        }


        private string GetWasteKey(string productId, WasteType wasteType)
            => $"{productId}_{wasteType.ToString().ToLower()}";

    }
}