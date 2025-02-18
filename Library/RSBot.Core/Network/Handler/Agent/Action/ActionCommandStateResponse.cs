﻿using RSBot.Core.Event;

namespace RSBot.Core.Network.Handler.Agent.Action
{
    internal class ActionCommandStateResponse : IPacketHandler
    {
        #region Properites

        /// <summary>
        /// Gets or sets the opcode.
        /// </summary>
        /// <value>
        /// The opcode.
        /// </value>
        public ushort Opcode => 0xB074;

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public PacketDestination Destination => PacketDestination.Client;

        #endregion Properites

        /// <summary>
        /// Invokes the specified packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void Invoke(Packet packet)
        {
            var state = packet.ReadByte();
            var recurring = packet.ReadByte();
            if (recurring == 0)
            {
                Core.Game.Player.InAction = false;
                Log.Debug("Player has exited in action!");
                EventManager.FireEvent("OnPlayerExitAction");
            }
            else
            {
                Core.Game.Player.InAction = true;
                Log.Debug("Player has entered in action!");
                EventManager.FireEvent("OnPlayerInAction");
            }
            /*
            switch (state)
            {
                case 0x01:
                    Core.Game.Player.InAction = true;
                    EventManager.FireEvent("OnPlayerInAction");
                    break;

                case 0x02:
                    Core.Game.Player.InAction = recurring != 0;

                    EventManager.FireEvent("OnPlayerExitAction");
                    break;
            }*/
        }
    }
}