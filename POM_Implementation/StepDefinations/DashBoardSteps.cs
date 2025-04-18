using OpenQA.Selenium;
using POM_Implementation.ALLPagesCommonControls;
using POM_Implementation.CommonControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll;

namespace POM_Implementation.AllStepDefinations
{

    [Binding]
    public class DashBoardSteps
    {

        IWebDriver _driver;
        LoginPageControls loginPage;
        public DashBoardSteps(IWebDriver driver)
        {
            _driver = driver;
            loginPage = new LoginPageControls(driver);
        }
        

        [Then(@"I verify that i am on Logout Page")]
        public void WhenIVerifyThatIAmOnDDashboard()
        {
            loginPage.VerifyLogout();           
        }

        [Then(@"I Logout from current page")]
        public void ThenICloseCurrentPage()
        {
            loginPage.LogoutDashboard();
        }

    }
}
