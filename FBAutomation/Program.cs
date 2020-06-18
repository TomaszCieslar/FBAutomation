using System;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FBAutomation
{
    public class Program
    {
        static void Main(string[] args)
        {
            InitiateAutomation();

           // IWebDriver driver = new ChromeDriver(Directory.GetCurrentDirectory());
           // driver.Url = "https://www.facebook.com/groups/2143039459260122/members/";
           

        }

        static void InitiateAutomation()
        {
            //Kill Processes
            Process[] chromedriverProcesses = Process.GetProcessesByName("chromedriver");
            foreach (var process in chromedriverProcesses)

            {
                Console.WriteLine(process.ProcessName);
            }
        }
    }
}
