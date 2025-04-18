using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM_Implementation.ALLPagesCommonControls
{
    public class AllPagesCommonControls
    {


        IWebDriver _driver;
        public AllPagesCommonControls(IWebDriver driver) => _driver = driver;
        public void NavigateToPage(string URL)
        {
            _driver.Navigate().GoToUrl(URL);
        }
        public void WaitUntilPageLoad(int timeout = 120)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout)).Until(webDriver => ((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (NoSuchElementException)
            {
                Assert.Warn("Element with locator: was not found in current context page.");
                throw;
            }
        }
        public ReadOnlyCollection<IWebElement> GetTableData(string TableName)
        {
            return _driver.FindElements(By.CssSelector($"#{TableName} tbody tr"));
        }
        public  List<string> ExtractColumnData(string tableId,int ColumnHeader)
        {
            List<string> columnData = new List<string>();
            var rows = _driver.FindElements(By.CssSelector($"#{tableId} tbody tr"));
            var a = GetColumnID(tableId,"First Name");
            foreach (var row in rows)
            {
                var firstName = row.FindElements(By.TagName("td"))[1].Text;
                var LastName = row.FindElements(By.TagName("td"))[0].Text;
                //For ID and Class
                //var firstName = row.FindElement(By.XPath(".//td[@class='first-name']")).Text;
                //var LastName = row.FindElement(By.XPath(".//td[@class='last-name']")).Text;

                columnData.Add(firstName + " " + LastName);
            }
            return columnData;
        }
        public int GetColumnID(string TableName, string ColumnHeader)
        {
            try
            {
                int headerID = -1;
                var headers = _driver.FindElements(By.CssSelector($"#{TableName} thead tr th"));
                for (int i = 0; i < headers.Count; i++)
                {
                    if (headers[i].Text.ToUpper() == ColumnHeader.ToUpper())
                    {
                        headerID = i;
                        return headerID;
                    }
                }
                return headerID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return -1;
            }
        }
        
        

        public void CloseCurrentPage()
        {
            //string cbase = _driver.WindowHandles[1];
            _driver.Close();

          
        }
        public void SetExtentReportMessage(string Type, string Message)
        {
            Reqnroll.ScenarioStepContext.Current.Add(Type, Message);
        }
        public void SetExtentReportSummary(string Type, object table)
        {
            Reqnroll.ScenarioStepContext.Current.Add(Type, table);
        }
        public void TakeScreenshot(string fileName)
        {
            try
            {
                ITakesScreenshot screenshotDriver = _driver as ITakesScreenshot;
                if (screenshotDriver != null)
                {
                    Screenshot screenshot = screenshotDriver.GetScreenshot();
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                    screenshot.SaveAsFile(filePath+ ".PNG");
                    Console.WriteLine("Screenshot saved to: " + filePath);
                }
                else
                {
                    Console.WriteLine("Driver does not support screenshot capture.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: " + ex.Message);
            }
        }
    }

}
