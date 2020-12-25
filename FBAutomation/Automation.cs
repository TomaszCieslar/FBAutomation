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
using System.Configuration;

namespace FBAutomation
{
    public class Automation
    {
        IWebDriver driver;
        ChromeOptions option;
        String GroupID = "2143039459260122";
        string FirstName = "";
        string LastName = "";
        string id = "";
        int index = 0;
        IWebElement listOfUsers;


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

            driver.Url = ConfigurationManager.AppSettings["GroupPage"];

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            IList<IWebElement> allElements = driver.FindElements(By.XPath(".//span"));


            string value = driver.FindElement(By.XPath(".//span[contains(text(), 'Członkowie: ')]")).Text;
            value = value.Substring(value.Length - 4, 4);

            
            IList<IWebElement> record;

            listOfUsers = driver.FindElement(By.TagName("div"));
            record = listOfUsers.FindElements(By.CssSelector("div[data-visualcompletion='ignore-dynamic']"));

            int numberOfRecords = Convert.ToInt32(value);

           

            do
            {
                try
                {
                    
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("window.scrollBy(0,document.body.scrollHeight)");

                    record = listOfUsers.FindElements(By.TagName("div[data-visualcompletion='ignore-dynamic']"));
                    
                }
               catch
                {
                    //Console.WriteLine("Brak rozszerzonej listy");
                }


                Actions hover = new Actions(driver);
                IWebElement person = record[index];

                hover.MoveToElement(person);
                hover.Perform();
                
                try
                {
                    string nameValue = person.FindElement(By.CssSelector("a")).GetAttribute("aria-label");
                    string idValue = person.FindElement(By.CssSelector("a")).GetAttribute("href");

                    if (!string.IsNullOrEmpty(nameValue))
                    {
                        FirstName = nameValue.Substring(0, nameValue.IndexOf(" "));
                        LastName = nameValue.Substring(nameValue.IndexOf(" ") + 1, (nameValue.Length - FirstName.Length - 1));
                        Console.WriteLine(index + " " + FirstName + " " + LastName);
                    }

                    if (!string.IsNullOrEmpty(idValue))
                    {
                        var start = idValue.IndexOf("user") + 5;
                        id = idValue.Substring(start, idValue.Length - start - 1);
                    }

                    if (!string.IsNullOrEmpty(nameValue) && !string.IsNullOrEmpty(idValue))
                    {
                        using (var context = new FBContext())
                        {
                            User user = new User();
                            user.FirstName = FirstName;
                            user.LastName = LastName;
                            user.FbID = id;
                            user.ContactInitiated = false;
                            user.PageLiked = false;
                            user.IsFriend = false;
                            user.SharedInfo = false;



                            if (context.Users.Any(e => e.FbID == id))
                            {
                                context.Entry(user).State = EntityState.Modified;
                            }
                            else
                            {
                                context.Entry(user).State = EntityState.Added;
                            }
                            context.SaveChanges();
                        }
                    }

               
                }
                catch (Exception ex)
                {

                   // Console.WriteLine(ex.ToString());          
            }


                index++;

            } while (index<numberOfRecords);

            driver.Quit();

        }
        public void GroupReview()
        {

            var context = new FBContext();

            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);


            List<User> users = context.Users.Where(u => u.ContactInitiated == false).Take(300).ToList(); 

            foreach (var user in users)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    driver.Url = ConfigurationManager.AppSettings["UserMessage"]+  user.FbID;

                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                    
                    
                    IList<IWebElement> conversation = driver.FindElements(By.CssSelector("div[class='ljqsnud1']"));;
                    StringBuilder conversationText = new StringBuilder();
                    foreach (var message in conversation)
                    {
                        if (!String.IsNullOrEmpty(message.Text))
                        {
                            conversationText.Append(message.Text);

                        }

                    }
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
                

                driver.Url = ConfigurationManager.AppSettings["UserMessage"] + user.FbID;
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
                Thread.Sleep(5000);
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
                IWebElement communication = driver.FindElement(By.CssSelector("div[class='_1mf _1mj']"));
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                    var clipboard = new Clipboard();
                    clipboard.SetText(message);
                    communication.SendKeys(Keys.Control + "v");

                Thread.Sleep(5000);
                communication.SendKeys(Keys.Enter);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

               
                user.ContactInitiated = true;
                context.SaveChanges();

