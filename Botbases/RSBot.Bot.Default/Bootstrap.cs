﻿using RSBot.Bot.Default.Bot;
using RSBot.Bot.Default.Bundle;
using RSBot.Bot.Default.Views;
using RSBot.Core;
using RSBot.Core.Components;
using RSBot.Core.Objects;
using RSBot.Core.Plugins;
using System;
using System.Windows.Forms;

namespace RSBot.Bot.Default
{
    public class Bootstrap : IBotbase
    {
        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        public BotbaseInfo Info => new BotbaseInfo
        {
            Name = "RSBot.Default",
            DisplayName = "RSBot",
            TabText = "Training"
        };

        /// <summary>
        /// Ticks this instance. It's the botbase main-loop
        /// </summary>
        public void Tick()
        {
            if (!Kernel.Bot.Running)
                return;

            if (Game.Player.Exchanging)
                return;

            if (Game.Player.Untouchable)
                return;

            if (Game.Player.State.LifeState == LifeState.Dead)
                return;

            if (Bundles.Loop.Running)
                return;

            //Nothing if in scroll state!
            if (Game.Player.State.ScrollState == ScrollState.NormalScroll ||
                Game.Player.State.ScrollState == ScrollState.ThiefScroll) 
                return;

            try
            {
                Container.Bot.Tick();
            }
            catch (Exception ex)
            {
                Log.Debug($"An exception was thrown in the botloop: {ex} {Environment.NewLine}------------------------------------------------------");
            }
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <returns></returns>
        public Control GetView()
        {
            return Container.View ?? (Container.View = new Main());
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Initialize()
        {
            Container.Lock = new object();
            Subscriber.ConfigSubscriber.SubscribeEvents();
            Subscriber.TeleportSubscriber.SubscribeEvents();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Bundles.Reload();
            Container.Bot = new Botbase();

            //Begin the loopback
            if (Container.Bot.Area.CenterPosition.DistanceTo(Game.Player.Movement.Source) > 80)
                Bundles.Loop.Start(); //Task.Run(() => { Bundles.Loop.Start(); });
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (Container.Lock)
            {
                if (Game.Player.InAction)
                    SkillManager.CancelAction();
            }
        }

        /// <summary>
        /// Translate the botbase plugin
        /// </summary>
        /// <param name="language">The language</param>
        public void Translate()
        {
            LanguageManager.Translate(GetView(), Kernel.Language);
        }
    }
}