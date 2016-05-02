using MegansBlog.Models;
using System;
using Xunit;

namespace MegansBlog.Tests
{
    public class CategoryTest
    {

        [Fact]
        public void GetCategoryTest()
        {
            //Arrange
            var category = new Category();

            //Act
            category.Name = "Name";

            //Assert
            Assert.Equal("Name", category.Name);
        }
    }
}