                Console.WriteLine("kontakt nawiazany:" + user.FirstName + " " + user.LastName);
               

            }

            driver.Quit();
        }

       public void GroupPeopleWhoLikePageAnalisys()
        {
            IList<IWebElement> listOfUsers;


            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);

            driver.Url = "https://www.facebook.com/szkolamlodegoprogramisty/settings/?tab=people_and_other_pages&ref=page_edit&cquick=jsc_c_l&cquick_token=AQ5ywqAW6TB0YqbF3e8&ctarget=https%3A%2F%2Fwww.facebook.com";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            listOfUsers = driver.FindElements(By.TagName("td"));

            do
            {
                try
                {

                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("window.scrollBy(0,document.body.scrollHeight)");

                    listOfUsers = driver.FindElements(By.TagName("tr"));

                }
                catch
                {
                    //Console.WriteLine("Brak rozszerzonej listy");
                }

                Actions hover = new Actions(driver);
                IWebElement person = listOfUsers[index];

                hover.MoveToElement(person);
                hover.Perform();


                try
                {
                    string nameValue = person.FindElement(By.CssSelector("a")).Text;
                    string idValue = person.FindElement(By.CssSelector("a")).GetAttribute("href");

                    if (!string.IsNullOrEmpty(nameValue))
                    {
                        FirstName = nameValue.Substring(0, nameValue.IndexOf(" "));
                        LastName = nameValue.Substring(nameValue.IndexOf(" ") + 1, (nameValue.Length - FirstName.Length - 1));
                        
                    }

                    if (!string.IsNullOrEmpty(idValue))
                    {
                        var start = idValue.IndexOf(".com/") + 5;
                        id = idValue.Substring(start, idValue.Length - start);
                    }

                    if (!string.IsNullOrEmpty(nameValue) && !string.IsNullOrEmpty(idValue))
                    {

                        var context = new FBContext();
                        var current = context.Users.FirstOrDefault(e => e.FbID == id);

                        if (current != null)
                        {
                            current.PageLiked = true;
                            Console.WriteLine("Aktualizacja: " + index + " " + FirstName + " " + LastName);
                            context.SaveChanges();
                        }
                        else
                        {
                            User user = new User();
                            user.FirstName = FirstName;
                            user.LastName = LastName;
                            user.FbID = id;
                            user.ContactInitiated = false;
                            user.PageLiked = true;
                            user.IsFriend = false;
                            user.SharedInfo = false;
                            context.Users.Add(user);
                            Console.WriteLine("Dodawanie: " + index + " " + FirstName + " " + LastName);
                            context.SaveChanges();
                        }



                    }


                }
                catch (Exception ex)
                {

                    // Console.WriteLine(ex.ToString());          
                }

                index++;
            } while (index < 2000);

        }

        

        public void AskForSharing()
        {


            var context = new FBContext();
            driver = new ChromeDriver(Directory.GetCurrentDirectory(), option);

            List<User> users = context.Users.Where(u => u.PageLiked == true && u.SharedInfo == false).Take(6).ToList();

            foreach (var user in users)
            {

                driver.Url = ConfigurationManager.AppSettings["UserMessage"] + user.FbID;
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                string message = "Cześć " + user.FirstName + Environment.NewLine +
                "dzięki za polubienie strony Szkoły Młodego Programisty. Jeżeli uważasz że to co robię jest" + Environment.NewLine +
                "wartościowe prośba o udostępnienie by inne osoby mogły skorzystać z publikowanych" + Environment.NewLine +
                "materiałów." + Environment.NewLine +
                Environment.NewLine +
                "Ostatnio odświeżyłem darmowy kurs scratcha i pythona. Fajnie gdyby trafił do dzieci które" + Environment.NewLine +
                "rozpoczynają przygodę z programowaniem." + Environment.NewLine +
                Environment.NewLine +
                "Aby udostępnić wystarczy wejść na stronę " + Environment.NewLine +
                Environment.NewLine +
                "https://www.facebook.com/watch/?v=592154964897479" + Environment.NewLine +
                Environment.NewLine +
                "I kliknąć 'udostępnij' ze słowem miłego komentarza ;)" + Environment.NewLine +
                Environment.NewLine +
                "Jeżeli mogę Ci jakoś pomóc to też proszę daj znać" + Environment.NewLine +
                Environment.NewLine +
                "Bardzo dziękuję Ci za wsparcie i zaufanie" + Environment.NewLine +
                "Pozdrawiam" + Environment.NewLine +
                "Tomek";
                Thread.Sleep(3000);
                try
                {
                    string alert = driver.SwitchTo().Alert().Text;
                    driver.SwitchTo().Alert().Accept();
                    Console.WriteLine(alert);
                }
                catch
                {
                   // Console.WriteLine("Brak alertow");
                }
                IWebElement communication = driver.FindElement(By.CssSelector("div[class='_1mf _1mj']"));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var clipboard = new Clipboard();
                clipboard.SetText(message);
                communication.SendKeys(Keys.Control + "v");

                Thread.Sleep(3000);
                communication.SendKeys(Keys.Enter);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


                user.SharedInfo = true;
                context.SaveChanges();

                
                
               Console.WriteLine("Informacja o udostępnianiu wysłana: " + user.FirstName + " " + user.LastName);


            }

            driver.Quit();
        }

    }
}