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
            botUser = new BotUser("Chrome", "http://tools.hmrc.gov.uk/");
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
            int[] GrossPays = {200,225,250,275,300,325,350,375,400,425,450,475,500,525,550,575,600,625,650,675,700,725,750,775,800,825,850,875,900,925,950,975,1000,1025,1050,1075,1100,1125,
1150,1175,1200,1225,1250,1275,1300,1325,1350,1375,1400,1425,1450,1475,1500,1525,1550,1575,1600,1625,1650,1675,1700,1725,1750,1775,1800,1825,1850,1875,1900,1925,
1950,1975,2000,2025,2050,2075,2100,2125,2150,2175,2200,2225,2250,2275,2300,2325,2350,2375,2400,2425,2450,2475,2500,2525,2550,2575,2600,2625,2650,2675,2700,2725,
2750,2775,2800,2825,2850,2875,2900,2925,2950,2975,3000,3025,3050,3075,3100,3125,3150,3175,3200,3225,3250,3275,3300,3325,3350,3375,3400,3425,3450,3475,3500,3525,
3550,3575,3600,3625,3650,3675,3700,3725,3750,3775,3800,3825,3850,3875,3900,3925,3950,3975,4000,4025,4050,4075,4100,4125,4150,4175,4200,4225,4250,4275,4300,4325,
4350,4375,4400,4425,4450,4475,4500,4525,4550,4575,4600,4625,4650,4675,4700,4725,4750,4775,4800,4825,4850,4875,4900,4925,4950,4975,5000};

            string[] TaxCodes = { "928L" };
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
            var restartButtonFromGridPage = By.CssSelector("input[type=\"button\"]");
            var secondRestartButton = By.Id("ACTIONOK");

            var onErrorPageRestartLink2 = By.Id("Attributeb2Rules_GlobalProceduralRules_docglobalglobal");

            var submitButton = By.Id("submit");

            //botUser.GoTo("/hmrctaxcalculator/screen/Personal+Tax+Calculator/en-GB/summary?user=guest");

            writer.WriteLine("*************************HOWTO: Place test cases here line by line*******************************");
            writer.WriteLine("GrossPay,	IncomeTax,	TaxCode,	NationalInsuranceEmployee,	WeekGrossWage,	WeekNo,	ExpectedWeekNetPay,	ExpectedWeekIncomeTax,	ExpectedWeekNationalInsurance,	ExpectedWeekNIEmployer");
            foreach (var TaxCode in TaxCodes)
            {

                foreach (var GrossPay in GrossPays)
                {
                    Thread.Sleep(3000);
                    string myGrossPayString = GrossPay.ToString();

                    if (botUser.IsPageContains(onErrorPageRestartLink))
                    {
                        //botUser.ReloadPageUntilContains(onErrorPageRestartLink, 2);
                        botUser.WebDriver.FindElement(onErrorPageRestartLink).Click();
                    }

                    //botUser.ReloadPageUntilContains(onErrorPageRestartLink2, 3);
                    //botUser.WebDriver.FindElement(onErrorPageRestartLink2).Click();
                    Thread.Sleep(3000);
                    botUser.ReloadPageUntilContains(quickCalculatorLinkSelector, 10);
                    botUser.WebDriver.FindElement(quickCalculatorLinkSelector).Click();

                    botUser.ReloadPageUntilContains(taxFieldSelector, 10);
                    Thread.Sleep(3000);
                    botUser.WebDriver.FindElement(taxFieldSelector).Clear();           
                    botUser.WebDriver.FindElement(taxFieldSelector).SendKeys(TaxCode);

                   
                    botUser.WebDriver.FindElement(grossFieldSelector).Clear();      
                    botUser.WebDriver.FindElement(grossFieldSelector).SendKeys(myGrossPayString);
                    Thread.Sleep(3000);

                    //botUser.ReloadPageUntilContains(submitButton, 3);
                    botUser.WebDriver.FindElement(submitButton).Click();

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
                        GrossPayPerWeek = GrossPayPerWeek.Replace(",", "");
                        Console.Write(GrossPayPerWeek);
                    }

                    foreach (var cell in tableCells.Skip(4).Take(1))
                    {
                        IncomeTaxPerWeek = cell.Text;
                        IncomeTaxPerWeek = IncomeTaxPerWeek.Replace("£", "");
                        IncomeTaxPerWeek = IncomeTaxPerWeek.Replace(",", "");
                        Console.Write(IncomeTaxPerWeek);
                    }

                    foreach (var cell in tableCells.Skip(7).Take(1))
                    {
                        NationalInsurancePerWeek = cell.Text;
                        NationalInsurancePerWeek = NationalInsurancePerWeek.Replace("£", "");
                        NationalInsurancePerWeek = NationalInsurancePerWeek.Replace(",", "");
                        Console.Write(NationalInsurancePerWeek);
                    }

                    foreach (var cell in tableCells.Skip(10).Take(1))
                    {
                        TotalDeductionsPerWeek = cell.Text;
                        TotalDeductionsPerWeek = TotalDeductionsPerWeek.Replace("£", "");
                        TotalDeductionsPerWeek = TotalDeductionsPerWeek.Replace(",", "");
                        Console.Write(TotalDeductionsPerWeek);
                    }

                    foreach (var cell in tableCells.Skip(13).Take(1))
                    {
                        YourTakeHomePayPerWeek = cell.Text;
                        YourTakeHomePayPerWeek = YourTakeHomePayPerWeek.Replace("£", "");
                        YourTakeHomePayPerWeek = YourTakeHomePayPerWeek.Replace(",", "");
                        Console.Write(YourTakeHomePayPerWeek);
                    }

                    foreach (var cell in tableCells.Skip(16).Take(1))
                    {
                        EmployerNationalInsurancePerWeek = cell.Text;
                        EmployerNationalInsurancePerWeek = EmployerNationalInsurancePerWeek.Replace("£", "");
                        EmployerNationalInsurancePerWeek = EmployerNationalInsurancePerWeek.Replace(",", "");
                        Console.Write(EmployerNationalInsurancePerWeek);
                    }

                    //  Thread.Sleep(5000);

                    Console.WriteLine("----------------Test Output End --------------");

                    Thread.Sleep(3000);

                    botUser.ReloadPageUntilContains(restartButtonFromGridPage, 10);
                    botUser.WebDriver.FindElement(restartButtonFromGridPage).Click();
                    Thread.Sleep(3000);
                    botUser.ReloadPageUntilContains(secondRestartButton, 10);
                    botUser.WebDriver.FindElement(secondRestartButton).Click();

                    newLine = 0 + ", " + 0 + ", " + TaxCode + ", " + 0 + ", " + GrossPay + ", " + 1 + ", " + YourTakeHomePayPerWeek + ", " + IncomeTaxPerWeek + ", " + NationalInsurancePerWeek + ", " + EmployerNationalInsurancePerWeek;

                    writer.WriteLine(newLine);
                    //csv.AppendLine(newLine);
                }
            }

            //  File.WriteAllText(@"D:\test\test1.csv", csv.ToString());
            //  File.WriteAllText("D:\\test\\test1.csv", "Test string");
        }
    }
}
    

