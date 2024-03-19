using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using DecoratorDesignPatternTests.Models;
using DataDrivenTestingPatternTests.ZeroVersion;

namespace DataDrivenTestingPatternTests.ZeroVersion;

[TestFixture]
public class ProductPurchaseTests
{
    private IWebDriver _driver;
    private WebSite _webSite;

    [SetUp]
    public void TestInit()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        _driver = new ChromeDriver();
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
            Region = "Texas"
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


    //[TestCase("1", "Austria", 20.0, "$33.00", "$204.00")]
    //[TestCase("9", "Austria", 20.0, "$289.00", "$1740.00")]
    //[TestCase("1", "Belgium", 21.0, "$34.65", "$205.65")]
    //[TestCase("9", "Belgium", 21.0, "$303.45", "$1754.45")]
    //[TestCase("1", "Bulgaria", 20.0, "$33.00", "$204.00")]
    //[TestCase("9", "Bulgaria", 20.0, "$289.00", "$1740.00")]
    //[TestCase("1", "Croatia", 25.0, "$41.25", "$212.25")]
    //[TestCase("9", "Croatia", 25.0, "$361.25", "$1812.25")]
    //[TestCase("1", "Cyprus", 19.0, "$31.35", "$202.35")]
    //[TestCase("9", "Cyprus", 19.0, "$274.55", "$1725.55")]
    //[TestCase("1", "Czech Republic", 21.0, "$34.65", "$205.65")]
    //[TestCase("9", "Czech Republic", 21.0, "$303.45", "$1754.45")]
    //[TestCase("1", "Denmark", 25.0, "$41.25", "$212.25")]
    //[TestCase("9", "Denmark", 25.0, "$361.25", "$1812.25")]
    //[TestCase("1", "Estonia", 20.0, "$33.00", "$204.00")]
    //[TestCase("9", "Estonia", 20.0, "$289.00", "$1740.00")]
    //[TestCase("1", "Finland", 24.0, "$39.60", "$210.60")]
    //[TestCase("9", "Finland", 24.0, "$346.80", "$1797.80")]
    //[TestCase("1", "France", 20.0, "$33.00", "$204.00")]
    //[TestCase("9", "France", 20.0, "$289.00", "$1740.00")]
    //[TestCase("1", "Germany", 19.0, "$31.35", "$202.35")]
    //[TestCase("9", "Germany", 19.0, "$274.55", "$1725.55")]
    //[TestCase("1", "Greece", 24.0, "$39.60", "$210.60")]
    //[TestCase("9", "Greece", 24.0, "$346.80", "$1797.80")]
    //[TestCase("1", "Hungary", 27.0, "$44.55", "$215.55")]
    //[TestCase("9", "Hungary", 27.0, "$390.15", "$1841.15")]
    //[TestCase("1", "Ireland", 23.0, "$37.95", "$208.95")]
    //[TestCase("9", "Ireland", 23.0, "$332.35", "$1783.35")]
    //[TestCase("1", "Italy", 22.0, "$36.30", "$207.30")]
    //[TestCase("9", "Italy", 22.0, "$317.90", "$1768.90")]
    //[TestCase("1", "Latvia", 21.0, "$34.65", "$205.65")]
    //[TestCase("9", "Latvia", 21.0, "$303.45", "$1754.45")]
    //[TestCase("1", "Lithuania", 21.0, "$34.65", "$205.65")]
    //[TestCase("9", "Lithuania", 21.0, "$303.45", "$1754.45")]
    //[TestCase("1", "Luxembourg", 17.0, "$28.05", "$199.05")]
    //[TestCase("9", "Luxembourg", 17.0, "$245.65", "$1696.65")]
    //[TestCase("1", "Malta", 18.0, "$29.70", "$200.70")]
    //[TestCase("9", "Malta", 18.0, "$260.10", "$1711.10")]
    //[TestCase("1", "Netherlands", 21.0, "$34.65", "$205.65")]
    //[TestCase("9", "Netherlands", 21.0, "$303.45", "$1754.45")]
    //[TestCase("1", "Poland", 23.0, "$37.95", "$208.95")]
    //[TestCase("9", "Poland", 23.0, "$332.35", "$1783.35")]
    //[TestCase("1", "Portugal", 23.0, "$37.95", "$208.95")]
    //[TestCase("9", "Portugal", 23.0, "$332.35", "$1783.35")]
    //[TestCase("1", "Romania", 19.0, "$31.35", "$202.35")]
    //[TestCase("9", "Romania", 19.0, "$274.55", "$1725.55")]
    //[TestCase("1", "Slovakia", 20.0, "$33.00", "$204.00")]
    //[TestCase("9", "Slovakia", 20.0, "$289.00", "$1740.00")]
    //[TestCase("1", "Slovenia", 22.0, "$36.30", "$207.30")]
    //[TestCase("9", "Slovenia", 22.0, "$317.90", "$1768.90")]
    //[TestCase("1", "Spain", 21.0, "$34.65", "$205.65")]
    //[TestCase("9", "Spain", 21.0, "$303.45", "$1754.45")]
    //[TestCase("1", "Sweden", 25.0, "$41.25", "$212.25")]
    //[TestCase("9", "Sweden", 25.0, "$361.25", "$1812.25")]
    [Test]
    public void CorrectVatDisplayed_When_CroatiaPickedInBilling_ForQuantity9()
    {
        // Flat Shipping Rate:	$5.00 (added to subtotal)
        // Eco Tax(-2.00):	$6.00
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$160.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "1"
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
            Country = "Belgium",
            Region = "NA"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);

