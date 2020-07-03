using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using Microsoft.EntityFrameworkCore;
using PopularMusic.com.popularmusic.db;
using PopularMusic.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDbStorage))]
namespace PopularMusic.iOS
{
	public class IosDbStorage : IDbStorage
	{
		public DatabaseContext InitDb()
		{
			var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", "pop.db");
			var db = new DatabaseContext(dbPath);
            db.Database.EnsureCreated();
			db.Database.Migrate();
			return db;
        }
	}
}