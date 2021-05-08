using Acr.UserDialogs;
using System;
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
            LoadWasteData();
        }
        private async void LoadWasteData()
        {
            WasteData<int> wasteData = await Store.GetData(Product.Barcode);
            plasticWasteInput.Text = wasteData.PlasticWaste.ToString();
            paperWasteInput.Text = wasteData.PaperWaste.ToString();
            glasWasteInput.Text = wasteData.GlasWaste.ToString();
            
            hasPlastic.IsChecked = wasteData.PlasticWaste != 0;
            hasGlas.IsChecked = wasteData.GlasWaste != 0;
            hasPaper.IsChecked = wasteData.PaperWaste != 0;

        }
        private async void SubmitButton_Clicked(object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();

            string plasticWaste = hasPlastic.IsChecked ? plasticWasteInput.Text : null;
            string paperWaste = hasPaper.IsChecked ? paperWasteInput.Text : null;
            string glasWaste = hasGlas.IsChecked ? glasWasteInput.Text : null;

            await Store.SaveData(Product.Barcode, plasticWaste, paperWaste, glasWaste);

            UserDialogs.Instance.HideLoading();
            await Navigation.PopAsync();
        }
    }
}