using MegansBlog.Models;
using System;
using Xunit;

namespace MegansBlog.Tests
{
    public class PostTest
    {
        [Fact]
        public void GetPostTest()
        {
            //Arrange
            var post = new Post();
            var currentTime = DateTime.Now;


            //Act
            post.Title = "Title";
            post.Body = "Body";
            post.PostDate = currentTime;


            //Assert
            Assert.Equal("Title", post.Title);
            Assert.Equal("Body", post.Body);
            Assert.Equal(currentTime, post.PostDate);
        }
    }
}