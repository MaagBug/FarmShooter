using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace FarmShooter
{
    class Player
    {
        public Entity MainEntity;

        public bool Paused = false;

        public RectangleShape CellSelectionMark = new RectangleShape() { OutlineThickness = 9, OutlineColor = new Color(255, 255, 255), FillColor = new Color(0, 0, 0, 0) };
        public Cell SelectedCell;

        public Handheld CurrentHandheld;
        public ImagePoint Handle;

        public void Start() 
        {
            Program.OtherDrawable.Add(CellSelectionMark);
            Handle = new ImagePoint() { Point = MainEntity.Sprite.Position + new Vector2f(100, 0) };
        }

        public void Update() 
        {
            int vert_input = 0;
            int horiz_input = 0;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) vert_input = -1;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) vert_input = 1;

            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) horiz_input = -1;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) horiz_input = 1;

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

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && SelectedCell != null) 
            {
                SelectedCell.ID = 1;
            }

            Program.MainWindow.SetView(new View(Program.MainWindow.GetView()) { Center = MainEntity.Sprite.Position });
        }
    }
}
