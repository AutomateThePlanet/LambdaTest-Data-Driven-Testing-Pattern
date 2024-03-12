namespace DataDrivenTestingPatternTests.SecondVersion;
public class WebSite
{
    private readonly IDriver _driver;

    public WebSite(IDriver driver)
    {
        _driver = driver;

        HomePage = new HomePage(_driver);
        ProductPage = new ProductPage(_driver);
        CartPage = new CartPage(_driver);
        CheckoutPage = new CheckoutPage(_driver);
    }

    public HomePage HomePage { get; private set; }
    public ProductPage ProductPage { get; private set; }
    public CartPage CartPage { get; private set; }
    public CheckoutPage CheckoutPage { get; private set; }
}
