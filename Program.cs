using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
using System.IO;
using System.Threading;

namespace SomeGame
{
    class Program
    {
        public RenderWindow window;
        public static Random rng = new Random();
        //static float speed = 7.5f;
        public Vector2u cResolution = new Vector2u(1200, 800);
        public Vector2u Resolution = new Vector2u(1200, 800);
        public float boundsOffset = 120;
        public int tick;

        public RectangleShape BlackCoverShape;

        public int mapCount = 0;
        public static int roomCount = 0;

        public bool hasGeneratedMap = false;

        // FloatRect AreaOfReach; //Set below
        public FloatRect[] wallsProjectile; //Set below
        public FloatRect[] walls; //Set below
        public Grid grid;

        public View view = new View(new FloatRect(0, 0, 1200, 800));

        public Sprite deathScreen = new Sprite(Resources._deathScreen) { Scale = new Vector2f(10, 10) };
        public Sprite deathDetails;

        public ItemInfo itemInfo;
        public Sprite heartShadow = new Sprite(Resources._heartShadow) { Scale = new Vector2f(2.5f, 2.5f), Position = new Vector2f(-25, -35) };
        public Sprite minimapShadow = new Sprite(Resources._mmShadow) { Position = new Vector2f(50 + 1200 - Resources._mmShadow.Size.X, -50) };
        public Sprite scrollingBG = new Sprite(Resources._scrollingBG) { Scale = new Vector2f(7, 7), TextureRect = new IntRect(0, 0, (int)Resources._scrollingBG.Size.X * 2, (int)Resources._scrollingBG.Size.Y * 2), Color = new Color(100, 100, 100) };
        public Sprite Orb;
        public Sprite[] menuButtons;
        Sprite[] optionsButtons;

        Sprite[] gameOverButtons;

        static Slider[] sliders;

        static Sprite[] texts;
        public Sprite[] resolutions;
        Vector2u[] iResolutions = new Vector2u[]
        {
            new Vector2u(600, 400),
            new Vector2u(900, 600),
            new Vector2u(1200, 800),
            new Vector2u(1425, 950)
        };
        public Thread shakeThread;
        public Thread blackDipThread;
        public List<Thread> playSound = new List<Thread>();

        //Actions outside gameplay
        public bool gameOn = false;
        public bool gameOver = false;
        public bool optionsOn = false;

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            menuButtons = new Sprite[]
            {
                new Sprite(Resources._buttonNewGame) { Position = new Vector2f(cResolution.X / 2, cResolution.Y / 2 + 170 - Resources._buttonNewGame.Size.Y*6), Origin = (Vector2f)Resources._buttonNewGame.Size / 2, Scale = new Vector2f(6, 6) },
                new Sprite(Resources._buttonPlay) { Position = new Vector2f(cResolution.X / 2, cResolution.Y / 2 + 170), Origin = (Vector2f)Resources._buttonPlay.Size / 2, Scale = new Vector2f(6, 6), Color = new Color(100,100,100) },
                new Sprite(Resources._buttonOptions) { Position = new Vector2f(cResolution.X / 2, cResolution.Y / 2 + 170 + Resources._buttonOptions.Size.Y*6), Origin = (Vector2f)Resources._buttonOptions.Size / 2, Scale = new Vector2f(6, 6) },
                new Sprite(Resources._buttonQuit) { Position = new Vector2f(cResolution.X / 2, cResolution.Y / 2 + 170 + Resources._buttonQuit.Size.Y*6*2), Origin = (Vector2f)Resources._buttonQuit.Size / 2, Scale = new Vector2f(6, 6) },
            };
            sliders = new Slider[]
            {
                new Slider("Music", 25, new Vector2f(cResolution.X/2, 150)),
                new Slider("SFX", 25, new Vector2f(cResolution.X/2, 300)),
                new Slider("Resolution", 0, new Vector2f(cResolution.X/2, 450))
            };
            texts = new Sprite[]
            {
            new Sprite(Resources._textMusic) { Position = new Vector2f(cResolution.X/2, 75), Origin = (Vector2f)Resources._textMusic.Size/2, Scale = new Vector2f(5, 5)},
            new Sprite(Resources._textSFX) { Position = new Vector2f(cResolution.X/2, 225), Origin = (Vector2f)Resources._textSFX.Size/2, Scale = new Vector2f(5, 5)},
            new Sprite(Resources._textResolution) { Position = new Vector2f(sliders[0].slider.GetGlobalBounds().Left + 20, 350), Scale = new Vector2f(5, 5)},
            };
            resolutions = new Sprite[]
            {
                 new Sprite(Resources._text600x400) { Position = new Vector2f(sliders[0].slider.GetGlobalBounds().Left + texts[2].GetGlobalBounds().Width + 20, 350), Scale = new Vector2f(5, 5) },
                 new Sprite(Resources._text900x600) { Position = new Vector2f(sliders[0].slider.GetGlobalBounds().Left + texts[2].GetGlobalBounds().Width + 20, 350),Scale = new Vector2f(5, 5) },
                 new Sprite(Resources._text1200x800) { Position = new Vector2f(sliders[0].slider.GetGlobalBounds().Left + texts[2].GetGlobalBounds().Width + 20, 350),Scale = new Vector2f(5, 5) },
                 new Sprite(Resources._text1400x950) { Position = new Vector2f(sliders[0].slider.GetGlobalBounds().Left + texts[2].GetGlobalBounds().Width + 20, 350),Scale = new Vector2f(5, 5) }
            };
            gameOverButtons = new Sprite[]
            {
                 new Sprite(Resources._buttonNewGame) { Position = new Vector2f(cResolution.X / 2, cResolution.Y - 250), Origin = (Vector2f)Resources._buttonNewGame.Size / 2, Scale = new Vector2f(6, 6) },
                 new Sprite(Resources._buttonQuit) { Position = new Vector2f(cResolution.X / 2, cResolution.Y - 250 + Resources._buttonNewGame.Size.Y*6), Origin = (Vector2f)Resources._buttonQuit.Size / 2, Scale = new Vector2f(6, 6) },
            };
            optionsButtons = new Sprite[]
            {
                new Sprite(Resources._buttonSave) { Position = new Vector2f(cResolution.X / 2 - Resources._buttonSave.Size.X*6/2, cResolution.Y - 150), Origin = (Vector2f)Resources._buttonSave.Size / 2, Scale = new Vector2f(6, 6) },
                new Sprite(Resources._buttonCancel) { Position = new Vector2f(cResolution.X / 2  + Resources._buttonCancel.Size.X*6/2, cResolution.Y - 150), Origin = (Vector2f)Resources._buttonCancel.Size / 2, Scale = new Vector2f(6, 6) }
            };
            Orb = new Sprite(Resources._projectile) { Position = new Vector2f(cResolution.X / 2, cResolution.Y / 2 - 150), Origin = (Vector2f)Resources._projectile.Size / 2, Scale = new Vector2f(20, 20) };
            deathDetails = new Sprite(Resources._deathDetails) { Scale = new Vector2f(7, 7), Position = new Vector2f(10, cResolution.Y - Resources._deathDetails.Size.Y * 7 - 10) };
            itemInfo = new ItemInfo(cResolution);

