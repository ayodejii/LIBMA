using LIBMAData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIBMAData.Repositories
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        IEnumerable<Hold> GetCurrentHolds(int id);
        IEnumerable<CheckoutHistory> GetCheckoutHistories(int id);

        void MarkLost(int assetId);
        void MarkFound(int assetId);
        void Add(Checkout newCheckout);
        void CheckoutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
        void PlaceHold(int assetId, int libraryCardId);
        string GetCurrentHoldPatronName(int id);
        string GetCurrentHoldCheckoutPatron(int assetId);
        bool IsCheckedout(int id);
        DateTime GetCurrentHoldPlaced(int id);

        Checkout GetLatestCheckout(int id);
        Checkout GetById(int checkoutId);
    }
}
