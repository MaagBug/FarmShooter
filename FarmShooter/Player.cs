namespace FarmShooter
{
    enum PlayerState { None, Paused, Texting, Crafting }

    class Player : Drawable
    {
        public Entity MainEntity;

        public Item[,] Inventory = new Item[3, 8];
        public Item SelectedItem;

        public PlayerState State = PlayerState.None;

        public bool Paused = false;

        public RectangleShape CellSelectionMark = new RectangleShape() { OutlineThickness = 9, OutlineColor = new Color(255, 255, 255), FillColor = new Color(0, 0, 0, 0) };
        public object SelectedObject;

        public Handheld CurrentHandheld;
        public ImagePoint Handle;

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(CellSelectionMark);
            target.Draw(MainEntity);
            if (CurrentHandheld != null) target.Draw(CurrentHandheld);
            else if (SelectedItem != null) target.Draw(new Sprite(SelectedItem.InventorySprite) { Position = MainEntity.Sprite.Position });
        }

        public void Start() 
        {
            Handle = new ImagePoint() { Point = new Vector2f(25, 0) };
            Inventory[0, 0] = new Tool(0) { Owner = this };
            Inventory[0, 1] = new Tool(1) { Owner = this };
            Inventory[0, 2] = new Tool(2) { Owner = this };
            Inventory[0, 3] = new Item(0);
            Inventory[0, 4] = new Item(1);
            Inventory[0, 5] = new Item(2);
            Inventory[0, 6] = new Item(3);
            Inventory[0, 7] = new Item(4);
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
                    if (SelectedItem is Handheld) CurrentHandheld = SelectedItem as Handheld;
                    else CurrentHandheld = null;

                    break;
                }
            }

            MainEntity.Sprite.Position += new Vector2f(horiz_input, vert_input).Normalized() * MainEntity.Speed * Program.MainWindow.DeltaTime.AsSeconds();
            Handle.Angle = MathF.Atan2(Program.TrueMousePosition.Y - MainEntity.Sprite.Position.Y, Program.TrueMousePosition.X - MainEntity.Sprite.Position.X) * (180 / 3.14F);

            CellSelectionMark.OutlineColor = new Color(255, 255, 255, 0);

            if (CurrentHandheld is Tool || (SelectedItem != null && SelectedItem.ItemTags.Contains("Seed")))
            {
                if ((CurrentHandheld is Tool tool && tool.Type == ToolType.Hoe) || SelectedItem.ItemTags.Contains("Seed"))
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
                    foreach (var ir in Program.interactableResources) 
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
            }

            if (CurrentHandheld != null) CurrentHandheld.Update();

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && SelectedItem != null) 
            {
                if (SelectedObject != null) 
                {
                    if (CurrentHandheld is Tool cur_tool && cur_tool.Type == ToolType.Hoe)
                    {
                        if (((Cell)SelectedObject).ID != 4) ((Cell)SelectedObject).ID = 4;
                    }
                    else if (SelectedItem.ItemTags.Contains("Seed")) 
                    {
                        if (((Cell)SelectedObject).ID == 4) ((FarmCell)SelectedObject).Plant(Plant.AllPlants.Find(x => x.PlantSeed.ID == SelectedItem.ID).ID);
                    }
                }
            }

            Program.MainView.Center = MainEntity.Sprite.Position;
            Program.MainWindow.SetView(Program.MainView);
        }
    }
}
