using System;
using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

using NUnit.Framework;

namespace Sealegs.UITests
{
    public class NewsPage : BasePage
    {
        readonly string NewsName = "NewsName";
        readonly string NewsLevel = "NewsLevel";
        readonly string NewsImage = "NewsImage";
        readonly string LoadingText = "Loading News...";
        readonly Func<string, Query> NewsNamed = name => x => x.Marked("NewsName").Text(name);

        public NewsPage()
            : base(x => x.Class("Toolbar").Descendant().Text("News"), x => x.Class("UINavigationBar").Id("News"))
        {
            app.WaitForNoElement(LoadingText);
        }

        public void SelectNews(string name)
        {
            app.ScrollDownTo(NewsNamed(name));
            app.Screenshot($"Scrolled down to: '{name}'");
            app.Tap(NewsNamed(name));
            app.Screenshot("Tapped News");
        }

        public NewsPage VerifyContentPresent()
        {
            Assert.IsNotNull(app.Query(NewsName).First().Text);
            Assert.IsNotNull(app.Query(NewsLevel).First().Text);

            app.WaitForElement(NewsImage);
            Assert.IsNotEmpty(app.Query(NewsImage));

            return this;
        }
    }
}

