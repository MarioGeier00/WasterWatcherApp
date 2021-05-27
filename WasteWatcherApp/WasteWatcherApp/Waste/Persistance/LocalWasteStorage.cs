using System;
using System.Threading.Tasks;
using WasteWatcherApp.Waste;

namespace WasteWatcherApp
{
    public class LocalWasteStorage : IWasteStore
    {

        /// <summary>
        /// Load a <see cref="WasteCollection"/> by a given barcode from the local storage.
        /// </summary>
        /// <param name="productId">The barcode string</param>
        /// <returns>The waste amount task</returns>
        public Task<WasteCollection> GetData(string productId)
        {
            WasteCollection wasteCollection = new();
            foreach (var wasteType in WasteTypeHelper.WasteTypesEnumerator)
            {
                WasteAmount wasteAmount = GetWasteValue(productId, wasteType);
                if (wasteAmount is not null)
                {
                    wasteCollection
                        .Modify()
                        .SetWasteAmount(wasteType, wasteAmount.Amount);
                }
            }

            return Task.FromResult(wasteCollection);
        }


        /// <summary>
        /// Asynchronously saves the given <see cref="WasteCollection"/> in the local storage.
        /// </summary>
        /// <param name="productId">The barcode string</param>
        /// <param name="waste">The waste amount to store</param>
        /// <returns>An awaitable Task of the store process</returns>
        public async Task SaveData(string productId, WasteCollection wasteCollection)
        {
            foreach (var wasteType in WasteTypeHelper.WasteTypesEnumerator)
            {
                RemoveWasteValue(wasteType.WithProductId(productId));
            }

            foreach (var waste in wasteCollection)
            {
                SetWasteValue(waste.WasteType.WithProductId(productId), waste.Amount);
            }
            await App.Current.SavePropertiesAsync();
            return;
        }


        /// <summary>
        /// Removes a saved waste entry for a given key.
        /// </summary>
        /// <param name="wasteKey">The key of the stored data to delete</param>
        void RemoveWasteValue(string wasteKey)
          => App.Current.Properties.Remove(wasteKey);


        /// <summary>
        /// Adds or updates the waste value for a given key.
        /// </summary>
        /// <param name="wasteKey">The name for the stored value</param>
        /// <param name="value">The waste amount</param>
        void SetWasteValue(string wasteKey, int value)
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

        /// <summary>
        /// Loads the waste amount for a given waste key.
        /// Returns null when no data has been stored for the given waste key.
        /// </summary>
        /// <param name="wasteKey">The name for the stored value</param>
        /// <returns>A nullable integer that represents the waste amount</returns>
        int? GetWasteValue(string wasteKey)
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

        /// <summary>
        /// Loads the waste amount for a given barcode and waste type.
        /// </summary>
        /// <param name="productId">The barcode of the product</param>
        /// <param name="wasteType">The waste type</param>
        /// <returns>An instance of WasteAmount or null</returns>
        WasteAmount GetWasteValue(string productId, WasteType wasteType)
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