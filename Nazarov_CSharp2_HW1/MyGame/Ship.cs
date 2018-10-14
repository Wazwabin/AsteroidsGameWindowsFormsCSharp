using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Ship:BaseObject
    {
        private int _hp = 100;
        public int HP => _hp;

        public void HPRise(int n)
        {
            _hp += n;
        }

        public void HPLow(int n)
        {
            _hp -= n;
        }

        static Image img3 = Image.FromFile("Ship.png");

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img3, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
        }
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }
        public void Die()
        {
            MessageDie?.Invoke();
        }

        public static event Message MessageDie;
    }
}
