using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Content.DamageClasses;

namespace PenumbraMod.Content.Buffs
{

	public class MeltedForce : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Melted Force"); // Buff display name
			/* Description.SetDefault("The hell acomplishes you"
                + "\nMovement speed increased by 15%" +
                "\nReaper attack speed increased by 25%" +
                "\nDamage increased by 5%" +
                "\nYou damage enemies when hit"); */ // Buff description
            Main.buffNoSave[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex)
        {
            player.thorns = 0.50f;
            player.moveSpeed += 0.15f;
            player.GetAttackSpeed<ReaperClass>() += 0.25f;
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetModPlayer<BuffPlayer>().MeltedArmor = true;
        }


    }

    
}
