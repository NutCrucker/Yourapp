using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;

namespace SeleniumingIT
{
    public class FirstSelenium
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        public string[] Answers = {"Y","N"};
        [OneTimeSetUp]
        public void LoadDriver()
        {
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--start-maximized");
            options.AddExcludedArgument("ignore-certificate-errors");

            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));


            Console.WriteLine("SetUp Finished Successfully");
        }

        [OneTimeTearDown]
        public void UnloadDriver()
        {
            driver.Quit();
            Console.WriteLine("TearDown Finished Successfully");
        }

        [TestCase]
        public void LaunchFacebook()
        {
            LoadDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            wait.Until(d => d.FindElement(By.Id("gsr")));
            IWebElement searchBarElement = driver.FindElement(By.Id("lst-ib"));
            searchBarElement.SendKeys("Facebook");
            searchBarElement.SendKeys(Keys.Enter);
            wait.Until(d => d.FindElement(By.Id("gsr")));
            IWebElement ResultElement = driver.FindElement(By.LinkText("Facebook"));
            ResultElement.Click();
            string expectedPageTitle = "https://www.facebook.com/";
            string actualPageTitle = driver.Url;
            Assert.AreEqual(expectedPageTitle, actualPageTitle, "Title results are not equal");
        }
        public string getPath()
        {
            Console.WriteLine("Please insert the path of the message you would like to post: ");
            return Console.ReadLine();
        }
        public void Login()
        {
            Console.WriteLine("Please insert your Email: ");
            string Email = Console.ReadLine();
            Console.WriteLine("Please insert your Password: ");
            string Password = Console.ReadLine();
            LoginToFacebook(Email, Password);
        }
        public void LoginToFacebook(string email,string password)
        {
            LaunchFacebook();
            IWebElement searchBarElement = driver.FindElement(By.Id("email"));
            searchBarElement.SendKeys(email);
            searchBarElement = driver.FindElement(By.Id("pass"));
            searchBarElement.SendKeys(password);
            searchBarElement.SendKeys(Keys.Enter);
            ignorePrem();
        }
        public void ignorePrem()
        {
            System.Threading.Thread.Sleep(5000);
            Actions builder = new Actions(driver);
            builder.MoveToElement(driver.FindElement(By.Id("blueBarDOMInspector")), 10, 25).Click().Build().Perform();
        }
        public void Search(string Value)
        {
            IWebElement MainPageElement = driver.FindElement(By.Name("q"));
            MainPageElement.Click();
            MainPageElement.SendKeys(Value);
            MainPageElement.SendKeys(Keys.Enter);
        }
        public void GoIntoGroup(string name)
        {
            Search(name);
            try
            {
                IWebElement Group = driver.FindElement(By.PartialLinkText(name));
                Group.Click();
            }
            catch
            {
                Console.WriteLine("Please be more specific about the groups name:");
                name = Console.ReadLine();
                GoIntoGroup(name);
            }
        }
        public bool ValidateGroup()
        {
            string Valid = ValidAnswer();
            if (Valid == "Y") return true;
            return false;
        }
        public string ValidAnswer()
        {
            Console.WriteLine("Is this the group you wanted to post at?[Y/N]");
            string answer = Console.ReadLine();
            while (!Answers.Contains(answer))
            {
                Console.WriteLine("Invalid Answer! Is this the group you wanted to post at?[Y/N]");
                answer = Console.ReadLine();
            }
            return answer;
        }
        public void ReturnToMain()
        {
            IWebElement MainPageElement = driver.FindElement(By.Id("u_0_c"));
            MainPageElement.Click();
        }
        public void PostText(string path)
        {
            IWebElement MainPageElement = driver.FindElement(By.Name("xhpc_message_text"));
            MainPageElement.Click();
            MainPageElement.SendKeys(File.ReadAllText(path));
            System.Threading.Thread.Sleep(2000);
            IWebElement ClickPost = driver.FindElement(By.CssSelector("button._1mf7._4jy0._4jy3._4jy1._51sy.selected._42ft"));
            ClickPost.Click();
            System.Threading.Thread.Sleep(5000);
        }
        public void PostToGroup(string path, string name)
        {
            GoIntoGroup(name);
            if (ValidateGroup())
            {
                PostText(path);
                Console.WriteLine("Posted! Going back to the main page.");
            }
            else
            {
                Console.WriteLine("Couldn't get into the right group, going back to main page - let's try again!");
            }
            ReturnToMain();
        }
        public void exitChrome()
        {
            driver.Close();
        }
        public string getGroupName()
        {
            Console.WriteLine("Please Write the names (can be partial) of the groups you would like to post at: [to press N]");
            return Console.ReadLine();
        }
        public int getNumOfGroups()
        {
            int NumOfGroups;
            Console.WriteLine("In how much groups would you like to post?");
            if (Int32.TryParse(Console.ReadLine(), out NumOfGroups)) { }
            else
            {
                while(!Int32.TryParse(Console.ReadLine(), out NumOfGroups))
                {
                    Console.WriteLine("Invalid Answer, In how much groups would you like to post?");
                }
            }
            return NumOfGroups;
        }
        public string[] StringOfGroups()
        {
            string[] GroupsToPost = new string[getNumOfGroups()];
            for(int i = 0; i < GroupsToPost.Length; i++)
            {
                GroupsToPost[i] = getGroupName();
            }
            return GroupsToPost;
        }
    }
}
