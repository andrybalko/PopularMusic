using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PopularMusic.com.popularmusic.api
{
	public class ConnectionManager : IDisposable
	{

		public static bool IsConnected { get => isConnected; 
			set 
			{ 
				isConnected = value;
				CalculateState();
			} 
		}

		public const string ConnectionChanged = "ConnectionChanged";
		private static bool isConnected;
		private static bool useOnline;


		private static ConnectionManager _instance;
		public ConnectionManager()
		{
			_instance = this;

			//This event handler is not removed by GC because this object supposed to exist during all app lifecicle
			Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
			//bump event up initially
			Connectivity_ConnectivityChanged(this, new ConnectivityChangedEventArgs(Connectivity.NetworkAccess, Connectivity.ConnectionProfiles));
		}

		void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
		{
			var access = e.NetworkAccess;
			var profiles = e.ConnectionProfiles;

			IsConnected = access == NetworkAccess.Internet;
		}

		public static bool UseOnline
		{
			get => useOnline;
			set
			{
				useOnline = value;
				CalculateState();
			}
		}

		private static void CalculateState()
		{
			MessagingCenter.Send(_instance, ConnectionChanged, IsConnected && UseOnline);
		}

		public void Dispose()
		{
			Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

		}
	}
}
