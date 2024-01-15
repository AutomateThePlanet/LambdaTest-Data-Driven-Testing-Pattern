using System;
using System.Threading;
using DecoratorDesignPatternTests.Models;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebDriverManager.DriverConfigs.Impl;

namespace DecoratorDesignPattern.FirstVersion;

[TestFixture]
public class ProductPurchaseTests
{
    private IWebDriver _driver;
    //private WebDriverWait _wait;
    private Actions _actions;

    [SetUp]
    public void TestInit()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        _driver = new ChromeDriver();
        _actions = new Actions(_driver);
        //_wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        _driver.Manage().Window.Maximize();

        _driver.Navigate().GoToUrl("https://ecommerce-playground.lambdatest.io/");
        _driver.Manage().Cookies.DeleteAllCookies();
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
            UnitPrice = "$194.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        var expectedProduct2 = new ProductDetails
        {
            Name = "iPod Shuffle",
            Id = 34,
            UnitPrice = "$182.00",
            Model = "Product 7",
            Brand = "Apple",
            Weight = "5.00kg"
        };

        CompareProduct("ip", expectedProduct1.Id);
        CompareProduct("ip", expectedProduct2.Id);

        var compareButton = _driver.FindElement(By.XPath("//a[@aria-label='Compare']"));
        compareButton.Click();

        AssertCompareProductDetails(expectedProduct1, 1);
        AssertCompareProductDetails(expectedProduct2, 2);
    }

    [Test]
    public void PurchaseTwoSameProduct()
    {
        var expectedProduct1 = new ProductDetails
        {
            Name = "iPod Touch",
            Id = 32,
            UnitPrice = "$194.00",
            Model = "Product 5",
            Brand = "Apple",
            Weight = "5.00kg",
            Quantity = "2"
        };

        var searchInput = _driver.FindElement(By.XPath("//input[@name='search']"));
        searchInput.SendKeys("ip");

        Thread.Sleep(1000);
        var autocompleteItemXPath = $"//ul[contains(@class, 'dropdown-menu autocomplete')]/li/div/h4/a[contains(@href, 'product_id={expectedProduct1.Id}')]";
        var autocompleteItem = _driver.FindElement(By.XPath(autocompleteItemXPath));
        //var autocompleteItem = wait.Until(ExpectedConditions.ElementExists(By.XPath(autocompleteItemXPath)));
        autocompleteItem.Click();

        var itemNumber = _driver.FindElements(By.XPath("//input[@name='quantity']")).Last();
        itemNumber.Clear();
        itemNumber.SendKeys(expectedProduct1.Quantity);

        var addToCartButton = _driver.FindElements(By.XPath("//button[@title='Add to Cart']")).Last();
        addToCartButton.Click();

        Thread.Sleep(1000);
        var viewCartButton = _driver.FindElements(By.XPath("//a[normalize-space(.)='View Cart']")).Last();
        viewCartButton.Click();

        var checkoutTotal = _driver.FindElements(By.XPath("//td[text()='Total:']/following-sibling::td/strong")).Last();
        Assert.That(checkoutTotal.Text, Is.EqualTo("$388.00"));

        var checkoutButton = _driver.FindElements(By.XPath("//a[normalize-space(.)='Checkout']")).Last();
        checkoutButton.Click();

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
            Region = "Any State"
        };

        FillUserDetails(userDetails);
        FillAddressForm(billingAddress);

        checkoutTotal = _driver.FindElements(By.XPath("//td[text()='Total:']/following-sibling::td/strong")).Last();
        Assert.That(checkoutTotal.Text, Is.EqualTo("$396.00"));
        
        var termsAgreeButton = _driver.FindElement(By.XPath("//input[@id='input-agree']//following-sibling::label"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", termsAgreeButton);
        termsAgreeButton.Click();
        //_actions.MoveToElement(termsAgreeButton).Click().Perform();

        var continueButton = _driver.FindElement(By.XPath("//button[@id='button-save']"));
        continueButton.Click();
    }

    private void FillUserDetails(UserDetails userDetails)
    {
        if (userDetails.AccountType.Equals(AccountOption.Register))
        {
            _driver.FindElement(By.Id("input-payment-firstname")).SendKeys(userDetails.FirstName);
            _driver.FindElement(By.Id("input-payment-lastname")).SendKeys(userDetails.LastName);
            _driver.FindElement(By.Id("input-payment-email")).SendKeys(userDetails.Email);
            _driver.FindElement(By.Id("input-payment-telephone")).SendKeys(userDetails.Telephone);
            _driver.FindElement(By.Id("input-payment-password")).SendKeys(userDetails.Password);
            _driver.FindElement(By.Id("input-payment-confirm")).SendKeys(userDetails.ConfirmPassword);
        }
    }

    private void FillAddressForm(BillingAddress address)
    {
        _driver.FindElement(By.Id("input-payment-firstname")).SendKeys(address.FirstName);
        _driver.FindElement(By.Id("input-payment-lastname")).SendKeys(address.LastName);
        _driver.FindElement(By.Id("input-payment-company")).SendKeys(address.Company);
        _driver.FindElement(By.Id("input-payment-address-1")).SendKeys(address.Address1);
        _driver.FindElement(By.Id("input-payment-address-2")).SendKeys(address.Address2);
        _driver.FindElement(By.Id("input-payment-city")).SendKeys(address.City);
        _driver.FindElement(By.Id("input-payment-postcode")).SendKeys(address.PostCode);
    }

    private void CompareProduct(string searchText, int productId)
    {
        var searchInput = _driver.FindElement(By.XPath("//input[@name='search']"));
        searchInput.SendKeys(searchText);

        var autocompleteItemXPath = $"//ul[contains(@class, 'dropdown-menu autocomplete')]/li/div/h4/a[contains(@href, 'product_id={productId}')]";
        
        //var autocompleteItem = _wait.Until(ExpectedConditions.ElementExists(By.XPath(autocompleteItemXPath)));
        Thread.Sleep(1000);
        var autocompleteItem = _driver.FindElement(By.XPath(autocompleteItemXPath));
        autocompleteItem.Click();

        var compareButton = _driver.FindElements(By.XPath("//button[@title='Compare this Product']")).Last();
        compareButton.Click();
    }

    private void AssertCompareProductDetails(ProductDetails expectedProductDetails, int productCompareIndex)
    {
        Thread.Sleep(1000);
        //var productName = _wait.Until(ExpectedConditions.ElementExists(By.XPath(GetCompareProductDetailsCellXPath("Product", productCompareIndex))));
        var productName = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Product", productCompareIndex)));
        var productPrice = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Price", productCompareIndex)));
        var productModel = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Model", productCompareIndex)));
        var productBrand = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Brand", productCompareIndex)));
        var productWeight = _driver.FindElement(By.XPath(GetCompareProductDetailsCellXPath("Weight", productCompareIndex)));

        Assert.That(productName.Text, Is.EqualTo(expectedProductDetails.Name));
        Assert.That(productPrice.Text, Is.EqualTo(expectedProductDetails.UnitPrice));
        Assert.That(productModel.Text, Is.EqualTo(expectedProductDetails.Model));
        Assert.That(productBrand.Text, Is.EqualTo(expectedProductDetails.Brand));
        Assert.That(productWeight.Text, Is.EqualTo(expectedProductDetails.Weight));
    }

    private string GetCompareProductDetailsCellXPath(string cellName, int productCompareIndex)
    {
        return $"//table/tbody/tr/td[text()='{cellName}']/following-sibling::td[{productCompareIndex}]";
    }
}
