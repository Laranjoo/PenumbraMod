using Microsoft.Xna.Framework;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class CrimsonScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 36;
            Item.height = 38;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.value = 2170;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Blood);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CrimtaneBar, 12)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}