using PenumbraMod.Content.ExpertAccessorySlot;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Consumables;
using System.IO;
using Terraria;
using Terraria.ID;
using static PenumbraMod.Content.Items.Stigmata;

namespace PenumbraMod
{
    partial class PenumbraMod
	{
		internal enum MessageType : byte
		{
			Stigmata,
            CosmicBeetSync,
            Hibiscus,
        }

        // Override this method to handle network packets sent for this mod.
        //TODO: Introduce OOP packets into tML, to avoid this god-class level hardcode.
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.Stigmata:
                    byte playernumber = reader.ReadByte();
                    StigmataPlayer stigmataPlayer = Main.player[playernumber].GetModPlayer<StigmataPlayer>();
                    stigmataPlayer.DamageGiven = reader.ReadInt32();
                    // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        stigmataPlayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.CosmicBeetSync:
                    byte beetsync = reader.ReadByte();
                    CosmicBeetPlayer syncbeetplayer = Main.player[beetsync].GetModPlayer<CosmicBeetPlayer>();
                    syncbeetplayer.ReceivePlayerSync(reader);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        syncbeetplayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                case MessageType.Hibiscus:
                    byte hibisync = reader.ReadByte();
                    HibiscusPlayer synchibiplayer = Main.player[hibisync].GetModPlayer<HibiscusPlayer>();
                    synchibiplayer.ReceivePlayerSync(reader);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        synchibiplayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;           
                default:
                    Logger.WarnFormat("PenumbraMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}