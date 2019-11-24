using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using SFML.Window;
using System.Threading;

namespace Another_SFML_Project
{
    class Entity : Program
    {
        public Sprite sprite;
        public Sprite shadow = new Sprite(Resources._shadow) { Scale = new Vector2f(7, 7) };
        public Sprite cross;
        public AnimatedSprite animSprite;
        public bool isPlayer;
        public bool isStatic;
        public bool isShooting;
        public bool isPlacingBomb;
        public bool isWalking;
        public bool isColliding = true;
        public bool isInvincible;
        public bool isAnimated;
        public bool isDestroyable;
        public bool hasCross;

        public bool isWalkingLR;
        public bool isWalkingUD;

        public int freezeTicks = 30;
        public bool freezeMovement;

        public bool isDead;

        public FloatRect collider;
        public FloatRect viewRange; // Used for enemies only

        // projectile info
        public Vector2f pSize = new Vector2f(1, 1);
        public Color pColor = Color.White;

        public Entity Followed;
        public List<Entity> Group = new List<Entity>();
        public List<List<Entity>> CollidesWith = new List<List<Entity>>();

        public Perks perks = new Perks();

        public int LastHealth;
        public int Health = 6;
        public int Damage = 1;

        public Vector2f dir = new Vector2f();

        public float speed;

        public int fireRate = 20;
        public int lastShot;

        public int shotsFired;

        public int bombCount = 3;
        public int bombFireRate = 40;
        public int lastBombPlaced;

        public Vector2f directionOfShot;
        public int bulletVelocity = 10;

        public List<Texture> animTextures = new List<Texture>();
        bool animSeq = false;
        int animTickSpeed = 0;
        int animDelay = 0;
        float animSize;
        public void SetupAnimation(List<Texture> _animTextures, bool _animSeq, int _animSize, int _animTickSpeed, int _animDelay, int _startSprite = 0)
        {
            animTextures = _animTextures;
            animSeq = _animSeq;
            animSize = _animSize;
            animTickSpeed = _animTickSpeed;
            animDelay = _animDelay;
            animSprite = new AnimatedSprite(animTextures, animSeq, animSize, animTickSpeed, animDelay, _startSprite: _startSprite);
        }

        public Texture texD;
        public Texture texU;
        public Texture texL;
        public Texture texR;

        // Invincibility frames
        private int invLast;
        private int invStart;

        private new int tick;
        private bool isConstDir;
        private Vector2f constDir;
        private int setConstDir;
        private int constDirTime = 100 + rng.Next(-20, 20);

        public Entity(int X, int Y, Texture Texture_D, Texture Texture_U = null, Texture Texture_L = null, Texture Texture_R = null, bool IsAnimated = false, bool IsPlayer = false, bool IsStatic = false, bool IsDestroyable = true, float Speed = 7f, bool _hasCross = false)
        {
            texD = new Texture(Texture_D);
            if (Texture_U != null) texU = new Texture(Texture_U);
            if (Texture_L != null) texL = new Texture(Texture_L);
            if (Texture_R != null) texR = new Texture(Texture_R);
            speed = Speed;
            sprite = new Sprite(texD) { Position = new Vector2f(X, Y), Scale = new Vector2f(7f, 7f) };
            collider = new FloatRect(sprite.GetGlobalBounds().Left, sprite.GetGlobalBounds().Top, sprite.GetGlobalBounds().Width, sprite.GetGlobalBounds().Height);
            isPlayer = IsPlayer;
            isStatic = IsStatic;
            isDestroyable = IsDestroyable;
            isAnimated = IsAnimated;
            hasCross = _hasCross;
            if (!IsPlayer && !IsStatic) freezeMovement = true;
            if (isStatic) shadow.Texture = Resources._shadow1;
            if (hasCross) cross = new Sprite(Resources._cross) { Scale = new Vector2f(7, 7), Position = sprite.Position + new Vector2f(42, 42), Color = new Color(255, 255, 255, 125) };
        }

