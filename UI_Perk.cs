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
    class UI_Perk
    {
        public Sprite itemSprite;
        public Sprite barSprite;

        uint ptr;
        int lastUsage = -1;
        
        public UI_Perk()
        {
            itemSprite = new Sprite() { Position = new Vector2f(10, 10) };
            barSprite = new Sprite() { Scale = new Vector2f(5, 5), Position = new Vector2f(itemSprite.Position.X + Resources._JetpackPerk.Size.X*6, 10) };
        }

        public void Update(Entity player, int usage) //TODO add charges (>/< than 2)
        {
            if (player.perks.hasPerk)
            {
                barSprite.Color = new Color(Resources._ChargeBarGradient.GetPixel(ptr++, 0));
                if (ptr >= Resources._ChargeBarGradient.Size.X - 1) ptr = 0;


                if (lastUsage != usage)
                {
                    if (player.perks.hasJetpack)
                    {
                        itemSprite.Scale = new Vector2f(6, 6);
                        itemSprite.Texture = Resources._JetpackPerk;
                    }
                    if (player.perks.hasKatana)
                    {
                        itemSprite.Scale = new Vector2f(2f, 2f);
                        itemSprite.Texture = Resources._katanaPerk;
                    }

                    barSprite.Texture = Resources._uiPowerBars2[usage];
                }

                lastUsage = usage;
            }
        }
    }
}
