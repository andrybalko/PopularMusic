using System;
using System.Collections.Generic;
using System.Text;

namespace PopularMusic.com.popularmusic.config
{
	public class Config
	{
		public static List<string> Countries => new List<string>() { "Ukraine", "Canada", "Nigeria", "Japan" };

		public static readonly string BaseUrl = "https://ws.audioscrobbler.com/2.0/";
		public static readonly string ApiKey = "e81f61890b7ff8633ca024d0faa449e7";
	}



}
