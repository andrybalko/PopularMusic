using PopularMusic.com.popularmusic.api;
using PopularMusic.com.popularmusic.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PopularMusic.com.popularmusic.view.viewmodel
{
	public class ArtistsListViewModel : BaseViewModel
	{
		private string _currentCountry;

		public string CurrentCountry
		{
			get => _currentCountry; 
			set
			{
				_currentCountry = value;
				RaisePropertyChanged(nameof(CurrentCountry));
			}
		}

		public ObservableCollection<Artist> Artists { get; set; }


		public Artist SelectedArtist { get; set; }

		#region Commands
		public ICommand SetCountryCommand => new Command<string>(async (string c) =>
		{
			//Load artists for the country
			CurrentCountry = c;
			Artists = null;
			ApiTopArtistsModel res = await apiRequestExecutor.LoadTopArtists(new TopArtistsApiRequest().ForCountry(c));
			if (res != null)
			{
				Artists = new ObservableCollection<Artist>(res.TopArtists.Artist.OrderBy(x => x.Name));
				RaisePropertyChanged(nameof(Artists));
			}
			IsRefreshing = false;
		});

		public ICommand RefreshCommand => new Command(() =>
		{
			IsRefreshing = true;
			SetCountryCommand.Execute(CurrentCountry);
		});

		#endregion
		public ArtistsListViewModel() : base()
		{
			//just initialize the screen
			CurrentCountry = "Ukraine";
			RefreshCommand.Execute(null);
		}
	}
}
