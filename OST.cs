using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.System;
using SFML.Window;
using System.IO;

namespace Another_SFML_Project
{
    class OST
    {
        public static Sound Fireball = AddSound("Fireball");
        public static Sound Explosion = AddSound("Explosion");
        public static Sound Flesh = AddSound("Flesh");
        public static Sound DoorOpen = AddSound("DoorOpen");
        public static Sound Slash = AddSound("Slash");
        public static Sound Whoosh = AddSound("Whoosh");

        public static Music bg = new Music(@"Target\Music\BG.ogg") { Volume = 25, Loop = true };
        public static Music menu = new Music(@"Target\Music\Menu.ogg") { Volume = 25, Loop = true };
        public static Music deathScreen = new Music(@"Target\Music\DeathScreen.ogg") { Volume = 25, Loop = true };

        static Sound AddSound(string ogg)
        {
            return new Sound(new SoundBuffer($@"Target\Music\{ogg}.ogg"));
        }
    }
}
