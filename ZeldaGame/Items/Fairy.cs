// Fairy Item Class
// Author : Michael Frank

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using ZeldaGame.Enums;
using ZeldaGame.GameObject.GameObjectData;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;
using ZeldaGame.Player;
using ZeldaGame.Sprites;

namespace ZeldaGame.Items
{
    class Fairy : IItem
    {
        private Point position;
        private readonly ISprite sprite;
        private ISprite font;
        private int updateCounter, currentTime;
        private readonly int timeToChangeDireciton, cost, width, height, animFramerate;
        private readonly float velocity, layer;
        private Direction direction;

        public Fairy(Point position, ItemDataManager data, int cost = 0)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            string name = NameLookupTable.GetName(this);

            this.position = position;
            this.cost = cost;
            this.layer = data.GetLayer(name);
            this.sprite = SpriteFactory.Instance.CreateSprite(name);
            data.GetItemHitbox(name, out width, out height);
            animFramerate = data.GetAnimationFramerate(name);
            timeToChangeDireciton = data.GetDirectionChange(name);
            velocity = data.GetVelocity(name);
            CreateFont();
        }

        private void CreateFont()
        {
            if (cost != 0)
            {
                string costString = cost.ToString(CultureInfo.InvariantCulture);
                this.font = SpriteFactory.Instance.CreateFontSprite(costString, costString.Length);
                for (int i = 0; i < costString.Length; i++)
                {
                    this.font.UpdateFrame();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            sprite.Draw(spriteBatch, position, color, layer);
            if (font != null)
            {
                Point fontPosition = new Point(position.X - ((int)(cost.ToString(CultureInfo.InvariantCulture).Length / 2.0) * SpriteFactory.FontWidth), position.Y - (height / 2) - SpriteFactory.FontHeight);
                this.font.Draw(spriteBatch, fontPosition, Color.White, layer);
            }
        }

        public void Update(GameTime gametime)
        {
            //Update sprite animation
            updateCounter++;
            updateCounter %= animFramerate;
            if (updateCounter == 0)
            {
                sprite.UpdateFrame();
            }

            //Change direction
            currentTime++;
            if (currentTime > timeToChangeDireciton)
            {
                ChangeDirection();
            }

            //Movement
            if (updateCounter % 2 == 0)
            {
                int movementVal = (int)(velocity * gametime.ElapsedGameTime.TotalMilliseconds);
                MoveFairy(movementVal);
            }
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(position.X - (width / 2), position.Y - (height / 2), width, height);
        }
        public void MoveFairy(int distance)
        {
            switch (direction)
            {
                case Direction.East:
                    UpdateLocation(distance, 0);
                    break;
                case Direction.West:
                    UpdateLocation(-1 * distance, 0);
                    break;
                case Direction.South:
                    UpdateLocation(0, distance);
                    break;
                case Direction.North:
                    UpdateLocation(0, -1 * distance);
                    break;
                case Direction.SouthEast:
                    UpdateLocation(distance, distance);
                    break;
                case Direction.NorthEast:
                    UpdateLocation(distance, -1 * distance);
                    break;
                case Direction.SouthWest:
                    UpdateLocation(-1 * distance, distance);
                    break;
                case Direction.NorthWest:
                    UpdateLocation(-1 * distance, -1 * distance);
                    break;
            }
        }
        public void ChangeDirection()
        {
            Random random = new Random();
            int rndNumber = random.Next(1, 9);
            switch (rndNumber)
            {
                case 1:
                    direction = Direction.East;
                    break;
                case 2:
                    direction = Direction.West;
                    break;
                case 3:
                    direction = Direction.North;
                    break;
                case 4:
                    direction = Direction.South;
                    break;
                case 5:
                    direction = Direction.SouthEast;
                    break;
                case 6:
                    direction = Direction.NorthEast;
                    break;
                case 7:
                    direction = Direction.SouthWest;
                    break;
                case 8:
                    direction = Direction.NorthWest;
                    break;
            }
            currentTime = 0;
        }
        public void UpdateLocation(int x, int y)
        {
            this.position.X += x;
            this.position.Y += y;
        }

        public int GetCost()
        {
            return this.cost;
        }

        public void PlayerPickUp(IAdventurePlayer player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            AdventurePlayerInventory inventory = player.GetInventory();

            if (this.cost <= inventory.GetBalance())
            {
                inventory.RemoveBalance(cost);
                IncreasePlayerHealth(inventory, 6);
                SoundFactory.Instance.GetSound(Sound.GetItemOrFairy).Play();
                ItemManager.RemoveItem(this);
            }
        }

        private static void IncreasePlayerHealth(PlayerInventory inventory, int amount)
        {
            int currentHealth = inventory.GetHealth() + amount;
            int maxHealth = inventory.GetMaxHealth();

            if (currentHealth > maxHealth) currentHealth = maxHealth; //Prevents player from healing over max health

            inventory.SetHealth(currentHealth);
        }
    }
}
