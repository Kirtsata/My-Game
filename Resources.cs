using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using System.IO;

namespace Another_SFML_Project
{
    class Resources
    {
        public static Image _icon = new Image(@"Target\Resources\icon.png");

        public static Texture _scrollingBG = new Texture(@"Target\Resources\bg.png") { Repeated = true };
        public static Texture _walls = AddTexture(@"walls");
        public static Texture _ground = AddTexture(@"ground");
        public static Texture _deathScreen = AddTexture(@"DeathScreen");
        public static Texture _deathDetails = AddTexture(@"TimeScore");

        public static Texture _buttonNewGame = AddTexture(@"Buttons\NewGame");
        public static Texture _buttonPlay = AddTexture(@"Buttons\Play");
        public static Texture _buttonOptions = AddTexture(@"Buttons\Options");
        public static Texture _buttonQuit = AddTexture(@"Buttons\Quit");
        public static Texture _buttonSlider = AddTexture(@"Buttons\Slider");
        public static Texture _buttonPointer = AddTexture(@"Buttons\Pointer");
        public static Texture _buttonSave = AddTexture(@"Buttons\Save");
        public static Texture _buttonCancel = AddTexture(@"Buttons\Cancel");

        public static Texture _textResolution = AddTexture(@"Resolutions\Resolution");
        public static Texture _text600x400 = AddTexture(@"Resolutions\600x400");
        public static Texture _text900x600 = AddTexture(@"Resolutions\900x600");
        public static Texture _text1200x800 = AddTexture(@"Resolutions\1200x800");
        public static Texture _text1400x950 = AddTexture(@"Resolutions\1400x950");

        public static Texture _textSFX = AddTexture(@"textSFX");
        public static Texture _textMusic = AddTexture(@"textMusic");

        public static Texture _uiBomb = AddTexture(@"Stats\Bombs");
        public static Texture _uiDamage = AddTexture(@"Stats\Damage");
        public static Texture _uiFireRate = AddTexture(@"Stats\FireRate");
        public static Texture _uiSpeed = AddTexture(@"Stats\Speed");

        public static Texture _itemInfo = AddTexture(@"Items\ItemInfo");

        public static Texture _playerUp = AddTexture(@"player_up");
        public static Texture _playerDown = AddTexture(@"player_down");
        public static Texture _playerLeft = AddTexture(@"player_left");
        public static Texture _playerRight = AddTexture(@"player_right");

        public static Texture[] _hearts = new Texture[]
        {
            AddTexture(@"heart_full"),
            AddTexture(@"heart_half"),
            AddTexture(@"heart_empty")
        };

        public static List<Texture> _enemyUp = AddMultipleTextures("enemy", "_up");
        public static List<Texture> _enemyDown = AddMultipleTextures("enemy", "_down");
        public static List<Texture> _enemyLeft = AddMultipleTextures("enemy", "_left");
        public static List<Texture> _enemyRight = AddMultipleTextures("enemy", "_right");

        public static List<Texture> _iron = AddMultipleTextures(@"Blocks\Iron");

        public static List<Texture> _rock = AddMultipleTextures("rock");
        public static Texture _cross = AddTexture(@"Cross");

        public static List<Texture> _uiPowerBars2 = AddMultipleTextures(@"Perks\power2bars");

        public static List<Texture> _bloodStains = AddMultipleTextures("blood_stain");
        public static List<Texture> _sBloodParticles = AddMultipleTextures("blood_particle");

        public static List<Texture> _katanaSlash = AddMultipleTextures(@"Perks\KatanaSlash\katana");
        public static Texture _katana = AddTexture(@"Perks\Katana");
        public static Texture _katanaPerk = AddTexture(@"Perks\KatanaPerk");
        public static Texture _slash = AddTexture(@"Perks\Slash");

        public static List<Texture> _jetPack = AddMultipleTextures(@"Perks\Jetpack");
        public static Texture _JetpackPerk = AddTexture(@"Perks\JetpackPerk");

