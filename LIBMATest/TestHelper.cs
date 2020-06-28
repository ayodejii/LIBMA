using LIBMAData;
using LIBMAData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIBMATest
{
    internal static class TestHelper
    {
        public static LibraryAsset LibraryAsset()
        {
            var book = new Book()
            {
                Id = 233,
                Title = "Testing",
                Author = "Isaac",
                Year = 2020,
                Status = new Status() { Id = 1, Name = "Checked out", Description = "A library asset that has been checked out" },
                Cost = 1.1M
            };
            return book;
        }
        public static IEnumerable<LibraryAsset> LibraryAssets()
        {
            var assets = new List<LibraryAsset>();
            assets.Add(new Book()
            {
                Id = 233,
                Title = "Testing",
                Author = "Isaac",
                Year = 2020,
                Status = new Status() { Id = 1, Name = "Checked out", Description = "A library asset that has been checked out" },
                Cost = 1.1M
            });
            assets.Add(new Video()
            {
                Id = 200,
                Title = "Video Out",
                Director = "Tunji",
                Year = 2020,
                Status = new Status() { Id = 1, Name = "Checked out", Description = "A library asset that has been checked out" },
                Cost = 1.1M
            });
            return assets;
        }
    }
}
