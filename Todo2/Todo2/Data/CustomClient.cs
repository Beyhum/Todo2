using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Todo2.Data
{
    class CustomClient : HttpClient
    {
        private static CustomClient client = new CustomClient();

        public static CustomClient Instance { get { return client; } }

        private CustomClient()
        {
            BaseAddress = new Uri("http://w3todo.azurewebsites.net");
        }
    }
}