            BlackCoverShape = new RectangleShape() { Position = new Vector2f(0, 0), Size = new Vector2f(1200, 800), FillColor = new Color(0, 0, 0, 0) };

            OST.menu.Play();
            window = new RenderWindow(new VideoMode(1200, 800), "Unbinding of Kiril", Styles.Close);
            //window.SetView(new View(new FloatRect(-1200, -800, 1200*3, 800*3)));

            window.SetFramerateLimit(60);
            window.Closed += Win_Closed;

            if (File.Exists("settings.txt"))
            {
                string[] lines = File.ReadAllLines("settings.txt");
                foreach (var l in lines)
                {
                    string[] entries = l.Split(' ');
                    if (entries[0] == "resolution:")
                    {
                        Resolution = new Vector2u(uint.Parse(entries[1]), uint.Parse(entries[2]));
                        for (int i = 0; i < iResolutions.Length; i++)
                            if (int.Parse(entries[1]) == iResolutions[i].X)
                            {
                                sliders[2].lastValue = i * (100 / iResolutions.Length) + 1;
                                sliders[2].value = sliders[2].lastValue;
                            }
                    }
                    if (entries[0] == "music:")
                        sliders[0].value = float.Parse(entries[1]);

                    if (entries[0] == "sfx:")
                        sliders[1].value = float.Parse(entries[1]);
                }
            }

            window.Size = Resolution;
            window.Position = new Vector2i((int)VideoMode.DesktopMode.Width / 2 - (int)window.Size.X / 2, (int)VideoMode.DesktopMode.Height / 2 - (int)window.Size.Y / 2 - 20);

            Sprite Ground = new Sprite(Resources._ground) { Scale = new Vector2f(10, 10) };
            Sprite Walls = new Sprite(Resources._walls) { Scale = new Vector2f(10, 10) };

            grid = new Grid(1200, 800, 80, 120);

            // =============================== Load Map ===============================

            while (File.Exists("map" + mapCount + ".txt"))
                mapCount++;
            if (File.Exists("layout.txt"))
                roomCount = File.ReadAllLines("layout.txt").Count();
            Console.WriteLine($"Room Count: {roomCount}");

            List<Room> rooms = new List<Room>();

            wallsProjectile = new FloatRect[4]
            {
                new FloatRect(0, 0, cResolution.X, 50),
                new FloatRect(cResolution.X - 50, 0, cResolution.X, cResolution.Y),
                new FloatRect(0, cResolution.Y - 50, cResolution.X, cResolution.Y - 50),
                new FloatRect(0, 0, 50, cResolution.Y - 50)
            };

            walls = new FloatRect[4]
            {
                new FloatRect(0, 0, cResolution.X, 120),
                new FloatRect(cResolution.X - 120, 0, cResolution.X, cResolution.Y),
                new FloatRect(0, cResolution.Y - 120, cResolution.X, cResolution.Y - 120),
                new FloatRect(0, 0, 120, cResolution.Y - 120)
            };

            bool BigBreak = false;

            uint rainbowPointer = 0;

