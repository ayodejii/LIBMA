using LIBMA.Models.Patron;
using LIBMAData;
using LIBMAData.Model;
using LIBMAData.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIBMA.Controllers
{
    public class PatronController : Controller
    {
        IPatron _patron;
        public PatronController(IPatron patron)
        {
            _patron = patron;
        }

        public IActionResult Index()
        {
            var allPatrons = _patron.Get();

            var patronModels = allPatrons
                .Select(p => new PatronDetailModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    LibraryCardId = p.LibraryCard.Id,
                    OverdueFees = p.LibraryCard.Fees,
                    HomeLibraryBranch = p.HomeLibraryBranch.Name
                }).ToList();

            var model = new PatronIndexModel()
            {
                Patrons = patronModels
            };

            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var patron = _patron.GetPatronId(id);
            var model = new PatronDetailModel
            {
                LastName = patron.LastName,
                FirstName = patron.FirstName,
                Address = patron.Address,
                HomeLibraryBranch = patron.HomeLibraryBranch.Name,
                MemberSince = patron.LibraryCard.Created,
                OverdueFees = patron.LibraryCard.Fees,
                LibraryCardId = patron.LibraryCard.Id,
                Telephone = patron.Telephone,
                AssetsCheckedOut = _patron.GetCheckouts(id).ToList() ?? new List<Checkout>(),
                CheckoutHistories = _patron.GetCheckoutHistory(id),
                Holds = _patron.GetHolds(id)
            };
            return View(model);
        }
    }
}
