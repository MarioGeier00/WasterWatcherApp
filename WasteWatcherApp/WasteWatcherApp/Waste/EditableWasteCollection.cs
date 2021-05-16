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


        public EditableWasteCollection SetWasteAmount(WasteType wasteType, int wasteAmount)
        {
            RemoveWasteAmount(wasteType);
            WasteList.Add(new(wasteType, wasteAmount));

            return this;
        }

        public EditableWasteCollection RemoveWasteAmount(WasteType wasteType)
        {
            WasteAmount waste = WasteList.Find((value) => value.WasteType == wasteType);
            WasteList.Remove(waste);

            return this;
        }

        public EditableWasteCollection ClearAllWaste()
        {
            WasteList.Clear();
         
            return this;
        }

    }
}
