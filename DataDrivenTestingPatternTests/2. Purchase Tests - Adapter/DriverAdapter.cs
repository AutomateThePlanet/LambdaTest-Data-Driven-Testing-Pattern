using System.Collections.ObjectModel;
using System.Xml.Linq;
using AngleSharp.Dom;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace DataDrivenTestingPatternTests.SecondVersion;

public class DriverAdapter : IDriver
{
    private IWebDriver _webDriver;
    private WebDriverWait _webDriverWait;

    public string Url => _webDriver.Url;

    public void Start(Browser browser)
    {
        switch (browser)
        {
            case Browser.Chrome:
                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                _webDriver = new ChromeDriver();
                break;
            case Browser.Firefox:
                new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                _webDriver = new FirefoxDriver();
                break;
            case Browser.Edge:
                new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                _webDriver = new EdgeDriver();
                break;
            case Browser.Safari:
                _webDriver = new SafariDriver();
                break;
            case Browser.InternetExplorer:
                new WebDriverManager.DriverManager().SetUpDriver(new InternetExplorerConfig());
                _webDriver = new InternetExplorerDriver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(browser), browser, null);
        }

        _webDriver.Manage().Window.Maximize();
        _webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(30));
    }

    public void Quit()
    {
        _webDriver.Quit();
    }

    public void GoToUrl(string url)
    {
        _webDriver.Navigate().GoToUrl(url);
    }

    public IComponent FindComponent(By locator)
    {
        IWebElement nativeWebElement = 
            _webDriverWait.Until(ExpectedConditions.ElementExists(locator));
        IComponent element = new ComponentAdapter(_webDriver, nativeWebElement, locator);

        ScrollIntoView(element);
        ////HighlightElement(element);
        return element;
    }

    public List<IComponent> FindComponents(By locator)
    {
        ReadOnlyCollection<IWebElement> nativeWebElements = 
            _webDriverWait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
        var elements = new List<IComponent>();
        foreach (var nativeWebElement in nativeWebElements)
        {
            IComponent element = new ComponentAdapter(_webDriver, nativeWebElement, locator);
            elements.Add(element);
        }

        //if (elements.Any())
        //{
        //    ScrollIntoView(elements.Last());
        //    HighlightElement(elements.Last());
        //}

        return elements;
    }

    public void Refresh()
    {
        _webDriver.Navigate().Refresh();
    }

    public bool ComponentExists(IComponent component)
    {
        try
        {
            _webDriver.FindElement(component.By);

            return true;
        }
        catch
        {
            // The component was not found
            return false;
        }
    }

    public void DeleteAllCookies()
    {
        _webDriver.Manage().Cookies.DeleteAllCookies();
    }

    public void ExecuteScript(string script, params object[] args)
    {
        ((IJavaScriptExecutor)_webDriver).ExecuteScript(script, args);
    }

    public void WaitForAjax()
    {
        _webDriverWait.Until(driver =>
        {
            var script = "return window.jQuery != undefined && jQuery.active == 0";
            return (bool)((IJavaScriptExecutor)driver).ExecuteScript(script);
        });
    }

    private void ScrollIntoView(IComponent element)
    {
        ExecuteScript("arguments[0].scrollIntoView(true);", element.WrappedElement);
    }

    private void HighlightElement(IComponent element)
    {
        try
        {
            // Get original styles
            var originalElementBackground = element.WrappedElement.GetCssValue("background");
            var originalElementColor = element.WrappedElement.GetCssValue("color");
            var originalElementOutline = element.WrappedElement.GetCssValue("outline");

            // JavaScript to modify and then revert the element's style
            var script = @"
                let defaultBG = arguments[0].style.backgroundColor;
                let defaultOutline = arguments[0].style.outline;
                arguments[0].style.backgroundColor = '#FDFF47';
                arguments[0].style.outline = '#f00 solid 2px';

                setTimeout(function()
                {
                    arguments[0].style.backgroundColor = defaultBG;
                    arguments[0].style.outline = defaultOutline;
                }, 500);";

            ExecuteScript(script, element.WrappedElement);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
