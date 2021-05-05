using System;

namespace WasteWatcherApp
{
    public struct WasteData
    {
        public WasteData(string plasticWaste, string paperWaste, string glasWaste)
        {
            PlasticWaste = plasticWaste;
            PaperWaste = paperWaste;
            GlasWaste = glasWaste;
        }

        public string PlasticWaste { get; }
        public string PaperWaste { get; }
        public string GlasWaste { get; }

        public override string ToString()
        {
            string result = string.Empty;

            if (PlasticWaste != null)
            {
                result += "Plastik: " +  PlasticWaste + Environment.NewLine;
            }
            if (PaperWaste != null)
            {
                result += "Papier: " +  PaperWaste + Environment.NewLine;
            }
            if (GlasWaste != null)
            {
                result += "Glas: " + GlasWaste + Environment.NewLine;
            }

            return result;
        }
    }
}