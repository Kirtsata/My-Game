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
using System.Diagnostics;

namespace Another_SFML_Project
{
    class Room : Program
    {
        public List<Entity> enemies = new List<Entity>();
        public List<Entity> rocks = new List<Entity>();
        public List<Entity> iron = new List<Entity>();
        public List<Sprite> static_entities = new List<Sprite>();
        public List<List<Entity>> Collidables = new List<List<Entity>>();
        public List<Projectile> projectiles = new List<Projectile>();
        public List<Bomb> bombs = new List<Bomb>();
        public List<Particles> particles = new List<Particles>();
        public List<Door> doors = new List<Door>();
        public List<Item> items = new List<Item>();
        public bool hasSpawnedItem = false;

        public Entity player = new Entity((int)cResolution.X / 2, (int)cResolution.Y / 2, Resources._playerDown, Resources._playerUp, Resources._playerLeft, Resources._playerRight, IsPlayer: true);
        public UI_Heart ui_Heart;
        public UI_Minimap ui_Minimap;
        public UI_Stat[] uiStats = new UI_Stat[]
        {
             new UI_Stat(Resources._uiBomb, 0),
             new UI_Stat(Resources._uiDamage, 1),
             new UI_Stat(Resources._uiFireRate, 2),
             new UI_Stat(Resources._uiSpeed, 3)
        };
        public UI_Perk uiPerk = new UI_Perk();

        public Vector2f playerSpawnPoint;

        public bool isActive;
        public bool hasBeenActive;

        public Room(int number)
        {
            player.CollidesWith = Collidables;
            ui_Heart = new UI_Heart(Resources._hearts, player.Health);
            ui_Minimap = new UI_Minimap(number);

            List<string> mapInfo = new List<string>();
            mapInfo = File.ReadAllLines(@"Target\map" + rng.Next(mapCount) + ".txt").ToList();
            for (int i = 0; i < mapInfo.Count; i++)
            {
                string[] entries = mapInfo[i].Split(' ');

                for (int j = 0; j < grid.grid.Count; j++)
                {
                    if (j == Int32.Parse(entries[0]))
                    {
                        int offset = 5;
                        switch (entries[1])
                        {
                            case "rock":
                                rocks.Add(new Entity((int)grid.grid[j].Left - offset, (int)grid.grid[j].Top - offset, Resources._rock[rng.Next(Resources._rock.Count)], IsStatic: true, _hasCross: rng.Next(69) == 0 ? true : false));
                                byte darken = (byte)(255 - rng.Next(100));
                                rocks[rocks.Count - 1].sprite.Color = new Color(darken, darken, darken);
                                if (rng.Next(2) == 0) rocks[rocks.Count - 1].sprite.TextureRect = new IntRect((int)rocks[rocks.Count - 1].sprite.Texture.Size.X, 0, (int)-rocks[rocks.Count - 1].sprite.Texture.Size.X, (int)rocks[rocks.Count - 1].sprite.Texture.Size.Y);
                                break;
                            case "iron":
                                iron.Add(new Entity((int)grid.grid[j].Left - offset, (int)grid.grid[j].Top - offset, Resources._iron[0], IsStatic: true, IsDestroyable: false, IsAnimated: true));
                                //iron[iron.Count - 1].SetupAnimation(Resources._iron, true, 7, 1, 100, rng.Next(Resources._iron.Count));
                                iron[iron.Count - 1].SetupAnimation(Resources._iron, true, 7, 1, rng.Next(60,100), rng.Next(Resources._iron.Count));
                                break;
                            case "player":
                                player.sprite.Position = new Vector2f((int)grid.grid[j].Left - offset, (int)grid.grid[j].Top - offset);
                                playerSpawnPoint = player.sprite.Position;
                                break;
                            case "enemy":
                                int rngEnemy = rng.Next(Resources._enemyDown.Count);
                                enemies.Add(new Entity(
                                    (int)grid.grid[j].Left - offset, (int)grid.grid[j].Top - offset,
                                    Resources._enemyDown[rngEnemy], Resources._enemyUp[rngEnemy], Resources._enemyLeft[rngEnemy], Resources._enemyRight[rngEnemy],
                                    Speed: 2.2f)
                                { Followed = player, Group = enemies, CollidesWith = Collidables });
                                break;

                        }
                    }
                }
            }

            List<string> layout = new List<string>();
            if (File.Exists(@"Target\layout.txt"))
            {
                layout = File.ReadAllLines(@"Target\layout.txt").ToList();

                string[] entries = layout[number].Split(' ');
                int breakPoint = -1;

                isActive = bool.Parse(entries[2]);

                List<int> doorPos = new List<int>();
                List<int> doorLeadsTo = new List<int>();

                for (int i = 3; i <= 7; i++)
                {
                    if (entries[i] == "|")
                        breakPoint = i;

                    if (breakPoint >= 0)
                        break;

                    doorPos.Add(int.Parse(entries[i]));
                }

                for (int i = breakPoint; i < breakPoint + doorPos.Count; i++)
                    doorLeadsTo.Add(int.Parse(entries[i + 1]));


                for (int i = 0; i < doorPos.Count; i++)
                    doors.Add(new Door(doorPos[i], doorLeadsTo[i], true));
            }

            Collidables.Add(rocks);
            Collidables.Add(iron);
        }

