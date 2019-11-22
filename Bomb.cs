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
    class Bomb : Program
    {
        public int lifetime;
        public new int tick;

        public AnimatedSprite sprite = new AnimatedSprite(Resources._bomb.ToList(), true, 4, 7);
        public Sprite sprite_explosion = new Sprite(Resources._explosion) { Scale = new Vector2f(6, 6), Origin = (Vector2f)Resources._explosion.Size / 2 * 6 };
        public Sprite shadow = new Sprite(Resources._shadow) { Scale = new Vector2f(5f, 5f) };
        public FloatRect AOE; // Area of effect
        public Vector2f Position;

        public Bomb(Vector2f pos, int LifeTime, float AreaOfEffect)
        {
            Vector2f temp = new Vector2f(AreaOfEffect, AreaOfEffect);
            sprite.Update(pos);
            Position = pos;
            lifetime = LifeTime;
            AOE = new FloatRect(pos.X + sprite.currentSprite.GetGlobalBounds().Width / 2 - AreaOfEffect, pos.Y + sprite.currentSprite.GetGlobalBounds().Height / 2 - AreaOfEffect, AreaOfEffect * 2f, AreaOfEffect * 2f);
            shadow.Position = new Vector2f(sprite.currentSprite.Position.X - 6, sprite.currentSprite.Position.Y - 0);
        }

        public void Update()
        {
            if (tick == lifetime)
                sprite.currentSprite = new Sprite(Resources._explosion) { Scale = new Vector2f(6, 6), Origin = (Vector2f)Resources._explosion.Size / 2, Position = sprite.currentSprite.Position + (Vector2f)sprite.currentSprite.Texture.Size / 2 * sprite.currentSprite.Scale.X, Rotation = rng.Next(360) };
            if (tick > lifetime)
            {
                shadow.Texture = null;
                sprite.currentSprite.Rotation = rng.Next(360);
            }

            if (tick % sprite.perTicks == 0 && tick < lifetime)
            {
                sprite.Update(Position);
                //currentSprite = currentSprite >= Resources._bomb.Length - 1 ? 0 : currentSprite + 1;
                //sprite.Texture = Resources._bomb[currentSprite];
            }
            if (tick % 10 == 0 && sprite.perTicks > 1) sprite.perTicks--;
            tick++;
        }
    }

}
