using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using PopularMusic.com.popularmusic.config;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PopularMusic.com.popularmusic.api
{
	public class ApiProvider
	{
		private static ApiProvider _instance;

		//public static ApiProvider Instance => _instance;

		static ApiProvider()
		{
			FlurlHttp.Configure(settings =>
			  {
				  settings.FlurlClientFactory = new PMFlurlClientFactory();
			  });
		}

		public static void Init()
		{
			//just to configure Flurl
			_instance = new ApiProvider();
		}
	}

	public class UrlFactory
	{
		public static Url Build()
		{
			var s = new Url(Config.BaseUrl);
			
			return s;
		}
	}


	public class PMFlurlClientFactory : PerHostFlurlClientFactory
	{
		private string HeaderName = "api_key";

		public override IFlurlClient Get(Url url)
		{
			var client = base.Get(url);


			if (!string.IsNullOrEmpty(Config.ApiKey))
			{
				client.WithHeader(HeaderName, Config.ApiKey);
			}
			client.Configure(settings =>
			{
				settings.Timeout = TimeSpan.FromSeconds(60);
			});
			return client;
		}
	}

	
}
