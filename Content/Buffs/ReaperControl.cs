using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class ReaperControl : ModBuff
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                player.controlUseItem = true;
        }


    }
}