            while (window.IsOpen)
            {
                tick++;
                //Gameplay                                
                if (gameOn)
                    foreach (var r in rooms)
                    {
                        // =============================== Updating every object ===============================
                        if (r.isActive)
                        {
                            //if (tick % 1 == 0)
                            //    itemInfo.Slide("Title", "Subtitle");

                            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                                gameOn = AntiHoldDown(gameOn, "Escape");
                            if (Keyboard.IsKeyPressed(Keyboard.Key.B))
                                r.player.bombCount++;
                            r.player.Update();

                            r.Update();
                            for (int i = 0; i < rooms.Count; i++)
                                if (rooms[i].hasBeenActive) r.ui_Minimap.AddExplored(i);

                            foreach (var projectile in r.projectiles) projectile.Update();
                            foreach (var enemy in r.enemies) enemy.Update();
                            foreach (var particle in r.particles) particle.Update();
                            foreach (var bomb in r.bombs) bomb.Update();
                            foreach (var door in r.doors) door.Update();
                            foreach (var item in r.items) item.Update();
                            foreach (var Collidable in r.Collidables)
                                foreach (var collidable in Collidable)
                                {
                                    collidable.Update();
                                    if (collidable.animSprite != null) collidable.animSprite.Update(collidable.sprite.Position);
                                }
                            r.ui_Heart.Update(r.player.Health);
                            r.ui_Minimap.Update();
                            r.uiStats[0].Update(r.player.bombCount);
                            r.uiStats[1].Update(r.player.Damage);
                            r.uiStats[2].Update(r.player.fireRate);
                            r.uiStats[3].Update(r.player.speed);
                            r.uiPerk.Update(r.player, r.player.perks.charges);
                            itemInfo.Update();



                            // =============================== Outside class checks and interactions with World ===============================

                            // ------------------ Projectile Interactions ------------------
                            foreach (var projectile in r.projectiles.ToList())
                            {
                                bool CreateProjectileParticlesUponImpact = false;

                                foreach (var enemy in r.enemies.ToList())
                                {
                                    if (!enemy.isDead && projectile.collider.Intersects(enemy.collider))
                                    {
                                        r.projectiles.Remove(projectile);
                                        enemy.Health -= r.player.Damage;
                                        r.particles.Add(new Particles(Resources._bloodParticles, new Vector2f(enemy.sprite.Position.X + enemy.sprite.Texture.Size.X * enemy.sprite.Scale.X / 2, enemy.sprite.Position.Y + enemy.sprite.Texture.Size.Y * enemy.sprite.Scale.Y / 2), 5, Lifespan: 15));
                                        if (enemy.Health <= 0)
                                        {
                                            r.particles.Add(new Particles(Resources._bloodParticles, new Vector2f(enemy.sprite.Position.X + enemy.sprite.Texture.Size.X * enemy.sprite.Scale.X / 2, enemy.sprite.Position.Y + enemy.sprite.Texture.Size.Y * enemy.sprite.Scale.Y / 2), 20, Speed: 1.2f, Lifespan: 15));
                                            int t = rng.Next(Resources._bloodStains.Count);
                                            r.static_entities.Add(new Sprite(Resources._bloodStains[t]) { Position = enemy.sprite.Position + new Vector2f(enemy.sprite.GetGlobalBounds().Width / 2, enemy.sprite.GetGlobalBounds().Height / 2), Origin = (Vector2f)Resources._bloodStains[t].Size / 2, Scale = new Vector2f(4, 4), Rotation = rng.Next(360), Color = new Color(100, 100, 100, (byte)rng.Next(100, 200)) });
                                            for (int i = 0; i < rng.Next(3, 8); i++)
                                            {
                                                int t1 = rng.Next(Resources._sBloodParticles.Count);
                                                float s = rng.Next(20, 40) / 10;
                                                r.static_entities.Add(new Sprite(Resources._sBloodParticles[t1]) { Position = new Vector2f(enemy.sprite.Position.X + enemy.sprite.GetGlobalBounds().Width / 2 + rng.Next(-50, 50), enemy.sprite.Position.Y + enemy.sprite.GetGlobalBounds().Height / 2 + rng.Next(-50, 50)), Origin = (Vector2f)Resources._sBloodParticles[t1].Size / 2, Scale = new Vector2f(s, s), Rotation = rng.Next(360), Color = new Color(100, 100, 100, (byte)rng.Next(200, 255)) });
                                            }

                                        }
                                        CreateProjectileParticlesUponImpact = true;
                                    }
                                }

                                foreach (var wall in wallsProjectile)
                                    if (projectile.collider.Intersects(wall)) CreateProjectileParticlesUponImpact = true;

                                foreach (var Collidable in r.Collidables)
                                    foreach (var collidable in Collidable)
                                        if (projectile.collider.Intersects(collidable.collider)) CreateProjectileParticlesUponImpact = true;

                                if (CreateProjectileParticlesUponImpact)
                                {
                                    r.particles.Add(new Particles(Resources._projectileParticles, projectile.sprite.Position, 10, CustomColor: projectile.sprite.Color));
                                    r.projectiles.Remove(projectile);
                                }

                                //if (!projectile.sprite.GetGlobalBounds().Intersects(AreaOfReach)) projectiles.Remove(projectile);
                            }

                            // ------------------ Katana Interactions ------------------
                            if (r.player.perks.katDamage != 0)
                            {
                                bool hasHit = false;
                                foreach (var e in r.enemies.ToList())
                                {
                                    if (e.collider.Intersects(r.player.perks.KatanaColliderL) && r.player.perks.katDir == 0 ||
                                        e.collider.Intersects(r.player.perks.KatanaColliderR) && r.player.perks.katDir == 1 ||
                                        e.collider.Intersects(r.player.perks.KatanaColliderU) && r.player.perks.katDir == 2 ||
                                        e.collider.Intersects(r.player.perks.KatanaColliderD) && r.player.perks.katDir == 3)
                                    {
                                        hasHit = true;
                                        e.Health -= (int)r.player.perks.katDamage;
                                        r.particles.Add(new Particles(Resources._bloodParticles, new Vector2f(e.sprite.Position.X + e.sprite.Texture.Size.X * e.sprite.Scale.X / 2, e.sprite.Position.Y + e.sprite.Texture.Size.Y * e.sprite.Scale.Y / 2), 15, Speed: 1.2f, Lifespan: 15));

                                        int t = rng.Next(Resources._bloodStains.Count);
                                        r.static_entities.Add(new Sprite(Resources._bloodStains[t]) { Position = e.sprite.Position + new Vector2f(e.sprite.GetGlobalBounds().Width / 2, e.sprite.GetGlobalBounds().Height / 2), Origin = (Vector2f)Resources._bloodStains[t].Size / 2, Scale = new Vector2f(4, 4), Rotation = rng.Next(360), Color = new Color(100, 100, 100, (byte)rng.Next(100, 200)) });

                                        for (int i = 0; i < rng.Next(5, 10); i++)
                                        {
                                            int t1 = rng.Next(Resources._sBloodParticles.Count);
                                            float s = rng.Next(20, 40) / 10;
                                            r.static_entities.Add(new Sprite(Resources._sBloodParticles[t1]) { Position = new Vector2f(e.sprite.Position.X + e.sprite.GetGlobalBounds().Width / 2 + rng.Next(-50, 50), e.sprite.Position.Y + e.sprite.GetGlobalBounds().Height / 2 + rng.Next(-50, 50)), Origin = (Vector2f)Resources._sBloodParticles[t1].Size / 2, Scale = new Vector2f(s, s), Rotation = rng.Next(360), Color = new Color(100, 100, 100, (byte)rng.Next(200, 255)) });
                                        }
                                        playSound.Add(new Thread(() => PlaySound(OST.Flesh, true, rng.Next(-100, -25))));
                                        playSound[playSound.Count - 1].Start();
                                    }
                                }
                                if (hasHit) r.player.perks.katDamage = 0;
                            }
                            // ------------------ Bomb Interactions ------------------
                            foreach (var bomb in r.bombs.ToList())
                            {
                                if (bomb.tick == bomb.lifetime)
                                {
                                    playSound.Add(new Thread(() => PlaySound(OST.Explosion, true)));
                                    playSound[playSound.Count - 1].Start();

                                    foreach (var c in r.Collidables.ToList())
                                        foreach (var collidable in c.ToList())
                                            if (bomb.AOE.Intersects(collidable.collider) && collidable.isDestroyable)
                                            {
                                                if (collidable.hasCross) r.SpawnItem(1, 1, (Vector2i)collidable.sprite.Position + new Vector2i(15, 15));
                                                r.particles.Add(new Particles(Resources._rockParticles, new Vector2f(collidable.sprite.Position.X + collidable.sprite.GetGlobalBounds().Width, collidable.sprite.Position.Y + collidable.sprite.GetGlobalBounds().Height / 2), 10, 3));
                                                c.Remove(collidable);
                                            }

                                    foreach (var door in r.doors)
                                        if (bomb.AOE.Intersects(door.sprite.GetGlobalBounds()))
                                            door.forceOpen = true;

                                    if (bomb.AOE.Intersects(r.player.collider) && !r.player.isInvincible) r.player.Health -= 2;

                                    foreach (var e in r.enemies)
                                        if (bomb.AOE.Intersects(e.collider))
                                        {
                                            e.Health -= 6;
                                            r.particles.Add(new Particles(Resources._bloodParticles, new Vector2f(e.sprite.Position.X + e.sprite.Texture.Size.X * e.sprite.Scale.X / 2, e.sprite.Position.Y + e.sprite.Texture.Size.Y * e.sprite.Scale.Y / 2), e.isDead ? 5 : 20, Speed: 1.2f, Lifespan: 15));
                                            int t = rng.Next(Resources._bloodStains.Count);
                                            r.static_entities.Add(new Sprite(Resources._bloodStains[t]) { Position = e.sprite.Position + new Vector2f(e.sprite.GetGlobalBounds().Width / 2, e.sprite.GetGlobalBounds().Height / 2), Origin = (Vector2f)Resources._bloodStains[t].Size / 2, Scale = new Vector2f(4, 4), Rotation = rng.Next(360), Color = new Color(100, 100, 100, (byte)rng.Next(100, 200)) });

                                            for (int i = 0; i < rng.Next(5, 10); i++)
                                            {
                                                int t1 = rng.Next(Resources._sBloodParticles.Count);
                                                float s = rng.Next(20, 40) / 10;
                                                r.static_entities.Add(new Sprite(Resources._sBloodParticles[t1]) { Position = new Vector2f(e.sprite.Position.X + e.sprite.GetGlobalBounds().Width / 2 + rng.Next(-50, 50), e.sprite.Position.Y + e.sprite.GetGlobalBounds().Height / 2 + rng.Next(-50, 50)), Origin = (Vector2f)Resources._sBloodParticles[t1].Size / 2, Scale = new Vector2f(s, s), Rotation = rng.Next(360), Color = new Color(100, 100, 100, (byte)rng.Next(200, 255)) });
                                            }
                                        }

                                    shakeThread = new Thread(ShakeScreen);
                                    shakeThread.Start();

                                }
                                if (bomb.tick - bomb.lifetime > 15)
                                {
                                    r.static_entities.Add(new Sprite(Resources._burntOut) { Position = new Vector2f(bomb.sprite.currentSprite.Position.X - Resources._burntOut.Size.X * 5 / 2, bomb.sprite.currentSprite.Position.Y - Resources._burntOut.Size.Y * 5 / 2), Scale = new Vector2f(5, 5) });
                                    r.bombs.Remove(bomb);
                                }
                            }

                            // ------------------ Particle checks ------------------
                            foreach (var particle in r.particles.ToList())
                                if (particle.sprites[0].Scale.X < 1)
                                    r.particles.Remove(particle);

                            // ------------------ Door checks ------------------
                            for (int i = 0; i < r.doors.Count; i++)
                            {
                                if (r.doors[i].isOpen && r.player.collider.Intersects(r.doors[i].collider))
                                {
                                    if ((r.doors[i].position == 0 && Keyboard.IsKeyPressed(Keyboard.Key.A)) ||
                                        (r.doors[i].position == 1 && Keyboard.IsKeyPressed(Keyboard.Key.W)) ||
                                        (r.doors[i].position == 2 && Keyboard.IsKeyPressed(Keyboard.Key.D)) ||
                                        (r.doors[i].position == 3 && Keyboard.IsKeyPressed(Keyboard.Key.S)))
                                    {
                                        GoToAnotherRoom(i);
                                    }
                                }
                            }
                            void GoToAnotherRoom(int i = 0, int specificRoom = -1)
                            {
                                blackDipThread = new Thread(DipToBlack);
                                blackDipThread.Start();
                                Image img = window.Capture();
                                while (blackDipThread.IsAlive)
                                {
                                    window.Draw(new Sprite(new Texture(img)) { Scale = new Vector2f((float)cResolution.X / (float)Resolution.X, (float)cResolution.Y / (float)Resolution.Y) });
                                    window.Draw(BlackCoverShape);
                                    window.Display();
                                }
                                // TODO: Maybe faster transition without a little stutter

                                blackDipThread = new Thread(DipToNormal);
                                blackDipThread.Start();

                                if (specificRoom == -1)
                                {
                                    rooms[r.doors[i].leadsTo].player = r.player;
                                    rooms[r.doors[i].leadsTo].player.CollidesWith = rooms[r.doors[i].leadsTo].Collidables;
                                    foreach (var e in rooms[r.doors[i].leadsTo].enemies)
                                        e.Followed = rooms[r.doors[i].leadsTo].player;

                                    rooms[r.doors[i].leadsTo].isActive = true;
                                    r.isActive = false;

                                    rooms[r.doors[i].leadsTo].player.Health = r.player.Health;
                                    rooms[r.doors[i].leadsTo].player.perks.isUsingperk = false;

                                    if (r.doors[i].position == 0) rooms[r.doors[i].leadsTo].player.sprite.Position = new Vector2f(cResolution.X - boundsOffset - r.player.sprite.GetGlobalBounds().Width, cResolution.Y / 2 - r.player.sprite.GetGlobalBounds().Height / 2);
                                    if (r.doors[i].position == 1) rooms[r.doors[i].leadsTo].player.sprite.Position = new Vector2f(cResolution.X / 2 - r.player.sprite.GetGlobalBounds().Width / 2 + 5, cResolution.Y - boundsOffset - r.player.sprite.GetGlobalBounds().Height);
                                    if (r.doors[i].position == 2) rooms[r.doors[i].leadsTo].player.sprite.Position = new Vector2f(boundsOffset, cResolution.Y / 2 - r.player.sprite.GetGlobalBounds().Height / 2);
                                    if (r.doors[i].position == 3) rooms[r.doors[i].leadsTo].player.sprite.Position = new Vector2f(cResolution.X / 2 - r.player.sprite.GetGlobalBounds().Width / 2 + 5, boundsOffset);
                                }
                                else
                                {
                                    rooms[specificRoom].player = r.player;
                                    rooms[specificRoom].player.CollidesWith = rooms[specificRoom].Collidables;
                                    rooms[specificRoom].player.sprite.Position = rooms[specificRoom].playerSpawnPoint;
                                    rooms[specificRoom].player.perks.isUsingperk = false;

                                    foreach (var e in rooms[specificRoom].enemies)
                                        e.Followed = rooms[specificRoom].player;

                                    rooms[specificRoom].isActive = true;
                                    r.isActive = false;
                                }
                            }

                            // ------------------ Item checks ------------------
                            foreach (var i in r.items.ToList())
                            {
                                if (r.player.collider.Intersects(i.sprite.GetGlobalBounds()))
                                {
                                    if (i.rarity == 1 && i.item == Item.ItemsRare.SwitchRoom)
                                    {
                                        int a = rng.Next(roomCount);
                                        for (int j = 0; j < rooms.Count; j++)
                                            if (r == rooms[j])
                                                while (a == j)
                                                    a = rng.Next(roomCount);
                                        GoToAnotherRoom(specificRoom: a);
                                    }
                                    i.Action(r.player, ref r.uiPerk);
                                    if (i.RemoveMe) r.items.Remove(i);
                                }
                            }

                            // ------------------ Enemy checks ------------------
                            foreach (var e in r.enemies.ToList())
                            {
                                if (e.isDead) r.enemies.Remove(e);
                                if (!e.isDead && e.collider.Intersects(r.player.collider) && !r.player.isInvincible)
                                {
                                    r.player.Health--;
                                    break;
                                }
                            }

                            // =============================== Player Interacting with World ===============================

                            // Shooting
                            if (r.player.isShooting && tick > r.player.lastShot + r.player.fireRate)
                            {
                                r.player.shotsFired++;
                                r.player.lastShot = tick;
                                Projectile projectile = new Projectile(
                                    r.player.sprite.Position.X + r.player.sprite.Scale.X * r.player.sprite.Texture.Size.X / 2,
                                    r.player.sprite.Position.Y + r.player.sprite.Scale.Y * r.player.sprite.Texture.Size.Y / 2,
                                    r.player.directionOfShot.X,
                                    r.player.directionOfShot.Y,
                                    r.player.pSize,
                                    r.player.pColor,
                                    Resources._projectile);
                                r.projectiles.Add(projectile);
                            }

                            //Placing bombs
                            if (r.player.isPlacingBomb)
                            {
                                r.player.bombCount--;
                                r.bombs.Add(new Bomb(new Vector2f(r.player.collider.Left, r.player.collider.Top), 100, 100));
                            }

                            /////// =============================== Drawing =============================== ///////


                            window.DispatchEvents();
                            window.Clear(Color.Black);


                            // ================== Non-UI ==================

                            window.Draw(Ground);

                            foreach (var SE in r.static_entities)
                                window.Draw(SE);

                            window.Draw(Walls);

                            foreach (var door in r.doors)
                                window.Draw(door.sprite);

                            foreach (var bomb in r.bombs)
                                window.Draw(bomb.shadow);
                            foreach (var Collidable in r.Collidables)
                                foreach (var cS in Collidable)
                                    window.Draw(cS.shadow);
                            foreach (var enemy in r.enemies)
                                window.Draw(enemy.shadow);
                            window.Draw(r.player.shadow);



                            foreach (var Collidable in r.Collidables)
                                foreach (var cS in Collidable)
                                {
                                    window.Draw((cS.isAnimated) ? cS.animSprite.currentSprite : cS.sprite);
                                    if (cS.hasCross) window.Draw(cS.cross);
                                }

                            foreach (var enemy in r.enemies)
                                if (enemy.isDead) window.Draw(enemy.sprite);
                            foreach (var enemy in r.enemies)
                                if (!enemy.isDead) window.Draw(enemy.sprite);

                            foreach (var bomb in r.bombs)
                                window.Draw(bomb.sprite.currentSprite);

                            foreach (var i in r.items)
                                window.Draw(i.sprite);

                            foreach (var particle in r.particles)
                                foreach (var sprite in particle.sprites)
                                    window.Draw(sprite);

                            window.Draw(r.player.perks.KatanaSlashR.currentSprite);
                            window.Draw(r.player.perks.KatanaSlashL.currentSprite);
                            window.Draw(r.player.perks.KatanaSlashU.currentSprite);
                            window.Draw(r.player.perks.KatanaSlashD.currentSprite);

                            if (r.player.perks.hasKatana)
                            {
                                if (r.player.sprite.Texture == r.player.texR) window.Draw(r.player.perks.KatanaR);
                                if (r.player.sprite.Texture == r.player.texL) window.Draw(r.player.perks.KatanaL);
                                if (r.player.sprite.Texture == r.player.texU) window.Draw(r.player.perks.KatanaU);
                            }

                            if (r.player.perks.hasJetpack && r.player.sprite.Texture != r.player.texU)
                            {
                                if (r.player.sprite.Texture == r.player.texR || r.player.sprite.Texture == r.player.texD)
                                    window.Draw(r.player.perks.JetPackL.currentSprite);
                                if (r.player.sprite.Texture == r.player.texL || r.player.sprite.Texture == r.player.texD)
                                    window.Draw(r.player.perks.JetPackR.currentSprite);
                            }

                            if (r.player.shotsFired % 2 == 0 || r.player.sprite.Texture == r.player.texU)
                            {
                                if (r.player.sprite.Texture == r.player.texD)
                                {
                                    window.Draw(r.player.sprite);
                                    foreach (var projectile in r.projectiles)
                                        window.Draw(projectile.sprite);
                                }
                                else
                                {
                                    foreach (var projectile in r.projectiles)
                                        window.Draw(projectile.sprite);
                                    window.Draw(r.player.sprite);
                                }
                            }
                            else
                            {
                                window.Draw(r.player.sprite);
                                foreach (var projectile in r.projectiles)
                                    window.Draw(projectile.sprite);
                            }

                            if (r.player.perks.hasKatana && r.player.sprite.Texture == r.player.texD)
                                window.Draw(r.player.perks.KatanaD);

                            if (r.player.perks.hasJetpack && r.player.sprite.Texture == r.player.texU)
                            {
                                window.Draw(r.player.perks.JetPackL.currentSprite);
                                window.Draw(r.player.perks.JetPackR.currentSprite);
                            }


                            // ================== UI ==================

                            window.Draw(heartShadow);
                            window.Draw(minimapShadow);

                            foreach (var heart in r.ui_Heart.sprites)
                                window.Draw(heart);

                            if (itemInfo.isWorking)
                            {
                                window.Draw(itemInfo.bg);
                                window.Draw(itemInfo.text);
                                window.Draw(itemInfo.subtext);
                            }

                            window.Draw(BlackCoverShape);

                            // ================== Debug ==================

                            if (Keyboard.IsKeyPressed(Keyboard.Key.Numpad1))
                                r.SpawnItem(1, 0);

                            //window.Draw(new RectangleShape() { Position = new Vector2f(r.player.collider.Left, r.player.collider.Top), Size = new Vector2f(r.player.collider.Width, r.player.collider.Height), FillColor = new Color(255, 100, 255, 100) });

                            //window.Draw(new RectangleShape() { Position = new Vector2f(player.sprite.Position.X + sprite.Texture.Size.X * (sprite.Scale.X / 2), sprite.Position.Y + 20}, 20, speed })

                            //foreach (var draw in rocks)
                            //    window.Draw(new RectangleShape() { Position = new Vector2f(draw.collider.Left, draw.collider.Top), Size = new Vector2f(draw.collider.Width, draw.collider.Height), FillColor = Color.Transparent, OutlineColor = Color.Red, OutlineThickness = 2 });
                            //
                            //foreach (var draw in r.projectiles)
                            //    window.Draw(new RectangleShape() { Position = new Vector2f(draw.collider.Left, draw.collider.Top), Size = new Vector2f(draw.collider.Width, draw.collider.Height), FillColor = Color.Transparent, OutlineColor = Color.Red, OutlineThickness = 2 });
                            //
                            if (Keyboard.IsKeyPressed(Keyboard.Key.K))
                            {
                                foreach (var enemy in r.enemies)
                                {
                                    window.Draw(new RectangleShape(new Vector2f(enemy.collider.Width, enemy.collider.Height)) { Position = new Vector2f(enemy.collider.Left, enemy.collider.Top), FillColor = new Color(255, 255, 255, 255 / 2) });
                                    window.Draw(new RectangleShape(new Vector2f(enemy.viewRange.Width, enemy.viewRange.Height)) { Position = new Vector2f(enemy.viewRange.Left, enemy.viewRange.Top), FillColor = new Color(255 / 2, 255, 255, 255 / 3) });
                                    window.Draw(new RectangleShape() { Position = new Vector2f(enemy.collider.Left + enemy.collider.Width / 4, enemy.collider.Top - 5), Size = new Vector2f(enemy.collider.Width / 2, 5) });
                                    window.Draw(new RectangleShape() { Position = new Vector2f(enemy.collider.Left + enemy.collider.Width / 4, enemy.collider.Top + enemy.collider.Height), Size = new Vector2f(enemy.collider.Width / 2, 5) });
                                    window.Draw(new RectangleShape() { Position = new Vector2f(enemy.collider.Left - 5, enemy.collider.Top + enemy.collider.Height / 4), Size = new Vector2f(5, enemy.collider.Height / 2) });
                                    window.Draw(new RectangleShape() { Position = new Vector2f(enemy.collider.Left + enemy.collider.Width, enemy.collider.Top + enemy.collider.Height / 4), Size = new Vector2f(5, enemy.collider.Height / 2) });
                                }

                                foreach (var g in grid.grid)
                                    window.Draw(new RectangleShape() { Position = new Vector2f(g.Left, g.Top), Size = new Vector2f(g.Width, g.Height), FillColor = Color.Transparent, OutlineColor = new Color(255, 255, 255, 50), OutlineThickness = 1 });
                                window.Draw(new RectangleShape() { Position = new Vector2f(0, boundsOffset), Size = new Vector2f(cResolution.X, 2), FillColor = Color.Yellow });
                                window.Draw(new RectangleShape() { Position = new Vector2f(0, cResolution.Y - boundsOffset), Size = new Vector2f(cResolution.X, 2), FillColor = Color.Yellow });
                                window.Draw(new RectangleShape() { Position = new Vector2f(boundsOffset, 0), Size = new Vector2f(2, cResolution.Y), FillColor = Color.Yellow });
                                window.Draw(new RectangleShape() { Position = new Vector2f(cResolution.X - boundsOffset, 0), Size = new Vector2f(2, cResolution.Y), FillColor = Color.Yellow });

                                foreach (var d in r.doors)
                                    if (d.isOpen) window.Draw(new RectangleShape() { Position = new Vector2f(d.collider.Left, d.collider.Top), Size = new Vector2f(d.collider.Width, d.collider.Height), FillColor = new Color(200, 100, 0, 100) });
                            }

                            //window.Draw(new RectangleShape() { Position = new Vector2f(300, 300), Scale = new Vector2f(15, 500), FillColor = Color.White });
                            //window.Draw(new RectangleShape() {Position = new Vector2f(r.player.perks.KatanaColliderL.Left, r.player.perks.KatanaColliderL.Top), Size = new Vector2f(r.player.perks.KatanaColliderL.Width, r.player.perks.KatanaColliderL.Height) });
                            //window.Draw(new RectangleShape() {Position = new Vector2f(r.player.perks.KatanaColliderR.Left, r.player.perks.KatanaColliderR.Top), Size = new Vector2f(r.player.perks.KatanaColliderR.Width, r.player.perks.KatanaColliderR.Height) });
                            //window.Draw(new RectangleShape() {Position = new Vector2f(r.player.perks.KatanaColliderU.Left, r.player.perks.KatanaColliderU.Top), Size = new Vector2f(r.player.perks.KatanaColliderU.Width, r.player.perks.KatanaColliderU.Height) });
                            //window.Draw(new RectangleShape() {Position = new Vector2f(r.player.perks.KatanaColliderD.Left, r.player.perks.KatanaColliderD.Top), Size = new Vector2f(r.player.perks.KatanaColliderD.Width, r.player.perks.KatanaColliderD.Height) });
                            //Console.WriteLine("l: " + r.player.perks.KatanaColliderL.Left + " t: " + r.player.perks.KatanaColliderL.Top + " w: " + r.player.perks.KatanaColliderL.Width + " h: "+r.player.perks.KatanaColliderL.Height);
                            //
                            //window.Draw(new RectangleShape() { Position = new Vector2f(player.collider.Left + player.collider.Width/4, player.collider.Top - 5), Size = new Vector2f(player.collider.Width/2, 5)});
                            //window.Draw(new RectangleShape() { Position = new Vector2f(player.collider.Left + player.collider.Width/4, player.collider.Top + player.collider.Height), Size = new Vector2f(player.collider.Width/2, 5)});
                            //window.Draw(new RectangleShape() { Position = new Vector2f(player.collider.Left - 5, player.collider.Top + player.collider.Height/4), Size = new Vector2f(5, player.collider.Height/2) });
                            //window.Draw(new RectangleShape() { Position = new Vector2f(player.collider.Left + player.collider.Width, player.collider.Top+player.collider.Height / 4), Size = new Vector2f(5, player.collider.Height/2) });
                            //foreach (var bomb in bombs)
                            //{
                            //    window.Draw(new RectangleShape() { Position = new Vector2f(bomb.AOE.Left, bomb.AOE.Top), Size = new Vector2f(bomb.AOE.Width, bomb.AOE.Height), FillColor = new Color(200, 200, 0, 100) });
                            //    window.Draw(new RectangleShape() { Position = new Vector2f(bomb.sprite.Position.X, bomb.sprite.Position.Y), Size = new Vector2f(bomb.sprite.GetGlobalBounds().Width, bomb.sprite.GetGlobalBounds().Height), FillColor = new Color(200, 0, 0, 100) });
                            //}
                            //foreach (var door in r.doors)
                            //    window.Draw(new RectangleShape() { Position = new Vector2f(door.collider.Left, doo.collider.Top), Size = new Vector2f(door.collider.Width, door.collider.Height), FillColor = new Color(200, 100, 0, 100) });
                            //foreach (var g in r.ui_Minimap.grid)
                            //    window.Draw(new RectangleShape() { Position = g, Size = new Vector2f(30, 30), FillColor = new Color(10,10,10), OutlineThickness = 1, OutlineColor = new Color(70,70,70) });

                            foreach (var g in r.ui_Minimap.miniRooms)
                                window.Draw(g.sprite);

                            foreach (var ui in r.uiStats)
                            {
                                window.Draw(ui.sprite);
                                window.Draw(ui.text);
                            }
                            window.Draw(r.uiPerk.itemSprite);
                            window.Draw(r.uiPerk.barSprite);

                            window.Display();
                        }
                    }
                else if (!gameOver)
                {
                    if (OST.menu.Status != SoundStatus.Playing)
                    {
                        OST.menu.Play();
                        OST.bg.Pause();
                    }
                    int speedOfScrolling = -2;
                    scrollingBG.Position = new Vector2f(scrollingBG.Position.X + speedOfScrolling, scrollingBG.Position.Y);
                    if (scrollingBG.Position.X < -scrollingBG.GetGlobalBounds().Width / 2) scrollingBG.Position = new Vector2f(0, 0);

                    Orb.Rotation += 0.5f;
                    if (tick % 1 == 0)
                    {
                        //Orb.Color = new Color((byte)rng.Next(100, 200), (byte)rng.Next(100, 200), (byte)rng.Next(100, 200));
                        rainbowPointer = rainbowPointer >= Resources._RainbowStrip.Size.X - 1 ? 0 : rainbowPointer + 1;
                        Orb.Color = Resources._RainbowStrip.GetPixel(rainbowPointer, 0);
                    }

                    window.DispatchEvents();

                    //Scrolling background
                    window.Draw(scrollingBG);

                    //UI
                    foreach (var s in sliders)
                        s.Update();

                    if (!optionsOn)
                    {
                        window.Draw(Orb);

                        // ====================== Button Checks ======================
                        foreach (var b in menuButtons)
                        {
                            window.Draw(b);
                            if (IsMouseOver(b.GetGlobalBounds()))
                            {
                                if (!(b == menuButtons[1] && !hasGeneratedMap))
                                {
                                    window.Draw(new RectangleShape() { Position = new Vector2f(b.GetGlobalBounds().Left, b.GetGlobalBounds().Top), Size = new Vector2f(b.GetGlobalBounds().Width, b.GetGlobalBounds().Height), FillColor = Color.Transparent, OutlineColor = new Color(215, 155, 0), OutlineThickness = 3 });
                                    b.Color = new Color(215, 155, 0);
                                }

                                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                                {
                                    if (b == menuButtons[0])
                                    {
                                        AntiHoldDown();
                                        hasGeneratedMap = true;
                                        rooms.Clear();
                                        for (int i = 0; i < roomCount; i++)
                                            rooms.Add(new Room(i, cResolution, mapCount, grid, boundsOffset));
                                    }
                                    if ((hasGeneratedMap && b == menuButtons[1]) || b == menuButtons[0])
                                    {
                                        gameOn = true;
                                        OST.bg.Play();
                                        OST.menu.Pause();
                                    }
                                    if (b == menuButtons[2])
                                        optionsOn = AntiHoldDown(optionsOn);
                                    if (b == menuButtons[3])
                                        Environment.Exit(1);
                                }
                            }
                            else b.Color = (b == menuButtons[1] && !hasGeneratedMap) ? new Color(100, 100, 100) : Color.White;
                        }
                    }
                    else
                    {
                        foreach (var s in sliders)
                        {
                            window.Draw(s.slider);
                            window.Draw(s.pointer);

                            if (IsMouseOver(s.pointer.GetGlobalBounds()))
                            {
                                s.slider.Color = new Color(215, 155, 0);
                                s.pointer.Color = new Color(215, 155, 0);

                                window.Draw(new RectangleShape() { Position = new Vector2f(s.pointer.GetGlobalBounds().Left, s.pointer.GetGlobalBounds().Top), Size = new Vector2f(s.pointer.GetGlobalBounds().Width, s.pointer.GetGlobalBounds().Height), FillColor = Color.Transparent, OutlineColor = new Color(215, 155, 0), OutlineThickness = 3 });

                                if (Mouse.IsButtonPressed(Mouse.Button.Left)) s.isSliding = true;
                                else s.isSliding = false;
                            }
                            else
                            {
                                s.slider.Color = Color.White;
                                s.pointer.Color = Color.White;
                            }
                            if (s.isSliding && !Mouse.IsButtonPressed(Mouse.Button.Left)) s.isSliding = false;
                        }

                        foreach (var t in texts)
                            window.Draw(t);

                        foreach (var b in optionsButtons)
                        {
                            b.Color = Color.White;
                            if (IsMouseOver(b.GetGlobalBounds()))
                            {
                                if (b == optionsButtons[0])
                                {
                                    b.Color = new Color(0, 200, 75);
                                    window.Draw(new RectangleShape() { Position = new Vector2f(b.GetGlobalBounds().Left, b.GetGlobalBounds().Top), Size = new Vector2f(b.GetGlobalBounds().Width, b.GetGlobalBounds().Height), FillColor = Color.Transparent, OutlineColor = new Color(0, 200, 75), OutlineThickness = 3 });
                                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                                    {
                                        foreach (var s in sliders)
                                        {
                                            s.lastValue = s.value;
                                            if (s.controls == "Resolution")
                                            {
                                                Resolution = iResolutions[s.specialValue];
                                                window.Size = Resolution;
                                                window.Position = new Vector2i((int)VideoMode.DesktopMode.Width / 2 - (int)window.Size.X / 2, (int)VideoMode.DesktopMode.Height / 2 - (int)window.Size.Y / 2 - 20);
                                            }
                                        }
                                        if (File.Exists("settings.txt"))
                                        {
                                            string output = "";
                                            output += "resolution: " + Resolution.X + " " + Resolution.Y;
                                            output += "\nmusic: " + sliders[0].value;
                                            output += "\nsfx: " + sliders[1].value;
                                            File.WriteAllText("settings.txt", output);
                                        }
                                        optionsOn = AntiHoldDown(optionsOn);
                                    }
                                }
                                if (b == optionsButtons[1])
                                {
                                    b.Color = new Color(200, 0, 75);
                                    window.Draw(new RectangleShape() { Position = new Vector2f(b.GetGlobalBounds().Left, b.GetGlobalBounds().Top), Size = new Vector2f(b.GetGlobalBounds().Width, b.GetGlobalBounds().Height), FillColor = Color.Transparent, OutlineColor = new Color(200, 0, 75), OutlineThickness = 3 });
                                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                                    {
                                        foreach (var s in sliders)
                                            s.value = s.lastValue;
                                        optionsOn = AntiHoldDown(optionsOn);
                                    }
                                }
                            }
                            window.Draw(b);
                        }
                    }

                    if (BigBreak && !Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        BigBreak = false;
                        optionsOn = false;
                    }
                    window.Display();
                }
                else if (gameOver)
                {
                    OST.menu.Stop();
                    OST.bg.Stop();
                    if (OST.deathScreen.Status != SoundStatus.Playing) OST.deathScreen.Play();

                    window.Draw(deathScreen);
                    window.Draw(deathDetails);

                    foreach (var b in gameOverButtons)
                    {
                        b.Color = new Color(255, 0, 0);

                        if (IsMouseOver(b.GetGlobalBounds()))
                        {
                            window.Draw(new RectangleShape() { Position = new Vector2f(b.GetGlobalBounds().Left, b.GetGlobalBounds().Top), Size = new Vector2f(b.GetGlobalBounds().Width, b.GetGlobalBounds().Height), FillColor = Color.Transparent, OutlineColor = b.Color, OutlineThickness = 3 });

                            if (b == gameOverButtons[0] && Mouse.IsButtonPressed(Mouse.Button.Left))
                            {
                                rooms.Clear();
                                for (int i = 0; i < roomCount; i++)
                                    rooms.Add(new Room(i, cResolution, mapCount, grid, boundsOffset));
                                gameOver = false;
                                gameOn = true;
                                OST.deathScreen.Stop();
                                OST.bg.Play();
                            }
                            if (b == gameOverButtons[1] && Mouse.IsButtonPressed(Mouse.Button.Left))
                            {
                                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                                    AntiHoldDown();
                                Environment.Exit(1);
                            }
                        }

                        window.Draw(b);
                    }

                    window.DispatchEvents();
                    window.Display();
                }
            }
        }

