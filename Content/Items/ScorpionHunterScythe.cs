using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class ScorpionHunterScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Golden Sickle"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("This sickle can extract hay from the grass" +
                "\nIt has a small chance to inflict a buff that makes enemies drop more cash." +
                "\n[c/f9c720:Special ability:] When used, the scythe swing faster for 5 seconds, at cost of reaper energy will not increase."); */
			
		}

		public override void SetDefaults()
		{
            Item.damage = 50;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 52;
            Item.height = 42;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 7850;
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ScorpionHunterBubble>();
            Item.shootSpeed = 10f;
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
			{
				float NumProjectiles = Main.rand.NextFloat(3, 5);
				for (float i = 0; i < NumProjectiles; i++)
				{
					Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(50));
					// Create a projectile.
					Projectile.NewProjectileDirect(source, position, newVelocity, type, Item.damage, knockback, player.whoAmI);
				}
				SoundEngine.PlaySound(SoundID.Item86, player.position);
				return true;
			}
			else
				return false;
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Main.rand.NextBool(24))
			{
				player.AddBuff(BuffID.Venom, 120);
			}
           
        }
        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient<ScorpionShell>(14)
            .AddTile(TileID.Anvils)
			.Register();
		}
	}
}