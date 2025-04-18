using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM_Implementation.AllPagesControls
{
    public class FileUploadControls
    {
        IWebDriver _driver;
        public FileUploadControls(IWebDriver driver) => _driver = driver;
        IWebElement chooseFileButton => _driver.FindElement(By.XPath("//*[@id='file-upload']"));
        IWebElement uploadButton => _driver.FindElement(By.XPath("//*[@id='file-submit']"));

        public void UploadFile(string fileName)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                string FilePathAutoIT = "UploadFileScript.exe";
                string autoitfile = Path.Combine(Directory.GetCurrentDirectory(), FilePathAutoIT);
                Actions actions = new Actions(_driver);                
                actions.MoveToElement(chooseFileButton).Click().Perform();
                Thread.Sleep(3000);
                System.Diagnostics.Process.Start(autoitfile);
                Thread.Sleep(3000);                
                uploadButton.Click();
            }
            catch (Exception ex)
            {
                Assert.Warn("Something Went Wrong While click/Getting element" + ex);
            }
        }
        public void CheckFileUploadStatus(string expectedFileName)
        {
            WebDriverWait _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement uploadedFileName = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("uploaded-files")));
            Assert.That(expectedFileName, Is.EqualTo(uploadedFileName.Text), "Uploaded file name does not match.");
        }
    }
}
