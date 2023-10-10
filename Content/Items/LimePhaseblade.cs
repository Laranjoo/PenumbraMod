using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class LimePhaseblade : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WhitePhaseblade);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Peridot>(), 10);
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}