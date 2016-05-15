using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsPlatformer.GameObjects;

namespace WindowsFormsPlatformer
{
    public partial class MainForm : Form
    {
        Bitmap BackBuffer;
        IList<Keys> KeysPressed = new List<Keys>();
        GameContext Context;

        public MainForm()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            CreateContext();
            
            Timer GameTimer = new Timer();
            GameTimer.Interval = 1;
            GameTimer.Tick += new EventHandler(Tick);
            GameTimer.Start();

            ResizeEnd += new EventHandler(CreateBackBuffer);
            Load += new EventHandler(CreateBackBuffer);
            base.Paint += new PaintEventHandler(Paint);
            base.KeyDown += new KeyEventHandler(KeyDown);
            base.KeyUp += new KeyEventHandler(KeyUp);
        }

        new void KeyDown(object sender, KeyEventArgs e)
        {
            if (!KeysPressed.Contains(e.KeyCode))
                KeysPressed.Add(e.KeyCode);
            Context.World.KeyDown(e.KeyCode);
        }

        new void KeyUp(object sender, KeyEventArgs e)
        {
            KeysPressed.Remove(e.KeyCode);
        }

        new void Paint(object sender, PaintEventArgs e)
        {
            if (BackBuffer != null)
            {
                e.Graphics.DrawImageUnscaled(BackBuffer, Point.Empty);
            }
        }

        void CreateBackBuffer(object sender, EventArgs e)
        {
            if (BackBuffer != null)
                BackBuffer.Dispose();

            BackBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            Context.WindowSize = new Size(ClientSize.Width, ClientSize.Height);
        }

        void Tick(object sender, EventArgs e)
        {
            if (Context.IsQuit)
            {
                Application.Exit();
            }

            Context.World.HandleKeyboardState(KeysPressed);

            //process world
            Context.World.Clean();
            Context.World.ProcessPhysics();
            Context.World.DetectCollisions();
            Context.World.Animate();

            if (BackBuffer != null)
            {
                using (var renderer = Graphics.FromImage(BackBuffer))
                {
                    renderer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    renderer.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                    //clear screen
                    renderer.Clear(Color.White);

                    //render
                    Context.World.Render(renderer);
                    Context.UI.Render(renderer);
                }

                //update screen
                Invalidate();
            }
        }

        void CreateContext()
        {
            Context = new GameContext(new Size(ClientSize.Width, ClientSize.Height));

            var player = new Player(Context, new Rect(0, 20, 10, 20));
            player.Speed = 1.3;
            player.JumpSpeed = 2.5;
            player.Physics.GravityForce = 0.1;
            player.AddChild(Context.World.Camera);
            Context.World.AddChild(player);

            var frame = new Frame(Context, new Rect(0, 0, Context.World.Frame.Size.Width, Context.World.Frame.Size.Height), 10);
            frame.Ceiling.RenderObject = RenderObject.FromColor(Color.Black);
            frame.WallLeft.RenderObject = RenderObject.FromColor(Color.Black);
            frame.WallRight.RenderObject = RenderObject.FromColor(Color.Black);
            frame.Floor.RenderObject = RenderObject.FromColor(Color.Black);
            Context.World.AddChild(frame);
            
            var rnd = new Random();
            int count = 200;
            int powerCount = 100;
            int x = (int)(Context.World.Frame.Size.Width / 10 - 2);
            int y = (int)(Context.World.Frame.Size.Height / 10 - 2);
            int rndX, rndY;
            int[] takenX = new int[count];
            int[] takenY = new int[count];
            for (int i = 0; i < count; i++)
            {
                bool taken;
                do
                {
                    taken = false;
                    rndX = rnd.Next() % x;
                    rndY = rnd.Next() % y;
                    for (int j = 0; j <= i; j++)
                    {
                        if (rndX == takenX[j] && rndY == takenY[j])
                        {
                            taken = true;
                            break;
                        }
                    }
                } while (taken);

                takenX[i] = rndX;
                takenY[i] = rndY;

                if (powerCount > 0)
                {
                    var gameObject = new Consumable(Context, new Rect(
                        Context.World.Frame.Size.Width / 2 - 15 - rndX * 10,
                        Context.World.Frame.Size.Height / 2 - 15 - rndY * 10,
                        10,
                        10));
                    gameObject.RenderObject = RenderObject.FromColor(Color.FromArgb(0x80, 0x00, 0xFF, 0x00));
                    Context.World.AddChild(gameObject);
                    powerCount--;

                }
                else
                {
                    var gameObject = new Solid(Context, new Rect(
                        Context.World.Frame.Size.Width / 2 - 15 - rndX * 10,
                        Context.World.Frame.Size.Height / 2 - 15 - rndY * 10,
                        10,
                        10));
                    gameObject.RenderObject = new RenderObject(Properties.Resources.brick);
                    Context.World.AddChild(gameObject);
                }
            }
        }
    }
}
