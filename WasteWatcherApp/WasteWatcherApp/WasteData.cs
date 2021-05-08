using System;

namespace WasteWatcherApp
{
    public class WasteData<T>
    {
        public WasteData(T plasticWaste, T paperWaste, T glasWaste, T metalWaste)
        {
            PlasticWaste = plasticWaste;
            PaperWaste = paperWaste;
            GlasWaste = glasWaste;
            MetalWaste = metalWaste;
        }

        public T PlasticWaste { get; }
        public T PaperWaste { get; }
        public T GlasWaste { get; }
        public T MetalWaste { get; }

        public static string ConvertToString( WasteData<int> wasteData)
        {
            string result = string.Empty;

            if(wasteData == null)
            {
                return null;
            }
            if (wasteData.PlasticWaste != 0)
            {
                result += "Plastik: " + wasteData.PlasticWaste + "g" + Environment.NewLine;
            }
            if (wasteData.PaperWaste != 0)
            {
                result += "Papier: " + wasteData.PaperWaste + "g" +  Environment.NewLine;
            }
            if (wasteData.GlasWaste != 0)
            {
                result += "Glas: " + wasteData.GlasWaste + "g" + Environment.NewLine;
            }
            if (wasteData.MetalWaste != 0)
            {
                result += "Metall: " + wasteData.MetalWaste + "g" + Environment.NewLine;
            }

            return result;
        }
    }
}