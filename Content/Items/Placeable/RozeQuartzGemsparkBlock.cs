using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable
{
    public class RozeQuartzGemsparkBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.RozeQuartzGemsparkBlock>();
            Item.width = 12;
            Item.height = 12;
        }
        public override void AddRecipes()
        {
            CreateRecipe(20)
                 .AddIngredient(ItemID.Glass, 20)
                 .AddIngredient<RozeQuartz>()
                 .AddTile(TileID.WorkBenches)
                 .Register();
        }
    }
}