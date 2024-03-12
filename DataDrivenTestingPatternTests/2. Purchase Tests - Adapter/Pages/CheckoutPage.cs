using System.Threading;
using DecoratorDesignPatternTests.Models;

namespace DataDrivenTestingPatternTests.SecondVersion;
public class CheckoutPage : WebPage
{
    public CheckoutPage(IDriver driver) 
        : base(driver)
    {
    }

    public IComponent FirstNameInput => Driver.FindComponent(By.Id("input-payment-firstname"));
    public IComponent LastNameInput => Driver.FindComponent(By.Id("input-payment-lastname"));
    public IComponent EmailInput => Driver.FindComponent(By.Id("input-payment-email"));
    public IComponent TelephoneInput => Driver.FindComponent(By.Id("input-payment-telephone"));
    public IComponent PasswordInput => Driver.FindComponent(By.Id("input-payment-password"));
    public IComponent ConfirmPasswordInput => Driver.FindComponent(By.Id("input-payment-confirm"));
    public IComponent CompanyInput => Driver.FindComponent(By.Id("input-payment-company"));
    public IComponent Address1Input => Driver.FindComponent(By.Id("input-payment-address-1"));
    public IComponent Address2Input => Driver.FindComponent(By.Id("input-payment-address-2"));
    public IComponent CityInput => Driver.FindComponent(By.Id("input-payment-city"));
    public IComponent PostCodeInput => Driver.FindComponent(By.Id("input-payment-postcode"));
    public IComponent ShippingAddressCountrySelect => Driver.FindComponent(By.Id("input-payment-country"));
    public IComponent ShippingAddressCountryOption(string country) =>
        ShippingAddressCountrySelect.FindComponent(By.XPath($".//option[contains(text(), '{country}')]"));
    public IComponent BillingAddressRegionSelect => Driver.FindComponent(By.Id("input-payment-zone"));
    public IComponent BillingAddressRegionOption(string region) =>
        BillingAddressRegionSelect.FindComponent(By.XPath($".//option[contains(text(), '{region}')]"));
    public IComponent TermsAgreeCheckbox => Driver.FindComponent(By.XPath("//input[@id='input-agree']//following-sibling::label"));
    public IComponent ContinueButton => Driver.FindComponent(By.XPath("//button[@id='button-save']"));
    public IComponent TotalPrice => Driver.FindComponents(By.XPath("//td[text()='Total:']/following-sibling::td/strong")).Last();
    public IComponent VatPercentage => Driver.FindComponent(By.XPath("//*[@id='checkout-total']/tbody/tr[4]/td"));
    public IComponent VatTax => Driver.FindComponents(By.XPath("//*[contains(text(), 'VAT ')]/following-sibling::td/strong")).Last();
    
    public void FillUserDetails(UserDetails userDetails)
    {
        FirstNameInput.TypeText(userDetails.FirstName);
        LastNameInput.TypeText(userDetails.LastName);
        EmailInput.TypeText(userDetails.Email);
        TelephoneInput.TypeText(userDetails.Telephone);
        PasswordInput.TypeText(userDetails.Password);
        ConfirmPasswordInput.TypeText(userDetails.ConfirmPassword);
    }

    public void FillBillingAddress(BillingAddress billingAddress)
    {
        CompanyInput.TypeText(billingAddress.Company);
        Address1Input.TypeText(billingAddress.Address1);
        Address2Input.TypeText(billingAddress.Address2);
        CityInput.TypeText(billingAddress.City);
        PostCodeInput.TypeText(billingAddress.PostCode);
        ShippingAddressCountrySelect.Click();
        ShippingAddressCountryOption(billingAddress.Country).Click();
        //Thread.Sleep(1000);
        Driver.WaitForAjax();
        BillingAddressRegionSelect.Click();
        BillingAddressRegionOption(billingAddress.Region).Click();
    }

    public void AgreeToTerms()
    {
        Driver.WaitForAjax();
        // TODO: Move to Driver as addition to FindComponent as decoratr
        // TODO: Add addtional decorator for highlighting element
        //((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", TermsAgreeCheckbox);
        TermsAgreeCheckbox.Click(true);
    }

    public void ClickContinue()
    {
        ContinueButton.Click();
    }

    public void AssertTotalPrice(string expectedPrice)
    {
        Assert.That(TotalPrice.Text, Is.EqualTo(expectedPrice));
    }

    public void CompleteCheckout()
    {
        var continueButton = Driver.FindComponent(By.XPath("//button[@id='button-save']"));
        continueButton.Click();
    }
}
