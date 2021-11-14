﻿global using SFML.Graphics;
global using SFML.Window;
global using SFML.System;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Threading.Tasks;
global using System.Net.Sockets;
global using BasicSFMLUI;
global using System.Linq;
global using Newtonsoft.Json;
global using System.Data;

namespace FarmShooter
{
    class Program
    {
        public static Socket MainSocket;

        public static List<Keyboard.Key> NumKeys = new List<Keyboard.Key> { Keyboard.Key.Num1, Keyboard.Key.Num2, Keyboard.Key.Num3, Keyboard.Key.Num4, Keyboard.Key.Num5, Keyboard.Key.Num6, Keyboard.Key.Num7, Keyboard.Key.Num8, Keyboard.Key.Num9, };

        public static DTRenderWindow MainWindow = new DTRenderWindow(new VideoMode(1200, 800), "test");
        public static View MainView = new View(new FloatRect(0, 0, MainWindow.Size.X, MainWindow.Size.Y));
        public static View UIView = new View(new FloatRect(0, 0, MainWindow.Size.X, MainWindow.Size.Y));

        public static Canvas UICanvas = new Canvas();

        public static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();
        public static Dictionary<string, object> OtherResources = new Dictionary<string, object>();

        public static Vector2f TrueMousePosition;

        public static int[,,] Map = new int[3, 20, 20];
        public static Cell[,] Field = new Cell[20, 20];
        public static Player Player;

        public static float Zoom = 1;

        static void LoadTexturesAndResources() 
        {
            List<string> files = Directory.GetFiles("Textures").ToList();
            files.AddRange(Directory.GetFiles("Resources"));

            foreach (var file in files) 
            {
                string ext = Path.GetExtension(file);

                if (ext == ".jpg" || ext == ".png")
                {
                    Textures.Add(Path.GetFileNameWithoutExtension(file), new Texture(file));
                }
                if (ext == ".ttf" || ext == ".otf") 
                {
                    OtherResources.Add(Path.GetFileNameWithoutExtension(file), new Font(file));
                }
            }
        }

