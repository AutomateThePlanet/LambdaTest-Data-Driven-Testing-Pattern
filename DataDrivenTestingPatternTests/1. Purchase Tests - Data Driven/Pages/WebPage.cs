﻿using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace DataDrivenTestingPatternTests.FirstVersion;
public abstract class WebPage
{
    protected readonly IWebDriver _driver;
    protected readonly WebDriverWait _wait;
    protected readonly Actions _actions;

    public WebPage(IWebDriver driver, WebDriverWait wait, Actions actions)
    {
        _driver = driver;
        _wait = wait;
        _actions = actions;
    }
}
