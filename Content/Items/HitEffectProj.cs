using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class HitEffectProj : ModProjectile
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().HitEffect;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("HitEffectProj");
        }

        public override void SetDefaults()
        {
            Projectile.width = 7;
            Projectile.height = 22;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            Projectile.alpha += 25;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return ModContent.GetInstance<PenumbraConfig>().HitEffectColor * Projectile.Opacity;
        }

    }
}