        static void LoadingScreen() 
        {
            MainWindow.SetActive(true);

            var load = Task.Run(LoadTexturesAndResources);

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

        static void Menu() 
        {
            bool in_menu = true;

            MainWindow.Closed += (o, e) => { MainWindow.Close(); };
            MainWindow.MouseWheelScrolled += (o, e) =>
            {
                if ((e.Delta > 0 && Zoom < 2) || Zoom >= 2) Zoom += e.Delta;
                MainView.Size = ((Vector2f)MainWindow.Size) * Zoom;
            };
            MainWindow.Resized += (o, e) =>
            {
                MainView.Size = new Vector2f(e.Width, e.Height) * Zoom;
                UIView.Size = new Vector2f(e.Width, e.Height);
                UIView.Center = new Vector2f(e.Width / 2, e.Height / 2);
                UICanvas.SetPlaneSize(new Vector2f(e.Width, e.Height));
            };

            MainWindow.SetFramerateLimit(60);

            Text GameTitle = new Text() { Font = (Font)OtherResources["NeutralFace-Bold"], DisplayedString = "[БЕЗ НАЗВАНИЯ]", CharacterSize = 60 };

            Text PlayButtonText = new Text() { Font = (Font)OtherResources["NeutralFace-Bold"], DisplayedString = "ИГРАТЬ" };
            Text ExitButtonText = new Text() { Font = (Font)OtherResources["NeutralFace-Bold"], DisplayedString = "ВЫЙТИ" };

            Text MadeBy = new Text() { Font = (Font)OtherResources["NeutralFace-Bold"], DisplayedString = "СДЕЛАНО МА\'АГ БАГОМ", CharacterSize = 24};
            MadeBy.Origin = new Vector2f(0, MadeBy.CharacterSize + 5);

            UICanvas.Items.Add(new (new Vector2f(0.015F, 0.035F), GameTitle));
            UICanvas.Items.Add(new (new Vector2f(0.035F, 0.3F), PlayButtonText));
            UICanvas.Items.Add(new (new Vector2f(0.035F, 0.4F), ExitButtonText));
            UICanvas.Items.Add(new (new Vector2f(0.015F, 1), MadeBy));

            UICanvas.SetPlaneSize((Vector2f)MainWindow.Size);

            Button PlayButton = new Button(MainWindow, PlayButtonText.GetGlobalBounds());
            PlayButton.Clicked += (o, e) => { in_menu = false; };

            Button ExitButton = new Button(MainWindow, ExitButtonText.GetGlobalBounds());
            ExitButton.Clicked += (o, e) => { MainWindow.Close(); };

            EventHandler<SizeEventArgs> RepositionButtonsOnResize = (o, e) =>
            {
                PlayButton.BoundingBox = PlayButtonText.GetGlobalBounds();
                ExitButton.BoundingBox = ExitButtonText.GetGlobalBounds();
            };

            MainWindow.Resized += RepositionButtonsOnResize;

            while (MainWindow.IsOpen && in_menu) 
            {
                MainWindow.DispatchEvents();
                MainWindow.UpdateDeltaTime();

                MainWindow.Clear();

                MainWindow.SetView(UIView);

                MainWindow.Draw(UICanvas);

                MainWindow.Display();
            }

            UICanvas.Items.Remove(new(new Vector2f(0.015F, 0.035F), GameTitle));
            UICanvas.Items.Remove(new(new Vector2f(0.035F, 0.3F), PlayButtonText));
            UICanvas.Items.Remove(new(new Vector2f(0.035F, 0.4F), ExitButtonText));
            UICanvas.Items.Remove(new(new Vector2f(0.015F, 1), MadeBy));

            MainWindow.Resized -= RepositionButtonsOnResize;

            PrepareSinglePlayer();
        }

        static void PrepareSinglePlayer()
        {
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

            Cell.CellUpdated += (o, e) =>
            {
                Map[1, e.X, e.Y] = ((Cell)o).ID;

                if (Map[1, e.X, e.Y] == 4)
                {
                    FarmCell q = new FarmCell();
                    q.MainSprite.Position = new Vector2f(100 * e.X, 100 * e.Y);
                    q.MapCoords = e;

                    Field[e.X, e.Y] = q;
                }
            };

            FileStream file = new FileStream("BaseHandhelds.json", FileMode.Open);
            Handheld.LoadHandhelds(new StreamReader(file).ReadToEnd());
            file.Dispose();
        }

        static void PrepaseMultiPlayer() { }

        static void MainGameScreen() 
        {
            TransformableVertexArray UIInvenotyBar = new TransformableVertexArray();

            UIInvenotyBar.Vertexes.Append(new Vertex(new Vector2f(0, 0), new Color(40, 40, 40, 120)));
            UIInvenotyBar.Vertexes.Append(new Vertex(new Vector2f(800, 0), new Color(40, 40, 40, 120)));
            UIInvenotyBar.Vertexes.Append(new Vertex(new Vector2f(800, 100), new Color(40, 40, 40, 255)));
            UIInvenotyBar.Vertexes.Append(new Vertex(new Vector2f(0, 100), new Color(40, 40, 40, 255)));
            UIInvenotyBar.Vertexes.PrimitiveType = PrimitiveType.Quads;
            UIInvenotyBar.Origin = new Vector2f(400, 100);

            UICanvas.Items.Add(new(new Vector2f(0.5F, 1), UIInvenotyBar));

            RectangleShape UIInventoryCell = new RectangleShape(new Vector2f(80, 80)) { FillColor = new Color(0, 0, 0, 0), OutlineColor = new Color(160, 160, 160), OutlineThickness = -8 };

            Sprite UIInventoryItem;

            UICanvas.SetPlaneSize((Vector2f)MainWindow.Size);

            Player = new Player() { MainEntity = new Entity(Textures["Player"]) { Speed = 500 }};
            Player.Start();

            while (MainWindow.IsOpen) 
            {
                MainWindow.DispatchEvents();
                MainWindow.UpdateDeltaTime();

                TrueMousePosition = ((Vector2f)Mouse.GetPosition(MainWindow) - (Vector2f)MainWindow.Size) * Zoom + MainWindow.GetView().Center + MainWindow.GetView().Size / 2;

                Player.Update();

                MainWindow.Clear(new Color(63, 132, 229));

                foreach (var tile in Field)
                {
                    if (tile.MainSprite.GetGlobalBounds().Intersects(new FloatRect(MainView.Center - MainView.Size / 2, MainView.Size)))
                    {
                        MainWindow.Draw(tile);
                    }
                }

                MainWindow.Draw(Player);

                MainWindow.SetView(UIView);

                MainWindow.Draw(UICanvas);

                for (int i = 0; i < 8; ++i) 
                {
                    UIInventoryCell.Position = UIInvenotyBar.Position - UIInvenotyBar.Origin + new Vector2f(100 * i + ((100 - 80) / 2), 10);

                    UIInventoryCell.OutlineColor = new Color(160, 160, 160);
                    if (Player.Inventory[0, i] == Player.SelectedItem) UIInventoryCell.OutlineColor = new Color(200, 200, 200);

                    MainWindow.Draw(UIInventoryCell);

                    if (Player.Inventory[0, i] != null)
                    {
                        UIInventoryItem = new Sprite(Player.Inventory[0, i].InventorySprite) 
                        { 
                            Position = UIInventoryCell.Position - new Vector2f(UIInventoryCell.OutlineThickness, UIInventoryCell.OutlineThickness) + new Vector2f(3, 3)
                        };

                        MainWindow.Draw(UIInventoryItem);
                    }
                }

                MainWindow.SetView(MainView);

                MainWindow.Display();
            }
        }

        static void Main()
        {
            MainWindow.SetActive(false);

            LoadingScreen();
            Menu();
            MainGameScreen();

            //using FileStream file = new FileStream("test.json", FileMode.OpenOrCreate);
            //using StreamReader reader = new StreamReader(file);
            //using StreamWriter writer = new StreamWriter(file);

            //DataTable table = new DataTable();
            //table.Columns.Add("ID", typeof(int));
            //table.Columns.Add("HandheldType", typeof(string));
            //table.Columns.Add("TextureName", typeof(string));
            //table.Columns.Add("InventoryTextureName", typeof(string));
            //table.Columns.Add("Name", typeof(string));
            //table.Columns.Add("Material", typeof(MaterialType));
            //table.Columns.Add("Type", typeof(ToolType));
            //table.Columns.Add("Efficiency", typeof(int));

            //table.Rows.Add(0, "Tool", "Base_Axe_Iron", "Base_Axe_Iron_Inventory", "Iron Axe", MaterialType.Iron, ToolType.Axe, 30);
            //table.Rows.Add(1, "Tool", "Base_Hoe_Iron", "Base_Hoe_Iron_Inventory", "Iron Hoe", MaterialType.Iron, ToolType.Hoe, 30);
            //table.Rows.Add(2, "Tool", "Base_Pickaxe_Iron", "Base_Pickaxe_Iron_Inventory", "Iron Pickaxe", MaterialType.Iron, ToolType.Pickaxe, 30);

            //writer.WriteLine(JsonConvert.SerializeObject(table));
        }
    }
}
