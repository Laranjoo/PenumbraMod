using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	internal class AquamarineHook : ModItem
	{
		public override void SetDefaults() {
			// Copy values from the Amethyst Hook
			Item.CloneDefaults(ItemID.AmethystHook);
			Item.shootSpeed = 10f; // This defines how quickly the hook is shot.
			Item.shoot = ModContent.ProjectileType<AquamarineHookProjectile>(); // Makes the item shoot the hook's projectile when used.

			// If you do not use Item.CloneDefaults(), you must set the following values for the hook to work properly:
			// Item.useStyle = ItemUseStyleID.None;
			// Item.useTime = 0;
			// Item.useAnimation = 0;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Placeable.Aquamarine>(15)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	internal class AquamarineHookProjectile : ModProjectile
	{
		private static Asset<Texture2D> chainTexture;

		public override void Load() { // This is called once on mod (re)load when this piece of content is being loaded.
			// This is the path to the texture that we'll use for the hook's chain. Make sure to update it.
			chainTexture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/AquamarineHookChain");
		}

		public override void Unload() { // This is called once on mod reload when this piece of content is being unloaded.
			// It's currently pretty important to unload your static fields like this, to avoid having parts of your mod remain in memory when it's been unloaded.
			chainTexture = null;
		}

		/*
		public override void SetStaticDefaults() {
			// If you wish for your hook projectile to have ONE copy of it PER player, uncomment this section.
			ProjectileID.Sets.SingleGrappleHook[Type] = true;
		}
		*/

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.GemHookAmethyst); // Copies the attributes of the Amethyst hook's projectile.
			Projectile.width = 14;
			Projectile.height = 16;
		}
		// Amethyst Hook is 300, Static Hook is 600.
		public override float GrappleRange() {
			return 300f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks) {
			numHooks = 1; // The amount of hooks that can be shot out
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed) {
			speed = 10f; // How fast the grapple returns to you after meeting its max shoot distance
		}

		public override void GrapplePullSpeed(Player player, ref float speed) {
			speed = 10; // How fast you get pulled to the grappling hook projectile's landing position
		}

		// Can customize what tiles this hook can latch onto, or force/prevent latching alltogether, like Squirrel Hook also latching to trees
		public override bool? GrappleCanLatchOnTo(Player player, int x, int y) {
			// In any other case, behave like a normal hook
			return null;
		}

		// Draws the grappling hook's chain.
		public override bool PreDrawExtras() {
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Center;
			Vector2 directionToPlayer = playerCenter - Projectile.Center;
			float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
			float distanceToPlayer = directionToPlayer.Length();

			while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer)) {
				directionToPlayer /= distanceToPlayer; // get unit vector
				directionToPlayer *= chainTexture.Height(); // multiply by chain link length

				center += directionToPlayer; // update draw position
				directionToPlayer = playerCenter - center; // update distance
				distanceToPlayer = directionToPlayer.Length();

				Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

				// Draw chain
				Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
					chainTexture.Value.Bounds, drawColor, chainRotation,
					chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}
			// Stop vanilla from drawing the default chain.
			return false;
		}
	}
}
