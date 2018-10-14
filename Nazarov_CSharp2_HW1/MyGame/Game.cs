using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
        static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        private static Timer timer = new Timer ();
        public static Random Rnd = new Random();
        public static BaseObject[] _objs;
        private static List<Bullet> _bullets = new List<Bullet>(1000);
        private static List <Asteroid> _asteroids;
        private static List<MedicalKit> medkit;
        static public Random rnd = new Random();

        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(70, 70));

        public static void Load()

        {
            var rnd = new Random();
            _objs = new BaseObject[10];
            _asteroids = new List <Asteroid>(20);
            medkit = new List<MedicalKit>(5);
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 10);
                _objs[i] = new Star(new Point(1000, Game.rnd.Next(10, Game.Height)), new Point(-r / 4, r), new Size(100,100));
            }
            for (var i = 0; i < 20; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids.Add (new Asteroid(new Point(1200, Game.rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));
            }
            for (var i = 0; i < 5; i++)
            {
                int r = rnd.Next(1, 5);
                medkit.Add(new MedicalKit(new Point(800, Game.rnd.Next(0, Game.Height)), new Point(-r / 6, r), new Size(50, 50)));
            }
        }
        /// <summary>
        /// Свойства игрового поля (ширина и высота)
        /// </summary>
        public static int width;
        public static int Height { get; set; }

        static public int Width
        {
            get { return width; }
            set { if (value < 200 || value > 1000) throw new ArgumentOutOfRangeException("Введите значения ширины 200-1000");
                else width = value;
            }
        }
        
        static Game()
        {
        }
        public static void Init(Form form)
        {
            //form.BackgroundImage B_img = Image.FromFile("BackGRDImg.jpg");

            Graphics g;
            
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            
            Width = 1000;
            Height = form.Height;
            
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            
            timer.Start();
            timer.Tick += Timer_Tick;

            Load();

            form.KeyDown += Form_KeyDown;

            Ship.MessageDie += Finish;
        }
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullets.Add(new Bullet (new Point(_ship.Rect.X + 10, _ship.Rect.Y + 28), new Point(4, 0), new Size(40, 10)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }
            foreach (Bullet b in _bullets) b.Draw();
            _ship?.Draw();
            foreach (MedicalKit a in medkit)
            {
                a?.Draw();
            }
            if (_ship != null)
                Buffer.Graphics.DrawString("HP:" + _ship.HP, SystemFonts.DefaultFont, Brushes.White, 20, 20);
            Buffer.Render();
        }
        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            foreach (Bullet b in _bullets) b.Update();
            for (var i = 0; i < _asteroids.Count; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                for (int j = 0; j < _bullets.Count; j++)
                    if (_asteroids[i] != null && _bullets[j].Collision(_asteroids[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids[i]=null;
                        _bullets.RemoveAt(j);
                        j--;
                    }
                if (_asteroids[i] == null || !_ship.Collision(_asteroids[i])) continue;
                _ship.HPLow(Rnd.Next(1, 10));
                System.Media.SystemSounds.Asterisk.Play();
                if (_ship.HP <= 0) _ship.Die();
            }

            foreach (MedicalKit a in medkit)
            {
                a.Update();
            }

            /*for (var i = 0; i < medkit.Count; i++)
            {
                medkit[i].Update();
                if (medkit[i] == null) continue;
                medkit[i].Update();
                for (int j = 0; j < medkit.Count; j++)
                    if (medkit[i] != null && _ship.Collision(medkit[i]))
                    {
                        System.Media.SystemSounds.Question.Play();
                        medkit[i] = null;
                    }
                if (medkit[i] == null || !_ship.Collision(medkit[i])) continue;
                _ship.HPRise(Rnd.Next(1, 20));
               System.Media.SystemSounds.Asterisk.Play(); 
        }*/
        }
        public static void Finish()
        {
            timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }
    }
}
