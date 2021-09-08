using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class Cart
    {
        public string BuyerId { get; set; } //username coming from token; eg: me@myemail.com
        public List<CartItem> Items { get; set; }

        public Cart() //default construction has to be here 
        { }

        public Cart(string cartId)
        {
            BuyerId = cartId;
            Items = new List<CartItem>();
        }
    }
}
