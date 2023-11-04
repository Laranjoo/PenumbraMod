using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Marshmellow Blade"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("This fluffy sharp blade makes your enemies fear!");
        }

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Melee;
			Item.width = 20;
			Item.height = 20;
			Item.value = 1500;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MarshmellowBladeProj>();
            Item.useTime = 4;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.5f;
            Item.shootSpeed = 20f; // Adjusts how far away from the player to hold the projectile
            Item.noMelee = true; // Turns off damage from the item itself, as we have a projectile
            Item.noUseGraphic = true; // Stops the item from drawing in your hands, for the aforementioned reason
            Item.channel = true; // Important as the projectile checks if the player channels
        }
        bool notboollol = true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int basic = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (notboollol == true)
            {
                Main.projectile[basic].ai[1] = -1;
                notboollol = false;
            }
            else
            {
                Main.projectile[basic].ai[1] = 1;
                notboollol = true;
            }
            return false;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Marshmellow>(), 10);
			recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
    public class MarshmellowBladeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Prevents jitter when steping up and down blocks and half blocks
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            Main.projFrames[Projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = -1; // Replace with 20 if you do not want custom code
            Projectile.hide = true; // Hides the projectile, so it will draw in the player's hand when we set the player's heldProj to this one.
        }
        // This code is adapted and simplified from aiStyle 20 to use a different dust and more noises. If you want to use aiStyle 20, you do not need to do any of this.
        // It should be noted that this projectile has no effect on mining and is mostly visual.
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.timeLeft = 60;

            // Animation code could go here if the projectile was animated. 

            // Plays a sound every 20 ticks. In aiStyle 20, soundDelay is set to 30 ticks.
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 6;
            }

            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                // This code must only be ran on the client of the projectile owner
                if (player.channel)
                {
                    float holdoutDistance = player.HeldItem.shootSpeed * Projectile.scale;
                    // Calculate a normalized vector from player to mouse and multiply by holdoutDistance to determine resulting holdoutOffset
                    Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);
                    if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
                    {
                        // This will sync the projectile, most importantly, the velocity.
                        Projectile.netUpdate = true;
                    }

                    // Projectile.velocity acts as a holdoutOffset for held projectiles.
                    Projectile.velocity = holdoutOffset;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            if (Projectile.velocity.X > 0f)
            {
                player.ChangeDir(1);
            }
            else if (Projectile.velocity.X < 0f)
            {
                player.ChangeDir(-1);
            }
            player.ChangeDir(Projectile.direction); // Change the player's direction based on the projectile's own
            player.heldProj = Projectile.whoAmI; // We tell the player that the drill is the held projectile, so it will draw in their hand
            player.SetDummyItemTime(2); // Make sure the player's item time does not change while the projectile is out
            Projectile.Center = playerCenter; // Centers the projectile on the player. Projectile.velocity will be added to this in later Terraria code causing the projectile to be held away from the player at a set distance.
            Projectile.rotation = Projectile.velocity.ToRotation();
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            if (++Projectile.frameCounter >= 1)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }
    }
}