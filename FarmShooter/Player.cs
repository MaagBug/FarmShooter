namespace FarmShooter
{
    enum PlayerState { None, Paused, Texting, Crafting }

    class Player : Drawable
    {
        public Entity MainEntity;

        public Inventory Inventory = new Inventory();
        public Item SelectedItem;

        public PlayerState State = PlayerState.None;

        public bool Paused = false;

        public RectangleShape CellSelectionMark = new RectangleShape() { OutlineThickness = 9, OutlineColor = new Color(255, 255, 255), FillColor = new Color(0, 0, 0, 0) };
        public object SelectedObject;

        public ImagePoint Handle;

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(CellSelectionMark);
            target.Draw(MainEntity);
            if (SelectedItem != null) 
            {
                if(SelectedItem is Handheld held) target.Draw(held);
                else target.Draw(new Sprite(SelectedItem.InventorySprite) { Position = MainEntity.Sprite.Position });
            }
        }

        public void Start() 
        {
            Handle = new ImagePoint() { Point = new Vector2f(25, 0) };
            Inventory.AddItem(new Tool(6) { Owner = this }, out _);
            Inventory.AddItem(new Tool(7) { Owner = this }, out _);
            Inventory.AddItem(new Tool(8) { Owner = this }, out _);
            Inventory.AddItem(new Item(0), out _);
            Inventory.AddItem(new Item(1), out _);
            Inventory.AddItem(new Item(2), out _);
            Inventory.AddItem(new Item(3), out _);
            Inventory.AddItem(new Item(4), out _);
        }

        public void Update()
        {
            int vert_input = 0;
            int horiz_input = 0;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) vert_input = -1;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) vert_input = 1;

            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) horiz_input = -1;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) horiz_input = 1;

            for (int i = 0; i < 9; ++i)
            {
                if (Keyboard.IsKeyPressed(Program.NumKeys[i]))
                {
                    SelectedItem = Inventory[0, i];
                    break;
                }
            }

            MainEntity.Sprite.Position += new Vector2f(horiz_input, vert_input).Normalized() * MainEntity.Speed * Program.MainWindow.DeltaTime.AsSeconds();
            Handle.Angle = MathF.Atan2(Program.TrueMousePosition.Y - MainEntity.Sprite.Position.Y, Program.TrueMousePosition.X - MainEntity.Sprite.Position.X) * (180 / 3.14F);

            CellSelectionMark.OutlineColor = new Color(255, 255, 255, 0);

            if ((SelectedItem is Tool tool && tool.Type == ToolType.Hoe) || (SelectedItem != null && SelectedItem.ItemTags.Contains("Seed")))
            {
                foreach (var tile in Program.Field)
                {
                    if (tile.MainSprite.GetGlobalBounds().Contains(Program.TrueMousePosition.X, Program.TrueMousePosition.Y))
                    {
                        CellSelectionMark.Position = tile.MainSprite.Position;
                        CellSelectionMark.Size = new Vector2f(tile.MainSprite.GetLocalBounds().Width, tile.MainSprite.GetLocalBounds().Height);

                        if ((tile.MainSprite.Position + (Vector2f)tile.MainSprite.Texture.Size / 2 - MainEntity.Sprite.Position).Length() <= 400)
                        {
                            CellSelectionMark.OutlineColor = new Color(255, 255, 255, 255);
                            SelectedObject = tile;
                        }
                        else
                        {
                            CellSelectionMark.OutlineColor = new Color(130, 130, 130, 190);
                            SelectedObject = null;
                        }

                        break;
                    }
                }
            }
            else
            {
                foreach (var ir in Program.InteractableResources)
                {
                    if (ir.MainSprite.GetGlobalBounds().Contains(Program.TrueMousePosition.X, Program.TrueMousePosition.Y))
                    {
                        CellSelectionMark.Position = new Vector2f(ir.MainSprite.GetGlobalBounds().Left, ir.MainSprite.GetGlobalBounds().Top);
                        CellSelectionMark.Size = new Vector2f(ir.MainSprite.GetGlobalBounds().Width, ir.MainSprite.GetGlobalBounds().Height);

                        if ((ir.MainSprite.Position + (Vector2f)ir.MainSprite.Texture.Size / 2 - MainEntity.Sprite.Position).Length() <= 400)
                        {
                            CellSelectionMark.OutlineColor = new Color(255, 255, 255, 255);
                            SelectedObject = ir;
                        }
                        else
                        {
                            CellSelectionMark.OutlineColor = new Color(130, 130, 130, 190);
                            SelectedObject = null;
                        }

                        break;
                    }
                }
            }

            if (SelectedItem is Handheld) ((Handheld)SelectedItem).Update();

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && SelectedItem != null)
            {
                if (SelectedObject != null)
                {
                    if (SelectedItem is Tool cur_tool)
                    {
                        if (cur_tool.Type == ToolType.Hoe)
                        {
                            if (((Cell)SelectedObject).ID != 4) ((Cell)SelectedObject).ID = 4;
                        }
                        else ((InteractableResource)SelectedObject).Interact(cur_tool);
                    }
                    else if (SelectedItem.ItemTags.Contains("Seed"))
                    {
                        if (((Cell)SelectedObject).ID == 4 && ((FarmCell)SelectedObject).PlantedPlant == null) ((FarmCell)SelectedObject).Plant(new Plant(Plant.AllPlants.Find(x => x.PlantSeed.ID == SelectedItem.ID)) { PlantedCell = (FarmCell)SelectedObject });
                    }
                }

                SelectedObject = null;
            }

            Program.MainView.Center = MainEntity.Sprite.Position;
            Program.MainWindow.SetView(Program.MainView);
        }
    }
}
