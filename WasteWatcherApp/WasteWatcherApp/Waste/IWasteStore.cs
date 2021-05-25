using System;
using System.Threading.Tasks;

namespace WasteWatcherApp.Waste
{
    public interface IWasteStore
    {
        /// <summary>
        /// Asynchronously saves the given <see cref="WasteCollection"/>.
        /// </summary>
        /// <param name="productId">The barcode string</param>
        /// <param name="waste">The waste amount to store</param>
        /// <returns>An awaitable Task of the store process</returns>
        Task SaveData(string productId, WasteCollection waste);
        
        /// <summary>
        /// Asynchronously get a <see cref="WasteCollection"/> by a given barcode.
        /// </summary>
        /// <param name="productId">The barcode string</param>
        /// <returns>The waste amount task</returns>
        Task<WasteCollection> GetData(string productId);
    }
}