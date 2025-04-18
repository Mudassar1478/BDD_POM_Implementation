using OpenQA.Selenium;
using POM_Implementation.ALLPagesCommonControls;
using POM_Implementation.AllStepDefinations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM_Implementation.AllPagesControls
{
    public class DynamicTableControls
    {
        IWebDriver _driver;
        public DynamicTableControls(IWebDriver driver) => _driver = driver;

        public static List<string> companyNames;
        public void ExtractCompanyNames()
        {
            AllPagesCommonControls ALLPagesCommonControls = new AllPagesCommonControls(_driver);
            var TableData = ALLPagesCommonControls.GetTableData("table1");
            int firstNameID = ALLPagesCommonControls.GetColumnID("table1", "First Name");
            int lastNameID = ALLPagesCommonControls.GetColumnID("table1", "Last Name");
            companyNames = new List<string>();
            foreach (var row in TableData)
            {
                string fName = row.FindElements(By.TagName("td"))[firstNameID].Text;
                string lName = row.FindElements(By.TagName("td"))[lastNameID].Text;
                companyNames.Add(fName + " " + lName);
            }
            string[,] companyNamess = new string[companyNames.Count, 1];
            int i = 0;
            foreach (var company in companyNames)
            {
                companyNamess[i, 0] = company;
                i++;
            }
            ALLPagesCommonControls.SetExtentReportSummary("Table", companyNamess);
        }
        public void CheckCompanyName(string CompnayName)
        {
            Assert.That(companyNames.Contains(CompnayName), Is.True, $"Company {CompnayName} not found in the table.");
        }

    }
}
