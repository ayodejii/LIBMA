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
    public class CheckoutService : ICheckout
    {
        private LibmaContext _context;

        public CheckoutService(LibmaContext context)
        {
            _context = context;
        }
        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return _context.Checkouts
                .FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistories(int id)
        {
            var entity = _context.CheckoutHistories
                .Include(history => history.LibraryAsset)
                .Include(history => history.LibraryCard);
                //if it is ienumerable, 
                //use *where instead of firstordefault
                //.Where(history => history.LibraryCard.Id == id).ToList();
            return entity;
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        //gets current ssets on hold
        {
            return _context.Holds
                .Include(hold => hold.LibraryAsset)
                .Where(hold => hold.LibraryAsset.Id == id);
        }

        public Checkout GetLatestCheckout(int id)
        {
            return _context.Checkouts
                .Where(checkout => checkout.LibraryAsset.Id == id)
                .OrderByDescending(checkout => checkout.Since)
                .FirstOrDefault();
        }

        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;

            UpdateAssetStatus(assetId, "Available");

            //remove any exstinng checkouts on the item
            RemoveExistingCheckout(assetId);

            //remove the existing chekout history

            RemoveExistingCheckoutHistory(assetId, now);

            _context.SaveChanges();

        }

        public void MarkLost(int assetId)
        {
            UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var item = _context.LibraryAssets
                .Include(asset => asset.Status)
                 .FirstOrDefault(asset => asset.Id == assetId); //get the asset/item that matches the id

            var libraryCard = _context.LibraryCards ////get the library card deets that matches the id
                .FirstOrDefault(card => card.Id == libraryCardId);
            //check if the asset is available so it can be placed on hold
            if (item.Status.Name == "Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };
            _context.Add(hold);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId)
        {
            var now = DateTime.Now;

            var item = _context.LibraryAssets
                .FirstOrDefault(asset => asset.Id == assetId);

            // remove any existing checkout
            RemoveExistingCheckout(assetId);

            //remove checkout history
            RemoveExistingCheckoutHistory(assetId, now);

            //look for existing holds on the item
            var currentHold = _context.Holds
                .Include(hold => hold.LibraryAsset)
                .Include(hold => hold.LibraryCard)
                .Where(hold => hold.LibraryAsset.Id == assetId); //shows the number of people that ave holds on the item
            //if there are holds, checkout item to the 
            //library card with the earliest hold
            if (currentHold.Any()) //if here are people with holds on this item
            {
                CheckoutToEarliestHold(assetId, currentHold);
                return;
            }
            //otherwise change status to available
            UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();
        }
        public DateTime GetCurrentHoldPlaced(int id)
        {
            return
                _context.Holds
               .Include(h => h.LibraryAsset)
               .Include(h => h.LibraryCard)
               .FirstOrDefault(h => h.Id == id)
               .HoldPlaced;
        }

        public string GetCurrentHoldCheckoutPatron(int assetId) //current ptron who has an item checkd out
        {
            var checkout = GetCheckoutByAssetId(assetId);

            if (checkout == null)
            {
                return "";
            }

            var libraryCardId = checkout.LibraryCard.Id;
            var patron = _context.Patrons
                .Include(c => c.LibraryCard).
                FirstOrDefault(c => c.LibraryCard.Id == assetId);

            return patron.FirstName + " " + patron.LastName;
        }
        public string GetCurrentHoldPatronName(int holdId)
        {
            var hold = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id == holdId); //get the hold corresponding to the holdid

            var cardId = hold?.LibraryCard.Id; //get the id of the library card
            //the ? means null for when he cardId is null
            //to get the patron that owns the librarycardid above
            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId); //te patron whose library card corresponds to the cardid above
            //then return the name (or details) of the patron in question
            var FullName = patron?.FirstName + " " + patron?.LastName;
            return FullName;

        }
        public void CheckoutItem(int assetId, int libraryCardId)
        {
            try
            {
                var ischeckedout = IsCheckedout(assetId);

                if (ischeckedout) //if the item is checked out i.e. if the assetId carries (or shows) checkedout
                {
                    return;
                    //add logic to handke feedback to the user
                }
                var item = _context.LibraryAssets
                     .FirstOrDefault(asset => asset.Id == assetId);
                //get the item id

                UpdateAssetStatus(assetId, "Checked Out"); //update the item status to check out

                var libraryCard = _context.LibraryCards
                    .Include(card => card.Checkouts)
                    .FirstOrDefault(card => card.Id == libraryCardId);
                //check whose library card checked the item out and grab it

                var now = DateTime.Now;

                var checkout = new Checkout
                {
                    LibraryAsset = item,
                    LibraryCard = libraryCard,
                    Since = now,
                    Until = GetDefaultCheckoutTime(now)
                };

                _context.Add(checkout);

                var checkoutHistory = new CheckoutHistory
                {
                    CheckedOut = now,
                    LibraryAsset = item,
                    LibraryCard = libraryCard
                };

                _context.Add(checkoutHistory);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                //throw;
            }

        }
        public bool IsCheckedout(int assetid)
        {
            return _context.Checkouts
               .Where(c => c.LibraryAsset.Id == assetid)
               .Any(); //to check the checkout taable and see if the an item (with the passed ID) is checked out.
            //this method shows 
        }

        #region Misc Methods
        private void UpdateAssetStatus(int assetId, string newStatus)
        {
            var item = _context.LibraryAssets
                 .FirstOrDefault(asset => asset.Id == assetId); //gets the item which matches the id passed in the LibraryAssets table

            _context.Update(item);

            item.Status = _context.Statuses
               .FirstOrDefault(status => status.Name == newStatus);

        }

        private void RemoveExistingCheckoutHistory(int assetId, DateTime now)
        {
            var history = _context.CheckoutHistories
                .FirstOrDefault(h => h.LibraryAsset.Id == assetId
                && h.CheckedIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckout(int assetId)
        {
            var checkout = _context.Checkouts
                .FirstOrDefault(check => check.Id == assetId);
            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHold)
        {
            var earliestHold = currentHold
                .OrderBy(hold => hold.HoldPlaced)
                .FirstOrDefault(); //the person with the earliest hold on the item

            var card = earliestHold.LibraryCard; //get the earliest person's library card 

            _context.Remove(earliestHold); //the hold on the item is removed so that when it is checked back in, it goes to the (another) next person 
            _context.SaveChanges();

            CheckoutItem(assetId, card.Id);

        }
        
        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }
        
        private Checkout GetCheckoutByAssetId(int assetId) //gets items that are checked out by its id
        {
            return _context.Checkouts
                .Include(c => c.LibraryAsset)
                .Include(c => c.LibraryCard)
                .FirstOrDefault(c => c.LibraryCard.Id == assetId);
        }
        #endregion
    }
}
