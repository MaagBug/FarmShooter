using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmShooter
{
    internal class Inventory
    {
        public Item this[int x, int y] 
        {
            get { return _Items[x, y]; }
            set { }
        }

        private Item[,] _Items = new Item[3, 8];

        public bool AddItem(Item item) { }

        public bool GetItem(int item_id, out Item item) { }
    }
}
