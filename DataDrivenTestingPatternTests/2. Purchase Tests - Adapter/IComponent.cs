namespace DataDrivenTestingPatternTests.SecondVersion;

public interface IComponent 
{
    public By By { get; }
    public IWebElement WrappedElement { get; }
    public string Text { get; }
    public bool? Enabled { get; }
    public bool? Displayed { get; }
    public void TypeText(string text);
    public void Click(bool waitToBeClickable = false);
    public string GetAttribute(string attributeName);
    public void Hover();
    public IComponent FindComponent(By locator);
    public List<IComponent> FindComponents(By locator);
}
