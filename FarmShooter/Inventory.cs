namespace FarmShooter
{
    public class Inventory
    {
        public Item this[int x, int y] 
        {
            get { return _Items[x, y]; }
            private set { }
        }

        private Item[,] _Items = new Item[3, 8];

        public bool AddItem(Item added_item, out int dropped)
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int k = 0; k < 8; ++k)
                {
                    if (_Items[i, k] != null && _Items[i, k].ID == added_item.ID && _Items[i, k].Quantity != 0 && _Items[i, k].Quantity < 65)
                    {
                        _Items[i, k].Quantity += added_item.Quantity;
                        if (_Items[i, k].Quantity > 64)
                        {
                            dropped = _Items[i, k].Quantity - 64;
                            _Items[i, k].Quantity = 64;
                        }
                        else dropped = 0;

                        return true;
                    }
                    else if (_Items[i, k] == null)
                    {
                        if (added_item.ItemTags.Contains("Tool")) _Items[i, k] = new Tool(added_item as Tool);
                        else _Items[i, k] = new Item(added_item);

                        dropped = 0;

                        return true;
                    }
                }
            }

            if (added_item.Quantity != 0) dropped = added_item.Quantity;
            else dropped = 1;

            return false;
        }

        public bool GetItem(Item item, int amount, out Item out_item)
        {
            for (int i = 0; i < 3; ++i) 
            {
                for (int k = 0; k < 8; ++k) 
                {
                    if (_Items[i, k].ID == item.ID) 
                    {
                        if (_Items[i, k].Quantity == 0)
                        {
                            out_item = new Item(_Items[i, k]);
                            _Items[i, k] = null;

                            return true;
                        }
                        else if (_Items[i, k].Quantity >= amount) 
                        {
                            out_item = new Item(_Items[i, k]) { Quantity = amount };
                            _Items[i, k].Quantity -= amount;

                            if(_Items[i, k].Quantity == 0) _Items[i, k] = null;

                            return true;
                        }
                    }
                }
            }

            out_item = null;

            return false;
        }
    }
}
