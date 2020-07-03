using Microsoft.EntityFrameworkCore;
using PopularMusic.com.popularmusic.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PopularMusic.com.popularmusic.db
{
	public class DbHelper
	{
        private static DatabaseContext CreateContext()
        {
            DatabaseContext dbc = DependencyService.Get<IDbStorage>().InitDb();
            return dbc;
        }

        public static async Task AddOrUpdateArtistsAsync(List<Artist> artists)
        {
            using (var context = CreateContext())
            {
                try
                {
                    var newArtists = artists.Where(art => context.Artists.Any(a => a.Name == art.Name) == false);
                    await context.Artists.AddRangeAsync(newArtists);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {

                }
               
            }
        }

        public static async Task AddOrUpdateAlbumsAsync(List<Album> albums)
        {
            using (var context = CreateContext())
            {
                try
                {
                    var newAlbums = albums.Where(
                    alb => context.Albums.Any(a => a.Name == alb.Name) == false);
                    await context.Albums.AddRangeAsync(newAlbums);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                }
                
            }
        }

        public static async Task<List<Artist>> GetArtistsAsync(string country)
        {
            using (var context = CreateContext())
            {
                return await context.Artists
                                    .AsNoTracking()
                                    .Include(a => a.Image)
                                    .Where(a => a.Country == country)
                                    .ToListAsync();
            }
        }

        public static async Task<List<Album>> GetArtistAlbumsAsync(Artist a)
        {
            using (var context = CreateContext())
            {
                return await context.Albums
                                    .Where(x=> x.Artist.Name == a.Name)
                                    .Include(x => x.Image)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }
    }
}
