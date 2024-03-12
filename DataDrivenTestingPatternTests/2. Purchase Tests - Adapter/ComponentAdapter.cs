using System.Threading;

namespace DataDrivenTestingPatternTests.SecondVersion;

public class ComponentAdapter : IComponent
{
    private readonly IWebDriver _webDriver;
    private readonly Actions _actions;
    private readonly IWebElement _webElement;
    private readonly By _by;

    public ComponentAdapter(IWebDriver webDriver, IWebElement webElement, By by)
    {
        _webDriver = webDriver;
        _actions = new Actions(_webDriver);
        _webElement = webElement;
        _by = by;
        WrappedElement = webElement;
    }

    public By By => _by;

    public string Text => _webElement?.Text;

    public bool? Enabled => _webElement?.Enabled;

    public bool? Displayed => _webElement?.Displayed;

    public IWebElement WrappedElement { get; }

    public void Click(bool waitToBeClickable = false)
    {
        if (waitToBeClickable)
        {
           WaitToBeClickable(_by);
        }

        _webElement?.Click();
    }

    public string GetAttribute(string attributeName)
    {
        return _webElement?.GetAttribute(attributeName);
    }

    public void TypeText(string text)
    {
        _webElement?.Clear();
        _webElement?.SendKeys(text);
    }

    public void Hover()
    {
        _actions.MoveToElement(_webElement).Perform();
    }

    private void WaitToBeClickable(By by)
    {
        var webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(30));
        webDriverWait.Until(ExpectedConditions.ElementToBeClickable(by));
    }

    public IComponent FindComponent(By locator)
    {
        var element = _webElement.FindElement(locator);
        return new ComponentAdapter(_webDriver, element, locator);
    }

    public List<IComponent> FindComponents(By locator)
    {
        var elements = _webElement.FindElements(locator);
        var components = new List<IComponent>();
        foreach (var element in elements)
        {
            components.Add(new ComponentAdapter(_webDriver, element, locator));
        }
        return components;
    }
}
