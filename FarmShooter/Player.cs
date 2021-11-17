﻿namespace FarmShooter
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
        public Cell SelectedCell;

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
                    break;
                }
            }

            MainEntity.Sprite.Position += new Vector2f(horiz_input, vert_input).Normalized() * MainEntity.Speed * Program.MainWindow.DeltaTime.AsSeconds();
            Handle.Angle = MathF.Atan2(Program.TrueMousePosition.Y - MainEntity.Sprite.Position.Y, Program.TrueMousePosition.X - MainEntity.Sprite.Position.X) * (180 / 3.14F);

            CellSelectionMark.OutlineColor = new Color(255, 255, 255, 0);

            foreach (var tile in Program.Field)
            {
                if (tile.MainSprite.GetGlobalBounds().Contains(Program.TrueMousePosition.X, Program.TrueMousePosition.Y))
                {
                    CellSelectionMark.Position = tile.MainSprite.Position;
                    CellSelectionMark.Size = (Vector2f)tile.MainSprite.Texture.Size;

                    if ((tile.MainSprite.Position + (Vector2f)tile.MainSprite.Texture.Size / 2 - MainEntity.Sprite.Position).Length() <= 400)
                    {
                        CellSelectionMark.OutlineColor = new Color(255, 255, 255, 255);
                        SelectedCell = tile;
                    }
                    else
                    {
                        CellSelectionMark.OutlineColor = new Color(130, 130, 130, 190);
                        SelectedCell = null;
                    }
                    
                    break;
                }
            }

            if (CurrentHandheld != null) CurrentHandheld.Update();

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && SelectedCell != null) 
            {
                if (CurrentHandheld != null && CurrentHandheld is Tool) 
                {
                    switch (((Tool)CurrentHandheld).Type) 
                    {
                        case ToolType.Hoe:
                            if (SelectedCell.ID != 4) SelectedCell.ID = 4;
                            break;
                    }
                }
            }

            Program.MainView.Center = MainEntity.Sprite.Position;
            Program.MainWindow.SetView(Program.MainView);
        }
    }
}
