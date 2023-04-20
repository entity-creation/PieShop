using Microsoft.AspNetCore.Mvc;
using PieShop.Controllers;
using PieShop.ViewModels;
using PieShopTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShopTest.Controllers
{
    public class PieControllerTest
    {
        [Fact]
        public void List_EmptyCategory_ReturnsAllPies()
        {
            //Arrange
            var mockPieRepository = RepositoryMocks.GetPieRepository();
            var mockCategoryRepository = RepositoryMocks.GetCategoryRepository();

            var PieController = new PieController(mockPieRepository.Object, mockCategoryRepository.Object);

            //Act
            var result = PieController.List("");

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var pieListViewModel = Assert.IsAssignableFrom<PieListViewModel>(viewResult.ViewData.Model);
            Assert.Equal(10, pieListViewModel.Pies.Count());
        }
    }
}
