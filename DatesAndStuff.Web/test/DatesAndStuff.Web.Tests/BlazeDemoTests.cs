using System;
using System.Collections.ObjectModel;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver;
    private StringBuilder verificationErrors;
    private const string BaseURL = "https://blazedemo.com/";

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
        verificationErrors = new StringBuilder();
    }

    [TearDown]
    public void TeardownTest()
    {
        try
        {
            driver.Quit();
            driver.Dispose();
        }
        catch (Exception)
        {
            // Ignore errors if unable to close the browser
        }
        Assert.That(verificationErrors.ToString(), Is.EqualTo(""));
    }

    [Test]
    public void BlazeDemo_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        var fromPortDropdown = wait.Until(ExpectedConditions.ElementExists(By.Name("fromPort")));
        var fromPortSelect = new SelectElement(fromPortDropdown);
        fromPortSelect.SelectByText("Mexico City");

        var toPortDropdown = wait.Until(ExpectedConditions.ElementExists(By.Name("toPort")));
        var toPortSelect = new SelectElement(toPortDropdown);
        toPortSelect.SelectByText("Dublin");

        // Act
        var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@type='submit']")));
        submitButton.Click();

        // Assert
        wait.Until(ExpectedConditions.ElementExists(By.TagName("table")));

        ReadOnlyCollection<IWebElement> flightRows = driver.FindElements(By.XPath("//table/tbody/tr"));

        flightRows.Count.Should().BeGreaterThanOrEqualTo(3);
    }
}