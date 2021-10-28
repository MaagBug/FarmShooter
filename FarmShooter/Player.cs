using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace FarmShooter
{
    class Player : Drawable
    {
        public Entity MainEntity;

        public Item[,] Inventory = new Item[3, 9];
        public Item SelectedItem;

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
        }

        public void Start() 
        {
            Handle = new ImagePoint() { Point = new Vector2f(0, 0) };
            Inventory[0, 0] = new Tool(0, "Axe", "Base_Axe_Iron", "", ToolType.Axe) { Owner = this };
            Inventory[0, 1] = new Tool(0, "Hoe", "Base_Hoe_Iron", "", ToolType.Hoe) { Owner = this };
            Inventory[0, 2] = new Tool(0, "Pickaxe", "Base_Pickaxe_Iron", "", ToolType.Pickaxe) { Owner = this };
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
                SelectedCell.ID = 1;
            }

            Program.MainView.Center = MainEntity.Sprite.Position;
            Program.MainWindow.SetView(Program.MainView);
        }
    }
}
