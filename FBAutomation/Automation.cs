using FBAutomation.DAL;
using FBAutomation.Models;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TextCopy;

namespace FBAutomation
{
    public class Automation
    {
        IWebDriver driver;
        ChromeOptions option;
        String GroupID = "2143039459260122";

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

            //using (var context = new FBContext())
            //{
            //    Group group = new Group();
            //    group.GroupName = "Nauka programowania wśród dzieci i młodzieży";
            //    group.FbGroupID = GroupID;

            //    context.Groups.Add(group);
            //    context.SaveChanges();
            //}

            




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


                    using (var context = new FBContext())
                    {
                        User user = new User();
                        user.FirstName = FirstName;
                        user.LastName = LastName;
                        user.FbID = id;
                        user.ContactInitiated = false;
                        user.PageLiked = false;
                        user.IsFriend = false;

                        //Assigment assigment = new Assigment();
                        //assigment.GroupID = 2;
                        //assigment.UserID = user.ID;

                        if (context.Users.Any(e => e.FbID == id))
                        {
                            context.Entry(user).State = EntityState.Modified;
                        }
                        else
                        {
                            context.Entry(user).State = EntityState.Added;
                        }


                        //if (context.Assigments.Any(e => e.UserID == user.ID))
                        //{
                        //    context.Entry(assigment).State = EntityState.Modified;
                        //}
                        //else
                        //{
                        //    context.Entry(assigment).State = EntityState.Added;
                        //}




                        context.SaveChanges();
                    }
                    

               
                }
                catch (Exception)
                {

   
                }


                index++;
            } while (index<numberOfRecords);

            driver.Quit();

        }
        public void GroupReview()
        {

            var context = new FBContext();

            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);


           // var users = context.Users.Take(10);
            List<User> users = context.Users.Where(u => u.ContactInitiated == false).Take(30).ToList(); 

            foreach (var user in users)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    driver.Url = "https://www.facebook.com/messages/t/" + user.FbID;

                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                    IWebElement communication= driver.FindElement(By.XPath("//*[@id='js_1']"));

                    
                    //wait until all component will be visible
                    
                    IList<IWebElement> conversation = communication.FindElements(By.TagName("span"));
                    StringBuilder conversationText = new StringBuilder();
                    foreach (var message in conversation)
                    {
                        if (!String.IsNullOrEmpty(message.Text))
                        {
                            conversationText.Append(message.Text);

                        }

                    }
                    //Console.WriteLine(conversationText.ToString());
                    if (conversationText.ToString().Contains("myślę że mogą Ci się przydać te darmowe materiały"))
                    {
                        user.ContactInitiated = true;
                        Console.WriteLine(user.FirstName + user.LastName + " - Contact initiated");
                        context.SaveChanges();
                    }
                }
                catch (Exception) 
                {
                    Console.WriteLine("Empty conversation");

                }
            }

            

        }
        public void SendInformation()
        {


            var context = new FBContext();

            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);

            List<User> users = context.Users.Where(u => u.ContactInitiated == false).Take(20).ToList();

            foreach (var user in users)
            {
                

                driver.Url = "https://www.facebook.com/messages/t/" + user.FbID;
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                string message = "Cześć " + user.FirstName + Environment.NewLine+
                "myślę że mogą Ci się przydać te darmowe materiały." + Environment.NewLine +
                Environment.NewLine +
                "Na stronie" + Environment.NewLine +
                "https://www.facebook.com/szkolamlodegoprogramisty/" + Environment.NewLine +
                Environment.NewLine +
                "przygotowuję DARMOWE ćwiczenia i kursy dla dzieci z programowania do rozwiązywania i " + Environment.NewLine +
                "przejścia w domu" + Environment.NewLine +
                Environment.NewLine +
                "DARMOWY Kurs SCRATCH(6 +)(ponad 1000 osób się zapisało) i PYTHON(9 +)(ponad 800)" + Environment.NewLine +
                "https://szkolamlodegoprogramisty.pl/kursy/" + Environment.NewLine +
                Environment.NewLine +
                "A w między czasie zapraszam na blog" + Environment.NewLine +
                "https://szkolamlodegoprogramisty.pl/" + Environment.NewLine +
                Environment.NewLine +
                "Pozdrawiam" + Environment.NewLine +
                "Tomek";

                try
                {
                    string alert = driver.SwitchTo().Alert().Text;
                    driver.SwitchTo().Alert().Accept();
                    Console.WriteLine(alert);
                }
                catch
                {
                    Console.WriteLine("Brak alertow");
                }
                
                    IWebElement communication = driver.FindElement(By.CssSelector("div[data-offset-key]"));
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                    var clipboard = new Clipboard();
                    clipboard.SetText(message);
                    communication.SendKeys(Keys.Control + "v");

                Thread.Sleep(5000);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                IWebElement submitElement = driver.FindElement(By.XPath("//div//a[@aria-label='Wyślij']"));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                submitElement.Click();


                user.ContactInitiated = true;
                context.SaveChanges();

               

            }

            driver.Quit();
        }

       public void GroupPeopleWhoLikePageAnalisys()
        {

            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);

            driver.Url = "https://www.facebook.com/szkolamlodegoprogramisty/settings/?tab=people_and_other_pages&ref=page_edit";

            IWebElement ListOfFuns = driver.FindElement(By.XPath("//*[@id='u_0_u']/div/div/div/div[3]/div/div[2]/table/tbody"));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IList<IWebElement> record;

            record = ListOfFuns.FindElements(By.TagName("tr"));

        }

        public void AddToFriend()
        {

            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);

            driver.Url = "https://www.facebook.com/groups/503657533808113/members/";

            //IWebElement ListOfFuns = driver.FindElement(By.XPath("//*[@id='u_0_u']/div/div/div/div[3]/div/div[2]/table/tbody"));
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //IList<IWebElement> record;

            //record = ListOfFuns.FindElements(By.TagName("tr"));

        }

    }
}