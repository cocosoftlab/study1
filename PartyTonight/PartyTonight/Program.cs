using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PartyTonight.Login;

namespace PartyTonight
{
    class Program
    {
        private static string baseAddress = "http://localhost:29181/";

        static void Main(string[] args)
        {
            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                //TestCreateAccount();
                TestLogin();
                //while (true)
                //{
                //    string s = Console.ReadLine();
                //    if (s.Equals("exit"))
                //    {
                //        break;
                //    }
                //}
            }

            Console.ReadLine();
        }

        private static void TestLogin()
        {
            // Create HttpCient and make a request to api/values 
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);

            //client.PostAsync<LoginRequestData>(null, null, System.Net.Http.Formatting.JsonMediaTypeFormatter).Wait()

            LoginRequestData widget = new LoginRequestData();
            widget.id = "peter@cocosoft.co.kr";
            widget.password = "asdasd";
            var response = client.PostAsJsonAsync("api/login", widget).ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode()).Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static void TestCreateAccount()
        {
            // Create HttpCient and make a request to api/values 
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);

            CreateAccountRequestData widget = new CreateAccountRequestData();
            widget.id = "peter@cocosoft.co.kr";
            widget.password = "asdasd";
            var response = client.PostAsJsonAsync("api/createaccount", widget).ContinueWith((postTask) => postTask.Result).Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
