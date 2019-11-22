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
    class UI_Stat : Program
    {
        public Sprite sprite = new Sprite() { Scale = new Vector2f(2, 2), Color = new Color(255, 255, 255, 125) };
        public Text text = new Text { Font = Resources._FontPixeled, CharacterSize = 35, Color = new Color(255, 255, 255, 125) };

        private double lastCount = -1;

        public UI_Stat(Texture _texture, int _slot)
        {
            sprite.Texture = _texture;
            sprite.Position = new Vector2f(10, 80 + _slot * sprite.GetGlobalBounds().Height);
        }

        public void Update(double _count)
        {
            _count = Math.Round(_count, 2);
            if (lastCount != _count)
            {
                lastCount = _count;
                text.Position = new Vector2f(sprite.Position.X + sprite.GetGlobalBounds().Width + 5, sprite.Position.Y + 7);
                text.DisplayedString = (_count >= 10) ? _count.ToString() : ("0" + _count.ToString());
            }
        }
    }
}
