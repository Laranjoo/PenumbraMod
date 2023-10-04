using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.MeltedBlaster
{
	public class MeltedTrident : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		  // DisplayName.SetDefault("Melted Trident"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

		public override void SetDefaults()
		{
			Projectile.damage = 50;
			Projectile.width = 56;
			Projectile.height = 20;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft= 180;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
            Projectile.alpha = 150;
            AIType = ProjectileID.Bullet;

        }
        
        public override void AI()
		{
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.LavaMoss, 1f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 2f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.012f;

            Player owner = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (owner.ownedProjectileCounts[ModContent.ProjectileType<MeltedTrident2>()] >= 1)
            {
                Projectile.Kill();
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.2f);
                Main.dust[dust].velocity *= 5.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class MeltedTrident2 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/MeltedBlaster/MeltedTrident";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Trident"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 70;
            Projectile.width = 56;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 150;
            AIType = ProjectileID.Bullet;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.LavaMoss, 1f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 2f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.012f;
            Player owner = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (owner.ownedProjectileCounts[ModContent.ProjectileType<MeltedTrident3>()] >= 1)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.2f);
                Main.dust[dust].velocity *= 5.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class MeltedTrident3 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/MeltedBlaster/MeltedTrident";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Trident"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 90;
            Projectile.width = 56;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 150;
            AIType = ProjectileID.Bullet;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.LavaMoss, 1f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 2f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.012f;
            Player owner = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (owner.ownedProjectileCounts[ModContent.ProjectileType<MeltedTrident4>()] >= 1)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.2f);
                Main.dust[dust].velocity *= 5.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class MeltedTrident4 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/MeltedBlaster/MeltedTrident";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Trident"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 120;
            Projectile.width = 56;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 150;
            AIType = ProjectileID.Bullet;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.LavaMoss, 1f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 2f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.012f;
            Player owner = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (owner.ownedProjectileCounts[ModContent.ProjectileType<MeltedTrident5>()] >= 1)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.2f);
                Main.dust[dust].velocity *= 5.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class MeltedTrident5 : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/MeltedBlaster/MeltedTrident";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Trident"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 200;
            Projectile.width = 56;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 150;
            AIType = ProjectileID.Bullet;

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<Melting>(), 120);
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.LavaMoss, 1f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 2f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.012f;
            Projectile.rotation = Projectile.velocity.ToRotation();


        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 2.2f);
                Main.dust[dust].velocity *= 7.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class MeltedFire : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted fire"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 10;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (modifiers.PvP)
            target.AddBuff(BuffID.OnFire, 120);
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.OrangeTorch, 2f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 2f;
            Main.dust[dust].scale = (float)Main.rand.Next(135, 160) * 0.012f;
            int dust2 = Dust.NewDust(Projectile.Center, 1, 1, DustID.LavaMoss, 2f, 0f, 0);
            Main.dust[dust2].noGravity = false;
            Main.dust[dust2].velocity *= 2f;
            Main.dust[dust2].scale = (float)Main.rand.Next(135, 160) * 0.012f;

        }
        
        public override void OnKill(int timeLeft)
        {
           
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }
    public class MeltedTridentHostile : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/MeltedBlaster/MeltedTrident";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Trident"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 80;
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 150;
            AIType = ProjectileID.Bullet;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<Melting>(), 180);
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.LavaMoss, 1f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 2f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.012f;
            Player player = Main.player[Projectile.owner];
            Projectile.velocity = player.Center;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 50; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 2.2f);
                Main.dust[dust].velocity *= 7.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item109, Projectile.position);
        }
    }  
}