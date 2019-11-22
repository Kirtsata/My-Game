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
    class AnimatedSprite : Program
    {
        public List<Sprite> sprites = new List<Sprite>();
        public Sprite currentSprite;
        public bool sequential;

        public new int tick = 1;
        public int delay = 0;
        public int cDelay = 0; //const delay
        public int lastChange;
        public int perTicks;

        public bool hasStoppedState;
        public bool isRunningState;

        int current = 0;

        public AnimatedSprite(List<Texture> _sprites, bool _sequential, float _size, int _perTicks, int _delay = 0, int _startSprite = 0, bool _flipX = false, bool _flipY = false, float _rotation = 0f, bool _hasStoppedState = false, bool _isPlayedOnce = false)
        {
            foreach (var s in _sprites)
            {
                sprites.Add(new Sprite(s) { Scale = new Vector2f(_size, _size) });
                sprites[sprites.Count - 1].Rotation = _rotation;
                if (_flipX)
                    sprites[sprites.Count - 1].TextureRect = new IntRect((int)sprites[sprites.Count - 1].Texture.Size.X, 0, -(int)sprites[sprites.Count - 1].Texture.Size.X, (int)sprites[sprites.Count - 1].Texture.Size.Y);
                //if (_flipY)
                //    sprites[sprites.Count - 1].TextureRect = new IntRect(0, (int)sprites[sprites.Count - 1].Texture.Size.Y, (int)sprites[sprites.Count - 1].Texture.Size.X, -(int)sprites[sprites.Count - 1].Texture.Size.Y);
            }
            delay = _startSprite == 0 ? _delay : 0;
            cDelay = _delay;
            currentSprite = sprites[_startSprite];
            current = _startSprite;
            sequential = _sequential;
            perTicks = _perTicks;
            hasStoppedState = _hasStoppedState;
        }

        public void Update(Vector2f _position, bool runningState = false)
        {
            isRunningState = runningState;
            if (tick - delay > lastChange + perTicks)
            {
                delay = 0;
                lastChange = tick;
                if (current >= sprites.Count)
                {
                    delay = cDelay;
                    current = 0;
                }
                currentSprite = sprites[current];
                current = sequential ? current + 1 : rng.Next(sprites.Count);
            }
            if (hasStoppedState)
            {
                if (runningState && currentSprite == sprites[0])
                {
                    currentSprite = sprites[1];
                }
                else if (!runningState)
                {
                    current = 0;
                    currentSprite = sprites[current];
                }
            }
            currentSprite.Position = _position;
            tick++;
        }
    }
}