        public void Update()
        {
            tick++;

            if (!isDead)
            {
                if (isPlayer)
                {
                    PlayerMovement();
                    PlayerShoot();
                    PlayerSide();
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && !perks.isUsingperk && perks.hasPerk && perks.charges == 2) 
                    {
                        perks.charges = 0;
                        perks.isUsingperk = true;
                    }
                    isColliding = !(perks.hasJetpack && perks.isUsingperk);
                    perks.Update(sprite);

                    if (Health < LastHealth)
                    {
                        invStart = tick;
                        isInvincible = true;
                    }
                    if (Health <= 0)
                    {
                        gameOn = false;
                        gameOver = true;
                    }
                    if (isInvincible)
                        PlayerInvinsible();
                    LastHealth = Health;
                }

                else if (!isPlayer && !isStatic) //aka enemy
                {
                    if (!freezeMovement)
                        EnemyMovement();
                    else
                    {
                        freezeTicks--;
                        if (freezeTicks <= 0) freezeMovement = false;
                    }
                    if (Health <= 0)
                    {
                        isDead = true;
                        //sprite.Texture = new Texture(texDead);
                        int darken = 175 - rng.Next(50);
                        sprite.Color = new Color((byte)darken, (byte)darken, (byte)darken);
                        //enemies.Remove(enemy);
                        if (dir.X < 0)
                            sprite.TextureRect = new IntRect((int)sprite.Texture.Size.X, 0, -(int)sprite.Texture.Size.X, (int)sprite.Texture.Size.Y);
                    }
                }

                if (isStatic)
                {
                    collider = new FloatRect(sprite.Position.X + 5, sprite.Position.Y + 5, sprite.Texture.Size.X * sprite.Scale.X - 10, sprite.Texture.Size.Y * sprite.Scale.Y - 10);
                    shadow.Position = new Vector2f(sprite.Position.X - 14, sprite.Position.Y - 14);
                }
                else shadow.Position = new Vector2f(sprite.Position.X - 14, sprite.Position.Y - 7);

            }
        }

        private bool canGoUp;
        private bool canGoDown;
        private bool canGoLeft;
        private bool canGoRight;

        private Vector2f accelerateLR = new Vector2f(0, 0);
        private Vector2f accelerateUD = new Vector2f(0, 0);
        private Vector2f deccelerateLR = new Vector2f(1, 1);
        private Vector2f deccelerateUD = new Vector2f(1, 1);

        private Vector2f lastDirLR = new Vector2f(0, 0);
        private Vector2f lastDirUD = new Vector2f(0, 0);

