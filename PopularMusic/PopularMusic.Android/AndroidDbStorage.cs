using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.EntityFrameworkCore;
using PopularMusic.com.popularmusic.db;
using PopularMusic.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDbStorage))]
namespace PopularMusic.Droid
{
	public class AndroidDbStorage : IDbStorage
	{
		public DatabaseContext InitDb()
		{
			var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "pop.db");
			var db = new DatabaseContext(dbPath);
			//for testing purposes
			//db.Database.EnsureDeleted();
			db.Database.EnsureCreated();
			db.Database.Migrate();
			return db;
		}
	}
}