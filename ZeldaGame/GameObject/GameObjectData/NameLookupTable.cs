using System;
using System.Collections.Generic;
using ZeldaGame.Interfaces;

namespace ZeldaGame.GameObject.GameObjectData
{
    public static class NameLookupTable
    {
        private static readonly Dictionary<Type, string> classTable
            = new Dictionary<Type, string>()
            {
                //Enemies
                {typeof(Enemies.Aquamentus), "Aquamentus"},
                {typeof(Enemies.Bubble), "Bubble" },
                {typeof(Enemies.Dodongo), "Dodongo" },
                {typeof(Enemies.EnemyDeath), "EnemyDeath" },
                {typeof(Enemies.EnemySpawn), "EnemySpawn" },
                {typeof(Enemies.Gel), "Gel" },
                {typeof(Enemies.Keese), "Keese" },
                {typeof(Enemies.Merchant), "Merchant" },
                {typeof(Enemies.OldMan), "OldMan"},
                {typeof(Enemies.Goriya), "Goriya" },
                {typeof(Enemies.Rope), "Rope" },
                {typeof(Enemies.Stalfos), "Stalfos" },
                {typeof(Enemies.LeftStatueOne), "LeftStatue1" },
                {typeof(Enemies.RightStatueOne), "RightStatue1" },
                {typeof(Enemies.SpikeTrap), "SpikeTrap" },
                {typeof(Enemies.WallMaster), "WallMaster" },
                {typeof(Enemies.Zol), "Zol" },
                {typeof(Enemies.ManHandlaController), "ManHandlaController" },
                {typeof(Enemies.ManhandlaHeads),"ManHandlaHeads" },
                {typeof(Enemies.LikeLike),"LikeLike" },
                {typeof(Enemies.Gibdo),"Gibdo" },
                {typeof(Enemies.Vire),"Vire" },
                {typeof(Enemies.MoldormBodyParts),"MoldormBody" },
                {typeof(Enemies.MoldormController),"Moldorm" },
                {typeof(Enemies.Wizzrobe),"Wizzrobe" },
                {typeof(Enemies.Darknut),"Darknut" },
                {typeof(Enemies.Gohma),"Gohma" },

                //Enemy projectiles
                {typeof(EnemyProjectiles.Fireball), "Fireball" },
                {typeof(EnemyProjectiles.MagicalBoomerang), "MagicalBoomerang" },
                {typeof(EnemyProjectiles.WoodenBoomerang), "WoodenBoomerang" },
                {typeof(EnemyProjectiles.MagicalRodBeam),"MagicalRodBeam" },
                //Player projectiles
                {typeof(PlayerProjectiles.Arrow), "Arrow" },
                {typeof(PlayerProjectiles.Bomb), "Bomb" },
                {typeof(PlayerProjectiles.Flame), "Flame" },
                {typeof(PlayerProjectiles.Food), "Food" },
                {typeof(PlayerProjectiles.MagicalBoomerang), "MagicalBoomerang" },
                {typeof(PlayerProjectiles.MagicalRodBeam), "MagicalRodBeam" },
                {typeof(PlayerProjectiles.SwordBeam), "SwordBeam" },
                {typeof(PlayerProjectiles.WoodenBoomerang), "WoodenBoomerang" },

                //Particles
                {typeof(Projectiles.Particles.Arrow), "Arrow" },
                {typeof(Projectiles.Particles.Spark), "Spark" },
                {typeof(Projectiles.Particles.SwordBlast), "SwordBlast" },
                {typeof(Projectiles.Particles.WallMaster),"WallMaster" },
                {typeof(Projectiles.Particles.Raft),"Raft" },
                {typeof(Projectiles.Particles.Stepladder),"StepLadder" },

                //Items
                {typeof(Items.Arrow), "Arrow" },
                {typeof(Items.BlueCandle), "BlueCandle" },
                {typeof(Items.BlueRing), "BlueRing" },
                {typeof(Items.BlueRupee), "BlueRupee" },
                {typeof(Items.Bomb), "Bomb" },
                {typeof(Items.BossKey), "Key" },
                {typeof(Items.Bow), "Bow" },
                {typeof(Items.Clock), "Clock" },
                {typeof(Items.Compass), "Compass" },
                {typeof(Items.Fairy), "Fairy" },
                {typeof(Items.Food), "Food" },
                {typeof(Items.Heart), "Heart" },
                {typeof(Items.HeartContainer), "HeartContainer" },
                {typeof(Items.Key), "Key" },
                {typeof(Items.Letter), "Letter" },
                {typeof(Items.LifePotion), "LifePotion" },
                {typeof(Items.MagicalShield), "MagicalShield" },
                {typeof(Items.MagicalBoomerang), "MagicalBoomerang" },
                {typeof(Items.Map), "Map" },
                {typeof(Items.Raft), "Raft" },
                {typeof(Items.RedCandle), "RedCandle" },
                {typeof(Items.RedRing), "RedRing" },
                {typeof(Items.RedRupee), "RedRupee" },
                {typeof(Items.SecondLifePotion), "SecondLifePotion" },
                {typeof(Items.TriForcePiece), "TriForcePiece" },
                {typeof(Items.WoodenBoomerang), "WoodenBoomerang" },
                {typeof(Items.StepLadder),"StepLadder" },
                {typeof(Items.Holdable), "Holdable" },

                //Blocks
                {typeof(Blocks.Brick), "Brick" },
                {typeof(Blocks.Dirt), "Dirt" },
                {typeof(Blocks.Divider), "Divider" },
                {typeof(Blocks.Door), "Door" },
                {typeof(Blocks.Empty), "Empty" },
                {typeof(Blocks.Flame), "Flame" },
                {typeof(Blocks.Floor), "Floor" },
                {typeof(Blocks.Ladder), "Ladder" },
                {typeof(Blocks.LeftStatue), "LeftStatue" },
                {typeof(Blocks.RightStatue), "RightStatue" },
                {typeof(Blocks.Stair), "Stair" },
                {typeof(Blocks.Wall), "Wall" },
                {typeof(Blocks.Water), "Water" },
                {typeof(Blocks.Text), "Text" },
                {typeof(Blocks.HalfDivider),"HalfDivider" },
                

                //Players
                {typeof(Player.GreenLink), "Link" },
                {typeof(Player.WhiteLink), "Link" },
                {typeof(Player.RedLink), "Link" },
                {typeof(Player.LinkDead), "LinkWoodShield" }
            };

