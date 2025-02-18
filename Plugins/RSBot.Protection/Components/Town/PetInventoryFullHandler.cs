﻿using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Event;

namespace RSBot.Protection.Components.Town
{
    public class PetInventoryFullHandler
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            SubscribeEvents();
        }

        /// <summary>
        /// Subscribes the events.
        /// </summary>
        private static void SubscribeEvents()
        {
            EventManager.SubscribeEvent("OnInventoryUpdate", OnUpdateInventory);
        }

        /// <summary>
        /// </summary>
        private static void OnUpdateInventory()
        {
            if (!Kernel.Bot.Running) 
                return;

            if (!PlayerConfig.Get<bool>("RSBot.Protection.checkFullPetInventory")) 
                return;

            if (Game.Player.AbilityPet == null) 
                return;

            if (!Game.Player.AbilityPet.Full) 
                return;

            if (ScriptManager.Running) 
                return;

            Log.NotifyLang("ReturnToTownPetInventoryFull");
            Game.Player.UseReturnScroll();
        }
    }
}