using OpenQA.Selenium;
using POM_Implementation.ALLPagesCommonControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace POM_Implementation.CommonControls
{
    public class LoginPageControls
    {
        IWebDriver _driver;
        public LoginPageControls(IWebDriver driver) => _driver = driver;

        IWebElement txtUserName => _driver.FindElement(By.Id("username"));
        IWebElement txtPassword => _driver.FindElement(By.Id("password"));
        IWebElement btnLogin => _driver.FindElement(By.XPath("//*[@id='login']/button"));
        IWebElement logoutbtn => _driver.FindElement(By.XPath("//*[@id=\"content\"]/div/a"));

        public void EnterUserAndPassword(string User, string Password)
        {
            txtUserName.SendKeys(User);
            txtPassword.SendKeys(Password);
        }

        public void ClickSubmitButton()
        {
            btnLogin.Click();            
        }

        public void LogoutDashboard()
        {
            logoutbtn.Click();
        }
        public void VerifyLogout()
        {
            AllPagesCommonControls allPagesCommon = new AllPagesCommonControls(_driver);
            allPagesCommon.WaitUntilPageLoad(180);            
            allPagesCommon.WaitUntilPageLoad(180);
            try
            {
                var btnlogout = _driver.FindElement(By.XPath("//*[@id=\"content\"]/div/a"));
                Console.WriteLine("Element Exist");
            }
            catch (NoSuchElementException ex)
            {
                allPagesCommon.TakeScreenshot("Login Error");
                Assert.Fail("Login Failed");
            }            
        }
    }
}
