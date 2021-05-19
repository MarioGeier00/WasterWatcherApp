using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using WasteWatcherApp.Product;
using WasteWatcherApp.Waste;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WasteWatcherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditWasteInfo : ContentPage
    {

        public ProductData Product { get; }
        public IWasteStore Store { get; }

        public WasteCollection WasteData { get; private set; }

        public EditWasteInfo(ProductData product, IWasteStore store)
        {
            InitializeComponent();
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Store = store ?? throw new ArgumentNullException(nameof(store));

            this.Title = product.ProductName;
            UserDialogs.Instance.ShowLoading();
            LoadWasteData().ContinueWith(new Action<object>((_) => UserDialogs.Instance.HideLoading()));
        }

        /// <summary>
        /// Load waste data and fill constrols with that data
        /// </summary>
        private async Task LoadWasteData()
        {
            WasteData = await Store.GetData(Product.Barcode);

            plasticWasteInput.Text = WasteData[WasteType.Plastic].ToString();
            paperWasteInput.Text = WasteData[WasteType.Paper].ToString();
            glasWasteInput.Text = WasteData[WasteType.Glas].ToString();

            hasPlastic.IsChecked = WasteData[WasteType.Plastic].HasValue;
            hasGlas.IsChecked = WasteData[WasteType.Glas].HasValue;
            hasPaper.IsChecked = WasteData[WasteType.Paper].HasValue;
        }

        /// <summary>
        /// Method triggered by the submit button to save the waste data
        /// </summary>
        private async void SubmitButton_Clicked(object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();

            WasteData ??= new WasteCollection();
            EditableWasteCollection editableWasteCollection = WasteData.Modify();
            editableWasteCollection.ClearAllWaste();

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

            try
            {
                await Store.SaveData(Product.Barcode, WasteData);
            }
            catch (Exception)
            {
                ToastService.ShowToastLong("Daten können nicht abgespeichert werden. Bitte erneut versuchen.");
                return;
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

            await Navigation.PopAsync();
        }
    }
}