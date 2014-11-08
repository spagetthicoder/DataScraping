using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace BotUserTesting
{
    public class BotUser : IDisposable
    {
        public const string InternetExplorer = "IE";
        public const string Firefox = "Firefox";
        public const string Chrome = "Chrome";

        private readonly string _baseUrl;
        private readonly IWebDriver _webDriver;

        public IWebDriver WebDriver
        {
            get { return _webDriver; }
        }

        public BotUser(string driverName, string baseUrl, int defaultTimeoutSeconds = 10)
        {
            _baseUrl = baseUrl;
            _webDriver = GetWebDriverByName(driverName);

            var timeout = TimeSpan.FromSeconds(defaultTimeoutSeconds);
            _webDriver.Manage().Timeouts().ImplicitlyWait(timeout);
            _webDriver.Manage().Timeouts().SetPageLoadTimeout(timeout);
            _webDriver.Manage().Timeouts().SetScriptTimeout(timeout);

            _webDriver.Navigate().GoToUrl(_baseUrl);
        }

        private static IWebDriver GetWebDriverByName(string driverName)
        {
            switch (driverName)
            {
                case Chrome:
                    return new ChromeDriver();
                case Firefox:
                    return new FirefoxDriver();
                case InternetExplorer:
                    return new InternetExplorerDriver();
                default:
                    throw new ArgumentOutOfRangeException("Available driver names: " + InternetExplorer + ", " + Chrome +
                                                          ", " + Firefox);
            }
        }

        public void GoTo(string relativeUrl)
        {
            _webDriver.Navigate().GoToUrl(GetAbsoluteUrl(relativeUrl));
        }

        private string GetAbsoluteUrl(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl += "/";
            }
            return _baseUrl + relativeUrl;
        }

        public void Refresh()
        {
            _webDriver.Navigate().Refresh();
        }

        public void RefreshAsCtrlF5()
        {
            var actions = new Actions(_webDriver);
            actions.KeyDown(Keys.Control).SendKeys(Keys.F5);
        }

        public void WaitUntilPageLoad(int timeoutSeconds = 30)
        {
            IWait<IWebDriver> wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(d => d.ExecuteJavaScript<string>("return document.readyState").Equals("complete"));
        }

        public void ReloadPageWhileContains(By findBy, int maxReloadCount)
        {
            for (int i = 0; IsPageContains(findBy) && i < maxReloadCount; i++)
            {
                Refresh();
                WaitUntilPageLoad();
            }
        }

        public void ReloadPageUntilContains(By findBy, int maxReloadCount)
        {
            for (int i = 0; !IsPageContains(findBy) && i < maxReloadCount; i++)
            {
                Refresh();
                WaitUntilPageLoad();
            }
        }

        public bool IsPageContains(By findBy)
        {
            var elements = _webDriver.FindElements(findBy);
            return elements.Any();
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}