        bool hasPlayedDoorSound = false;
        public void Update()
        {
            if (isActive) hasBeenActive = true;

            bool areEnemiesAlive = false;
            foreach (var e in enemies)
                if (!e.isDead) areEnemiesAlive = true;
            if (!areEnemiesAlive && !hasPlayedDoorSound)
            {
                OST.DoorOpen.Pitch = (100 * rng.Next(1, 3)) / 100f;
                OST.DoorOpen.Play();
                hasPlayedDoorSound = true;
                Debug.WriteLine("fard");
            }

            if (!areEnemiesAlive && !hasSpawnedItem)
            {
                hasSpawnedItem = true;
                SpawnItem(rng.Next(3), 0);
                if (player.perks.charges < 2) player.perks.charges++;
            }
            foreach (var d in doors)
                d.isOpen = areEnemiesAlive ? (d.forceOpen ? true : false) : true;
        }

        public void SpawnItem(int count, int rarity, Vector2i position = new Vector2i())
        {
            int tLength = 0;
            switch (rarity)
            {
                case 0: tLength = Resources._itemCommon.Length; break;
                case 1: tLength = Resources._itemsRare.Length; break;
            }

            for (int i = 0; i < count; i++)
            {
                int rngItem = rng.Next(tLength);
                while (rarity == 1 && rngItem == Item.ItemsRare.Fly0 && player.perks.hasJetpack)
                    rngItem = rng.Next(tLength);

                Vector2i placement = new Vector2i(rng.Next((int)boundsOffset, (int)cResolution.X - (int)boundsOffset - 15), rng.Next((int)boundsOffset, (int)cResolution.Y - (int)boundsOffset - 25));
                bool stayLooped = true;
                if (position != new Vector2i())
                {
                    placement = new Vector2i(position.X + 30, position.Y + 30);
                    stayLooped = false;
                }
                while (stayLooped)
                {
                    stayLooped = false;
                    foreach (var CW in Collidables)
                        foreach (var collidable in CW)
                            if (new IntRect(placement, (Vector2i)Resources._itemCommon[rngItem].Size * 3).Intersects((IntRect)collidable.sprite.GetGlobalBounds()))
                                stayLooped = true;
                    if (new IntRect(placement, (Vector2i)Resources._itemCommon[rngItem].Size * 3).Intersects((IntRect)player.sprite.GetGlobalBounds()))
                        stayLooped = true;
                    if (stayLooped)
                        placement = new Vector2i(rng.Next((int)boundsOffset, (int)cResolution.X - (int)boundsOffset - 15), rng.Next((int)boundsOffset, (int)cResolution.Y - (int)boundsOffset - 25));
                }
                items.Add(new Item(placement.X, placement.Y, rngItem, rarity));
            }

        }
    }
}
