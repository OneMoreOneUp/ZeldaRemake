using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZeldaGame.GameObjectHandler;
using ZeldaGame.Interfaces;

namespace ZeldaGame.Triggers
{
    public class TriggerBoxManager
    {
        private List<ITriggerBox> triggerBoxes;

        public TriggerBoxManager()
        {
            triggerBoxes = new List<ITriggerBox>();
        }

        public void AddTriggerBox(ITriggerBox triggerBox)
        {
            if (!triggerBoxes.Contains(triggerBox))
            {
                triggerBoxes.Add(triggerBox);
            }
            else
            {
                Trace.WriteLine("[WARNING] TriggerBox could not be added to game objects. It already exists.");
            }
        }

        public void AddTriggerBox(List<ITriggerBox> triggerBoxes)
        {
            if (triggerBoxes == null) throw new ArgumentNullException(nameof(triggerBoxes));

            foreach (ITriggerBox triggerBox in triggerBoxes)
            {
                AddTriggerBox(triggerBox);
            }
        }

        public void RemoveTriggerBox(ITriggerBox triggerBox)
        {
            if (triggerBoxes.Contains(triggerBox))
            {
                triggerBoxes.Remove(triggerBox);
            }
            else
            {
                Trace.WriteLine("[WARNING] TriggerBox could not be removed from game objects. It does not exist.");
            }
        }

        public void RemoveTriggerBox(List<ITriggerBox> triggerBoxes)
        {
            if (triggerBoxes == null) throw new ArgumentNullException(nameof(triggerBoxes));

            foreach (ITriggerBox triggerBox in triggerBoxes)
            {
                RemoveTriggerBox(triggerBox);
            }
        }

        public List<ITriggerBox> GetTriggerBoxes()
        {
            return triggerBoxes;
        }

        public void RemoveAllTriggerBoxes()
        {
            triggerBoxes = new List<ITriggerBox>();
        }

        public void DetectPlayerCollisions()
        {
            List<IAdventurePlayer> players = new List<IAdventurePlayer>(PlayerManager.GetPlayers());
            List<ITriggerBox> triggers = new List<ITriggerBox>(triggerBoxes);

            foreach (ITriggerBox trigger in triggers)
            {
                foreach (IAdventurePlayer player in players)
                {
                    if (DetectCollision(trigger, player)) trigger.Trigger();
                }
            }
        }

        private static bool DetectCollision(ITriggerBox trigger, IGameObject collided)
        {
            Rectangle triggerRect = trigger.GetRectangle();
            Rectangle collidedRect = collided.GetHitbox();
            Rectangle intersection = Rectangle.Intersect(triggerRect, collidedRect);

            return !intersection.IsEmpty;
        }
    }
}
