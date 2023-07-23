using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Common.Base;

namespace PenumbraMod.Content.Buffs
{

	public class ShieldCooldown : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Shield Cooldown"); // Buff display name
			// Description.SetDefault(""); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			
		}
       
		
		
	}
}
