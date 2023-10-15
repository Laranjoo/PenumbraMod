using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable
{
    public class PeridotGemsparkBlock : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.PeridotGemsparkBlock>();
            Item.width = 12;
            Item.height = 12;
            Item.value = 1000;
        }
        public override void AddRecipes()
        {
            CreateRecipe(20)
                 .AddIngredient(ItemID.Glass, 20)
                 .AddIngredient<Peridot>()
                 .AddTile(TileID.WorkBenches)
                 .Register();
        }
    }
}