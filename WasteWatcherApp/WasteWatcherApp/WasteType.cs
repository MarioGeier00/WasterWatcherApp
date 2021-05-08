using System;
using System.Collections.Generic;
using System.Linq;

namespace WasteWatcherApp
{
    public enum WasteType
    {
        Plastic,
        Paper,
        Glas,
        Metal,
    }

    static class WasteTypeHelper
    {

        /// <summary>
        /// Returns the german translation for a defined <see cref="WasteType"/>.
        /// </summary>
        /// <param name="wasteType">The <see cref="WasteType"/> to translate</param>
        /// <returns>The german word</returns>
        /// <exception cref="ArgumentException">Is thrown when the given WasteType has no translation</exception>
        public static string ToFriendlyName(this WasteType wasteType)
            => wasteType switch
            {
                WasteType.Plastic => "Platik",
                WasteType.Paper => "Papier",
                WasteType.Glas => "Glas",
                WasteType.Metal => "Metall",
                _ => throw new ArgumentException()
            };


        /// <summary>
        /// Joins productId and a <see cref="WasteType"/> together.
        /// </summary>
        /// <param name="wasteType">The WasteType</param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static string WithProductId(this WasteType wasteType, string productId)
            => $"{productId}-{wasteType.ToString().ToLower()}";



        public static IEnumerable<WasteType> WasteTypesEnumerator
            => Enum.GetValues(typeof(WasteType)).Cast<WasteType>();

    }
}