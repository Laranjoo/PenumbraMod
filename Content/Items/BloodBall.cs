using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class BloodBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("BloodBall"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

		public override void SetDefaults()
		{
			Projectile.damage = 30;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 68;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.timeLeft= 600;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;	
		}
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/BloodBallEf").Value;
            // Redraw the projectile with the color not influenced by light
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    return false;
                for (float j = 0; j < 1; j += 0.0625f)
                {
                    Vector2 lerpedPos;
                    if (i > 0)
                        lerpedPos = Vector2.Lerp(Projectile.oldPos[i - 1], Projectile.oldPos[i], easeInOutQuad(j));
                    else
                        lerpedPos = Vector2.Lerp(Projectile.position, Projectile.oldPos[i], easeInOutQuad(j));
                    float lerpedAngle;
                    if (i > 0)
                        lerpedAngle = Utils.AngleLerp(Projectile.oldRot[i - 1], Projectile.oldRot[i], easeInOutQuad(j));
                    else
                        lerpedAngle = Utils.AngleLerp(Projectile.rotation, Projectile.oldRot[i], easeInOutQuad(j));
                    lerpedPos += Projectile.Size / 2;
                    lerpedPos -= Main.screenPosition;
                    Main.EntitySpriteDraw(texture, lerpedPos, null, Color.White * 0.5f * (1 - ((float)i / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }
            return true;
        }
        public override void AI()
		{
			int dust2 = Dust.NewDust(Projectile.Center, 0, 0, DustID.RedMoss, 4f, 4f, 0, Color.DarkRed, 1f);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 1.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(150, 200) * 0.004f;
            Lighting.AddLight(Projectile.Center, Color.DarkRed.ToVector3() * 0.80f);
        }

        public override void OnKill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}