using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo2.Models
{
    class Item
    {
        public string Title { get; set; }

        public string Notes { get; set; }

        public int Id { get; set; }

        public Item(string title, string notes)
        {
            Title = title;
            Notes = notes;
        }
    }
}
