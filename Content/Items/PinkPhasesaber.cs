using Microsoft.Xna.Framework;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class PinkPhasesaber : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WhitePhasesaber);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Lighting.AddLight(player.position, Color.Pink.ToVector3() * 0.8f);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PinkPhaseblade>());
            recipe.AddIngredient(ItemID.CrystalShard, 25);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}