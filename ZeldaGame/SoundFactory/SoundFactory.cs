// SpriteSoundFactory Class
//
// @author Benjamin J Nagel

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using ZeldaGame.Enums;

namespace ZeldaGame.Sprites
{
    public class SoundFactory
    {
        private SoundEffect DeathSound, DeflectSound, LinkSwordSlashSound,
            MagicSwordShootSound, LinkCombinedSwordSound, ShootingArrowAndBoomerangSound,
            BombDropSound, BombExplosionSound, CandleSound, MagicRodSound,
            RecorderSound, EnemyHitSound, LinkHurtAndEraseSaveDataSound,
            LinkDeathSound, LinkLowHealthSound, NewItemFanFareSound,
            GetItemOrFairySound, GetHeartOrKeySound, GetRupeeSound,
            RefillHealthOrRupeeCountChangeSound, TextSound, KeyAppearSound,
            DoorUnlockSound, BossScream1Sound, BossScream2Sound, BossScream3Sound,
            StairsSound, ShoreSound, SecretSound, DungeonClearedSound,
            ZeldaRescuedSound, SelectedSound;
        private Song SelectedSong, DungeonSong, IntroSong;

        public static SoundFactory Instance { get; } = new SoundFactory();
        /// <summary>
        /// Private consuctor so that only one instance is created.
        /// </summary>
        private SoundFactory()
        {
            //This is intentionally empty
        }
        public void LoadAllSounds(ContentManager content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            DeathSound = content.Load<SoundEffect>("Sounds/kill");
            DeflectSound = content.Load<SoundEffect>("Sounds/Deflect");
            LinkSwordSlashSound = content.Load<SoundEffect>("Sounds/SwordSlash");
            MagicSwordShootSound = content.Load<SoundEffect>("Sounds/MagicSwordShoot");
            LinkCombinedSwordSound = content.Load<SoundEffect>("Sounds/LinkCombinedSword");
            ShootingArrowAndBoomerangSound = content.Load<SoundEffect>("Sounds/ShootingArrowAndBoomerang");
            BombDropSound = content.Load<SoundEffect>("Sounds/BombDrop");
            BombExplosionSound = content.Load<SoundEffect>("Sounds/BombExplosion");
            CandleSound = content.Load<SoundEffect>("Sounds/Candle");
            MagicRodSound = content.Load<SoundEffect>("Sounds/MagicRod");
            RecorderSound = content.Load<SoundEffect>("Sounds/Recorder");
            EnemyHitSound = content.Load<SoundEffect>("Sounds/EnemyHit");
            LinkHurtAndEraseSaveDataSound = content.Load<SoundEffect>("Sounds/LinkHurtAndEraseSaveData");
            LinkDeathSound = content.Load<SoundEffect>("Sounds/LinkDeath");
            LinkLowHealthSound = content.Load<SoundEffect>("Sounds/LinkLowHealth");
            NewItemFanFareSound = content.Load<SoundEffect>("Sounds/NewItem");
            GetItemOrFairySound = content.Load<SoundEffect>("Sounds/GetItemOrFairy");
            GetHeartOrKeySound = content.Load<SoundEffect>("Sounds/GetHeartOrKey");
            GetRupeeSound = content.Load<SoundEffect>("Sounds/GetRupee");
            RefillHealthOrRupeeCountChangeSound = content.Load<SoundEffect>("Sounds/RefillHealthOrRupeeCountChange");
            TextSound = content.Load<SoundEffect>("Sounds/Text");
            KeyAppearSound = content.Load<SoundEffect>("Sounds/KeyAppear");
            DoorUnlockSound = content.Load<SoundEffect>("Sounds/DoorUnlock");
            BossScream1Sound = content.Load<SoundEffect>("Sounds/BossScream1");
            BossScream2Sound = content.Load<SoundEffect>("Sounds/BossScream2");
            BossScream3Sound = content.Load<SoundEffect>("Sounds/BossScream3");
            StairsSound = content.Load<SoundEffect>("Sounds/Stairs");
            ShoreSound = content.Load<SoundEffect>("Sounds/Shore");
            SecretSound = content.Load<SoundEffect>("Sounds/Secret");
            DungeonSong = content.Load<Song>("Sounds/Dungeon");
            DungeonClearedSound = content.Load<SoundEffect>("Sounds/DungeonCleared");
            IntroSong = content.Load<Song>("Sounds/IntroMusic");
            ZeldaRescuedSound = content.Load<SoundEffect>("Sounds/ZeldaRescued");
        }
        public SoundEffect GetSound(Enums.Sound sound)
        {
            switch (sound)
            {
                case Sound.Death:
                    SelectedSound = DeathSound;
                    break;
                case Sound.Deflect:
                    SelectedSound = DeflectSound;
                    break;
                case Sound.LinkSwordSlash:
                    SelectedSound = LinkSwordSlashSound;
                    break;
                case Sound.MagicSwordShoot:
                    SelectedSound = MagicSwordShootSound;
                    break;
                case Sound.LinkCombinedSword:
                    SelectedSound = LinkCombinedSwordSound;
                    break;
                case Sound.ShootingArrowAndBoomerang:
                    SelectedSound = ShootingArrowAndBoomerangSound;
                    break;
                case Sound.BombDrop:
                    SelectedSound = BombDropSound;
                    break;
                case Sound.BombExplosion:
                    SelectedSound = BombExplosionSound;
                    break;
                case Sound.Candle:
                    SelectedSound = CandleSound;
                    break;
                case Sound.MagicRod:
                    SelectedSound = MagicRodSound;
                    break;
                case Sound.Recorder:
                    SelectedSound = RecorderSound;
                    break;
                case Sound.EnemyHit:
                    SelectedSound = EnemyHitSound;
                    break;
                case Sound.LinkHurtAndEraseSaveData:
                    SelectedSound = LinkHurtAndEraseSaveDataSound;
                    break;
                case Sound.LinkDeath:
                    SelectedSound = LinkDeathSound;
                    break;
                case Sound.LinkLowHealth:
                    SelectedSound = LinkLowHealthSound;
                    break;
                case Sound.NewItemFanFare:
                    SelectedSound = NewItemFanFareSound;
                    break;
                case Sound.GetItemOrFairy:
                    SelectedSound = GetItemOrFairySound;
                    break;
                case Sound.GetHeartOrKey:
                    SelectedSound = GetHeartOrKeySound;
                    break;
                case Sound.GetRupee:
                    SelectedSound = GetRupeeSound;
                    break;
                case Sound.RefillHealthOrRupeeCountChange:
                    SelectedSound = RefillHealthOrRupeeCountChangeSound;
                    break;
                case Sound.Text:
                    SelectedSound = TextSound;
                    break;
                case Sound.KeyAppear:
                    SelectedSound = KeyAppearSound;
                    break;
                case Sound.DoorUnlock:
                    SelectedSound = DoorUnlockSound;
                    break;
                case Sound.BossScream1:
                    SelectedSound = BossScream1Sound;
                    break;
                case Sound.BossScream2:
                    SelectedSound = BossScream2Sound;
                    break;
                case Sound.BossScream3:
                    SelectedSound = BossScream3Sound;
                    break;
                case Sound.Stairs:
                    SelectedSound = StairsSound;
                    break;
                case Sound.Shore:
                    SelectedSound = ShoreSound;
                    break;
                case Sound.Secret:
                    SelectedSound = SecretSound;
                    break;
                case Sound.DungeonCleared:
                    SelectedSound = DungeonClearedSound;
                    break;

                case Sound.ZeldaRescued:
                    SelectedSound = ZeldaRescuedSound;
                    break;
            }
            return SelectedSound;
        }
        public Song GetSong(Enums.Sound sound)
        {
            switch (sound)
            {
                case Sound.Dungeon:
                    SelectedSong = DungeonSong;
                    break;
                case Sound.Intro:
                    SelectedSong = IntroSong;
                    break;
            }
            return SelectedSong;
        }
    }
}