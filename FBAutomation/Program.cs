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
            int ans = 0; 

            do
            {

                Automation automation = new Automation();

                automation.KillExistingProcesses();

                automation.SettingParameters();

                Console.WriteLine("Choose option: ");
                Console.WriteLine("1. Group Analyze (information about users belongs to group");
                Console.WriteLine("2. Group Review (information about conversation / is Friend / like?");
                Console.WriteLine("---------------------");
                Console.WriteLine("3. Review Funs");
                Console.WriteLine("4. Add to Friend");
                Console.WriteLine("5. Conversation with people");
                Console.WriteLine("0. Exit");
                ans = Convert.ToInt32(Console.ReadLine());

                switch (ans)
                {
                    case 1:
                        automation.RunAutomation();
                        break;
                    case 2:
                        automation.GroupReview();
                        break;
                    case 3:
                        automation.GroupPeopleWhoLikePageAnalisys();                     
                        break;
                    case 4:
                        automation.AddToFriend();
                        break;
                    case 5:
                        automation.SendInformation();
                        break;
                        

                    default:
                        break;
                }
            } while (ans!=0);


        }

       
    }
}
