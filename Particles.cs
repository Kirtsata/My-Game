using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;

namespace SomeGame
{
    class Particles : Program
    {
        public List<Sprite> sprites = new List<Sprite>();
        List<int> RandomRotation = new List<int>();
        List<Vector2f> dir = new List<Vector2f>();
        int intensity;
        new int tick;
        int lifespan;

        public Particles(Texture[] Textures, Vector2f Position, int Intensity, float Speed = 1, int Lifespan = 25, Color? CustomColor = null)
        {
            intensity = Intensity;
            lifespan = Lifespan;
            for (int i = 0; i < intensity; i++)
            {
                RandomRotation.Add(rng.Next(360));
                sprites.Add(new Sprite(Textures[rng.Next(Textures.Length)]) { Position = Position, Scale = new Vector2f(5, 5), Origin = new Vector2f(Textures[0].Size.X / 2f, Textures[0].Size.Y / 2f) });
                if (CustomColor != null) sprites[sprites.Count - 1].Color = (Color)CustomColor;
                dir.Add(new Vector2f((float)rng.Next(-500, 500) / 100 * Speed, (float)rng.Next(-500, 500) / 100 * Speed));
            }
        }

        public void Update()
        {
            tick += 15;
            for (int i = 0; i < sprites.Count; i++)
            {
                RandomRotation[i] += 15;
                sprites[i].Position = new Vector2f(sprites[i].Position.X + dir[i].X, sprites[i].Position.Y + dir[i].Y);
                sprites[i].Rotation = RandomRotation[i];
            }

            if (tick / 15 > lifespan && sprites[0].Scale.X > 0)
                for (int i = 0; i < intensity; i++)
                    sprites[i].Scale = new Vector2f(sprites[i].Scale.X - 0.5f, sprites[i].Scale.Y - 0.5f);

        }
    }
}
