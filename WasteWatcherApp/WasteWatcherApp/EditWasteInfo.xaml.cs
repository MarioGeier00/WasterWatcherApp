using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using WasteWatcherApp.Waste;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditWasteInfo : ContentPage
    {

        public Product Product { get; }
        public IWasteStore Store { get; }

        public EditWasteInfo(Product product, IWasteStore store)
        {
            InitializeComponent();
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Store = store ?? throw new ArgumentNullException(nameof(store));

            this.Title = product.ProductName;
            UserDialogs.Instance.ShowLoading();
            LoadWasteData().ContinueWith(new Action<object>((_) => UserDialogs.Instance.HideLoading()));
        }

        private async Task LoadWasteData()
        {
            var wasteData = await Store.GetData(Product.Barcode);
            if (wasteData is null) return;

            plasticWasteInput.Text = wasteData[WasteType.Plastic].ToString();
            paperWasteInput.Text = wasteData[WasteType.Paper].ToString();
            glasWasteInput.Text = wasteData[WasteType.Glas].ToString();

            hasPlastic.IsChecked = wasteData[WasteType.Plastic].HasValue;
            hasGlas.IsChecked = wasteData[WasteType.Glas].HasValue;
            hasPaper.IsChecked = wasteData[WasteType.Paper].HasValue;
        }

        private async void SubmitButton_Clicked(object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();

            WasteCollection wasteCollection = new();
            EditableWasteCollection editableWasteCollection = wasteCollection.Modify();

            if (int.TryParse(plasticWasteInput.Text, out int plasticWaste) && hasPlastic.IsChecked)
            {
                editableWasteCollection.SetWasteAmount(WasteType.Plastic, plasticWaste);
            }
            if (int.TryParse(paperWasteInput.Text, out int paperWaste) && hasPaper.IsChecked)
            {
                editableWasteCollection.SetWasteAmount(WasteType.Paper, paperWaste);
            }
            if (int.TryParse(glasWasteInput.Text, out int glasWaste) && hasGlas.IsChecked)
            {
                editableWasteCollection.SetWasteAmount(WasteType.Glas, glasWaste);
            }

            await Store.SaveData(Product.Barcode, wasteCollection);

            UserDialogs.Instance.HideLoading();
            await Navigation.PopAsync();
        }
    }
}