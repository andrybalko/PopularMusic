using PopularMusic.com.popularmusic.config;
using PopularMusic.com.popularmusic.model;
using PopularMusic.com.popularmusic.view.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PopularMusic.com.popularmusic.view
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ArtistsListPage : ContentPage
	{
		public ArtistsListPage()
		{
			InitializeComponent();
			
		}

		private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//this handler is made here to realize navigation. 
			//Proper way is to use any framework like Prism/MvvmCross/etc and navigate from viewmodels
			if (e.CurrentSelection.Count > 0)
			{
				Artist a = e.CurrentSelection[0] as Artist;
				if (a != null)
				{
					await Navigation.PushAsync(new ArtistDetailsPage() { BindingContext = new ArtistDetailsViewModel(a) });
				}
				((CollectionView)sender).SelectedItem = null;
			}
		}
	}
}