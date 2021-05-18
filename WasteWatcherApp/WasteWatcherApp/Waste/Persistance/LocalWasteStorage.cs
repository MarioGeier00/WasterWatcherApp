using System;
using System.Threading.Tasks;
using WasteWatcherApp.Waste;

namespace WasteWatcherApp
{
    public class LocalWasteStorage : IWasteStore
    {

        public Task<WasteCollection> GetData(string productId)
        {
            WasteCollection collection = new(GetWasteValue(productId, WasteType.Plastic),
                                             GetWasteValue(productId, WasteType.Glas),
                                             GetWasteValue(productId, WasteType.Paper),
                                             GetWasteValue(productId, WasteType.Metal));
            return Task.FromResult(collection);
        }


        [Obsolete("Use SaveData(string productId, WasteCollection wasteCollection) instead.")]
        public async Task SaveData(string productId, string plasticWaste, string paperWaste, string glasWaste)
        {
            if (plasticWaste != null)
            {
                SetWasteValue(WasteType.Plastic.WithProductId(productId), plasticWaste);
            }
            if (paperWaste != null)
            {
                SetWasteValue(WasteType.Paper.WithProductId(productId), paperWaste);
            }
            if (glasWaste != null)
            {
                SetWasteValue(WasteType.Glas.WithProductId(productId), glasWaste);
            }

            await App.Current.SavePropertiesAsync();
            return;
        }


        public async Task SaveData(string productId, WasteCollection wasteCollection)
        {
            foreach (var wasteType in WasteTypeHelper.WasteTypesEnumerator)
            {
                RemoveWasteValue(wasteType.WithProductId(productId));
            }

            foreach (var waste in wasteCollection)
            {
                SetWasteValue(waste.WasteType.WithProductId(productId), waste.Amount.ToString());
            }
            await App.Current.SavePropertiesAsync();
            return;
        }



        private void RemoveWasteValue(string wasteKey)
            => App.Current.Properties.Remove(wasteKey);



        private void SetWasteValue(string wasteKey, string value)
                  => SetWasteValue(wasteKey, int.Parse(value));


        private void SetWasteValue(string wasteKey, int value)
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

        private int? GetWasteValue(string wasteKey)
        {
            if (App.Current.Properties.TryGetValue(wasteKey, out object savedObject))
            {
                if (savedObject.GetType() == typeof(int))
                {
                    return (int)savedObject;
                }
            }
            return null;
        }


        private WasteAmount GetWasteValue(string productId, WasteType wasteType)
        {
            var waste = GetWasteValue(wasteType.WithProductId(productId));
            if (waste.HasValue)
            {
                return new(wasteType, waste.Value);
            }
            return null;
        }

    }
}