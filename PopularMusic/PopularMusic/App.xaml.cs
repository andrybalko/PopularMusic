using PopularMusic.com.popularmusic.api;
using PopularMusic.com.popularmusic.theme;
using PopularMusic.com.popularmusic.view;
using PopularMusic.com.popularmusic.view.viewmodel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PopularMusic
{
	public partial class App : Application
	{
		ConnectionManager _connectionManager;
		public App()
		{
			InitializeComponent();

			InitTheme();

			_connectionManager = new ConnectionManager();

			ApiProvider.Init();

			//instead of hardcoding viewmodel it would be better to use any available DI framework like Prism/MVVMCross/else instead
			MainPage = new NavigationPage(new ArtistsListPage() { BindingContext = new ArtistsListViewModel() });


		}

		private void InitTheme()
		{
			ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
			if (mergedDictionaries != null)
			{
				//use prod theme
				mergedDictionaries.Clear();
				mergedDictionaries.Add(new ProdTheme());
#if DEBUG
				//use dev theme
				mergedDictionaries.Clear();
				mergedDictionaries.Add(new DevTheme());
#endif
			}
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
