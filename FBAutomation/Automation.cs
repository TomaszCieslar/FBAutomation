﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FBAutomation
{
    public class Automation
    {
        IWebDriver driver;
        ChromeOptions option;

        public void KillExistingProcesses()
        {
            Process[] chromedriverProcesses = Process.GetProcessesByName("chromedriver");
            Process[] chromeBrowser = Process.GetProcessesByName("chrome");

            Process[] chrome = chromedriverProcesses.Concat(chromeBrowser).ToArray();

            foreach (var process in chrome)

            {
                process.Kill();
            }

        }

        public void SettingParameters()
        {
            option = new ChromeOptions();
            option.AddArgument("--user-data-dir=/Users/tcieslar/AppData/Local/Google/Chrome/User Data");
            option.AddArgument("--profile-directory=Default");
            option.AddArgument("--disable-extensions");
            option.AddArgument("disable-infobars");
            option.AddArgument("--start-maximized");

        }


        public void RunAutomation()
        {
            driver = new ChromeDriver(Directory.GetCurrentDirectory(),option);

            driver.Url = "https://www.facebook.com/groups/2143039459260122/members/";
            
            IWebElement NumberOfUsers = driver.FindElement(By.XPath("//*[@id='groupsMemberBrowser']/div[1]/div/div[1]/span"));
            IWebElement listOfUsers;
            IWebElement listOfAdditionalUsers;
            IList<IWebElement> record;

            listOfUsers = driver.FindElement(By.TagName("div"));
            record = listOfUsers.FindElements(By.CssSelector("div[class='clearfix _60rh _gse']"));

            int index = 0;
            int numberOfRecords = Convert.ToInt32(NumberOfUsers.Text);
            do
            {
                try
                {
                    listOfAdditionalUsers = driver.FindElement(By.CssSelector("div[class='fbProfileBrowserList expandedList']"));
                    record = listOfUsers.FindElements(By.CssSelector("div[class='clearfix _60rh _gse']"));
                }
               catch
                {
                    //Console.WriteLine("Brak rozszerzonej listy");
                }


                Actions hover = new Actions(driver);
                IWebElement person = record[index];

                //Action go to element
                hover.MoveToElement(person);
                hover.Perform();

                try
                {
                    string nameValue = person.FindElement(By.CssSelector("a[title]")).GetAttribute("title");
                    string idValue = person.FindElement(By.CssSelector("a[title]")).GetAttribute("ajaxify");

                    string FirstName = nameValue.Substring(0, nameValue.IndexOf(" "));
                    string LastName = nameValue.Substring(nameValue.IndexOf(" ") + 1, (nameValue.Length - FirstName.Length - 1));

                    var start = idValue.IndexOf("member_id=") + 10;
                    var id = idValue.Substring(start, idValue.IndexOf("&ref=") - start);
                    Console.WriteLine(index + " " + FirstName + " " + LastName + " " + id);

                    User user = new User();
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.FbID = id;

               
                }
                catch (Exception)
                {

   
                }


                index++;
            } while (index<numberOfRecords);

            driver.Quit();

        }

        public void SendInformation()
        {

            string idUsera = "100001558372527";
            //            6 Kamila Kamińska 100001558372527
            //7 Kris Gontarek 693506551
            //8 Anna Świć 100004371964825
            //9 Marcin Joka 100001151982839
            //13 Basia Skowron 100005318172794
            //14 Dorota J-ka 100047367904924
            //
            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);

            driver.Url = "https://www.facebook.com/messages/t/" + idUsera;


           
                IWebElement communication = driver.FindElement(By.XPath("//*[@id='js_18']/div/div/div"));
                IList<IWebElement> conversation = communication.FindElements(By.TagName("span"));
            try
            {
                foreach (var message in conversation)
                {
                    if (!String.IsNullOrEmpty(message.Text))
                    {
                        Console.WriteLine(message.Text);
                    }

                }
            }
            catch (Exception)
            {

               
            }
         

        }
    }
}