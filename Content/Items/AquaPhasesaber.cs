using Microsoft.Xna.Framework;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class AquaPhasesaber : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WhitePhasesaber);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Lighting.AddLight(player.position, Color.Aqua.ToVector3() * 1f);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AquaPhaseblade>());
            recipe.AddIngredient(ItemID.CrystalShard, 25);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}