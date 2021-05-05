using System.Threading.Tasks;

namespace WasteWatcherApp
{
    public class LocalWasteStorage : IWasteStore
    {
        public async Task<WasteData<int>> GetData(string productId)
        {
            return new WasteData<int> (
                    plasticWaste: GetWasteValue(GetWasteKey(productId, WasteType.Plastic)),
                    glasWaste:  GetWasteValue(GetWasteKey(productId, WasteType.Glas)),
                    paperWaste: GetWasteValue(GetWasteKey(productId, WasteType.Paper)),
                    metalWaste: GetWasteValue(GetWasteKey(productId, WasteType.Metal))

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
                App.Current.Properties[wasteKey] = int.Parse(value);
            }
            else
            {
                App.Current.Properties.Add(wasteKey, int.Parse(value));
            }
        }

        private int GetWasteValue(string wasteKey)
        {
            if (App.Current.Properties.TryGetValue(wasteKey, out object savedObject))
            {
                if (savedObject.GetType() == typeof(int))
                {
                    return (int)savedObject;
                }
            }
            return 0;
        }


        private string GetWasteKey(string productId, WasteType wasteType)
            => $"{productId}_{wasteType.ToString().ToLower()}";

    }
}