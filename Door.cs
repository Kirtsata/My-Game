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
    class Door : Program
    {
        public Sprite sprite;
        public int leadsTo;
        public int position; // | 0 - Left | 1 - Up | 2 - Right | 3 - Bottom |
        public bool isOpen = false;
        public bool lastState;
        public bool forceOpen = false;
        public FloatRect collider;

        public Door(int Position, int LeadsTo, bool IsOpen = false)
        {
            position = Position;
            leadsTo = LeadsTo;
            isOpen = IsOpen;
            lastState = isOpen;

            Vector2f tempPos = new Vector2f();
            int rotation = 0;
            switch (position)
            {
                case 0:
                    rotation = 90;
                    tempPos = new Vector2f(0, cResolution.Y / 2 - 10);
                    collider = new FloatRect(tempPos.X, tempPos.Y - Resources._doorClosed.Size.Y * 5, Resources._doorClosed.Size.Y * 10, Resources._doorClosed.Size.Y * 10);
                    break;
                case 1:
                    rotation = 180;
                    tempPos = new Vector2f(cResolution.X / 2 + 10, 0);
                    collider = new FloatRect(tempPos.X - Resources._doorClosed.Size.Y * 6, tempPos.Y, Resources._doorClosed.Size.Y * 11, Resources._doorClosed.Size.Y * 10);
                    break;
                case 2:
                    rotation = -90;
                    tempPos = new Vector2f(cResolution.X, cResolution.Y / 2);
                    collider = new FloatRect(tempPos.X, tempPos.Y - Resources._doorClosed.Size.Y * 5, Resources._doorClosed.Size.Y * -10, Resources._doorClosed.Size.Y * 10);
                    break;
                case 3:
                    rotation = 0;
                    tempPos = new Vector2f(cResolution.X / 2, cResolution.Y);
                    collider = new FloatRect(tempPos.X - Resources._doorClosed.Size.Y * 5, tempPos.Y, Resources._doorClosed.Size.Y * 11, Resources._doorClosed.Size.Y * -10);
                    break;
                default:
                    Console.WriteLine($"Kiril Error: Poisiont was set to {position}...");
                    break;
            }

            sprite = new Sprite(isOpen ? Resources._doorOpened : Resources._doorClosed)
            {
                Position = tempPos,
                Scale = new Vector2f(10, 10),
                Origin = new Vector2f(Resources._doorClosed.Size.X / 2, Resources._doorClosed.Size.Y),
                Rotation = rotation
            };
        }

        public void Update()
        {
            if (lastState != isOpen)
            {
                if (isOpen) sprite.Texture = new Texture(Resources._doorOpened);
                else sprite.Texture = new Texture(Resources._doorClosed);
                lastState = isOpen;
            }
        }

    }
}
