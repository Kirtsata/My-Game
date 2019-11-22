using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using System.IO;

namespace SomeGame
{
    class Resources
    {
        public static Texture _scrollingBG = new Texture(@"Resources\bg.png") { Repeated = true };
        public static Texture _walls = new Texture(@"Resources\walls.png");
        public static Texture _ground = new Texture(@"Resources\ground.png");
        public static Texture _deathScreen = new Texture(@"Resources\DeathScreen.png");
        public static Texture _deathDetails = new Texture(@"Resources\TimeScore.png");

        public static Texture _buttonNewGame = new Texture(@"Resources\Buttons\NewGame.png");
        public static Texture _buttonPlay = new Texture(@"Resources\Buttons\Play.png");
        public static Texture _buttonOptions = new Texture(@"Resources\Buttons\Options.png");
        public static Texture _buttonQuit = new Texture(@"Resources\Buttons\Quit.png");
        public static Texture _buttonSlider = new Texture(@"Resources\Buttons\Slider.png");
        public static Texture _buttonPointer = new Texture(@"Resources\Buttons\Pointer.png");
        public static Texture _buttonSave = new Texture(@"Resources\Buttons\Save.png");
        public static Texture _buttonCancel = new Texture(@"Resources\Buttons\Cancel.png");

        public static Texture _textResolution = new Texture(@"Resources\Resolutions\Resolution.png");
        public static Texture _text600x400 = new Texture(@"Resources\Resolutions\600x400.png");
        public static Texture _text900x600 = new Texture(@"Resources\Resolutions\900x600.png");
        public static Texture _text1200x800 = new Texture(@"Resources\Resolutions\1200x800.png");
        public static Texture _text1400x950 = new Texture(@"Resources\Resolutions\1400x950.png");

        public static Texture _textSFX = new Texture(@"Resources\textSFX.png");
        public static Texture _textMusic = new Texture(@"Resources\textMusic.png");

        public static Texture _uiBomb = new Texture(@"Resources\Stats\Bombs.png");
        public static Texture _uiDamage = new Texture(@"Resources\Stats\Damage.png");
        public static Texture _uiFireRate = new Texture(@"Resources\Stats\FireRate.png");
        public static Texture _uiSpeed = new Texture(@"Resources\Stats\Speed.png");

        public static Texture _itemInfo = new Texture(@"Resources\Items\ItemInfo.png");

        public static Texture _playerUp = new Texture(@"Resources\player_up.png");
        public static Texture _playerDown = new Texture(@"Resources\player_down.png");
        public static Texture _playerLeft = new Texture(@"Resources\player_left.png");
        public static Texture _playerRight = new Texture(@"Resources\player_right.png");

        public static Texture[] _hearts = new Texture[]
        {
            new Texture(@"Resources\heart_full.png"),
            new Texture(@"Resources\heart_half.png"),
            new Texture(@"Resources\heart_empty.png")
        };

        public static List<Texture> _enemyUp = AddMultipleTextures("enemy", "_up");
        public static List<Texture> _enemyDown = AddMultipleTextures("enemy", "_down");
        public static List<Texture> _enemyLeft = AddMultipleTextures("enemy", "_left");
        public static List<Texture> _enemyRight = AddMultipleTextures("enemy", "_right");

        public static List<Texture> _iron = AddMultipleTextures(@"Blocks\Iron");

        public static List<Texture> _rock = AddMultipleTextures("rock");
        public static Texture _cross = new Texture(@"Resources\Cross.png");

        public static List<Texture> _uiPowerBars2 = AddMultipleTextures(@"Perks\power2bars");

        public static List<Texture> _bloodStains = AddMultipleTextures("blood_stain");
        public static List<Texture> _sBloodParticles = AddMultipleTextures("blood_particle");

        public static List<Texture> _katanaSlash = AddMultipleTextures(@"Perks\KatanaSlash\katana");
        public static Texture _katana = new Texture(@"Resources\Perks\Katana.png");
        public static Texture _katanaPerk = new Texture(@"Resources\Perks\KatanaPerk.png");

        public static List<Texture> _jetPack = AddMultipleTextures(@"Perks\Jetpack");
        public static Texture _JetpackPerk = new Texture(@"Resources\Perks\JetpackPerk.png");

