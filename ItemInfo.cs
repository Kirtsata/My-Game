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
    class ItemInfo
    {
        public Sprite bg;
        public Text text = new Text() { Font = Resources._FontPixeled, CharacterSize = 90};
        public Text subtext = new Text() { Font = Resources._FontPixeled, CharacterSize = 30 };
        public bool isWorking;
        Vector2u cResolution;

        public ItemInfo(Vector2u _cResolution)
        {
            cResolution = _cResolution;
            bg = new Sprite(Resources._itemInfo) { Position = new Vector2f(cResolution.X, cResolution.Y / 2 - Resources._itemInfo.Size.Y / 2) };
        }

        float saveTick = 0;
        float tick = 0;
        float aTick;
        static float speed = 0.1f;

        bool hasGoneToMiddle;

        float acc = 0.0459f * (speed*10f);

        List<string> queueText = new List<string>(); 
        List<string> queueSubText = new List<string>(); 

        public void Slide(string _text, string _subtext)
        {
            if (!isWorking)
            {
                isWorking = true;
                tick = 0;
                hasGoneToMiddle = false;
                ResetElementsPosition();
                text.DisplayedString = _text;
                subtext.DisplayedString = _subtext;
                //Thread th = new Thread(SlideThread) { IsBackground = true };
                //th.Start();
            }
            else
            {
                queueText.Add(_text);
                queueSubText.Add(_subtext);
            }
        }

        void WaitThread()
        {
        }

        public void Update()
        {
            aTick++;
            if (isWorking)
            {
                if (!hasGoneToMiddle)
                {
                    if (tick <= Math.PI - speed)
                    {
                        tick += speed;
                        SetElementsPosition(tick);
                    }
                    else
                    {
                        saveTick = aTick;
                        hasGoneToMiddle = true;
                    }
                }
                else if (aTick - saveTick > (queueText.Count == 0 ? 120 : 30))
                {
                    if (tick >= 0)
                    {
                        tick -= speed;
                        SetElementsPosition(tick);
                    }
                    else
                    {
                        ResetElementsPosition();
                        if (queueText.Count == 0) isWorking = false;
                        else
                        {
                            tick = 0;
                            hasGoneToMiddle = false;
                            ResetElementsPosition();
                            text.DisplayedString = queueText[queueText.Count-1];
                            subtext.DisplayedString = queueSubText[queueSubText.Count - 1];
                            queueText.RemoveAt(queueText.Count - 1);
                            queueSubText.RemoveAt(queueSubText.Count - 1);
                        }
                    }
                }
            }
        }

        void ResetElementsPosition()
        {
            bg.Position = new Vector2f(cResolution.X, bg.Position.Y);
            text.Position = new Vector2f(cResolution.X, bg.Position.Y);
            subtext.Position = new Vector2f(cResolution.X, bg.Position.Y);
        }

        void SetElementsPosition(float _tick)
        {
            bg.Position -= new Vector2f(cResolution.X * (float)Math.Sin(_tick) * acc, 0);
            text.Position = new Vector2f(bg.Position.X + bg.GetGlobalBounds().Width / 2 - text.GetGlobalBounds().Width / 2, bg.Position.Y + bg.GetGlobalBounds().Height / 2 - 90);
            subtext.Position = new Vector2f(bg.Position.X + bg.GetGlobalBounds().Width / 2 - subtext.GetGlobalBounds().Width / 2, bg.Position.Y + 100);
        }
    }
}
