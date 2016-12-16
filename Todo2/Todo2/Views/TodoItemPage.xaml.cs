using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo2.Data;
using Todo2.Models;
using Xamarin.Forms;

namespace Todo2.Views
{
    public partial class TodoItemPage : ContentPage
    {
        public TodoItemPage()
        {
            InitializeComponent();

            Title = "Add Item";

            addButton.Clicked += OnAddItem;
        }

        private async void OnAddItem(object sender, EventArgs e)
        {
            Item itemToAdd = new Item(titleEntry.Text, notesEntry.Text);


            // pops our navigation stack, returning us to the previous page (TodoListPage)
            await Navigation.PopAsync();

            await TodoItemManager.AddItem(itemToAdd);

        }


    }
}
