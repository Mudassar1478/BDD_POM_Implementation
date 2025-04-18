using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll;

namespace POM_Implementation.AllStepDefinations
{
    [Binding]
    public class JavaScriptAlertsSteps
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public JavaScriptAlertsSteps(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

      

        [When(@"I click on '(.*)' alert")]
        public void WhenIClickOnAlert(string alertType)
        {
            By alertLocator = alertType switch
            {
                "JS Alert" => By.XPath("//*[@id='content']/div/ul/li[1]/button"),
                "JS Confirm" => By.XPath("//*[@id='content']/div/ul/li[2]/button"),
                "JS Prompt" => By.XPath("//*[@id='content']/div/ul/li[3]/button"),
                _ => throw new ArgumentException("Invalid alert type"),
            };

            _driver.FindElement(alertLocator).Click();
        }

        [Then(@"I handle the '(.*)' alert with '(.*)'")]
        public void ThenIHandleTheAlertWith(string alertType, string action)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IAlert alert = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());

            switch (alertType)
            {
                case "JS Alert":
                    Assert.That("I am a JS Alert", Is.EqualTo(alert.Text));
                    alert.Accept();
                    break;

                case "JS Confirm":
                    Assert.That("I am a JS Confirm", Is.EqualTo(alert.Text));
                    if (action == "accept") alert.Accept();
                    else if (action == "dismiss") alert.Dismiss();
                    else throw new ArgumentException("Invalid action for JS Confirm");
                    break;

                case "JS Prompt":
                    Assert.That("I am a JS prompt", Is.EqualTo(alert.Text));
                    if (!string.IsNullOrEmpty(action)) alert.SendKeys(action);
                    alert.Accept();
                    break;

                default:
                    throw new ArgumentException("Invalid alert type");
            }
        }

        [Then(@"I should see the message '(.*)'")]
        public void ThenIShouldSeeTheMessage(string expectedMessage)
        {
            string actualMessage = _driver.FindElement(By.Id("result")).Text;
            Assert.That(expectedMessage, Is.EqualTo(actualMessage));
        }
    }
}
