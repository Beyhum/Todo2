using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo2.Views;
using Xamarin.Forms;

namespace Todo2
{
    // An instance of App is created whenever you start your application
    // Each platform project uses the LoadApplication() method and passes it an instance of App
    public class App : Application
    {
        public App()
        {
            // The root page of your application


            // Set the starting page of our application to be an instance of TodoListPage
            // Wrap our TodoListPage with a NavigationPage to enable page navigation
            MainPage = new NavigationPage(new TodoListPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
