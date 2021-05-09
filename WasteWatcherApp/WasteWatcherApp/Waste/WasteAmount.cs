namespace WasteWatcherApp.Waste
{
    public record WasteAmount(WasteType WasteType, int Amount)
    {
        public override string ToString() => $"{WasteType.ToFriendlyName()}: {Amount}g";
    }
}
