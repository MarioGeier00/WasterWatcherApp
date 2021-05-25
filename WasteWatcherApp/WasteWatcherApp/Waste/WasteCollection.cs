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


        /// <summary>
        /// Searches the saved waste amount of a given <see cref="WasteType"/>
        /// </summary>
        /// <param name="type">The <see cref="WasteType"/> to search for</param>
        /// <returns>The saved waste amount for the given <see cref="WasteType"/></returns>
        public int? this[WasteType type]
            => WasteEnumeration.Find(item => item.WasteType == type)?.Amount;


        /// <summary>
        /// Creates a new instance of the <see cref="EditableWasteCollection"/> to be able to change the stored waste information
        /// </summary>
        /// <returns>An instance of <see cref="EditableWasteCollection"/> to change the current instances' value</returns>
        public EditableWasteCollection Modify()
            => new(WasteEnumeration);


        /// <summary>
        /// Create a user friendly new line separated list of the <see cref="WasteType"/> and its amount.
        /// </summary>
        /// <returns>A user friendly string of the <see cref="WasteCollection"/></returns>
        public override string ToString()
            => WasteEnumeration.Select(i => i.ToString())
                               .Aggregate((i, j) => i + Environment.NewLine + j);


        /// <summary>
        /// Implements the <see cref="IEnumerable"/> interface to support for-each loop interation
        /// </summary>
        /// <returns>The enumerator for the current collection</returns>
        public IEnumerator<WasteAmount> GetEnumerator() => WasteEnumeration.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        /// <summary>
        /// Adds two instances of <see cref="WasteCollection"/> and returns a third immutable instance.
        /// </summary>
        /// <param name="wasteA">Left operand</param>
        /// <param name="wasteB">Right operand</param>
        /// <returns>The sum of the two given instances of <see cref="WasteCollection"/></returns>
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
