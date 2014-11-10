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


namespace BotUserTesting
{
    [TestFixture]
    public class HmrcTest
    {
        private StringBuilder verificationErrors;
        private BotUser botUser;
        private StreamWriter writer;
            
        [SetUp]
        public void SetupTest()
        {
            writer = new StreamWriter(@"D:\test\test1.csv") { AutoFlush = true };
            botUser = new BotUser("Firefox", "http://tools.hmrc.gov.uk/");
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            writer.Dispose();
            //   botUser.Dispose();
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestCase]
        public void TheHMRCtest1Test()
        {
            int[] GrossPays = { 500, 600, 700, 800, 900 };
            string[] TaxCodes = { "100L", "200L", "300L", "400L" };
            var csv = new StringBuilder();
            var newLine = "";

            string GrossPayPerWeek = "";
            string IncomeTaxPerWeek = "";
            string NationalInsurancePerWeek = "";
            string TotalDeductionsPerWeek = "";
            string YourTakeHomePayPerWeek = "";
            string EmployerNationalInsurancePerWeek = "";
            var onErrorPageRestartLink = By.LinkText("restart the Tax Calculator");
            var taxFieldSelector = By.Id("qss52Interviews_Screens_xintglobalglobal4");
            var grossFieldSelector = By.Id("qss52Interviews_Screens_xintglobalglobal6");
            var quickCalculatorLinkSelector = By.Id("Attributeb2Rules_GlobalProceduralRules_docglobalglobal");

            //botUser.GoTo("/hmrctaxcalculator/screen/Personal+Tax+Calculator/en-GB/summary?user=guest");

            writer.WriteLine("*************************HOWTO: Place test cases here line by line*******************************");
            writer.WriteLine("GrossPay,	IncomeTax,	TaxCode,	NationalInsuranceEmployee,	WeekGrossWage,	WeekNo,	ExpectedWeekNetPay,	ExpectedWeekIncomeTax,	ExpectedWeekNationalInsurance,	ExpectedWeekNIEmployer");
            foreach (var TaxCode in TaxCodes)
            {

                foreach (var GrossPay in GrossPays)
                {
                    Thread.Sleep(1000);
                    string myGrossPayString = GrossPay.ToString();
                    if (botUser.IsPageContains(onErrorPageRestartLink))
                    {
                        botUser.WebDriver.FindElement(onErrorPageRestartLink).Click();
                    }
                    botUser.WebDriver.FindElement(quickCalculatorLinkSelector).Click();

                    botUser.ReloadPageUntilContains(taxFieldSelector, 3);
                    botUser.WebDriver.FindElement(taxFieldSelector).Clear();
                    botUser.WebDriver.FindElement(taxFieldSelector).SendKeys(TaxCode);

                    botUser.WebDriver.FindElement(grossFieldSelector).Clear();
                    botUser.WebDriver.FindElement(grossFieldSelector).SendKeys(myGrossPayString);

                    botUser.WebDriver.FindElement(By.Id("submit")).Click();

                    Console.WriteLine("----------------Test Output Begin ------------");

                    Console.WriteLine("GrossPay: " + GrossPay);
                    Console.WriteLine("TaxCode: " + TaxCode);

                    var tableCells = botUser.WebDriver.FindElements(By.TagName("td"));
                    
                      //foreach (var cell in tableCells)
                      //{
                      //    Console.Write(cell.Text);
                      //} 
                    

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

                    foreach (var cell in tableCells.Skip(16).Take(1))
                    {
                        EmployerNationalInsurancePerWeek = cell.Text;
                        EmployerNationalInsurancePerWeek = EmployerNationalInsurancePerWeek.Replace("£", "");
                        Console.Write(EmployerNationalInsurancePerWeek);
                    }

                    //  Thread.Sleep(5000);

                    Console.WriteLine("----------------Test Output End --------------");
    

                    botUser.WebDriver.FindElement(By.CssSelector("input[type=\"button\"]")).Click();
                    botUser.WebDriver.FindElement(By.Id("ACTIONOK")).Click();

                    newLine = 0 + ", " + 0 + ", " + TaxCode + ", " + 0 + ", " + GrossPay + ", " + 1 + ", " + YourTakeHomePayPerWeek + ", " + IncomeTaxPerWeek + ", " + NationalInsurancePerWeek + ", " + EmployerNationalInsurancePerWeek;

                    writer.WriteLine(newLine);
                    //csv.AppendLine(newLine);
                }
            }

            File.WriteAllText(@"D:\test\test1.csv", csv.ToString());


            // File.WriteAllText("D:\\test\\test1.csv", "Test string");
        }
    }
}
    

