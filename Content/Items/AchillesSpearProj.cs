using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace PenumbraMod.Content.Items
{
	public class AchillesSpearProj : ModProjectile
	{
		// Define the range of the Spear Projectile. These are overridable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 50f;
		protected virtual float HoldoutRangeMax => 120f;

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = 3;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
			int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

			player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

			// Reset projectile time left if necessary
			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			float halfDuration = duration * 0.5f;
			float progress;

			// Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;
			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			// Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			// Apply proper rotation to the sprite.
			if (Projectile.spriteDirection == -1)
			{
				// If sprite is facing left, rotate 45 degrees
				Projectile.rotation += MathHelper.ToRadians(45f);
			}
			else
			{
				// If sprite is facing right, rotate 135 degrees
				Projectile.rotation += MathHelper.ToRadians(135f);
			}

			return false; // Don't execute vanilla AI.
		}
		const int radius1 = 30;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
			{
				if (hitInfo.Crit)
				{
                    hitInfo.Damage *= 3;
                    const int Repeats = 20;
                    for (int i = 0; i < Repeats; ++i)
                    {
                        Vector2 position2 = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                        int c = Dust.NewDust(position2, 1, 1, DustID.YellowTorch, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[c].noGravity = true;
                        Main.dust[c].velocity *= 8f;
                        Main.dust[c].rotation += 1.1f;
                    }
                }
                if (target.type == NPCID.Medusa)
                {
                    hitInfo.Damage *= 8;
                }
			};
			Projectile.CritChance += 6;

		}
	}
}
