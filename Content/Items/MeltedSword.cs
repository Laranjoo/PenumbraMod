using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static PenumbraMod.Content.Items.MeltedScythe;

namespace PenumbraMod.Content.Items
{
    public class MeltedSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Blade"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("This blade is so hot that your enemies will melt on it" +
				"\nOn critical hits, enemies explode, doing additional damage" +
				"\nIgnites enemies on hit"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 70;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.knockBack = 8;
            Item.value = 6400;
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 1f;
            Item.crit = 14;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.LavaMoss);
            }
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
            {
                if (hitInfo.Crit)
                {
                    hitInfo.Damage += 10;
                    Vector2 newVelocity = target.velocity.RotatedByRandom(MathHelper.ToRadians(0));
                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 10f - Main.rand.NextFloat(0.1f);
                    Projectile.NewProjectile(target.GetSource_FromThis(), target.position, newVelocity, ModContent.ProjectileType<MeltedShotEx>(), 50, 12, player.whoAmI);
                }
            };

        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 13);
            recipe.AddIngredient(ItemID.Obsidian, 18);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}