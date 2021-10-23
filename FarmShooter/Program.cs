using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace FarmShooter
{
    class Program
    {
        public static Socket MainSocket;

        public static DTRenderWindow MainWindow = new DTRenderWindow(new VideoMode(800, 600), "test");

        public static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        public static Vector2f TrueMousePosition;

        public static int[,,] Map = new int[3, 20, 20];
        public static Cell[,] Field = new Cell[20, 20];

        public static List<Drawable> OtherDrawable = new List<Drawable>();
        public static float Zoom = 1;

        static void LoadTextures() 
        {
            string[] files = Directory.GetFiles("Textures");

            foreach (var file in files) 
            {
                string ext = Path.GetExtension(file);

                if (ext == ".jpg" || ext == ".png")
                {
                    Textures.Add(Path.GetFileNameWithoutExtension(file), new Texture(file));
                }
            }
        }

        static void LoadingScreen() 
        {
            var load = Task.Run(LoadTextures);

            CircleShape load_shape = new CircleShape() { Radius = 30, Origin = new Vector2f(30, 30), Position = new Vector2f(400, 300), OutlineThickness = 5, OutlineColor = new Color(0, 0, 0) };
            load_shape.SetPointCount(4);

            while (MainWindow.IsOpen) 
            {
                MainWindow.DispatchEvents();
                MainWindow.UpdateDeltaTime();

                load_shape.Rotation += MainWindow.DeltaTime.AsSeconds() * 100;

                MainWindow.Clear(new Color(90, 90, 90));

                MainWindow.Draw(load_shape);

                MainWindow.Display();

                if (load.IsCompleted) break;
            }
        }

        static void MainGameScreen() 
        {
            if (MainSocket == null)
            {
                Cell.CellChanged += (o, e) =>
                {
                    Map[1, e.X, e.Y] = ((Cell)o).ID;
                    Field[e.X, e.Y] = new Cell(((Cell)o).ID);
                    Field[e.X, e.Y].MainSprite.Position = new Vector2f(e.X * Field[e.X, e.Y].MainSprite.Texture.Size.X, e.Y * Field[e.X, e.Y].MainSprite.Texture.Size.Y);
                };
            }
            else 
            {
                Cell.CellChanged += (o, e) =>
                {

                };
            }

            Player Player = new Player() { MainEntity = new Entity(Textures["Player"]) { Speed = 500 }};
            Player.Start();

            Random rand = new Random();
            for (int i = 0; i < Field.GetUpperBound(0) + 1; ++i) 
            {
                for (int k = 0; k < Field.GetUpperBound(1) + 1; ++k) 
                {
                    Map[1, i, k] = rand.Next(0, 4);
                    Field[i, k] = new Cell(Map[1, i, k]) { MapCoords = new Vector2i(i, k) };
                    Field[i, k].MainSprite.Position = new Vector2f(i * Field[i, k].MainSprite.Texture.Size.X, k * Field[i, k].MainSprite.Texture.Size.Y);
                }
            }

            while (MainWindow.IsOpen) 
            {
                MainWindow.DispatchEvents();
                MainWindow.UpdateDeltaTime();

                TrueMousePosition = ((Vector2f)Mouse.GetPosition(MainWindow) - MainWindow.GetView().Size / Zoom) * Zoom + MainWindow.GetView().Center + MainWindow.GetView().Size / 2;

                Player.Update();

                MainWindow.Clear(new Color(63, 132, 229));

                foreach (var tile in Field)
                {
                    MainWindow.Draw(tile);
                }

                foreach (var item in OtherDrawable) 
                {
                    MainWindow.Draw(item);
                }

                MainWindow.Draw(Player.MainEntity);

                MainWindow.Display();
            }
        }

        static void Main()
        {
            MainWindow.Closed += (o, e) => { MainWindow.Close(); };
            MainWindow.MouseWheelScrolled += (o, e) =>
            {
                if ((e.Delta > 0 && Zoom < 2) || Zoom >= 2) Zoom += e.Delta;
                MainWindow.SetView(new View(MainWindow.GetView()) { Size = ((Vector2f)MainWindow.Size) * Zoom });
            };
            MainWindow.Resized += (o, e) => 
            {
                Zoom = 1;
                MainWindow.SetView(new View(MainWindow.GetView()) { Size = new Vector2f(e.Width, e.Height) });
            };

            LoadingScreen();
            MainGameScreen();
        }
    }
}
