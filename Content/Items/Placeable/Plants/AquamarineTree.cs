using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Placeable.Plants
{
    public class AquamarineTree : ModTree
    {
        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };
        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { TileID.Stone };
        }
        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            // This is where fancy code could go, but let's save that for an advanced example
        }
        // This is the primary texture for the trunk. Branches and foliage use different settings.
        public override Asset<Texture2D> GetTexture()
        {
            return ModContent.Request<Texture2D>("PenumbraMod/Content/Items/Placeable/Plants/AquamarineTree");
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 2;
            return ModContent.TileType<Plants.AquamarineTreeSapling>();
        }

        // Branch Textures
        public override Asset<Texture2D> GetBranchTextures()
        {
            return ModContent.Request<Texture2D>("PenumbraMod/Content/Items/Placeable/Plants/AquamarineTree_Branches");
        }

        // Top Textures
        public override Asset<Texture2D> GetTopTextures()
        {
            return ModContent.Request<Texture2D>("PenumbraMod/Content/Items/Placeable/Plants/AquamarineTree_Tops");
        }

        public override int DropWood()
        {
            return ModContent.ItemType<Items.Placeable.Aquamarine>();
        }

        public override bool Shake(int x, int y, ref bool createLeaves)
        {
            return false;
        }

        public override int TreeLeaf()
        {
            return ModContent.GoreType<AquamarineTreeLeaf>();
        }
    }
}