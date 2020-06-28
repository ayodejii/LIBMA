using LIBMAData.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIBMAData.Repositories
{
    public interface ILibraryAsset
    {
        
        string GetAuthorOrDirector(int id);
        string GetDeweyIndex(int id);
        string GetType(int id);
        string GetTitle(int id);
        string GetIsbn(int id);
        IEnumerable<LibraryAsset> GetAll();
        LibraryAsset GetById(int id);
        LibraryBranch GetLibraryBranch(int id);
        LibraryBranch GetCurrentLocation(int id);
        void Add(LibraryAsset libraryAsset);

    }
}