        public static Texture _shadow = new Texture(@"Resources\shadow.png");
        public static Texture _shadow1 = new Texture(@"Resources\shadow1.png");
        public static Texture _heartShadow = new Texture(@"Resources\heart_shadow.png");
        public static Texture _mmShadow = new Texture(@"Resources\Minimap\minimap_shadow.png");

        public static Texture _explosion = new Texture(@"Resources\explosion.png");

        public static Texture[] _bomb = new Texture[] 
        {
            new Texture(@"Resources\bomb0.png"),
            new Texture(@"Resources\bomb1.png"),
            new Texture(@"Resources\bomb0.png"),
            new Texture(@"Resources\bomb2.png")
        };

        public static Texture _burntOut = new Texture(@"Resources\burnt_out.png");

        public static Texture[] _rockParticles = new Texture[]
        {
            new Texture(@"Resources\rock1_particle1.png"),
            new Texture(@"Resources\rock1_particle2.png")
        };

        public static Texture[] _itemCommon = new Texture[]
        {
             //new Texture(@"Resources\Items\Health.png"),
             new Texture(@"Resources\Items\Heart.png"),
             new Texture(@"Resources\Items\Heart_Half.png"),
             new Texture(@"Resources\Items\Nonster.png"),
             //new Texture(@"Resources\Items\FGuel.png"),
             new Texture(@"Resources\Items\Lax.png"),
             new Texture(@"Resources\Items\Pipse.png"),
             new Texture(@"Resources\Items\Bomb.png"),
             //new Texture(@"Resources\Items\Firerate.png"),
             //new Texture(@"Resources\Items\Speed.png"),
             //new Texture(@"Resources\Items\Damage.png")
        };

        public static Texture[] _itemsRare = new Texture[]
        {
             //new Texture(@"Resources\Items\Steroids.png"),
             new Texture(@"Resources\Items\RoomSwitch.png"),
             new Texture(@"Resources\Items\BedRule.png"),
             new Texture(@"Resources\Items\Manga.png")

        };

        public static Texture _projectile = new Texture(@"Resources\projectile.png");
        public static Texture[] _projectileParticles = new Texture[]
        {
            new Texture(@"Resources\projectile_particle1.png"),
            new Texture(@"Resources\projectile_particle2.png")
        };

        public static Texture[] _bloodParticles = new Texture[]
        {
            new Texture(@"Resources\blood1.png"),
            new Texture(@"Resources\blood2.png"),
        };

        public static Texture _doorClosed = new Texture(@"Resources\door_closed.png");
        public static Texture _doorOpened = new Texture(@"Resources\door_opened.png");

        public static Texture _L = new Texture(@"Resources\Minimap\L.png");
        public static Texture _U = new Texture(@"Resources\Minimap\U.png");
        public static Texture _R = new Texture(@"Resources\Minimap\R.png");
        public static Texture _D = new Texture(@"Resources\Minimap\D.png");
        public static Texture _UL = new Texture(@"Resources\Minimap\UL.png");
        public static Texture _UR = new Texture(@"Resources\Minimap\UR.png");
        public static Texture _ULR = new Texture(@"Resources\Minimap\ULR.png");
        public static Texture _DL = new Texture(@"Resources\Minimap\DL.png");
        public static Texture _DR = new Texture(@"Resources\Minimap\DR.png");
        public static Texture _LR = new Texture(@"Resources\Minimap\LR.png");
        public static Texture _UD = new Texture(@"Resources\Minimap\UD.png");
        public static Texture _DLR = new Texture(@"Resources\Minimap\DLR.png");
        public static Texture _UDL = new Texture(@"Resources\Minimap\UDL.png");
        public static Texture _UDR = new Texture(@"Resources\Minimap\UDR.png");
        public static Texture _ALL = new Texture(@"Resources\Minimap\ALL.png");

        public static Font _FontPixeled = new Font(@"Resources\Fonts\One Slice.otf");

        public static Image _RainbowStrip = new Image(@"Resources\RainbowStrip.png");
        public static Image _ChargeBarGradient = new Image(@"Resources\ChargeBarGradient.png");

        public static List<Texture> AddMultipleTextures(string png, string extention = "")
        {
            int count = 0;
            List<Texture> ReturnMe = new List<Texture>();
            while (File.Exists(@"Resources\" + png + count + extention + ".png"))
            {
                ReturnMe.Add(new Texture(@"Resources\" + png  + count + extention + ".png"));
                count++;
            }
            return ReturnMe;
        }
    }
}