        public void PlaySound(Sound sound, bool pitchShift = false, int range = 0)
        {
            Sound playMe = new Sound(sound);
            float pitch = 0;
            if (pitchShift) pitch = ((range == 0) ? rng.Next(1, 50) : range) / 100f;

            playMe.Pitch = 1 - pitch;
            playMe.Play();
        }

        public void ShakeScreen()
        {
            int actualamount = 50;
            int amount = actualamount;
            for (int i = 0; i <= actualamount; i++)
            {
                Thread.Sleep(10);
                if (amount > 0) amount--;
                Vector2f shake = new Vector2f(rng.Next(amount), rng.Next(amount));
                view = new View(new FloatRect(new Vector2f(0 + shake.X, 0 + shake.Y), new Vector2f(cResolution.X - amount, cResolution.Y - amount)));
                window.SetView(view);
            }
        }

        public Vector2f ToMousePos()
        {
            Vector2f pos = new Vector2f(Mouse.GetPosition(window).X * cResolution.X / Resolution.X, Mouse.GetPosition(window).Y * cResolution.Y / Resolution.Y);
            return pos;
        }

        public bool IsMouseOver(FloatRect Here)
        {
            if (new FloatRect(new Vector2f(Mouse.GetPosition(window).X * cResolution.X / Resolution.X, Mouse.GetPosition(window).Y * cResolution.Y / Resolution.Y), new Vector2f(1, 1)).Intersects(Here))
                return true;
            else return false;
        }

        public void DipToBlack()
        {
            int opacity = 0;
            while (opacity < 255)
            {
                Thread.Sleep(1);
                BlackCoverShape.FillColor = new Color(0, 0, 0, (byte)opacity);
                opacity += 5;
            }
        }
        public void DipToNormal()
        {
            int opacity = 255;
            while (opacity > 0)
            {
                Thread.Sleep(1);
                BlackCoverShape.FillColor = new Color(0, 0, 0, (byte)opacity);
                opacity -= 5;
            }
        }

        public bool AntiHoldDown(bool SwitchMe = false, string button = "")
        {
            switch (button)
            {
                case "":
                    while (Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                    }
                    break;
                case "Escape":
                    while (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    {
                    }
                    break;
                default:
                    Console.WriteLine("uh oh...");
                    break;

            }
            SwitchMe = !SwitchMe;
            return SwitchMe;
        }

        void Win_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
