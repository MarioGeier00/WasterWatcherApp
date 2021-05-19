using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WasteWatcherApp.Waste
{
    public class WasteCollection : IEnumerable<WasteAmount>
    {

        List<WasteAmount> WasteEnumeration { get; }

        public WasteCollection(params WasteAmount[] waste)
        {
            WasteEnumeration = new(waste);
            WasteEnumeration.RemoveAll(item => item.Amount == 0);
        }


        public int? this[WasteType type]
            => WasteEnumeration.Find(item => item.WasteType == type)?.Amount;


        public EditableWasteCollection Modify()
            => new(WasteEnumeration);


        public override string ToString()
            => WasteEnumeration.Select(i => i.ToString())
                               .Aggregate((i, j) => i + Environment.NewLine + j);


        public IEnumerator<WasteAmount> GetEnumerator() => WasteEnumeration.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



        public static WasteCollection operator +(WasteCollection wasteA, WasteCollection wasteB)
        {
            WasteCollection wasteCollection = new();

            foreach (var wasteType in WasteTypeHelper.WasteTypesEnumerator)
            {
                if (wasteA[wasteType].HasValue || wasteB[wasteType].HasValue)
                {
                    int wasteAmount = (wasteA[wasteType] ?? 0) +
                                      (wasteB[wasteType] ?? 0);

                    wasteCollection
                        .Modify()
                        .SetWasteAmount(wasteType, wasteAmount);
                }
            }

            return wasteCollection;
        }
    }
}
