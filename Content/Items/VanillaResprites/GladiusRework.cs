using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.VanillaResprites
{
	public class GladiusRework : ModProjectile
	{
		public const int FadeInDuration = 7;
		public const int FadeOutDuration = 4;

		public const int TotalDuration = 11;

		// The "width" of the blade
		public float CollisionWidth => 12f * Projectile.scale;

		public int Timer
		{
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gladius");
		}

		public override void SetDefaults()
		{
			Projectile.width = 68;
			Projectile.height = 14;
			Projectile.aiStyle = -1; // Use our own AI to customize how it behaves, if you don't want that, keep this at ProjAIStyleID.ShortSword. You would still need to use the code in SetVisualOffsets() though
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true; // Prevents hits through tiles. Most melee weapons that use projectiles have this
			Projectile.extraUpdates = 1; // Update 1+extraUpdates times per tick
			Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
			Projectile.hide = true; // Important when used alongside player.heldProj. "Hidden" projectiles have special draw conditions
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Timer += 1;
			if (Timer >= TotalDuration)
			{
				// Kill the projectile if it reaches it's intented lifetime
				Projectile.Kill();
				return;
			}
			else
			{
				// Important so that the sprite draws "in" the player's hand and not fully infront or behind the player
				player.heldProj = Projectile.whoAmI;
			}


			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);
			// Point towards where it is moving, applied offset for top right of the sprite respecting spriteDirection
			Projectile.rotation = Projectile.velocity.ToRotation();

			// The code in this method is important to align the sprite with the hitbox how we want it to
			SetVisualOffsets();
		}

		private void SetVisualOffsets()
		{
			// 32 is the sprite size (here both width and height equal)
			const int HalfSpriteWidth = 68 / 2;
			const int HalfSpriteHeight = 14 / 2;

			int HalfProjWidth = Projectile.width / 2;
			int HalfProjHeight = Projectile.height / 2;

			// Vanilla configuration for "hitbox in middle of sprite"
			DrawOriginOffsetX = 0;
			DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
			DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

			// Vanilla configuration for "hitbox towards the end"
			//if (Projectile.spriteDirection == 1) {
			//	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
			//	DrawOriginOffsetY = 0;
			//}
			//else {
			//	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
			//	DrawOffsetX = 0;
			//	DrawOriginOffsetY = 0;
			//}
		}

		public override bool ShouldUpdatePosition()
		{
			// Update Projectile.Center manually
			return false;
		}

		public override void CutTiles()
		{
			// "cutting tiles" refers to breaking pots, grass, queen bee larva, etc.
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
			Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 start = Projectile.Center;
			Vector2 end = start + Projectile.velocity * 6f;
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (target.type != NPCID.TargetDummy)
				Onhit2(target);
		}
		public void Onhit2(NPC target)
		{
            Vector2 pos = new Vector2(-120, 1);
            for (int i = 0; i < 1; i++)
            {
                pos = pos.RotatedByRandom(360);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center + pos, new Vector2(0, 0), ModContent.ProjectileType<GladiusReworkAlt>(), 25, 2f, Main.myPlayer);
            }
        }
	}
	public class GladiusReworkAlt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gladius"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
		}

		public override void SetDefaults()
		{
			Projectile.width = 68;
			Projectile.height = 14;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Projectile.ai[0]++;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation += MathHelper.Pi;
				// For vertical sprites use MathHelper.PiOver2
			}
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            float maxDetectRadius = 100f * 16f; // The maximum radius at which a projectile can detect a target
            float projSpeed = 10f; // The speed at which the projectile moves towards the target

            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
            {
                Projectile.alpha += 20;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }
                return;
            }
			if (Projectile.ai[0] <= 1)
				Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
            
			Projectile.alpha += 20;
			if (Projectile.alpha > 255)
			{
				Projectile.Kill();
			}
			int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.WhiteTorch, 0f, 0f, 0);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0.8f;
			Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.004f;
		}
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy(ignoreDontTakeDamage: true))
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }
    }
}
