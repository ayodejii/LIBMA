using LIBMAData.Repositories;
using System;
using Xunit;
using Moq;
using LIBMA.Controllers;
using Microsoft.AspNetCore.Mvc;
using LIBMA.Models.Checkout;
using LIBMAData;
using LIBMAData.Model;
using System.Collections.Generic;
using LIBMA.Models.Catalog;
using System.Linq;

namespace LIBMATest.Controllers
{
    public class CatalogControllerTest
    {
        //private readonly CatalogController _controller;
        //private readonly ICheckout checkoutService;
        //private readonly dynamic libraryAssetService;
        public CatalogControllerTest()
        {
            var libraryAssetService = new Mock<ILibraryAsset>();
            var checkoutService = new Mock<ICheckout>();
            var _controller = new CatalogController(libraryAssetService.Object, checkoutService.Object);
        }
        [Fact]
        public void CheckOutAction_WhenInvalidIdPassed_ReturnsNotFound()
        {
            var libraryAssetService = new Mock<ILibraryAsset>();
            var checkoutService = new Mock<ICheckout>();
            var _controller = new CatalogController(libraryAssetService.Object, checkoutService.Object);
            var notFoundResult = _controller.CheckOut(0);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }
        [Fact]
        public void CheckOutAction_WhenValidIdPassed_ReturnsLibraryAssetObject()
        {
            //arrange
            var libraryAssetService = new Mock<ILibraryAsset>();
            var checkoutService = new Mock<ICheckout>();
            libraryAssetService.Setup(r => r.GetById(233)).Returns(TestHelper.LibraryAsset());
            var _controller = new CatalogController(libraryAssetService.Object, checkoutService.Object);

            //act
            var checkoutModel = _controller.CheckOut(233);

            //assert
            var viewResult = Assert.IsType<ViewResult>(checkoutModel);
            var model = Assert.IsType<CheckoutModels>(viewResult.ViewData.Model);
            Assert.Equal(233, model.AssetId);
            Assert.Equal("Testing", model.Title);
        }


        [Fact] public void IndexAction_CheckIfReturnsCorrectListofCatalog_ReturnsAllLibraryAsset()
        {
            //arrange
            var libraryAssetService = new Mock<ILibraryAsset>();
            var checkoutService = new Mock<ICheckout>();
            libraryAssetService.Setup(a => a.GetAll()).Returns(TestHelper.LibraryAssets());
            var _controller = new CatalogController(libraryAssetService.Object, checkoutService.Object);

            //act
            var libraryAssetList = _controller.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(libraryAssetList);
            var model = Assert.IsAssignableFrom<AssetIndexModel>(viewResult.ViewData.Model);
            var modelList = model.Assets;
            Assert.Equal(2, modelList.Count());
        }

        [Fact]
        public void DetailAction_CheckIfReturnsCorrectDetailsOfACatalog_ReturnsPropertyOfAnyLibraryAsset()
        {
            //arrange
            var libraryAssetService = new Mock<ILibraryAsset>();
            var checkoutService = new Mock<ICheckout>();
            libraryAssetService.Setup(asset => asset.GetById(233)).Returns(TestHelper.LibraryAsset());
            var _controller = new CatalogController(libraryAssetService.Object, checkoutService.Object);

            //act
            var libraryAsset = _controller.Detail(233);

            //assert
            var viewResult = Assert.IsType<ViewResult>(libraryAsset);
            var model = Assert.IsType<AssetDetailModel>(viewResult.ViewData.Model);
            Assert.Equal(233, model.AssetId);
        }



    }
}
