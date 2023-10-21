using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    public class SpectreForce : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
    class SoulBoost : ModBuff
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(player.position, 30, player.height, DustID.SpectreStaff) ;
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity.Y -= 8f;
            }
        }
    }
}
