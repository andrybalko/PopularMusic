using Flurl.Http;
using Newtonsoft.Json;
using PopularMusic.com.popularmusic.config;
using PopularMusic.com.popularmusic.db;
using PopularMusic.com.popularmusic.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace PopularMusic.com.popularmusic.api
{
	public interface IApiRequestExecutor
	{
		//both methods can be unified as a generic, but this would be another story
		Task<ApiTopArtistsModel> LoadTopArtists(ApiRequest request);
		Task<ArtistTopAbumsModel> LoadArtistTopAlbums(ApiRequest request);
	}

	public class ApiRequestExecutor : IApiRequestExecutor
	{

		public async Task<ArtistTopAbumsModel> LoadArtistTopAlbums(ApiRequest request)
		{
			return await Task.Run(async () =>
			{

				if (!ConnectionManager.IsConnected || !ConnectionManager.UseOnline)
				{
					ArtistTopAbumsModel m = new ArtistTopAbumsModel() { Topalbums = new Topalbums() { Album = await DbHelper.GetArtistAlbumsAsync(((ArtistTopAlbumsApiRequest)request).Artist) } };
					return m;
				}

				try
				{
					string s = await DoLoad(request);

					var r = JsonConvert.DeserializeObject<ArtistTopAbumsModel>(s);

					if (ConnectionManager.IsConnected && ConnectionManager.UseOnline)
					{
						foreach (var item in r.Topalbums.Album)
						{
							foreach (var i in item.Image)
							{
								i.Album = item;
							}
						}
						await DbHelper.AddOrUpdateAlbumsAsync(r.Topalbums.Album);
					}

					return r;
				}
				catch (Exception e)
				{
					Debug.WriteLine("gor error: {0}, \nstacktrace: {1}", e.Message, e.StackTrace);
				}
				return null;
			});
		}


		public async Task<ApiTopArtistsModel> LoadTopArtists(ApiRequest request)
		{
			return await Task.Run(async () =>
			{
				if (!ConnectionManager.IsConnected || !ConnectionManager.UseOnline)
				{
					ApiTopArtistsModel m = new ApiTopArtistsModel() { TopArtists = new TopArtists() { Artist = await DbHelper.GetArtistsAsync(((TopArtistsApiRequest)request).Country) } };
					return m;
				}

				try
				{
					string s = await DoLoad(request);

					var r = JsonConvert.DeserializeObject<ApiTopArtistsModel>(s);
					if (ConnectionManager.IsConnected && ConnectionManager.UseOnline)
					{
						foreach (var item in r.TopArtists.Artist)
						{
							item.Country = ((TopArtistsApiRequest)request).Country;
							foreach (var i in item.Image)
							{
								i.Artist = item;
							}
						}


						await DbHelper.AddOrUpdateArtistsAsync(r.TopArtists.Artist);
					}
					return r;
				}
				catch (Exception e)
				{
					Debug.WriteLine("gor error: {0}, \nstacktrace: {1}", e.Message, e.StackTrace);
				}
				return null;
			});
		}

		private async Task<string> DoLoad(ApiRequest request)
		{
			var query = UrlFactory.Build();
			if (request.Filters?.Count > 0)
			{
				request.Filters.ForEach(filter =>
				{
					query.SetQueryParam(filter.FilterName, filter.FilterValue);
				});
			}

			if (!string.IsNullOrEmpty(request.Segment))
			{
				query.AppendPathSegment(request.Segment);
			}

			var response = await query.GetStringAsync();

			var s = response;
			return s;
		}
	}

	public class TopArtistsApiRequest:ApiRequest
	{
		public string Country { get; private set; }

		public TopArtistsApiRequest():base()
		{
			Filters.Add(new ApiRequestFilter("method", "geo.gettopartists"));
		}

		public override ApiRequest ForCountry(string country)
		{
			Country = country;
			return base.ForCountry(country);
		}
	}

	public class ArtistTopAlbumsApiRequest : ApiRequest
	{
		public Artist Artist { get; set; }

		public ArtistTopAlbumsApiRequest() : base()
		{
			Filters.Add(new ApiRequestFilter("method", "artist.gettopalbums"));
		}

		public override ApiRequest ForArtist(string art)
		{
			Artist = new Artist() { Name = art };
			return base.ForArtist(art);
		}
	}

	public partial class ApiRequest
	{
		public string Segment { get; set; }

		public ApiRequestFilter ApiRequestFilter
		{
			get
			{
				return Filters.Count >= 1 ? Filters[0] : null;
			}
		}

		public ApiRequest()
		{
			Filters = new List<ApiRequestFilter>();
			Filters.Add(new ApiRequestFilter("format", "json"));
			Filters.Add(new ApiRequestFilter("api_key", Config.ApiKey));
		}
	}
	public partial class ApiRequest
	{
		//Filters processing part of the class
		#region Filters
		public List<ApiRequestFilter> Filters { get; set; }

		public virtual ApiRequest ForCountry(string country)
		{
			Filters.Add(new ApiRequestFilter() { FilterName = "country", FilterValue = country });
			return this;
		}

		public virtual ApiRequest ForArtist(string art)
		{
			Filters.Add(new ApiRequestFilter() { FilterName = "artist", FilterValue = art });
			return this;
		}

		#endregion
	}


	public class ApiRequestFilter
	{
		public string FilterName { get; set; }
		public string FilterValue { get; set; }

		public ApiRequestFilter(string filterName, string filterValue)
		{
			this.FilterName = filterName;
			this.FilterValue = filterValue;
		}

		public ApiRequestFilter()
		{
		}
	}
}
