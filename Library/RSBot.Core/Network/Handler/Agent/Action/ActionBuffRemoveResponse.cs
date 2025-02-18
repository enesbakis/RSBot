﻿using RSBot.Core.Components;
using RSBot.Core.Event;
using RSBot.Core.Objects.Spawn;

namespace RSBot.Core.Network.Handler.Agent.Action
{
    internal class ActionBuffRemoveResponse : IPacketHandler
    {
        #region Properites

        /// <summary>
        /// Gets or sets the opcode.
        /// </summary>
        /// <value>
        /// The opcode.
        /// </value>
        public ushort Opcode => 0xB072;

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
            var buffTokensCount = packet.ReadByte();

            for (var i = 0; i < buffTokensCount; i++)
            {
                var token = packet.ReadUInt();
                if (token == 0) // debuffs
                    continue;

                var buff = Core.Game.Player.State.GetActiveBuff(token);
                if (buff != null)
                {
                    Log.Debug($"The buff [{buff.Record?.GetRealName()}] expired");

                    Core.Game.Player.Buffs.Remove(buff);
                    EventManager.FireEvent("OnRemoveBuff", buff);

                    var playerSkill = Core.Game.Player.Skills.GetSkillInfoById(buff.Id);
                    playerSkill?.Reset();

                    return;
                }

                if (!SpawnManager.TryGetEntity<SpawnedBionic>(p => p.State.HasActiveBuff(token, out _), out var bionic))
                    return;

                bionic.State.TryRemoveActiveBuff(token, out var removedBuff);
            }
        }
    }
}