using LIBMA.Models.Catalog;
using LIBMA.Models.Checkout;
using LIBMAData.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIBMA.Controllers
{
    public class CatalogController: Controller
    {
        private ILibraryAsset _asset;
        private ICheckout _checkout;

        public CatalogController(ILibraryAsset assets, ICheckout checkout)
        {
            _asset = assets;
            _checkout = checkout;
        }
        public IActionResult Index()
        {
            var assetModels = _asset.GetAll();

            var listingResult = assetModels
                .Select(a => new AssetIndexListingModel
                {
                    Id = a.Id,
                    ImageUrl = a.ImageUrl,
                    AuthorOrDirector = _asset.GetAuthorOrDirector(a.Id),
                    DeweyCallNumber = _asset.GetDeweyIndex(a.Id),
                    // CopiesAvailable = _checkout.GetNumberOfCopies(a.Id), // Remove
                    Title = _asset.GetTitle(a.Id),
                    Type = _asset.GetType(a.Id),
                    //NumberOfCopies = _checkout.GetNumberOfCopies(a.Id)
                });

            var model = new AssetIndexModel
            {
                Assets = listingResult,
                Title = "Library Catalog"
            };

            return View(model);
        } //index

        public IActionResult Detail(int id) //detail
        {
            var asset = _asset.GetById(id);

            var currentHolds = _checkout.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = _checkout.GetCurrentHoldPlaced(a.Id).ToString("d"),
                    PatronName = _checkout.GetCurrentHoldPatronName(a.Id),
                });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Type = _asset.GetType(id),
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _asset.GetAuthorOrDirector(id),
                CurrentLocation = _asset.GetCurrentLocation(id)?.Name,
                DeweyCallNumber = _asset.GetDeweyIndex(id),
                CheckoutHistory = _checkout.GetCheckoutHistories(id),
                //CurrentAssociatedLibraryCard = _asset.GetLibraryCardByAssetId(id),
                ISBN = _asset.GetIsbn(id),
                LatestCheckout = _checkout.GetLatestCheckout(id),
                CurrentHolds = currentHolds,
                PatronName = _checkout.GetCurrentHoldCheckoutPatron(id)
            };

            return View(model);
        }
        public IActionResult CheckOut(int id)
        {
            var asset = _asset.GetById(id);
            if (asset == null)
                return NotFound();
            var model = new CheckoutModels
            {
                AssetId = asset.Id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkout.IsCheckedout(id)
            };
            return View(model);
        }
        public IActionResult Hold(int id)
        {
            var asset = _asset.GetById(id);

            var model = new CheckoutModels
            {
                AssetId = asset.Id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkout.IsCheckedout(asset.Id),
                HoldCount = _checkout.GetCurrentHolds(asset.Id).Count()
            };
            return View(model);
        }
        public IActionResult MarkLost(int assetId)
        {
            _checkout.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult MarkFound(int assetId)
        {
            _checkout.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId) //what to happen when check out is clicked
        {
            try
            {
                _checkout.CheckoutItem(assetId, libraryCardId);

            }
            catch (Exception ex)
            {

                //throw;
            }
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId) //what to happen when check out is clicked
        {
            _checkout.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult CheckIn(int id)
        {
            _checkout.CheckInItem(id);

            return RedirectToAction("Detail", new { id = id });
        }
    }
}