        private static readonly Dictionary<Type, string> stateTable =
            new Dictionary<Type, string>()
            {
                //Move wooden shield
                {typeof(Player.States.LinkNorthWoodShield),  "LinkWoodShield"},
                {typeof(Player.States.LinkSouthWoodShield),  "LinkWoodShield"},
                {typeof(Player.States.LinkWestWoodShield),  "LinkWoodShield"},
                {typeof(Player.States.LinkEastWoodShield),  "LinkWoodShield"},

                //Move magical shield
                {typeof(Player.States.LinkNorthMagicalShield),  "LinkMagicalShield"},
                {typeof(Player.States.LinkSouthMagicalShield),  "LinkMagicalShield"},
                {typeof(Player.States.LinkWestMagicalShield),  "LinkMagicalShield"},
                {typeof(Player.States.LinkEastMagicalShield),  "LinkMagicalShield"},

                //Attack wood sword wood shield
                {typeof(Player.States.LinkNorthWoodShieldWoodSword),  "LinkWoodShieldWoodSword"},
                {typeof(Player.States.LinkSouthWoodShieldWoodSword),  "LinkWoodShieldWoodSword"},
                {typeof(Player.States.LinkWestWoodShieldWoodSword),  "LinkWoodShieldWoodSword"},
                {typeof(Player.States.LinkEastWoodShieldWoodSword),  "LinkWoodShieldWoodSword"},

                //Attack wood sword magical shield
                {typeof(Player.States.LinkNorthMagicalShieldWoodSword),  "LinkMagicalShieldWoodSword"},
                {typeof(Player.States.LinkSouthMagicalShieldWoodSword),  "LinkMagicalShieldWoodSword"},
                {typeof(Player.States.LinkWestMagicalShieldWoodSword),  "LinkMagicalShieldWoodSword"},
                {typeof(Player.States.LinkEastMagicalShieldWoodSword),  "LinkMagicalShieldWoodSword"},

                //Attack white sword wood shield
                {typeof(Player.States.LinkNorthWoodShieldWhiteSword),  "LinkWoodShieldWhiteSword"},
                {typeof(Player.States.LinkSouthWoodShieldWhiteSword),  "LinkWoodShieldWhiteSword"},
                {typeof(Player.States.LinkWestWoodShieldWhiteSword),  "LinkWoodShieldWhiteSword"},
                {typeof(Player.States.LinkEastWoodShieldWhiteSword),  "LinkWoodShieldWhiteSword"},

                //Attack white sword magical shield
                {typeof(Player.States.LinkNorthMagicalShieldWhiteSword),  "LinkMagicalShieldWhiteSword"},
                {typeof(Player.States.LinkSouthMagicalShieldWhiteSword),  "LinkMagicalShieldWhiteSword"},
                {typeof(Player.States.LinkWestMagicalShieldWhiteSword),  "LinkMagicalShieldWhiteSword"},
                {typeof(Player.States.LinkEastMagicalShieldWhiteSword),  "LinkMagicalShieldWhiteSword"},

                //Attack magical sword wood shield
                {typeof(Player.States.LinkNorthWoodShieldMagicalSword),  "LinkWoodShieldMagicalSword"},
                {typeof(Player.States.LinkSouthWoodShieldMagicalSword),  "LinkWoodShieldMagicalSword"},
                {typeof(Player.States.LinkWestWoodShieldMagicalSword),  "LinkWoodShieldMagicalSword"},
                {typeof(Player.States.LinkEastWoodShieldMagicalSword),  "LinkWoodShieldMagicalSword"},

                //Attack magical sword magical shield
                {typeof(Player.States.LinkNorthMagicalShieldMagicalSword),  "LinkMagicalShieldMagicalSword"},
                {typeof(Player.States.LinkSouthMagicalShieldMagicalSword),  "LinkMagicalShieldMagicalSword"},
                {typeof(Player.States.LinkWestMagicalShieldMagicalSword),  "LinkMagicalShieldMagicalSword"},
                {typeof(Player.States.LinkEastMagicalShieldMagicalSword),  "LinkMagicalShieldMagicalSword"},

                //Attack magical rod wood shield
                {typeof(Player.States.LinkNorthWoodShieldMagicalRod),  "LinkWoodShieldMagicalRod"},
                {typeof(Player.States.LinkSouthWoodShieldMagicalRod),  "LinkWoodShieldMagicalRod"},
                {typeof(Player.States.LinkWestWoodShieldMagicalRod),  "LinkWoodShieldMagicalRod"},
                {typeof(Player.States.LinkEastWoodShieldMagicalRod),  "LinkWoodShieldMagicalRod"},

                //Attack magical rod magical shield
                {typeof(Player.States.LinkNorthMagicalShieldMagicalRod),  "LinkMagicalShieldMagicalRod"},
                {typeof(Player.States.LinkSouthMagicalShieldMagicalRod),  "LinkMagicalShieldMagicalRod"},
                {typeof(Player.States.LinkWestMagicalShieldMagicalRod),  "LinkMagicalShieldMagicalRod"},
                {typeof(Player.States.LinkEastMagicalShieldMagicalRod),  "LinkMagicalShieldMagicalRod"},

                //Using item
                {typeof(Player.States.LinkNorthUseItem),  "LinkUseItem"},
                {typeof(Player.States.LinkSouthUseItem),  "LinkUseItem"},
                {typeof(Player.States.LinkWestUseItem),  "LinkUseItem"},
                {typeof(Player.States.LinkEastUseItem),  "LinkUseItem"},

                //Pick up item
                {typeof(Player.States.LinkPickUpItem), "LinkPickUp" }
            };

        public static string GetName(IGameObject gameObject)
        {
            if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
            return classTable[gameObject.GetType()];
        }

        public static string GetName(ILinkState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            return stateTable[state.GetType()];
        }
    }
}
