using OpenQA.Selenium;
using POM_Implementation.ALLPagesCommonControls;
using POM_Implementation.CommonControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll;

namespace POM_Implementation.AllStepDefinations
{

    [Binding]
    public class LoginPageSteps
    {
        IWebDriver _driver;
        LoginPageControls loginPage;
        public LoginPageSteps(IWebDriver driver)
        {
            _driver = driver;
            loginPage = new LoginPageControls(_driver);
        }
        public static string Environment = "";

        [Given(@"I am on ""(.*)"" environment")]
        public void I_Am_On_p0_Environment(string Env)
        {
            Environment = Env;
        }



        [Given(@"I have opened herokuapp page")]
        public void GivenIHaveOpenedLoginPage()
        {
            try
            {
                AllPagesCommonControls allPagesCommonControls = new AllPagesCommonControls(_driver);
                allPagesCommonControls.NavigateToPage(Hooks.Settings.Select("Environment = '" + Environment + "'")[0]["LoginURL"].ToString());                                               
                allPagesCommonControls.WaitUntilPageLoad(180);
            }
            catch (Exception e)
            {
                Assert.Warn(e.Message);
            }

        }

        [Given(@"I am logged in to ""(.*)"" with ""(.*)"" account")]
        public void GivenIAmLoggedInTo_P0_With_P1_Account(string Application, string User)
        {
            try
            {
                AllPagesCommonControls commoncontrols = new AllPagesCommonControls(_driver);                
                var row = Hooks.Settings.AsEnumerable().FirstOrDefault(r => r.Field<string>("Environment").Trim() == Environment.Trim() && r.Field<string>("UserName").Trim() == User.Trim());
                string Password = row["Password"].ToString();
                loginPage.EnterUserAndPassword(User, Password.ToString());
                loginPage.ClickSubmitButton();
                commoncontrols.WaitUntilPageLoad(180);
            }
            catch (Exception ex)
            {
                Assert.Warn(ex.Message);                
            }
        }

        [Then(@"I Close Current Page")]

        public void ThenICloseCurrentPage()
        {
            AllPagesCommonControls allPagesCommon = new AllPagesCommonControls(_driver);
            //allPagesCommon.CloseCurrentPage();
        }
    }
}
