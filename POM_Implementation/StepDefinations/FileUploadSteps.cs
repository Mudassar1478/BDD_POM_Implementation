using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll;
using OpenQA.Selenium.Interactions;
using POM_Implementation.AllPagesControls;
//using System.Windows.Automation;

namespace POM_Implementation.AllStepDefinations
{
    [Binding]
    public class FileUploadSteps
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        FileUploadControls fileUploadControls;

        public FileUploadSteps(IWebDriver driver)
        {
            _driver = driver;            
            fileUploadControls = new FileUploadControls(driver);
        }        

        [When(@"I upload a file ""(.*)""")]
        public void WhenIUploadAFile(string fileName)
        {
            fileUploadControls.UploadFile(fileName);            
        }

        [Then(@"I should see the uploaded file name ""(.*)""")]
        public void ThenIShouldSeeTheUploadedFileName(string expectedFileName)
        {
            fileUploadControls.CheckFileUploadStatus(expectedFileName);
        }

        // Method to Handle File Upload using Windows Automation
        private void UploadFileUsingWindowsAutomation(string filePath)
        {
            Process uploadProcess = new Process();
            uploadProcess.StartInfo.FileName = "notepad.exe"; // Dummy process (replace with actual automation tool if needed)
            uploadProcess.StartInfo.Arguments = $"\"{filePath}\"";
            uploadProcess.Start();
        }
    }
}
