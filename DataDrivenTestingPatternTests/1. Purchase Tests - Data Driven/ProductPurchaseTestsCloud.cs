using OpenQA.Selenium.Chrome;
using DecoratorDesignPatternTests.Models;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace DataDrivenTestingPatternTests.FirstVersion;

[TestFixture("chrome", "114.0", "Windows 11")]
[TestFixture("Safari", 16, "macOS Big sur")]
[TestFixture("chrome", "113.0", "Windows 10")]
[TestFixture("firefox", "122.0", "Linux")]
[TestFixture("Safari", 17, "macOS Catalina")]
public class ProductPurchaseTestsCloud
{
    private IWebDriver _driver;
    private WebSite _webSite;
    private readonly string browser;
    private readonly string version;
    private readonly string operatingSystem;

    public ProductPurchaseTestsCloud(string browser, string version, string operatingSystem)
    {
        this.browser = browser;
        this.version = version;
        this.operatingSystem = operatingSystem;
    }

    [SetUp]
    public void TestInit()
    {
        string userName = Environment.GetEnvironmentVariable("LT_USERNAME", EnvironmentVariableTarget.Machine);
        string accessKey = Environment.GetEnvironmentVariable("LT_ACCESSKEY", EnvironmentVariableTarget.Machine);
        dynamic options = default(ChromeOptions);

        switch (browser.ToLower())
        {
            case "chrome":
                options = new ChromeOptions();
                options.BrowserVersion = version;
                break;
            case "firefox":
                options = new FirefoxOptions();
                options.BrowserVersion = version;
                break;
            case "edge":
                options = new EdgeOptions();
                break;
            case "safari":
                options = new SafariOptions();
                break;
        }

        options.AddAdditionalCapability("user", userName, true);
        options.AddAdditionalCapability("accessKey", accessKey, true);

        var timestamp = $"{DateTime.Now:yyyyMMdd.HHmm}";
        options.AddAdditionalCapability("build", timestamp, true);
        options.PlatformName = operatingSystem;

        _driver = new RemoteWebDriver(new Uri($"https://{userName}:{accessKey}@hub.lambdatest.com/wd/hub"), options);
        _driver.Manage().Window.Maximize();

        _driver.Navigate().GoToUrl("https://ecommerce-playground.lambdatest.io/");
        _driver.Manage().Cookies.DeleteAllCookies();

        _webSite = new WebSite(_driver);
    }

    [TearDown]
    public void TestCleanup()
    {
        _driver.Quit();
    }

    [Test]
    public void CorrectInformationDisplayedInCompareScreen_WhenOpenProductFromSearchResults_TwoProducts()
    {
        // Arrange
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$194.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        var expectedProduct2 = new ProductDetails
        {
            Name = "iPod Shuffle",
            Id = 34,
            Price = "$182.00",
            Model = "Product 7",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.CompareLastProduct();
        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct2.Id);
        _webSite.ProductPage.CompareLastProduct();

        _webSite.ProductPage.GoToComparePage();

        _webSite.ProductPage.AssertCompareProductDetails(expectedProduct1, 1);
        _webSite.ProductPage.AssertCompareProductDetails(expectedProduct2, 2);
    }

    [Test]
    public void PurchaseTwoSameProduct()
    {
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$194.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "2"
        };

        var userDetails = new UserDetails
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Telephone = "1234567890",
            Password = "password123",
            ConfirmPassword = "password123",
            AccountType = AccountOption.Register
        };

        var billingAddress = new BillingAddress
        {
            FirstName = "John",
            LastName = "Doe",
            Company = "Acme Corp",
            Address1 = "123 Main St",
            Address2 = "Apt 4",
            City = "Metropolis",
            PostCode = "12345",
            Country = "United States",
            Region = "Alabama"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();
        _webSite.CartPage.AssertTotalPrice("$388.00");

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);
        _webSite.CheckoutPage.AssertTotalPrice("$396.00");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }
}
