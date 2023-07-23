using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace PenumbraMod.Content.Tiles
{
    public class AkuBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileBlockLight[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileSolid[Type] = true;

            TileID.Sets.CanBeDugByShovel[Type] = true;
            TileID.Sets.Grass[Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            TileID.Sets.NeedsGrassFramingDirt[Type] = ItemID.DirtBlock;
            TileID.Sets.ForcedDirtMerging[Type] = true;
            TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
            TileID.Sets.ResetsHalfBrickPlacementAttempt[Type] = false;
            TileID.Sets.DoesntPlaceWithTileReplacement[Type] = true;
            AddMapEntry(new Color(59, 126, 209));
            MinPick = 10;
            MineResist = 0.1f;
            DustType = DustID.BlueTorch;
            RegisterItemDrop(ItemID.DirtBlock);
        }
        public override void FloorVisuals(Player player)
        {
            if (player.velocity.X != 0f && Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustDirect(player.Bottom, 0, 0, DustType, 0f, -Main.rand.NextFloat(2f));
                dust.noGravity = true;
                dust.fadeIn = 1f;
            }
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!effectOnly)
            {
                fail = true;
                Main.tile[i, j].TileType = (ushort)ItemID.DirtBlock;
                WorldGen.SquareTileFrame(i, j);

                for (int k = 0; k < 3; k++)
                    Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.BlueTorch);
            }
        }
    }
}