using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;

namespace Another_SFML_Project
{
    class Grid : Program
    {
        //public FloatRect[,] grid = new FloatRect[(int)(1200 / (13 * 2)), (int)(800 / (13 * 2))];
        public List<FloatRect> grid = new List<FloatRect>();

        public Grid(float Width, float Height, float Size, float Offset)
        {
            for (int i = 0; i < Height / Size - 3; i++)
                for (int j = 0; j < Width / Size - 3; j++)
                    grid.Add(new FloatRect(Offset + j * Size, Offset + i * Size, Size, Size));
        }
    }
}
