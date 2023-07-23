using PenumbraMod.Content.Tiles;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Common.Systems
{
	public class AkuCheckSystem : ModSystem
	{
		public int AkuCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			AkuCount = tileCounts[ModContent.TileType<AkuBlock>()];
		}
	}
}
