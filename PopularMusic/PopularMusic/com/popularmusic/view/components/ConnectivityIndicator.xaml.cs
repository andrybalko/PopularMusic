using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PopularMusic.com.popularmusic.view.components
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConnectivityIndicator : ContentView
	{
		public ConnectivityIndicator()
		{
			InitializeComponent();
#if DEBUG
			IsVisible = true;
#endif
		}
	}
}