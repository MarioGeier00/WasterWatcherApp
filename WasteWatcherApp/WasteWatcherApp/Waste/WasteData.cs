using System;
using System.Collections.Generic;

namespace WasteWatcherApp.Waste
{
    public class WasteData<T>
    {

        Dictionary<WasteType, T> Waste { get; }


        public WasteData()
            => Waste = new Dictionary<WasteType, T>();


        public WasteData(params KeyValuePair<WasteType, T>[] waste)
            => Waste = new Dictionary<WasteType, T>(waste);



        public void SetWasteAmount(WasteType wasteType, T wasteAmount)
        {
            if (Waste.ContainsKey(wasteType))
            {
                Waste[wasteType] = wasteAmount;
            }
            else
            {
                Waste.Add(wasteType, wasteAmount);
            }
        }



        IEnumerator<string> ToStringList()
        {
            foreach (var waste in Waste)
            {
                yield return $"{waste.Key.ToFriendlyName()}: {waste.Value}{Environment.NewLine}";
            }
        }


        public override string ToString()
            => string.Join(Environment.NewLine, ToStringList());

    }

    static class WasteDataExtension
    {
        public static string WithProductId(this KeyValuePair<WasteType, int> wasteData, string productId)
            => $"{productId}-{wasteData.Key.ToString().ToLower()}";

    }


    // Before Refactoring the class WasteData 
    // looked like this:

    // public struct WasteData<T>
    // {
    //     public WasteData(T plasticWaste, T paperWaste, T glasWaste, T metalWaste)
    //     {
    //         PlasticWaste = plasticWaste;
    //         PaperWaste = paperWaste;
    //         GlasWaste = glasWaste;
    //         MetalWaste = metalWaste;
    //     }

    //     public T PlasticWaste { get; }
    //     public T PaperWaste { get; }
    //     public T GlasWaste { get; }
    //     public T MetalWaste { get; }

    //     public static string ConvertToString(WasteData<int> wasteData)
    //     {
    //         string result = string.Empty;

    //         if(wasteData == null)
    //         {
    //             return null;
    //         }

    //         if (wasteData.PlasticWaste != 0)
    //         {
    //             result += "Plastik: " + wasteData.PlasticWaste + Environment.NewLine;
    //         }
    //         if (wasteData.PaperWaste != 0)
    //         {
    //             result += "Papier: " + wasteData.PaperWaste + Environment.NewLine;
    //         }
    //         if (wasteData.GlasWaste != 0)
    //         {
    //             result += "Glas: " + wasteData.GlasWaste + Environment.NewLine;
    //         }
    //         if (wasteData.MetalWaste != 0)
    //         {
    //             result += "Metall: " + wasteData.MetalWaste + Environment.NewLine;
    //         }

    //         return result;
    //     }
    // }

}