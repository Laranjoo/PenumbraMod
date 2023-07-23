using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace PenumbraMod.Content.Buffs
{

	public class ElementalCooldown : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Elemental Cooldown"); // Buff display name
			// Description.SetDefault("Let your sword recharge their energies, You can't use Elemental Shots!"); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			
		}
       
		
		
	}

    
}
