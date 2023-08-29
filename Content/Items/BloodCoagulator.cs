using PenumbraMod.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;
using System;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items
{
	public class BloodCoagulator : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("BloodCoagulator"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("Fires 3 Blood Shots that inflicts ichor on enemies");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 89;
			Item.crit = 10; 
			Item.mana = 10;
			Item.DamageType = DamageClass.Magic;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 8;
			Item.value = 1000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<BloodShot>();
			Item.shootSpeed = 18f;
			Item.noMelee = true;
			
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
           
           const int NumProjectiles = 2;

           for (int i = 0; i < NumProjectiles; i++)
           {
                    Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                    float ceilingLimit = target.Y;
                    if (ceilingLimit > player.Center.Y - 5f)
                    {
                        ceilingLimit = player.Center.Y - 5f;
                    }

                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(20));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 1f - Main.rand.NextFloat(0.4f);

                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
           }
              return true;
           
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Ruby, 5);
			recipe.AddIngredient(ItemID.DiamondStaff, 1);
			recipe.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 7);
            recipe.AddIngredient(ItemID.SoulofNight, 2);
			recipe.AddIngredient(ItemID.Ichor, 5);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

            Recipe recipealternative = CreateRecipe();
            recipealternative.AddIngredient(ItemID.Ruby, 5);
            recipealternative.AddIngredient(ItemID.RubyStaff, 1);
            recipealternative.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 7);
            recipealternative.AddIngredient(ItemID.SoulofNight, 2);
            recipealternative.AddIngredient(ItemID.Ichor, 5);
            recipealternative.AddTile(TileID.MythrilAnvil);
            recipealternative.Register();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 43f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			
            target.AddBuff(BuffID.Ichor, 180);
        }
	}
}