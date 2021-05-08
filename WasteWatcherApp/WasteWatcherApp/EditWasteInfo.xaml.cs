﻿using Acr.UserDialogs;
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

            string plasticWaste = hasPlastic.IsChecked ? plasticWasteInput.Text : null;
            string paperWaste = hasPaper.IsChecked ? paperWasteInput.Text : null;
            string glasWaste = hasGlas.IsChecked ? glasWasteInput.Text : null;

            await Store.SaveData(Product.Barcode, plasticWaste, paperWaste, glasWaste);

            UserDialogs.Instance.HideLoading();
            await Navigation.PopAsync();
        }
    }
}