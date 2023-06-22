using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System.Globalization;

namespace AppiumVivinoAppTests
{
    public class VivinoTests
    {
        private const string UriString = "http://127.0.0.1:4723/wd/hub";
        private const string VivinoAppLocation = @"C:\projects\vivino_8.18.11-8181203.apk";
        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void PrepareApp()
        {
            this.options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", VivinoAppLocation);
            options.AddAdditionalCapability("appPackage", "vivino.web.app");
            options.AddAdditionalCapability("appActivity", "com.sphinx_solution.activities.SplashActivity");
            this.driver = new AndroidDriver<AndroidElement>(new Uri(UriString), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(200);
        }
        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }
        [Test]
        public void Test_SearchWine_VerifyNameAndRating()
        {
            var linkAccount = driver.FindElementById("vivino.web.app:id/txthaveaccount");
            linkAccount.Click();

            var inputUsername = driver.FindElementById("vivino.web.app:id/edtEmail");
            inputUsername.SendKeys("Elena@yahoo.com");

            var inputPass = driver.FindElementById("vivino.web.app:id/edtPassword");
            inputPass.SendKeys("Elena123");

            var linkLogin = driver.FindElementById("vivino.web.app:id/action_signin");
            linkLogin.Click();

            var buttomSearch = driver.FindElementById("vivino.web.app:id/wine_explorer_tab");
            buttomSearch.Click();

            var inputSearchButton = driver.FindElementById("vivino.web.app:id/search_header_text");
            inputSearchButton.Click();

            var inputSearchField = driver.FindElementById("vivino.web.app:id/editText_input");
            inputSearchField.SendKeys("Katarzyna Reserve Red 2006");

            var listWineResultElement = driver.FindElementById("vivino.web.app:id/winename_textView");
            listWineResultElement.Click();

            var labelWineName = driver.FindElementById("vivino.web.app:id/wine_name");

            var labelRatingText = driver.FindElementById("vivino.web.app:id/rating").Text;
            var rating = double.Parse(labelRatingText, CultureInfo.InvariantCulture);

            var labelHighlights = driver.FindElementById("vivino.web.app:id/highlight_description");
         //   ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", labelHighlights);

            // driver.FindElementByAndroidUIAutomator("new UiScrollable(new UiSelector().scrollable(true).instance(0)).scrollIntoView(new UiSelector().textContains("Highlights").instance(0))");

            //  var labelHighlights = driver.FindElementById("vivino.web.app:id/highlight_item");
            // labelHighlights.Click();

            Assert.That(labelWineName.Text, Is.EqualTo("Reserve Red 2006"));
            Assert.That(rating>=1.00 && rating<=5.00);

           Assert.That(labelHighlights.Text, Is.EqualTo("Among top 1% of all wines in the world"));

            
         var sectionSummary = driver.FindElementById("vivino.web.app:id/tabs");
         var linkFacts = sectionSummary.FindElementByXPath("//android.widget.TextView[2]");
            linkFacts.Click();

         var labelFactTitle = driver.FindElementById("vivino.web.app:id/wine_fact_title");

            Assert.That(labelFactTitle.Text, Is.EqualTo("Grapes"));

            var labelFactText = driver.FindElementById("vivino.web.app:id/wine_fact_text");
            Assert.That(labelFactText.Text, Is.EqualTo("Cabernet Sauvignon,Merlot"));
        }
    }
}