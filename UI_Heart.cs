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
    class UI_Heart : Program
    {
        public List<Sprite> sprites = new List<Sprite>();
        Texture[] textures;
        int count;

        public UI_Heart(Texture[] Textures, int Count)
        {
            textures = Textures;
            count = Count;
            for (int i = 0; i < 3; i++)
                sprites.Add(new Sprite(textures[0]) { Position = new Vector2f(90 + i * textures[0].Size.X * 4, 0), Scale = new Vector2f(4, 4) });
        }

        public void Update(int Health)
        {
            count = Health;
            foreach (var s in sprites)
                s.Texture = textures[2];
            for (int i = 0; i < count; i++)
            {
                sprites[i / 2].Texture = textures[0];
                if (i % 2 == 0)
                    sprites[i / 2].Texture = textures[1];
            }
        }
    }
}
