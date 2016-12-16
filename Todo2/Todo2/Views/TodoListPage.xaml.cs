using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo2.Data;
using Todo2.Models;
using Xamarin.Forms;

namespace Todo2.Views
{
    public partial class TodoListPage : ContentPage
    {
        public TodoListPage()
        {
            // InitializeComponent will create a partial class based on the XAML file associated with this class (TodoListPage.xaml)
            // The partial class allows you to reference components that you defined in XAML from your C# code
            InitializeComponent();

            Title = "Todo List App";

            SetupListView();
            SetupToolbar();
        }

        // OnAppearing is called whenever the page first appears on screen 
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await TodoItemManager.GetItems();
        }

        private void SetupListView()
        {
            // we are referencing the ListView which we defined in our XAML where we used the x:Name="todoListView" property
            // We are assigning the "source" of our data to represent visually
            // TodoItemManager.TodoList is an ObservableCollection<Item>, which is similar to a List<Item> but also tells the UI to update visually whenever changes to the list are made
            // If we used a List<Item> as our source, then we'd manually have to tell the ListView to redraw whenever we add/remove/change items in our list
            todoListView.ItemsSource = TodoItemManager.TodoList;

            // ItemSelected is an event which triggers whenever we select an item
            // we're setting the SelectedItem property to null to disable highlights over our items whenever we tap them
            // most views (buttons, labels, listviews, etc...) have Clicked/OnTapped events implemented for them
            // methods which subscribe to these events are usually under the form "void DoSomething(object sender, EventArgs e)"
            // the sender object that is passed to our method is the view that triggered the event (in this case the ListView)
            // the e EventArgs is an object which contains additional parameters that the event might want to pass
            todoListView.ItemSelected += (sender, e) =>
            {
                // we could also reference our todoListView like this:
                // var currentListView = sender as ListView;
                todoListView.SelectedItem = null;
            };

            // ItemTapped is another event which occurs whenever we tap an item
            // Whenever the ItemTapped event is triggered, our custom method OnItemTapped also gets called
            // The ItemTapped event is based on a delegate which takes 2 params: object sender, EventArgs e
            // The sender parameter is the object which raised the event, and EventArgs are any additional parameters that the sender would like to send
            todoListView.ItemTapped += OnItemTapped;
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // The ItemTapped event gives us an EventArgs with a property called Item
            // This property is a reference to the individual Item that was tapped
            // We cast it into an Item type to use its properties
            var tappedItem = e.Item as Item;

            // DisplayAlert displays a popup with a title, message body and cancel button 
            await DisplayAlert(tappedItem.Title, tappedItem.Notes, "Ok");
            
        }


        private void SetupToolbar()
        {
            // Create a new ToolbarItem
            ToolbarItem addButton = new ToolbarItem()
            {
                Text = "Add",

                // Device.OnPlatform takes 3 objects of the same type and returns the appropriate object depending on what Platform the App is running
                // the order is iOS, Android, Windows
                // iOS has its images in the Resources folder, Android is the Drawable folder and Windows in the Assets Folder
                // Resources are automatically searched for in the Resources and Drawable folders for iOS/Android, no need to specify the entire path
                Icon = Device.OnPlatform("Icon-Add.png", "ic_add.png", "Assets/ic_add_windows.png"),

                // Whenever we click on our ToolbarItem, OnAddItem gets called
                Command = new Command(OnAddItem)

            };

            // Add the ToolbarItem to our TodoListPage's list of ToolbarItems
            ToolbarItems.Add(addButton);
        }

        private void OnAddItem()
        {
            // The Navigation property lets us call PushAsync/PopAsync methods
            // This allows us to navigate through pages as if they were in a stack
            // Navigation.PopAsync will remove the latest page in the stack and return to the previous one
            Navigation.PushAsync(new TodoItemPage());
        }

        
        // called whenever we interact with an item in our list (long press on Android, swipe on iOS, right click on Windows) and choose the "Remove" Menu button
        private async void OnMenuRemoveItem(object sender, EventArgs e)
        {
            // the sender is the MenuItem object which we defined in our XAML
            // we set its Clicked property to the name of this method in XAML instead of giving it a name and referencing it in C#
            var menuItem = sender as MenuItem;

            // We had bound the CommandParameter property to the current Item that's selected through {Binding .}
            // We cast it into an Item to access its properties
            var currentItem = menuItem.CommandParameter as Item;

            await TodoItemManager.RemoveItem(currentItem);

        }
    }
}
