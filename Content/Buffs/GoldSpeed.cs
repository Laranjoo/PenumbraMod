using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Content.DamageClasses;

namespace PenumbraMod.Content.Buffs
{

	public class GoldSpeed : ModBuff
	{
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Gold Speed"); // Buff display name
			/* Description.SetDefault("You feel like the scythe is light"
                + "\nScythe speed increased by 70%"); */ // Buff description
            Main.buffNoTimeDisplay[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Generic) += 0.70f;
            player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy = 0;
        }


    }

    
}
