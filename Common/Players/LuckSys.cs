using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Common.Players
{
	public class LuckSys : ModPlayer
	{
		public bool bunny = false;
		public override void ModifyLuck(ref float luck) { 

			if (Main.hardMode) { // If is hardmode
				luck += 0.2f; // 0.2 luck if hardmode is active
			}
            if (bunny == true)
            { 
                luck += 0.3f; // bunny accessory
            }
           
        }
	}
}
