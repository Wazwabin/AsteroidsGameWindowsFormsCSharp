using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    class Star:BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        { }

        static Image img4 = Image.FromFile("Planet.png");

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img4, Pos.X, Pos.Y,  Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X*10;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

    }
}
