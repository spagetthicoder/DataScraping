using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.IO;


namespace PAYEtest
{
    [TestFixture]
    public class HMRCtest1
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "http://tools.hmrc.gov.uk/";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
               //driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestCase]
        public void TheHMRCtest1Test()
        {
            int[] GrossPays = { 500, 600 };
            string[] TaxCodes = {"700L","800L"};
            var csv = new StringBuilder();
            var newLine = "";

            string GrossPayPerWeek = "";
            string IncomeTaxPerWeek = "";
            string NationalInsurancePerWeek = "";
            string TotalDeductionsPerWeek = "";
            string YourTakeHomePayPerWeek = "";
            string EmployerNationalInsurancePerWeek = "";

            
            foreach (var TaxCode in TaxCodes)
            {

                foreach ( var GrossPay in GrossPays)
                {
                    string myGrossPayString = GrossPay.ToString();
                    driver.Navigate().GoToUrl(baseURL + "/hmrctaxcalculator/screen/Personal+Tax+Calculator/en-GB/summary?user=guest");
                    driver.FindElement(By.LinkText("restart the Tax Calculator")).Click();
                    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5000));
                    driver.FindElement(By.Id("Attributeb2Rules_GlobalProceduralRules_docglobalglobal")).Click();
                    driver.FindElement(By.Id("qss52Interviews_Screens_xintglobalglobal4")).Clear();
                    driver.FindElement(By.Id("qss52Interviews_Screens_xintglobalglobal4")).SendKeys(TaxCode);
                    driver.FindElement(By.Id("qss52Interviews_Screens_xintglobalglobal6")).Clear();
                    driver.FindElement(By.Id("qss52Interviews_Screens_xintglobalglobal6")).SendKeys(myGrossPayString);
                    Thread.Sleep(1000);
                    driver.FindElement(By.Id("submit")).Click();
                    Thread.Sleep(1000);

                    Console.WriteLine("----------------Test Output Begin ------------");
                    
                    Console.WriteLine("GrossPay: " + GrossPay);
                    Console.WriteLine("TaxCode: " + TaxCode);

                    var tableCells = driver.FindElements(By.TagName("td"));
                  /**
                    foreach (var cell in tableCells)
                    {
                        Console.Write(cell.Text);
                    } 
                  **/

                    foreach (var cell in tableCells.Skip(1).Take(1))
                    {                      
                        GrossPayPerWeek = cell.Text;
                        GrossPayPerWeek = GrossPayPerWeek.Replace("£", "");
                        Console.Write(GrossPayPerWeek);
                    }
                    
                    foreach (var cell in tableCells.Skip(4).Take(1))
                    {
                        IncomeTaxPerWeek = cell.Text;
                        IncomeTaxPerWeek = IncomeTaxPerWeek.Replace("£", "");
                        Console.Write(IncomeTaxPerWeek);
                    }
                    
                    foreach (var cell in tableCells.Skip(7).Take(1))
                    {
                        NationalInsurancePerWeek = cell.Text;
                        NationalInsurancePerWeek = NationalInsurancePerWeek.Replace("£", "");
                        Console.Write(NationalInsurancePerWeek);
                    }

                    foreach (var cell in tableCells.Skip(10).Take(1))
                    {
                        TotalDeductionsPerWeek = cell.Text;
                        TotalDeductionsPerWeek = TotalDeductionsPerWeek.Replace("£", "");
                        Console.Write(TotalDeductionsPerWeek);
                    }

                    foreach (var cell in tableCells.Skip(13).Take(1))
                    {
                        YourTakeHomePayPerWeek = cell.Text;
                        YourTakeHomePayPerWeek = YourTakeHomePayPerWeek.Replace("£", "");
                        Console.Write(YourTakeHomePayPerWeek);
                    }

                    foreach (var cell in tableCells.Skip(13).Take(1))
                    {
                        EmployerNationalInsurancePerWeek = cell.Text;
                        EmployerNationalInsurancePerWeek = EmployerNationalInsurancePerWeek.Replace("£", "");
                        Console.Write(EmployerNationalInsurancePerWeek);
                    }
                  
                  //  Thread.Sleep(5000);

                    Console.WriteLine("----------------Test Output End --------------");
                    /*foreach (var cell in tableCells.Skip(1).Take(1))
                    {
                        Console.Write(cell.Text);
                    }*/
                    
                    driver.FindElement(By.CssSelector("input[type=\"button\"]")).Click();
                   
                    driver.FindElement(By.Id("ACTIONOK")).Click();

                    newLine = GrossPay + ", " + IncomeTaxPerWeek + ", " + TaxCode + ", " + NationalInsurancePerWeek + ", " + GrossPayPerWeek + ", " + "1, " + YourTakeHomePayPerWeek + ", " + IncomeTaxPerWeek + ", " + NationalInsurancePerWeek + ", " + EmployerNationalInsurancePerWeek;
                                       
                    csv.AppendLine(newLine);  
                }
            }

            File.WriteAllText("D:\\test\\test1.csv", csv.ToString());
              
           
           // File.WriteAllText("D:\\test\\test1.csv", "Test string");
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