        public static Texture _shadow = AddTexture(@"shadow");
        public static Texture _shadow1 = AddTexture(@"shadow1");
        public static Texture _heartShadow = AddTexture(@"heart_shadow");
        public static Texture _mmShadow = AddTexture(@"Minimap\minimap_shadow");

        public static Texture _explosion = AddTexture(@"explosion");

        public static Texture[] _bomb = new Texture[] 
        {
            AddTexture(@"bomb0"),
            AddTexture(@"bomb1"),
            AddTexture(@"bomb0"),
            AddTexture(@"bomb2")
        };

        public static Texture _burntOut = AddTexture(@"burnt_out");

        public static Texture[] _rockParticles = new Texture[]
        {
            AddTexture(@"rock1_particle1"),
            AddTexture(@"rock1_particle2")
        };

        public static Texture[] _itemCommon = new Texture[]
        {
             //AddTexture(@"Items\Health"),
             AddTexture(@"Items\Heart"),
             AddTexture(@"Items\Heart_Half"),
             AddTexture(@"Items\Nonster"),
             //AddTexture(@"Items\FGuel"),
             AddTexture(@"Items\Lax"),
             AddTexture(@"Items\Pipse"),
             AddTexture(@"Items\Bomb"),
             //AddTexture(@"Items\Firerate"),
             //AddTexture(@"Items\Speed"),
             //AddTexture(@"Items\Damage")
        };

        public static Texture[] _itemsRare = new Texture[]
        {
             //AddTexture(@"Items\Steroids"),
             AddTexture(@"Items\RoomSwitch"),
             AddTexture(@"Items\BedRule"),
             AddTexture(@"Items\Manga")

        };

        public static Texture _projectile = AddTexture(@"projectile");
        public static Texture[] _projectileParticles = new Texture[]
        {
            AddTexture(@"projectile_particle1"),
            AddTexture(@"projectile_particle2")
        };

        public static Texture[] _bloodParticles = new Texture[]
        {
            AddTexture(@"blood1"),
            AddTexture(@"blood2"),
        };

        public static Texture _doorClosed = AddTexture(@"door_closed");
        public static Texture _doorOpened = AddTexture(@"door_opened");

        public static Texture _L = AddTexture(@"Minimap\L");
        public static Texture _U = AddTexture(@"Minimap\U");
        public static Texture _R = AddTexture(@"Minimap\R");
        public static Texture _D = AddTexture(@"Minimap\D");
        public static Texture _UL = AddTexture(@"Minimap\UL");
        public static Texture _UR = AddTexture(@"Minimap\UR");
        public static Texture _ULR = AddTexture(@"Minimap\ULR");
        public static Texture _DL = AddTexture(@"Minimap\DL");
        public static Texture _DR = AddTexture(@"Minimap\DR");
        public static Texture _LR = AddTexture(@"Minimap\LR");
        public static Texture _UD = AddTexture(@"Minimap\UD");
        public static Texture _DLR = AddTexture(@"Minimap\DLR");
        public static Texture _UDL = AddTexture(@"Minimap\UDL");
        public static Texture _UDR = AddTexture(@"Minimap\UDR");
        public static Texture _ALL = AddTexture(@"Minimap\ALL");

        public static Font _FontPixeled = new Font(@"Target\Resources\Fonts\Runescape.ttf");

        public static Image _RainbowStrip = new Image(@"Target\Resources\RainbowStrip.png");
        public static Image _ChargeBarGradient = new Image(@"Target\Resources\ChargeBarGradient.png");


        public static Texture AddTexture(string png)
        {
            return new Texture(@"Target\Resources\" + png + ".png");
        }
        public static List<Texture> AddMultipleTextures(string png, string extention = "")
        {
            int count = 0;
            List<Texture> ReturnMe = new List<Texture>();
            while (File.Exists(@"Target\Resources\" + png + count + extention + ".png"))
            {
                ReturnMe.Add(AddTexture(@"" + png  + count + extention));
                count++;
            }
            return ReturnMe;
        }
    }
}
