using SFML.Graphics;
using SFML.System;
using System.Threading;

namespace Another_SFML_Project
{
    class Projectile : Program
    {
        public Sprite sprite;
        public Vector2f dir;
        public FloatRect collider;
        public bool Friendly = true;

        private int rotationTick = rng.Next(100);
        private int rotationSpeed = rng.Next(2) == 1 ? rng.Next(-15, -5) : rng.Next(5, 15);

        public Projectile(float X, float Y, float DirX, float DirY, Vector2f _size, Color color, Texture Texture)
        {
            playSound.Add(new Thread(() => PlaySound(OST.Fireball, true)));
            playSound[playSound.Count - 1].Start();
            sprite = new Sprite(Texture) { Position = new Vector2f(X, Y), Scale = new Vector2f(3 + _size.X, 3 + _size.Y), Origin = new Vector2f(Texture.Size.X / 2f, Texture.Size.Y / 2f), Rotation = rng.Next(100) };
            //sprite.Color = new Color(0, (byte)rng.Next(100, 255), (byte)rng.Next(200, 255));
            byte darken = (byte)rng.Next(25, 50);
            sprite.Color = new Color((byte)(color.R / (100 / darken)), (byte)(color.G / (100 / darken)), (byte)(color.B / (100 / darken)));
            collider = new FloatRect(sprite.GetGlobalBounds().Left + sprite.GetGlobalBounds().Width / 2 - sprite.Scale.X, sprite.GetGlobalBounds().Top + sprite.GetGlobalBounds().Height / 2 - sprite.Scale.Y, sprite.Scale.X * 2, sprite.Scale.Y * 2);
            dir = new Vector2f(DirX, DirY);
        }

        public void Update()
        {
            sprite.Position = new Vector2f(sprite.Position.X + dir.X, sprite.Position.Y + dir.Y);
            collider = new FloatRect(collider.Left + dir.X, collider.Top + dir.Y, collider.Width, collider.Height);
            sprite.Rotation = rotationTick;
            rotationTick += 5 + rotationSpeed;
        }
    }
}
