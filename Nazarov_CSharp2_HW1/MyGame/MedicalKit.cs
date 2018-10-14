using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class MedicalKit:BaseObject
    {
        public int Power { get; set; }
        static public Random rnd = new Random();

        static Image img5 = Image.FromFile("MedicalKit.png");

        public MedicalKit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 20;
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img5, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X * 10;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
}

