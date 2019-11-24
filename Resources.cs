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
        public static Texture _walls = AddTexture(@"walls.png");
        public static Texture _ground = AddTexture(@"ground.png");
        public static Texture _deathScreen = AddTexture(@"DeathScreen.png");
        public static Texture _deathDetails = AddTexture(@"TimeScore.png");

        public static Texture _buttonNewGame = AddTexture(@"Buttons\NewGame.png");
        public static Texture _buttonPlay = AddTexture(@"Buttons\Play.png");
        public static Texture _buttonOptions = AddTexture(@"Buttons\Options.png");
        public static Texture _buttonQuit = AddTexture(@"Buttons\Quit.png");
        public static Texture _buttonSlider = AddTexture(@"Buttons\Slider.png");
        public static Texture _buttonPointer = AddTexture(@"Buttons\Pointer.png");
        public static Texture _buttonSave = AddTexture(@"Buttons\Save.png");
        public static Texture _buttonCancel = AddTexture(@"Buttons\Cancel.png");

        public static Texture _textResolution = AddTexture(@"Resolutions\Resolution.png");
        public static Texture _text600x400 = AddTexture(@"Resolutions\600x400.png");
        public static Texture _text900x600 = AddTexture(@"Resolutions\900x600.png");
        public static Texture _text1200x800 = AddTexture(@"Resolutions\1200x800.png");
        public static Texture _text1400x950 = AddTexture(@"Resolutions\1400x950.png");

        public static Texture _textSFX = AddTexture(@"textSFX.png");
        public static Texture _textMusic = AddTexture(@"textMusic.png");

        public static Texture _uiBomb = AddTexture(@"Stats\Bombs.png");
        public static Texture _uiDamage = AddTexture(@"Stats\Damage.png");
        public static Texture _uiFireRate = AddTexture(@"Stats\FireRate.png");
        public static Texture _uiSpeed = AddTexture(@"Stats\Speed.png");

        public static Texture _itemInfo = AddTexture(@"Items\ItemInfo.png");

        public static Texture _playerUp = AddTexture(@"player_up.png");
        public static Texture _playerDown = AddTexture(@"player_down.png");
        public static Texture _playerLeft = AddTexture(@"player_left.png");
        public static Texture _playerRight = AddTexture(@"player_right.png");

        public static Texture[] _hearts = new Texture[]
        {
            AddTexture(@"heart_full.png"),
            AddTexture(@"heart_half.png"),
            AddTexture(@"heart_empty.png")
        };

        public static List<Texture> _enemyUp = AddMultipleTextures("enemy", "_up");
        public static List<Texture> _enemyDown = AddMultipleTextures("enemy", "_down");
        public static List<Texture> _enemyLeft = AddMultipleTextures("enemy", "_left");
        public static List<Texture> _enemyRight = AddMultipleTextures("enemy", "_right");

        public static List<Texture> _iron = AddMultipleTextures(@"Blocks\Iron");

        public static List<Texture> _rock = AddMultipleTextures("rock");
        public static Texture _cross = AddTexture(@"Cross.png");

        public static List<Texture> _uiPowerBars2 = AddMultipleTextures(@"Perks\power2bars");

        public static List<Texture> _bloodStains = AddMultipleTextures("blood_stain");
        public static List<Texture> _sBloodParticles = AddMultipleTextures("blood_particle");

        public static List<Texture> _katanaSlash = AddMultipleTextures(@"Perks\KatanaSlash\katana");
        public static Texture _katana = AddTexture(@"Perks\Katana.png");
        public static Texture _katanaPerk = AddTexture(@"Perks\KatanaPerk.png");

        public static List<Texture> _jetPack = AddMultipleTextures(@"Perks\Jetpack");
        public static Texture _JetpackPerk = AddTexture(@"Perks\JetpackPerk.png");

        public static Texture _shadow = AddTexture(@"shadow.png");
        public static Texture _shadow1 = AddTexture(@"shadow1.png");
        public static Texture _heartShadow = AddTexture(@"heart_shadow.png");
        public static Texture _mmShadow = AddTexture(@"Minimap\minimap_shadow.png");

        public static Texture _explosion = AddTexture(@"explosion.png");

        public static Texture[] _bomb = new Texture[] 
        {
            AddTexture(@"bomb0.png"),
            AddTexture(@"bomb1.png"),
            AddTexture(@"bomb0.png"),
            AddTexture(@"bomb2.png")
        };

        public static Texture _burntOut = AddTexture(@"burnt_out.png");

        public static Texture[] _rockParticles = new Texture[]
        {
            AddTexture(@"rock1_particle1.png"),
            AddTexture(@"rock1_particle2.png")
        };

        public static Texture[] _itemCommon = new Texture[]
        {
             //AddTexture(@"Items\Health.png"),
             AddTexture(@"Items\Heart.png"),
             AddTexture(@"Items\Heart_Half.png"),
             AddTexture(@"Items\Nonster.png"),
             //AddTexture(@"Items\FGuel.png"),
             AddTexture(@"Items\Lax.png"),
             AddTexture(@"Items\Pipse.png"),
             AddTexture(@"Items\Bomb.png"),
             //AddTexture(@"Items\Firerate.png"),
             //AddTexture(@"Items\Speed.png"),
             //AddTexture(@"Items\Damage.png")
        };

        public static Texture[] _itemsRare = new Texture[]
        {
             //AddTexture(@"Items\Steroids.png"),
             AddTexture(@"Items\RoomSwitch.png"),
             AddTexture(@"Items\BedRule.png"),
             AddTexture(@"Items\Manga.png")

        };

        public static Texture _projectile = AddTexture(@"projectile.png");
        public static Texture[] _projectileParticles = new Texture[]
        {
            AddTexture(@"projectile_particle1.png"),
            AddTexture(@"projectile_particle2.png")
        };

        public static Texture[] _bloodParticles = new Texture[]
        {
            AddTexture(@"blood1.png"),
            AddTexture(@"blood2.png"),
        };

        public static Texture _doorClosed = AddTexture(@"door_closed.png");
        public static Texture _doorOpened = AddTexture(@"door_opened.png");

        public static Texture _L = AddTexture(@"Minimap\L.png");
        public static Texture _U = AddTexture(@"Minimap\U.png");
        public static Texture _R = AddTexture(@"Minimap\R.png");
        public static Texture _D = AddTexture(@"Minimap\D.png");
        public static Texture _UL = AddTexture(@"Minimap\UL.png");
        public static Texture _UR = AddTexture(@"Minimap\UR.png");
        public static Texture _ULR = AddTexture(@"Minimap\ULR.png");
        public static Texture _DL = AddTexture(@"Minimap\DL.png");
        public static Texture _DR = AddTexture(@"Minimap\DR.png");
        public static Texture _LR = AddTexture(@"Minimap\LR.png");
        public static Texture _UD = AddTexture(@"Minimap\UD.png");
        public static Texture _DLR = AddTexture(@"Minimap\DLR.png");
        public static Texture _UDL = AddTexture(@"Minimap\UDL.png");
        public static Texture _UDR = AddTexture(@"Minimap\UDR.png");
        public static Texture _ALL = AddTexture(@"Minimap\ALL.png");

        public static Font _FontPixeled = new Font(@"Target\Resources\Fonts\Runescape.ttf");

        public static Image _RainbowStrip = new Image(@"Target\Resources\RainbowStrip.png");
        public static Image _ChargeBarGradient = new Image(@"Target\Resources\ChargeBarGradient.png");


        public static Texture AddTexture(string png)
        {
            return new Texture(@"Target\Resources\" + png);
        }
        public static List<Texture> AddMultipleTextures(string png, string extention = "")
        {
            int count = 0;
            List<Texture> ReturnMe = new List<Texture>();
            while (File.Exists(@"Target\Resources\" + png + count + extention + ".png"))
            {
                ReturnMe.Add(AddTexture(@"" + png  + count + extention + ".png"));
                count++;
            }
            return ReturnMe;
        }
    }
}
