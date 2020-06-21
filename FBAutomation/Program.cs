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
            Automation automation = new Automation();

            automation.KillExistingProcesses();

            automation.SettingParameters();

            // automation.RunAutomation();

           automation.SendInformation();

            // klasa grupa

            // dodanie do tablicy
      
        }

       
    }
}
