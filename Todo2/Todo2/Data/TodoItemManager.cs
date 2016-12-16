using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Todo2.Models;

namespace Todo2.Data
{
    class TodoItemManager
    {

        private static CustomClient client = CustomClient.Instance;

        private static ObservableCollection<Item> todoList = new ObservableCollection<Item>();

        public static ObservableCollection<Item> TodoList { get { return todoList; } }


        public static async Task<ObservableCollection<Item>> GetItems()
        {
            try
            {
                // make an HTTP GET request to our server
                var response = await client.GetAsync("api/PublicItems");

                if (response.IsSuccessStatusCode)
                {
                    // retrieve the response content as a json string
                    var listAsString = await response.Content.ReadAsStringAsync();

                    // deserialize our json into a List of Items
                    var listFromServer = JsonConvert.DeserializeObject<List<Item>>(listAsString);

                    foreach (var item in listFromServer)
                    {
                        // for each item in our retrieved list, if the ID of that item doesn't exist in our local todoList, then add it
                        if (todoList.Where(i => i.Id == item.Id).FirstOrDefault() == null)
                        {
                            todoList.Add(item);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to retrieve items: " + e.StackTrace);
            }

            return todoList;

        }

        public static async Task<Item> RemoveItem(Item itemToRemove)
        {
            // remove the item from the local list
            todoList.Remove(itemToRemove);

            try
            {
                // make an HTTP Delete request to our server and try to remove the item from our server
                var response = await client.DeleteAsync("api/PublicItems/" + itemToRemove.Id);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Item Removed from server");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to remove item: " + e.StackTrace);
            }

            return itemToRemove;
        }


        public static async Task<Item> AddItem(Item itemToAdd)
        {
            // add the item locally
            todoList.Add(itemToAdd);

            // serialize our Item to a json string
            StringContent requestBody = new StringContent(JsonConvert.SerializeObject(itemToAdd), Encoding.UTF8, "application/json");
            try
            {

                var response = await client.PostAsync("api/PublicItems/", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Item Added to server");

                    // When we create a new Item on the client, we don't specify its Id (which defaults to 0)
                    // If we successfully add an Item to our server, then we get the added Item as a response with its Id set by the server
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    // Change the local item's Id to what it is on the server
                    // That way when we call RemoveItem, we will make a request to the correct Item on the server
                    // If we failed to add the item on the server, then calling RemoveItem will target an Item of Id = 0 (which does not exist on the server)
                    itemToAdd.Id = JsonConvert.DeserializeObject<Item>(stringResponse).Id;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to add item: " + e.StackTrace);
            }

            return itemToAdd;
        }

    }
}
