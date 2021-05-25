using System.Collections.Generic;

namespace WasteWatcherApp.Waste
{
    public class EditableWasteCollection
    {

        List<WasteAmount> WasteList { get; }

        public EditableWasteCollection(List<WasteAmount> wasteList)
        {
            WasteList = wasteList;
        }

        /// <summary>
        /// Adds the waste amount to the collection or updates the waste amount value.
        /// </summary>
        /// <param name="wasteType">The waste type to add or change the value of</param>
        /// <param name="wasteAmount">The amount of waste for the given waste type</param>
        /// <returns>The current instance for usage as fluent interface</returns>
        public EditableWasteCollection SetWasteAmount(WasteType wasteType, int wasteAmount)
        {
            RemoveWasteAmount(wasteType);
            WasteList.Add(new(wasteType, wasteAmount));

            return this;
        }

        /// <summary>
        /// Removes a WasteAmount entry in the current list.
        /// </summary>
        /// <param name="wasteType">The waste type to remove</param>
        /// <returns>The current instance for usage as fluent interface</returns>
        public EditableWasteCollection RemoveWasteAmount(WasteType wasteType)
        {
            WasteAmount waste = WasteList.Find((value) => value.WasteType == wasteType);
            WasteList.Remove(waste);

            return this;
        }

        /// <summary>
        /// Clears the 
        /// </summary>
        /// <returns></returns>
        public EditableWasteCollection ClearAllWaste()
        {
            WasteList.Clear();

            return this;
        }

    }
}
