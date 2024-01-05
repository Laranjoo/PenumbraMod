using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class ReaperControlDust : ModBuff
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            int Model = ModContent.GetInstance<PenumbraConfig>().ReaperBarModel;
            int type = DustID.LavaMoss;
            if (Model == 1)
                type = DustID.LavaMoss;
            if (Model == 2)
                type = DustID.PurpleMoss;
            if (Model == 3)
                type = DustID.YellowTorch;
            if (Model == 4)
                type = DustID.BlueMoss;
            if (Model == 5)
                type = DustID.UnusedWhiteBluePurple;
            int dust = Dust.NewDust(player.position, player.width, player.height, type, player.velocity.X * 0f, player.velocity.Y * 0f);
            Main.dust[dust].velocity *= 12f;
            Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.012f;
            Main.dust[dust].noGravity = true;
            if (player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 0)
                player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy -= 200;
        }
    }
}
