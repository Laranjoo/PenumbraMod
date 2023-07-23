using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Content.DamageClasses;

namespace PenumbraMod.Content.Buffs
{

	public class ReaperControl : ModBuff
	{
		public override void SetStaticDefaults() {
            Main.buffNoTimeDisplay[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                player.controlUseItem = true;
        }


    }
}
