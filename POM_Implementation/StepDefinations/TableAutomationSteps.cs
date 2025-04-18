using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using POM_Implementation.ALLPagesCommonControls;
using System;
using System.Collections.Generic;
using Reqnroll;
using POM_Implementation.AllPagesControls;

namespace POM_Implementation.AllStepDefinations
{
    [Binding]
    public class TableAutomationSteps
    {
        private IWebDriver _driver;
        DynamicTableControls tableControls; 
        public TableAutomationSteps(IWebDriver driver)
        {
            _driver = driver;
            tableControls = new DynamicTableControls(_driver);
        }

       
        
        //public TableSteps()
        //{
        //    driver = new ChromeDriver();
        //}

        [Given(@"I navigate to the table page")]
        public void GivenINavigateToTheTablePage()
        {
            _driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/tables");
        }
        
        [When(@"I extract all company names from the table")]
        public void WhenIExtractAllCompanyNamesFromTheTable()
        {
            tableControls.ExtractCompanyNames();
        }

        [Then(@"I verify if ""(.*)"" exists in the table")]
        public void ThenIVerifyIfExistsInTheTable(string companyName)
        {
            tableControls.CheckCompanyName(companyName);
        }
    }

  
    }

