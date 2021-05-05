using System;

namespace WasteWatcherApp
{
    public struct WasteData<T>
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

            if (wasteData.PlasticWaste != 0)
            {
                result += "Plastik: " + wasteData.PlasticWaste + Environment.NewLine;
            }
            if (wasteData.PaperWaste != 0)
            {
                result += "Papier: " + wasteData.PaperWaste + Environment.NewLine;
            }
            if (wasteData.GlasWaste != 0)
            {
                result += "Glas: " + wasteData.GlasWaste + Environment.NewLine;
            }
            if (wasteData.MetalWaste != 0)
            {
                result += "Metall: " + wasteData.MetalWaste + Environment.NewLine;
            }

            return result;
        }
    }
}