using PopularMusic.com.popularmusic.api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PopularMusic.com.popularmusic.view.viewmodel
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		private static bool _isOnline;
		

		protected IApiRequestExecutor apiRequestExecutor;

		public bool IsOnline
		{
			get => _isOnline;
			set
			{
				_isOnline = value;
				ConnectionManager.UseOnline = value;
				RaisePropertyChanged(nameof(IsOnline));
			}
		}

		public Color InternetColor
		{
			get => internetColor;
			set
			{
				internetColor = value;
				RaisePropertyChanged(nameof(InternetColor));
			}
		}

		public string State
		{
			get => state;
			set
			{
				state = value;
				RaisePropertyChanged(nameof(State));
			}
		}

		private bool isRefreshing;
		private Color internetColor;
		private string state;

		public BaseViewModel()
		{
			apiRequestExecutor = new ApiRequestExecutor();

			//set initial values
			IsOnline = _isOnline;

			MessagingCenter.Subscribe<ConnectionManager, bool>(this, ConnectionManager.ConnectionChanged, (sender, message) =>
			{
				if (message)
				{
					//internet avaialble
					InternetColor = (Color)App.Current.Resources["InternetAvailable"];
					State = "Connected";
				}
				else
				{
					//not available or limited or unknown
					InternetColor = (Color)App.Current.Resources["NoInternet"];
					State = "Disconnected";
					//avoid state recalculation
					_isOnline = false;
					RaisePropertyChanged(nameof(IsOnline));
				}
				Debug.WriteLine(message);
			});
		}

		public bool IsRefreshing
		{
			get => isRefreshing;
			set
			{
				isRefreshing = value;
				RaisePropertyChanged(nameof(IsRefreshing));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
