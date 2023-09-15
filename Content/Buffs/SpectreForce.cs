using Microsoft.Xna.Framework;
using PenumbraMod.Content.DamageClasses;
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
                Vector2 pos = player.Center + new Vector2(Main.rand.Next(0, 30), Main.rand.Next(40, 50));
                Dust.NewDust(pos, 0, 0, DustID.BlueTorch, 0, -8);
            }
        }
    }
}