        private void PlayerMovement()
        {
            bool goingDown = Keyboard.IsKeyPressed(Keyboard.Key.S);
            bool goingUp = Keyboard.IsKeyPressed(Keyboard.Key.W);
            bool goingLeft = Keyboard.IsKeyPressed(Keyboard.Key.A);
            bool goingRight = Keyboard.IsKeyPressed(Keyboard.Key.D);
            float ALR = 0.1f; // Acceleration

            // ~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~= General Movement ~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=

            if (goingUp && accelerateUD.X < 1)
            {
                accelerateUD.X += ALR;
                deccelerateUD.X = 1;
            }
            if (goingDown && accelerateUD.Y < 1)
            {
                accelerateUD.Y += ALR;
                deccelerateUD.Y = 1;
            }
            if (!goingUp && deccelerateUD.X > 0)
            {
                deccelerateUD.X -= ALR;
                accelerateUD.X = 0;
            }
            if (!goingDown && deccelerateUD.Y > 0)
            {
                deccelerateUD.Y -= ALR;
                accelerateUD.Y = 0;
            }

            if (goingLeft && accelerateLR.X < 1)
            {
                accelerateLR.X += ALR;
                deccelerateLR.X = 1;
            }
            if (goingRight && accelerateLR.Y < 1)
            {
                accelerateLR.Y += ALR;
                deccelerateLR.Y = 1;
            }
            if (!goingLeft && deccelerateLR.X > 0)
            {
                deccelerateLR.X -= ALR;
                accelerateLR.X = 0;
            }
            if (!goingRight && deccelerateLR.Y > 0)
            {
                deccelerateLR.Y -= ALR;
                accelerateLR.Y = 0;
            }

            if (canGoUp)
            {
                if (goingUp)
                {
                    lastDirUD.X = -speed * accelerateUD.X;
                    sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y + lastDirUD.X);
                }
                else
                    sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y + lastDirUD.X * deccelerateUD.X);
            }
            if (canGoDown)
            {
                if (goingDown)
                {
                    lastDirUD.Y = speed * accelerateUD.Y;
                    sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y + lastDirUD.Y);
                }
                else
                    sprite.Position = new Vector2f(sprite.Position.X, sprite.Position.Y + lastDirUD.Y * deccelerateUD.Y);
            }
            if (canGoLeft)
            {
                if (goingLeft)
                {
                    lastDirLR.X = -speed * accelerateLR.X;
                    sprite.Position = new Vector2f(sprite.Position.X + lastDirLR.X, sprite.Position.Y);
                }
                else
                    sprite.Position = new Vector2f(sprite.Position.X + lastDirLR.X * deccelerateLR.X, sprite.Position.Y);
            }
            if (canGoRight)
            {
                if (goingRight)
                {
                    lastDirLR.Y = speed * accelerateLR.Y;
                    sprite.Position = new Vector2f(sprite.Position.X + lastDirLR.Y, sprite.Position.Y);
                }
                else
                    sprite.Position = new Vector2f(sprite.Position.X + lastDirLR.Y * deccelerateLR.Y, sprite.Position.Y);
            }

            // ~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~= Collision Detection ~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=

            collider = new FloatRect(sprite.Position.X + 10, sprite.Position.Y + 15, sprite.Texture.Size.X * sprite.Scale.X - 20, sprite.Texture.Size.Y * sprite.Scale.Y - 20);

            canGoUp = true;
            canGoDown = true;
            canGoLeft = true;
            canGoRight = true;

            if (isColliding)
                foreach (var CW in CollidesWith)
                    foreach (var collidable in CW)
                    {
                        if (new FloatRect(collider.Left + collider.Width / 4, collider.Top - 5, collider.Width / 2, 5).Intersects(collidable.collider)) canGoUp = false;
                        if (new FloatRect(collider.Left + collider.Width / 4, collider.Top + collider.Height, collider.Width / 2, 5).Intersects(collidable.collider)) canGoDown = false;
                        if (new FloatRect(collider.Left - 5, collider.Top + collider.Height / 4, 5, collider.Height / 2).Intersects(collidable.collider)) canGoLeft = false;
                        if (new FloatRect(collider.Left + collider.Width, collider.Top + collider.Height / 4, 5, collider.Height / 2).Intersects(collidable.collider)) canGoRight = false;
                    }

            if (collider.Intersects(walls[0])) canGoUp = false;
            if (collider.Intersects(walls[1])) canGoRight = false;
            if (collider.Intersects(walls[2])) canGoDown = false;
            if (collider.Intersects(walls[3])) canGoLeft = false;

        }
        private void PlayerShoot()
        {
            isShooting = true;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num1) && tick % 10 == 0 && Health > 0) Health--;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Num2) && tick % 10 == 0 && Health < 6) Health++;

            if (perks.hasKatana)
            {
                isShooting = false;
                if (!perks.KatanaSlashL.isRunningState && !perks.KatanaSlashR.isRunningState && !perks.KatanaSlashU.isRunningState && !perks.KatanaSlashD.isRunningState)
                {
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) perks.katDir = 0;
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) perks.katDir = 1;
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) perks.katDir = 2;
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Down)) perks.katDir = 3;
                    if (perks.katDir != -1)
                    {
                        playSound.Add(new Thread(() => PlaySound(OST.Slash, true, rng.Next(-5, 30))));
                        playSound[playSound.Count - 1].Start();
                    }
                }
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) directionOfShot = new Vector2f(-bulletVelocity, 0);
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) directionOfShot = new Vector2f(bulletVelocity, 0);
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) directionOfShot = new Vector2f(0, -bulletVelocity);
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Down)) directionOfShot = new Vector2f(0, bulletVelocity);
                else
                    isShooting = false;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.E) && tick > lastBombPlaced + bombFireRate && bombCount > 0)
            {
                lastBombPlaced = tick;
                isPlacingBomb = true;
            }
            else isPlacingBomb = false;
        }
        private void PlayerSide()
        {
            if (!perks.hasKatana || (perks.hasKatana && !perks.KatanaSlashL.isRunningState && !perks.KatanaSlashR.isRunningState && !perks.KatanaSlashU.isRunningState && !perks.KatanaSlashD.isRunningState))
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.A)) sprite.Texture = texL;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.D)) sprite.Texture = texR;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.W)) sprite.Texture = texU;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.S)) sprite.Texture = texD;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) sprite.Texture = texL;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) sprite.Texture = texR;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) sprite.Texture = texU;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Down)) sprite.Texture = texD;
            }

        }
        private void PlayerInvinsible()
        {
            if (tick > invLast + 10)
                sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, 255 / 2);
            if (tick > invLast + 20)
            {
                invLast = tick;
                sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, 255);
            }
            if (tick > invStart + 20 * 5)
            {
                sprite.Color = new Color(sprite.Color.R, sprite.Color.G, sprite.Color.B, 255);
                isInvincible = false;
            }
        }

        private void EnemyMovement()
        {
            canGoUp = true;
            canGoDown = true;
            canGoLeft = true;
            canGoRight = true;
            dir = new Vector2f(0, 0);

            viewRange = new FloatRect(sprite.Position.X - 150, sprite.Position.Y - 150, sprite.GetGlobalBounds().Width + 300, sprite.GetGlobalBounds().Height + 300);
            foreach (var CW in CollidesWith)
                foreach (var collidable in CW)
                {
                    if (new FloatRect(collider.Left + collider.Width / 4, collider.Top - 5, collider.Width / 2, 5).Intersects(collidable.collider)) canGoUp = false;
                    if (new FloatRect(collider.Left + collider.Width / 4, collider.Top + collider.Height, collider.Width / 2, 5).Intersects(collidable.collider)) canGoDown = false;
                    if (new FloatRect(collider.Left - 5, collider.Top + collider.Height / 4, 5, collider.Height / 2).Intersects(collidable.collider)) canGoLeft = false;
                    if (new FloatRect(collider.Left + collider.Width, collider.Top + collider.Height / 4, 5, collider.Height / 2).Intersects(collidable.collider)) canGoRight = false;
                }
            if (sprite.Position.Y < boundsOffset) canGoUp = false;
            if (sprite.Position.Y + sprite.GetGlobalBounds().Height > cResolution.Y - boundsOffset) canGoDown = false;
            if (sprite.Position.X < boundsOffset) canGoLeft = false;
            if (sprite.Position.X + sprite.GetGlobalBounds().Width > cResolution.X - boundsOffset) canGoRight = false;

            if (!isConstDir)
            {
                if (Followed.collider.Intersects(viewRange))
                {
                    if (sprite.Position.X > Followed.sprite.Position.X && canGoLeft) dir.X = -speed;
                    if (sprite.Position.X < Followed.sprite.Position.X && canGoRight) dir.X = speed;
                    if (sprite.Position.Y > Followed.sprite.Position.Y && canGoUp) dir.Y = -speed;
                    if (sprite.Position.Y < Followed.sprite.Position.Y && canGoDown) dir.Y = speed;
                    if (Math.Abs(sprite.Position.Y - Followed.sprite.Position.Y) < speed) dir.Y = 0;
                    if (Math.Abs(sprite.Position.X - Followed.sprite.Position.X) < speed) dir.X = 0;
                }
                else
                {
                    isConstDir = true;
                    int doX = rng.Next(5) == 0 ? 0 : 1;
                    int doY = rng.Next(5) == 0 ? 0 : 1;
                    constDir = new Vector2f(rng.Next(2) == 0 ? -speed / rng.Next(1, 3) * doX * (canGoLeft ? 1 : 0) : speed / rng.Next(1, 3) * doX * (canGoRight ? 1 : 0), rng.Next(2) == 0 ? -speed / rng.Next(1, 3) * doY * (canGoUp ? 1 : 0) : speed / rng.Next(1, 3) * doY * (canGoDown ? 1 : 0));
                }
            }
            else
            {
                if (tick - setConstDir < constDirTime)
                {
                    if (Followed.collider.Intersects(viewRange)) isConstDir = false;
                    if (constDir.X < 0 && canGoLeft) dir.X = constDir.X;
                    if (constDir.X > 0 && canGoRight) dir.X = constDir.X;
                    if (constDir.Y < 0 && canGoUp) dir.Y = constDir.Y;
                    if (constDir.Y > 0 && canGoDown) dir.Y = constDir.Y;
                }
                else
                {
                    setConstDir = tick;
                    isConstDir = false;
                }
            }

            EnemySide(dir);
            sprite.Position = new Vector2f(sprite.Position.X + dir.X, sprite.Position.Y + dir.Y);
            collider = new FloatRect(sprite.Position.X + 10, sprite.Position.Y + 10, sprite.Texture.Size.X * sprite.Scale.X - 20, sprite.Texture.Size.Y * sprite.Scale.Y - 20);
        }
        private void EnemySide(Vector2f Dir)
        {
            if (Dir.X > 0 && Math.Abs(sprite.Position.X - Followed.sprite.Position.X) > Math.Abs(sprite.Position.Y - Followed.sprite.Position.Y))
                sprite.Texture = texR;

            else if (Dir.X < 0 && Math.Abs(sprite.Position.X - Followed.sprite.Position.X) > Math.Abs(sprite.Position.Y - Followed.sprite.Position.Y))
                sprite.Texture = texL;

            else if (Dir.Y > 0) sprite.Texture = texD;
            else if (Dir.Y < 0) sprite.Texture = texU;
        }

        public class Perks
        {
            public int charges = 2;
            public bool isUsingperk = false;
            public bool hasPerk = false;

            public bool hasJetpack;
            public AnimatedSprite JetPackL = new AnimatedSprite(Resources._jetPack, true, 6, 1, _flipX: true, _hasStoppedState: true);
            public AnimatedSprite JetPackR = new AnimatedSprite(Resources._jetPack, true, 6, 1, _hasStoppedState: true);

            public bool hasKatana;
            public float cKatDamage = 2;
            public float katDamage = 2;
            public int katDir = -1;
            public float katSpeed = 5.75f;

            float offsetL = 0, offsetR = 0, offsetU = 0, offsetD = 0;

            public Sprite KatanaL = new Sprite(Resources._katana) { Origin = (Vector2f)Resources._katana.Size / 2, Scale = new Vector2f(-7, 7) };
            public Sprite KatanaR = new Sprite(Resources._katana) { Origin = (Vector2f)Resources._katana.Size / 2, Scale = new Vector2f(7, 7) };
            public Sprite KatanaU = new Sprite(Resources._katana) { Origin = (Vector2f)Resources._katana.Size / 2, Scale = new Vector2f(-7, -7), Rotation = 90 };
            public Sprite KatanaD = new Sprite(Resources._katana) { Origin = (Vector2f)Resources._katana.Size / 2, Scale = new Vector2f(-7, -7), Rotation = -90 };
            public AnimatedSprite KatanaSlashR = new AnimatedSprite(Resources._katanaSlash, true, 7, 1, _hasStoppedState: true);
            public AnimatedSprite KatanaSlashL = new AnimatedSprite(Resources._katanaSlash, true, 7, 1, _flipX:true, _hasStoppedState: true);
            public AnimatedSprite KatanaSlashU = new AnimatedSprite(Resources._katanaSlash, true, 7, 1, _rotation: -90, _hasStoppedState: true);
            public AnimatedSprite KatanaSlashD = new AnimatedSprite(Resources._katanaSlash, true, 7, 1, _flipY: true, _rotation: 90, _hasStoppedState: true);

            public FloatRect KatanaColliderL, KatanaColliderR, KatanaColliderU, KatanaColliderD;

            public void Update(Sprite _sprite)
            {
                if (hasJetpack || hasKatana) hasPerk = true;

                // ============ Jetpack Position Update ============                            Not very optimized, but no one will see it (:
                JetPackR.Update(new Vector2f((_sprite.Position.X + JetPackR.currentSprite.GetGlobalBounds().Width) - 23 - 5, _sprite.Position.Y + 20), isUsingperk);
                JetPackL.Update(new Vector2f((_sprite.Position.X - JetPackL.currentSprite.GetGlobalBounds().Width) + 23 + 29, _sprite.Position.Y + 20), isUsingperk);

                // ============ Katana Position/Collision Update ============ 

                if (KatanaL.Rotation <= -90) { katDir = -1; KatanaL.Rotation = 0; offsetL = 0; }
                if (KatanaR.Rotation >= 90)  { katDir = -1; KatanaR.Rotation = 0; offsetR = 0; }
                if (KatanaU.Rotation >= 180) { katDir = -1; KatanaU.Rotation = 90; offsetU = 0; }
                if (KatanaD.Rotation >= 0)   { katDir = -1; KatanaD.Rotation = -90; offsetD = 0; }

                KatanaL.Position = _sprite.Position + new Vector2f(_sprite.GetGlobalBounds().Width - 90, 40 + offsetL);
                KatanaR.Position = _sprite.Position + new Vector2f(_sprite.GetGlobalBounds().Width + 5, 40 + offsetR);
                KatanaU.Position = _sprite.Position + new Vector2f(0 + offsetU + 20, 10);
                KatanaD.Position = _sprite.Position + new Vector2f(_sprite.GetGlobalBounds().Width / 2 + 10 - offsetD, 95);

                if (katDir == 0) { KatanaL.Rotation -= katSpeed; offsetL += 2; KatanaSlashL.Update(_sprite.Position + new Vector2f(-75, -25), true); }
                else { KatanaSlashL.Update(_sprite.Position, false); KatanaL.Rotation = 0; offsetL = 0;}

                if (katDir == 1) { KatanaR.Rotation += katSpeed; offsetR += 2; KatanaSlashR.Update(_sprite.Position + new Vector2f(35, -30), true); }
                else { KatanaSlashR.Update(_sprite.Position, false); KatanaR.Rotation = 0; offsetR = 0; }

                if (katDir == 2) { KatanaU.Rotation += katSpeed; offsetU += 2; KatanaSlashU.Update(_sprite.Position + new Vector2f(-45, 65), true); }
                else { KatanaSlashU.Update(_sprite.Position, false); KatanaU.Rotation = 90; offsetU = 0; }

                if (katDir == 3) { KatanaD.Rotation += katSpeed; offsetD += 2; KatanaSlashD.Update(_sprite.Position + new Vector2f(120, 40), true); }
                else { KatanaSlashD.Update(_sprite.Position, false); KatanaD.Rotation = -90; offsetD = 0; }

                if (katDir == -1) katDamage = cKatDamage;

                KatanaColliderL = new FloatRect(_sprite.Position - new Vector2f(45, 25), new Vector2f(35, 125));
                KatanaColliderR = new FloatRect(_sprite.Position + new Vector2f(_sprite.GetGlobalBounds().Width + 5, -25), new Vector2f(35, 125));
                KatanaColliderU = new FloatRect(_sprite.Position - new Vector2f(20, _sprite.GetGlobalBounds().Height - 45), new Vector2f(125, 35));
                KatanaColliderD = new FloatRect(_sprite.Position + new Vector2f(-20, _sprite.GetGlobalBounds().Height + 5), new Vector2f(125, 35));
            }
        }

    }
}
