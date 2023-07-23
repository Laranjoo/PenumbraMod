using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class ProjectileShockwave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shockwave"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 30;
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 23;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            DrawOffsetX = -8;
            Projectile.netImportant = true;
        }
        double deg;
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            projOwner.heldProj = Projectile.whoAmI;
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.BlueCrystalShard, 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.8f;
            Main.dust[dust].scale = (float) Main.rand.Next(100, 135) * 0.011f;
            Projectile.rotation += 0.11f;
            deg = Projectile.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = 10;
            //Orient projectile
            Projectile.direction = projOwner.direction;
            Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            
        }

    }
    public class ProjectileShockwave2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shockwave"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 30;
            Projectile.width = 130;
            Projectile.height = 130;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 23;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            DrawOffsetX = 13;
            DrawOriginOffsetY = -2;
            Projectile.netImportant = true;
        }
        double deg;
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            projOwner.heldProj = Projectile.whoAmI;
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.BlueCrystalShard, 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.8f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.011f;
            Projectile.rotation -= 0.11f;
            deg = Projectile.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = 4;
            //Orient projectile
            Projectile.direction = projOwner.direction;
            Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
        }

    }
}