        _webSite.CheckoutPage.AssertVatPercentage(25);
        _webSite.CheckoutPage.AssertVatTax("$361.25");

        _webSite.CheckoutPage.AssertTotalPrice("$1812.25");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }

    [Test]
    public void CorrectVatDisplayed_When_CroatiaPickedInBilling_ForQuantity1()
    {
        // Flat Shipping Rate:	$5.00 (added to subtotal)
        // Eco Tax(-2.00):	$6.00
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$160.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "1"
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
            Country = "Belgium",
            Region = "NA"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);

        _webSite.CheckoutPage.AssertVatPercentage(25);
        _webSite.CheckoutPage.AssertVatTax("$41.25");

        _webSite.CheckoutPage.AssertTotalPrice("$212.25");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }

    [Test]
    public void CorrectVatDisplayed_When_BelgiumPickedInBilling_ForQuantity9()
    {
        // Flat Shipping Rate:	$5.00 (added to subtotal)
        // Eco Tax(-2.00):	$6.00
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$160.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "1"
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
            Country = "Belgium",
            Region = "NA"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);

        _webSite.CheckoutPage.AssertVatPercentage(21);
        _webSite.CheckoutPage.AssertVatTax("$303.45");

        _webSite.CheckoutPage.AssertTotalPrice("$1754.45");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }

    [Test]
    public void CorrectVatDisplayed_When_BelgiumPickedInBilling_ForQuantity1()
    {
        // Flat Shipping Rate:	$5.00 (added to subtotal)
        // Eco Tax(-2.00):	$6.00
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$160.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "1"
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
            Country = "Belgium",
            Region = "NA"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);

        _webSite.CheckoutPage.AssertVatPercentage(21);
        _webSite.CheckoutPage.AssertVatTax("$34.65");

        _webSite.CheckoutPage.AssertTotalPrice("$1740.00");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }

    [Test]
    public void CorrectVatDisplayed_When_AustriaPickedInBilling_ForQuantity9()
    {
        // Flat Shipping Rate:	$5.00 (added to subtotal)
        // Eco Tax(-2.00):	$6.00
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$160.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "1"
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
            Country = "Austria",
            Region = "NA"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);

        _webSite.CheckoutPage.AssertVatPercentage(20);
        _webSite.CheckoutPage.AssertVatTax("$289.00");

        _webSite.CheckoutPage.AssertTotalPrice("$1740.00");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }

    [Test]
    public void CorrectVatDisplayed_When_AustriaPickedInBilling_ForQuantity1()
    {
        // Flat Shipping Rate:	$5.00 (added to subtotal)
        // Eco Tax(-2.00):	$6.00
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            Price = "$160.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "1"
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
            Country = "Austria",
            Region = "NA"
        };

        _webSite.HomePage.SearchProduct("ip");
        _webSite.ProductPage.SelectProductFromAutocomplete(expectedProduct1.Id);
        _webSite.ProductPage.AddToCart(expectedProduct1.Quantity);
        _webSite.CartPage.ViewCart();

        _webSite.CartPage.Checkout();
        _webSite.CheckoutPage.FillUserDetails(userDetails);
        _webSite.CheckoutPage.FillBillingAddress(billingAddress);

        _webSite.CheckoutPage.AssertVatPercentage(20);
        _webSite.CheckoutPage.AssertVatTax("$33.00");

        _webSite.CheckoutPage.AssertTotalPrice("$204.00");

        _webSite.CheckoutPage.AgreeToTerms();
        _webSite.CheckoutPage.CompleteCheckout();
    }
}
