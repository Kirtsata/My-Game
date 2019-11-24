using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
using System.IO;

namespace Another_SFML_Project
{
    class UI_Minimap : Program
    {
        public new List<Vector2f> grid = new List<Vector2f>();
        public List<MiniRoom> miniRooms = new List<MiniRoom>();
        public bool[] isRoomExplored = new bool[roomCount];
        public int number;

        public UI_Minimap(int Number)
        {
            //Initializing grid
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grid.Add(new Vector2f(cResolution.X - 9 * 30 + j * 30, i * 30));

            number = Number;

            if (File.Exists(@"Target\minimap_layout.txt"))
            {
                List<string> minimap = File.ReadAllLines(@"Target\minimap_layout.txt").ToList();
                foreach (var m in minimap)
                {
                    string[] mLine = m.Split(' ');

                    List<int> doorsToAdd = new List<int>();

                    if (mLine[1] != "null")
                    {
                        for (int i = 2; i < mLine.Length - 1; i++)
                            doorsToAdd.Add(int.Parse(mLine[i]));

                        miniRooms.Add(new MiniRoom(doorsToAdd, int.Parse(mLine[0])));
                    }
                }

                for (int i = 0; i < grid.Count; i++)
                    for (int j = 0; j < miniRooms.Count; j++)
                        if (miniRooms[j].pos == i) miniRooms[j].sprite.Position = grid[i];

            }

        }

        public void Update()
        {
            for (int i = 0; i < miniRooms.Count; i++)
                miniRooms[i].sprite.Color = new Color(255, 255, 255, (number == i) ? (byte)255 : ((isRoomExplored[i]) ? (byte)100 : (byte)0));
        }

        public void AddExplored(int exploredNumber)
        {
            isRoomExplored[exploredNumber] = true;
        }

        public class MiniRoom
        {
            public bool[] doors = new bool[4];
            public Sprite sprite;
            public int pos;

            public MiniRoom(List<int> Doors, int Pos)
            {
                foreach (var d in Doors)
                    doors[d] = true;

                pos = Pos;

                if (doors[0] && !doors[1] && !doors[2] && !doors[3]) sprite = new Sprite(Resources._L);
                if (!doors[0] && doors[1] && !doors[2] && !doors[3]) sprite = new Sprite(Resources._U);
                if (!doors[0] && !doors[1] && doors[2] && !doors[3]) sprite = new Sprite(Resources._R);
                if (!doors[0] && !doors[1] && !doors[2] && doors[3]) sprite = new Sprite(Resources._D);

                if (doors[0] && doors[1] && !doors[2] && !doors[3]) sprite = new Sprite(Resources._UL);
                if (!doors[0] && doors[1] && doors[2] && !doors[3]) sprite = new Sprite(Resources._UR);
                if (doors[0] && doors[1] && doors[2] && !doors[3]) sprite = new Sprite(Resources._ULR);

                if (doors[0] && !doors[1] && !doors[2] && doors[3]) sprite = new Sprite(Resources._DL);
                if (!doors[0] && !doors[1] && doors[2] && doors[3]) sprite = new Sprite(Resources._DR);
                if (doors[0] && !doors[1] && doors[2] && doors[3]) sprite = new Sprite(Resources._DLR);

                if (doors[0] && !doors[1] && doors[2] && !doors[3]) sprite = new Sprite(Resources._LR);
                if (!doors[0] && doors[1] && !doors[2] && doors[3]) sprite = new Sprite(Resources._UD);

                if (doors[0] && doors[1] && !doors[2] && doors[3]) sprite = new Sprite(Resources._UDL);
                if (!doors[0] && doors[1] && doors[2] && doors[3]) sprite = new Sprite(Resources._UDR);
                if (doors[0] && doors[1] && doors[2] && doors[3]) sprite = new Sprite(Resources._ALL);

                sprite.Scale = new Vector2f(3, 3);
            }
        }
    }
}
