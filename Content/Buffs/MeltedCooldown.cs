using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Common.Base;

namespace PenumbraMod.Content.Buffs
{

	public class MeltedCooldown : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Melted Cooldown"); // Buff display name
			// Description.SetDefault(""); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			
		}
       
		
		
	}
    public class MeltedEx : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Explosion"); // Buff display name
            // Description.SetDefault(""); // Buff description
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;

        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().MeltedEx = true;
        }

    }


}
