﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using PieShop.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieShopTest.TagHelpers
{
    public class EmailTagHelperTest
    {
        [Fact]
        public void Generate_Email_Link()
        {
            //Arrange
            EmailTagHelper emailTagHelper = new EmailTagHelper()
            {
                Address = "davidKings1604@gmail.com",
                Content = "Email"
            };
            var tagHelperContext = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(), string.Empty);

            var content = new Mock<TagHelperContent>();

            var tagHelperOutput = new TagHelperOutput("a",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));

            //Act
            emailTagHelper.Process(tagHelperContext, tagHelperOutput);

            //Assert
            Assert.Equal("Email", tagHelperOutput.Content.GetContent());
            Assert.Equal("a", tagHelperOutput.TagName);
            Assert.Equal("mailto:davidKings1604@gmail.com", tagHelperOutput.Attributes[0].Value);
        }

        
    }
}
