using System.Collections;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PenumbraMod.Common.Systems
{
	
	public class DownedBossSystem : ModSystem
	{
		public static bool downedEyestormBoss = false;
        // public static bool downedBuritiBoss = false;

        public override void OnWorldLoad() {
            downedEyestormBoss = false;
           // downedBuritiBoss = false;
        }

		public override void OnWorldUnload() {
            downedEyestormBoss = false;
          //  downedBuritiBoss = false;
        }
		public override void SaveWorldData(TagCompound tag) {
			if (downedEyestormBoss) {
				tag["downedEyestormBoss"] = true;
			}
			//if (downedBuritiBoss)
		//	{
		//		tag["downedBuritiBoss"] = true;
			//}
		}
		public override void LoadWorldData(TagCompound tag) {
			downedEyestormBoss = tag.ContainsKey("downedEyestormBoss");
		//	downedBuritiBoss = tag.ContainsKey("downedBuritiBoss");
        }

		public override void NetSend(BinaryWriter writer) {
			var flags = new BitsByte();
			flags[0] = downedEyestormBoss;
			//flags[1] = downedBuritiBoss;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			downedEyestormBoss = flags[0];
           // downedBuritiBoss = flags[1];
		}
	}
}
