using PenumbraMod.Common;
using PenumbraMod.Common.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class StunnedNPC : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stunned"); // Buff display name
			// Description.SetDefault("You are stuck!"); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			Main.buffNoTimeDisplay[Type] = true;
		}

		// Allows you to make this buff give certain effects to the given player
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.PenumbraNPCBuff().stunned = true;
		}
        public override void Update(Player player, ref int buffIndex)
        {
			player.velocity.X = 0;
			player.velocity.Y = 0;
			player.controlLeft = false;
			player.controlRight = false;
			player.controlUp = false;
			player.controlDown = false;
			player.controlJump = false;
			player.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene = true;
        }
    }
	
	
}
