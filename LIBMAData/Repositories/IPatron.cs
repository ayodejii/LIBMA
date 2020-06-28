using LIBMAData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIBMAData.Repositories
{
    public interface IPatron
    {

        Patron GetPatronId(int id);

        IEnumerable<Patron> Get();
        void Add(Patron newPatron);

        IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronId);
        IEnumerable<Hold> GetHolds(int patronId);
        IEnumerable<Checkout> GetCheckouts(int patronId);
    }
}
