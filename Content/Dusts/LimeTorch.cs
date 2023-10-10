using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Dusts
{
	public class LimeTorch : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
			dust.noGravity = false; // Makes the dust have no gravity.
			dust.scale *= 1.2f; // Multiplies the dust's initial scale by 1.5.
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.8f;
			dust.scale *= 0.97f;

			if (dust.scale < 0.5f) {
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}
