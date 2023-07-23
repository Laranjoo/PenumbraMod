using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Common.Base;

namespace PenumbraMod.Content.Buffs
{
	public class Corrosion : ModBuff
	{
		public override void SetStaticDefaults() {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().corrosion = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().corrosion = true;
        }


    }

    
}
