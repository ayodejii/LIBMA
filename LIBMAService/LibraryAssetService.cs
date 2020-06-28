using LIBMAData.Repositories;
using System;
using LIBMAData.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using LIBMAData;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LIBMAService
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibmaContext context;

        public LibraryAssetService(LibmaContext _context)
        {
            context = _context;
        }

        public void Add(LibraryAsset libraryAsset)
        {
            context.Add(libraryAsset);
            context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            var entity = context.LibraryAssets
                                .Include(m => m.Status)
                                .Include(m => m.Location);
            return entity;
        }

        

        public LibraryAsset GetById(int id)
        {
            return GetAll().FirstOrDefault(m => m.Id == id);
        }

        public LibraryBranch GetLibraryBranch(int id)
        {
            return GetById(id).Location;
        }

        public string GetTitle(int id)
        {
            return GetById(id).Title;
        }

        public string GetType(int id)
        {
            var book = context.LibraryAssets.OfType<Book>()
                .Where(asset => asset.Id == id);
            return book.Any() ? "Book" : "Video";
        }
        public string GetAuthorOrDirector(int id)
        {
            var isBook = context.LibraryAssets.OfType<Book>()
                .Where(asset => asset.Id == id).Any();

            //var isVideo = _libraryContext.LibraryAssetss.OfType<Video>()
            //    .Where(asset => asset.Id == id).Any();

            return isBook ?
                context.Books.FirstOrDefault(book => book.Id == id).Author
                : context.Videos.FirstOrDefault(video => video.Id == id).Director
                ?? "Unknown";
        }

        public string GetDeweyIndex(int id)
        {
            if (context.Books.Any(m => m.Id == id))
            {
                return context.Books.FirstOrDefault(m => m.Id == id).DeweyIndex;
            }
            return "";
        }

        public string GetIsbn(int id)
        {
            if (context.Books.Any(book => book.Id == id))
            {
                return context.Books
                    .FirstOrDefault(book => book.Id == id).ISBN;
            }
            else return "";
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }
    }

}
