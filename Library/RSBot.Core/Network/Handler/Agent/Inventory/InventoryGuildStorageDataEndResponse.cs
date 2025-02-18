﻿using RSBot.Core.Event;
using RSBot.Core.Objects;
using System.Collections.Generic;

namespace RSBot.Core.Network.Handler.Agent.Inventory
{
    internal class InventoryGuildStorageDataEndResponse : IPacketHandler
    {
        /// <summary>
        /// Gets or sets the opcode.
        /// </summary>
        /// <value>
        /// The opcode.
        /// </value>
        public ushort Opcode => 0x3254;

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public PacketDestination Destination => PacketDestination.Client;

        /// <summary>
        /// Handles the packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void Invoke(Packet packet)
        {
            if (Core.Game.ChunkedPacket == null)
                return;

            packet = Core.Game.ChunkedPacket;
            packet.Lock();

            Core.Game.Player.Storage = new Storage
            {
                Items = new List<InventoryItem>(),
                Size = packet.ReadByte()
            };

            var itemAmount = packet.ReadByte();

            for (var i = 0; i < itemAmount; i++)
                Core.Game.Player.Storage.Items.Add(InventoryItem.FromPacket(packet));

            EventManager.FireEvent("OnStorageData");

            Log.Notify($"Found {Core.Game.Player.Storage.Items.Count} item(s) in storage");

            Core.Game.ChunkedPacket = null;
        }
    }
}
