using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
using System.Threading;

namespace SomeGame
{
    class Slider : Program
    {
        public Sprite slider = new Sprite(Resources._buttonSlider) { Scale = new Vector2f(7, 7), Origin = (Vector2f)Resources._buttonSlider.Size / 2 };
        public Sprite pointer = new Sprite(Resources._buttonPointer) { Scale = new Vector2f(7, 7), Origin = (Vector2f)Resources._buttonPointer.Size / 2 };

        public string controls;

        public float value = 0;
        public float lastValue;
        public int specialValue;

        public bool isSliding = false;

        public Slider(string Controls, int Value, Vector2f centredPosition)
        {
            slider.Position = centredPosition;
            pointer.Position = new Vector2f(slider.Position.X - slider.GetGlobalBounds().Width / 2, slider.Position.Y);
            controls = Controls;
            value = Value;
            lastValue = Value;
        }

        public void Update()
        {
            pointer.Position = new Vector2f((slider.Position.X - slider.GetGlobalBounds().Width / 2) + 7 * value, pointer.Position.Y);
            if (isSliding)
            {
                pointer.Position = new Vector2f(ToMousePos().X, pointer.Position.Y);

                //Checks if pointer is out of bounds
                if (pointer.Position.X < slider.Position.X - slider.GetGlobalBounds().Width / 2)
                    pointer.Position = new Vector2f(slider.Position.X - slider.GetGlobalBounds().Width / 2, pointer.Position.Y);
                if (pointer.Position.X > slider.Position.X + slider.GetGlobalBounds().Width / 2)
                    pointer.Position = new Vector2f(slider.Position.X + slider.GetGlobalBounds().Width / 2, pointer.Position.Y);

                value = (pointer.Position.X - (slider.Position.X - slider.GetGlobalBounds().Width / 2)) / 7;
                if (controls == "SFX")
                    if (tick % 15 == 0)
                    {
                        playSound.Add(new Thread(() => PlaySound(OST.Fireball, true)));
                        playSound[playSound.Count - 1].Start();
                    }
            }
            switch (controls)
            {
                case "SFX":
                    OST.Fireball.Volume = value;
                    OST.Explosion.Volume = value;
                    OST.Flesh.Volume = value;
                    OST.Slash.Volume = value;
                    break;
                case "Music":
                    OST.bg.Volume = value;
                    OST.menu.Volume = value;
                    OST.deathScreen.Volume = value;
                    break;
                case "Resolution":
                    if (optionsOn)
                        for (int i = resolutions.Length; i > 0; i--)
                            if (value <= (resolutions.Length - i + 1) * (100 / resolutions.Length))
                            {
                                specialValue = resolutions.Length - i;
                                window.Draw(resolutions[specialValue]);
                                break;
                            }
                    break;

                default:
                    Console.WriteLine("Kiril did an oopsie");
                    break;
            }
        }
    }
}
