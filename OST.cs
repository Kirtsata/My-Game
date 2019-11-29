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
        public static SoundBuffer bDoorOpen = new SoundBuffer(@"Target\Music\DoorOpen.ogg");
        public static SoundBuffer bFireball = new SoundBuffer(@"Target\Music\Fireball.ogg");
        public static SoundBuffer bExplosion = new SoundBuffer(@"Target\Music\Explosion.ogg");
        public static SoundBuffer bSlash = new SoundBuffer(@"Target\Music\Slash.ogg");
        public static SoundBuffer bFlesh = new SoundBuffer(@"Target\Music\Flesh.ogg");

        public static Sound Fireball = new Sound(bFireball);
        public static Sound Explosion = new Sound(bExplosion);
        public static Sound Slash = new Sound(bSlash);
        public static Sound Flesh = new Sound(bFlesh);
        public static Sound DoorOpen = AddSound("DoorOpen");

        public static Music bg = new Music(@"Target\Music\BG.ogg") { Volume = 25, Loop = true };
        public static Music menu = new Music(@"Target\Music\Menu.ogg") { Volume = 25, Loop = true };
        public static Music deathScreen = new Music(@"Target\Music\DeathScreen.ogg") { Volume = 25, Loop = true };

        static Sound AddSound(string ogg)
        {
            return new Sound(new SoundBuffer($@"Target\Music\{ogg}.ogg"));
        }
    }
}
