using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports;
using OfficeOpenXml;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reqnroll;
using WebDriverManager.DriverConfigs.Impl;
using AventStack.ExtentReports.Reporter;
using Reqnroll.BoDi;

namespace POM_Implementation
{
    [Binding]
    public class Hooks
    {
        private static ExtentTest featureName;
        [ThreadStatic]
        private static ExtentTest scenario;
        private static ExtentReports extent;        

        private readonly IObjectContainer _objectContainer;
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        public static System.Data.DataTable Settings;
        private static bool loginExecuted = false;
        private static bool tableTestsExecuted = false;

        public Hooks(FeatureContext featureContext, ScenarioContext scenarioContext, IObjectContainer objectContainer)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
            _objectContainer = objectContainer;
        }
        private static System.Data.DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Excell");
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                System.Data.DataTable tbl = new System.Data.DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column { 0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
        public static Reqnroll.Table GetTableFromExcel(string path, string SheetName, bool hasHeader = true)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                //var ws = pck.Workbook.Worksheets.First();
                var ws = pck.Workbook.Worksheets[SheetName];
                List<string> headers = new List<string>();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    headers.Add(hasHeader ? firstRowCell.Text : string.Format("Column { 0}", firstRowCell.Start.Column));
                }
                Table tbl = new Table(headers.ToArray());
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    string[] row = new string[headers.Count];
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    tbl.AddRow(row.ToArray());
                }
                return tbl;
            }
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            extent = new ExtentReports();
            ExtentSparkReporter spark = new ExtentSparkReporter(Directory.GetCurrentDirectory() + "\\ExtentReport.html");            
            extent.AttachReporter(spark);
            var directory = Directory.GetCurrentDirectory() + "\\ApplicationSettings.xlsx";
            Settings = GetDataTableFromExcel(directory);
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            extent.Flush();
            string oldPath = Directory.GetCurrentDirectory() + @"\index.html";
            string newpath = Directory.GetCurrentDirectory() + @"\ExtentReports\";
           
            FileInfo f1 = new FileInfo(oldPath);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }
            }
        }



        [AfterStep]
        public void InsertReportingSteps()
        {
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            if (_scenarioContext.StepContext.StepInfo.Text.StartsWith("I Add Test") || _scenarioContext.StepContext.StepInfo.Text.StartsWith("I Add Node"))
            {
                return;
            }
            if (_scenarioContext.TestError == null)
            {
                if (_scenarioContext.StepContext.ContainsKey("Table"))
                {
                    var table = (string[,])_scenarioContext.StepContext["Table"];
                    IMarkup markup = MarkupHelper.CreateTable(table);
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Info(markup);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Info(markup);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Info(markup);
                    else if (stepType == "And")
                        scenario.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Info(markup);
                }
                else if (_scenarioContext.StepContext.ContainsKey("Message"))
                {
                    string Message = _scenarioContext.StepContext["Message"].ToString();
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Info(Message);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Info(Message);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Info(Message);
                    else if (stepType == "And")
                        scenario.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Info(Message);
                }
                else if (_scenarioContext.StepContext.ContainsKey("Error"))
                {
                    string Error = _scenarioContext.StepContext["Error"].ToString();
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(Error);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(Error);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(Error);
                    else if (stepType == "And")
                        scenario.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Fail(Error);
                }
                else
                {
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                    else if (stepType == "And")
                        scenario.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text);
                }
            }
            else if (_scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                else if (stepType == "When")
                    scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                else if (stepType == "And")
                    scenario.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
            }

            //Pending Status
            if (_scenarioContext.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "And")
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");

            }
        }

        [BeforeScenario]
        public void BeforeScenarioStart()
        {
            SelectBrowser(BrowserType.Chrome);                       
            scenario = extent.CreateTest<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }


        [AfterScenario]
        public void CleanUp()
        {
            _driver.Close();
            _driver.Quit();
        }


        internal void SelectBrowser(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    ChromeOptions option = new ChromeOptions();
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                    option.PlatformName = "windows";
                    option.AddUserProfilePreference("download.default_directory", @"c:\Downloads");
                    //_driver = new ChromeDriver(option);
                    _driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), option, TimeSpan.FromMinutes(4));
                    _driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(4);
                    _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
                    _driver.Manage().Window.Maximize();
                    break;                
                case BrowserType.Firefox:
                    var driverDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(driverDir, "geckodriver.exe");
                    service.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                    service.HideCommandPromptWindow = true;
                    service.SuppressInitialDiagnosticInformation = true;
                    _driver = new FirefoxDriver(service);
                    _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
                    break;
                case BrowserType.IE:
                    break;
                default:
                    break;
            }
        }
        public static void AddTest(string TestName)
        {
            featureName = extent.CreateTest<Feature>(TestName);            
        }
        public static void AddNode(string NodeText)
        {
            scenario = featureName.CreateNode<Scenario>(NodeText);
        }
    }

    enum BrowserType
    {
        Chrome,
        Firefox,
        IE
    }
}
