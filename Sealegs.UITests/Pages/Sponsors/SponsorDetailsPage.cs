using System;
using System.Linq;

using NUnit.Framework;

namespace Sealegs.UITests
{
    public class NewsDetailsPage : BasePage
    {
        readonly string NewsImage = "NewsDetailImage";
        readonly string NewsName = "NewsDetailName";
        readonly string NewsLevel = "NewsDetailLevel";
        readonly string NewsDescription = "NewsDetailDescription";
        readonly string NewsLinks = "NewsDetailLinks";

        public NewsDetailsPage()
            : base(x => x.Class("Toolbar").Descendant().Text("News Details"), x => x.Class("UINavigationBar").Id("News Details"))
        {
        }

        public NewsDetailsPage VerifyContentPresent()
        {
            app.WaitForElement(NewsImage);
            Assert.IsNotEmpty(app.Query(NewsImage));
            Assert.IsNotNull(app.Query(NewsName).First().Text);
            Assert.IsNotNull(app.Query(NewsLevel).First().Text);
            Assert.IsNotNull(app.Query(NewsDescription).First().Text);
            app.ScrollDownTo(NewsLinks);
            Assert.IsNotEmpty(app.Query(NewsLinks));

            return this;
        }
    }
}

