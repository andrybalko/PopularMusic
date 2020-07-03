using PopularMusic.com.popularmusic.api;
using PopularMusic.com.popularmusic.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PopularMusic.com.popularmusic.view.viewmodel
{
	public class ArtistDetailsViewModel : BaseViewModel
	{
		private ObservableCollection<Album> topAlbums;
		private Album selectedAlbum;

		public ObservableCollection<Album> TopAlbums
		{
			get => topAlbums;
			set
			{
				topAlbums = value;
				RaisePropertyChanged(nameof(TopAlbums));
			}
		}
		public ArtistDetailsViewModel(Artist artist) : base()
		{
			Artist = artist;
			LoadAlbums();
		}

		private void LoadAlbums()
		{
			Task.Run(async () =>
			{
				var res = await apiRequestExecutor.LoadArtistTopAlbums(new ArtistTopAlbumsApiRequest().ForArtist(Artist.Name));
				TopAlbums = new ObservableCollection<Album>(res.Topalbums.Album);
				if (TopAlbums.Count > 0)
				{
					SelectedAlbum = TopAlbums[0];
				}
				IsRefreshing = false;
			});
		}

		public Artist Artist { get; }

		public Album SelectedAlbum
		{
			get => selectedAlbum;
			set
			{
				selectedAlbum = value;
				RaisePropertyChanged(nameof(SelectedAlbum));
			}
		}

		public ICommand RefreshCommand => new Command(() =>
		{
			IsRefreshing = true;
			LoadAlbums();
		});

		public ICommand AlbumSelectedCommand => new Command<Album>(a =>
		{

		});
	}
}
