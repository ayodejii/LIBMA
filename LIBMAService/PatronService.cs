using LIBMAData.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using LIBMAData.Model;
using LIBMAData;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LIBMAService
{
    public class PatronService : IPatron
    {
        private LibmaContext _context;

        public PatronService(LibmaContext context)
        {
            _context = context;
        }
        public void Add(Patron newPatron)
        {
            _context.Add(newPatron);
            _context.SaveChanges();
        }

        public Patron GetPatronId(int id)
        {
            return Get()
                .FirstOrDefault(patron => patron.Id == id);
        }

        public IEnumerable<Patron> Get()
        {
            return _context.Patrons
                .Include(patron => patron.LibraryCard)
                .Include(patron => patron.HomeLibraryBranch);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronId)
        {
            var cardId = GetPatronId(patronId)
                .LibraryCard.Id;

            return _context.CheckoutHistories
                .Include(ch => ch.LibraryCard)
                .Include(ch => ch.LibraryAsset)
                .Where(ch => ch.LibraryCard.Id == cardId)
                .OrderByDescending(co => co.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int patronId)
        {
            var cardId = GetPatronId(patronId)
                .LibraryCard.Id;

            return _context.Checkouts
               .Include(ch => ch.LibraryCard)
               .Include(ch => ch.LibraryAsset)
               .Where(ch => ch.LibraryCard.Id == cardId);
        }

        public IEnumerable<Hold> GetHolds(int patronId)
        {
            var cardId = GetPatronId(patronId)
                .LibraryCard.Id;

            return _context.Holds
                .Include(h => h.LibraryCard)
                .Include(h => h.LibraryAsset)
                .Where(h => h.LibraryCard.Id == cardId)
                .OrderByDescending(co => co.HoldPlaced);
        }
    }
}
