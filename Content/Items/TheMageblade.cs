using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TheMageblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Mageblade"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("[c/00ffff:An ancient sword used by an old hero...]" +
                "\n[c/00ffff:This blade is the powerful combination of the 4 legendary shards]" +
                "\n[c/00ffff:This hero was brave to save his world from the darkness and corruption]" +
                "\n[c/00ffff:Nobody knew his name, but they will remember who saved them from the evil...]" +
				"\nThrows a floating blade that follows the cursor and explodes in a bunch of parts."); */

        }

        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = 30450;
            Item.rare = ItemRarityID.Cyan;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TheMagebladeSwing>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.crit = 25;
            Item.noMelee = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TheMagebladeSwing>(), damage, knockback, player.whoAmI);
            Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
            return false;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3() * 0.80f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FirstShardOfTheMageblade>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SecondShardOfTheMageblade>(), 1);
            recipe.AddIngredient(ModContent.ItemType<ThirdShardOfTheMageblade>(), 1);
            recipe.AddIngredient(ModContent.ItemType<FourthShardOfTheMageblade>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}