using LIBMAData.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using LIBMAData.Model;
using LIBMAData;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LIBMAService
{
    public class LibraryBranchService : ILibraryBranch
    {
        private LibmaContext _context;

        public LibraryBranchService(LibmaContext context)
        {
            _context = context;
        }
        public void Add(LibraryBranch branch)
        {
            _context.Add(branch);
            _context.SaveChanges();
        }

        public LibraryBranch Get(int branchId)
        {
            return GetAll().FirstOrDefault(br => br.Id == branchId);
        }

        public IEnumerable<LibraryBranch> GetAll()
        {
            return _context.LibraryBranches
                .Include(a => a.Patrons)
                .Include(a => a.LibraryAssets);
        }

        public int GetAssetCount(int branchId)
        {
            return Get(branchId).LibraryAssets.Count();
        }

        public IEnumerable<LibraryAsset> GetAssets(int branchId)
        {
            return _context.LibraryBranches.Include(a => a.LibraryAssets)
                .First(b => b.Id == branchId).LibraryAssets;
        }

        public decimal GetAssetsValue(int branchId)
        {
            var assetsValue = GetAssets(branchId).Select(a => a.Cost);
            return assetsValue.Sum();
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context.BranchHours.Where(a => a.Branch.Id == branchId);

            var displayHours = DataHelpers.HumanizeBusinessHours(hours);

            return displayHours;
        }

        public int GetPatronCount(int branchId)
        {
            return Get(branchId).Patrons.Count();
        }

        public IEnumerable<Patron> GetPatrons(int branchId)
        {
            return _context.LibraryBranches.Include(a => a.Patrons).First(b => b.Id == branchId).Patrons;
        }

        //TODO: Implement 
        public bool IsBranchOpen(int branchId)
        {
            //var currentHour = DateTime.Now.Hour;
            //var currentDayOfWeek = (int)DateTime.Now.DayOfWeek + 1;
            //var branchHours = _context.BranchHours.Where(n => n.Branch.Id == branchId);
            //var branchDays = branchHours.FirstOrDefault(n => n.DayOfWeek == currentDayOfWeek);
            //var isOpen = currentHour < branchDays.CloseTime && currentHour > branchDays.OpenTime;
            return true;
        }
    }
}
