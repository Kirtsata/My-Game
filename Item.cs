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
    class Item : Program
    {
        public int item;
        public int rarity;
        public Sprite sprite;
        public Vector2f cPosition;
        float incr;
        float radius = 10;
        byte opacity;
        float scale;
        public bool RemoveMe = true;

        public Item(int X, int Y, int _item, int _rarity)
        {
            item = _item;
            rarity = _rarity;
            switch (rarity)
            {
                case 0:
                    sprite = new Sprite(Resources._itemCommon[item]);
                    break;
                case 1:
                    sprite = new Sprite(Resources._itemsRare[item]);
                    break;
            }

            sprite.Origin = (Vector2f)sprite.Texture.Size / 2;
            sprite.Position = new Vector2f(X, Y);
            cPosition = sprite.Position;
        }
        public void Action(Entity entity, ref UI_Perk UI_Perk)
        {
            switch (rarity)
            {
                case 0:
                    switch (item)
                    {
                        case ItemsCommon.Heart:
                        case ItemsCommon.HeartHalf:
                            if (entity.Health < 6 - (item == ItemsCommon.Heart ? 1 : 0))
                            {
                                entity.Health += (item == ItemsCommon.Heart) ? 2 : 1;
                                RemoveMe = true;
                            }
                            else RemoveMe = false;
                            break;
                        //case ItemsCommon.Energy1:
                        case ItemsCommon.Energy0:
                            itemInfo.Slide("Nomster", "Gives energy and potentially diabetes");
                            entity.speed += 0.20f;
                            break;
                        case ItemsCommon.Firerate0:
                            itemInfo.Slide("Laxatives", "Oh lord it's coming!");
                            entity.fireRate -= 1;
                            break;
                        case ItemsCommon.Damage0:
                            itemInfo.Slide("Pipse", "The power of pipse has been granted");
                            entity.Damage += 1;
                            entity.pColor = new Color((byte)(entity.pColor.R - (entity.pColor.R > 127 ? 25 : 0)), entity.pColor.G, (byte)(entity.pColor.B - (entity.pColor.B > 10 ? 50 : 0)));
                            entity.sprite.Color = new Color((byte)(entity.pColor.R - (entity.pColor.R > 127 ? 25 : 0)), entity.pColor.G, (byte)(entity.pColor.B - (entity.pColor.B > 10 ? 50 : 0)));
                            break;
                        case ItemsCommon.Bomb0:
                            if (entity.bombCount < 99)
                            {
                                entity.bombCount++;
                                RemoveMe = true;
                            }
                            else RemoveMe = false;
                            break;
                    }
                    break;
                case 1:
                    switch (item)
                    {
                        case ItemsRare.SwitchRoom:
                            // Check Outside class checks
                            break;
                        case ItemsRare.Fly0:
                            itemInfo.Slide("Bed Rule", "It gives you wi-- a jetpack!");
                            UnbindItems(entity, ref UI_Perk);
                            entity.perks.hasJetpack = true;
                            break;
                        case ItemsRare.Manga:
                            itemInfo.Slide("Manga", "Protect the unprotected");
                            UnbindItems(entity, ref UI_Perk);
                            entity.perks.hasKatana = true;
                            break;

                    }
                    break;

            }

        }
        public void Update()
        {
            if (opacity < 255)
            {
                opacity += 5;
                sprite.Color = new Color(255, 255, 255, opacity);
            }
            if (scale < 3)
            {
                scale += 0.05f;
                sprite.Scale = new Vector2f(scale, scale);
            }
            incr += 0.07f;
            sprite.Position = new Vector2f(sprite.Position.X, cPosition.Y - (float)Math.Cos(incr) * radius);
        }

        public static class ItemsCommon
        {
            public const int Heart = 0;
            public const int HeartHalf = 1;
            public const int Energy0 = 2;
            public const int Firerate0 = 3;
            public const int Damage0 = 4;
            public const int Bomb0 = 6;
        }

        public static class ItemsRare
        {
            //public const int Steroids = 0;
            public const int SwitchRoom = 0;
            public const int Fly0 = 1;
            public const int Manga = 2;
        }

        private static void UnbindItems(Entity e, ref UI_Perk uiP)
        {
            e.perks.hasJetpack = false;
            e.perks.hasKatana = false;

            e.perks.hasPerk = true;
        }
    }
}
