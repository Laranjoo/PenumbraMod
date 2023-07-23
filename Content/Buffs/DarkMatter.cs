using PenumbraMod.Common.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class DarkMatter : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Dark Matter"); // Buff display name
			// Description.SetDefault("This is halving your soul..."); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}      

        // Allows you to make this buff give certain effects to the given player
        public override void Update(NPC npc, ref int buffIndex) {
			npc.PenumbraNPCBuff().DarkMatter = true;
		}
	}

	
	
}
