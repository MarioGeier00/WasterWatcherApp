namespace WasteWatcherApp.Waste
{
    public record WasteAmount(WasteType WasteType, int Amount)
    {
        /// <summary>
        /// Creates a user friendly string of the current WasteAmount
        /// </summary>
        /// <returns>A string in the format [WasteTypeString]: [WasteAmount]g</returns>
        public override string ToString() => $"{WasteType.ToFriendlyName()}: {Amount}g";
    }
}
