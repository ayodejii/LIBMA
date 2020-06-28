using LIBMAData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIBMAData.Repositories
{
    public interface ILibraryBranch
    {
        IEnumerable<LibraryBranch> GetAll();
        IEnumerable<Patron> GetPatrons(int branchId);
        IEnumerable<LibraryAsset> GetAssets(int branchId);
        IEnumerable<string> GetBranchHours(int branchId);
        LibraryBranch Get(int branchId);
        void Add(LibraryBranch branch);
        bool IsBranchOpen(int branchId);
        int GetAssetCount(int branchId);
        int GetPatronCount(int branchId);
        decimal GetAssetsValue(int branchId);

    }
}
