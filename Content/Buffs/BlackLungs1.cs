using PenumbraMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class BlackLungs1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Black Lungs");
            // Description.SetDefault("You've been smoking a lot!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;

        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().lifeRegenDebuff = true;
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(player.position + new Microsoft.Xna.Framework.Vector2(0, -30), 4, 3, ModContent.DustType<NargaDust>(), player.velocity.X * 0f, player.velocity.Y * 0f);
                Main.dust[dust].velocity.Y -= 5f;
                Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.008f;
                Main.dust[dust].noGravity = false;
            }
        }
    }
}
