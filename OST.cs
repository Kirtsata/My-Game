using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.System;
using SFML.Window;
using System.IO;

namespace SomeGame
{
    class OST
    {
        public static SoundBuffer bFireball = new SoundBuffer(@"Music\Fireball.ogg");
        public static SoundBuffer bExplosion = new SoundBuffer(@"Music\Explosion.ogg");
        public static SoundBuffer bSlash = new SoundBuffer(@"Music\Slash.ogg");
        public static SoundBuffer bFlesh = new SoundBuffer(@"Music\Flesh.ogg");

        public static Sound Fireball = new Sound(bFireball);
        public static Sound Explosion = new Sound(bExplosion);
        public static Sound Slash = new Sound(bSlash);
        public static Sound Flesh = new Sound(bFlesh);

        public static Music bg = new Music(@"Music\BG.ogg") { Volume = 25, Loop = true };
        public static Music menu = new Music(@"Music\Menu.ogg") { Volume = 25, Loop = true };
        public static Music deathScreen = new Music(@"Music\DeathScreen.ogg") { Volume = 25, Loop = true };
    }
}
