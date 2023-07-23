using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Common.Systems
{
	public class MarbleCheckSystem : ModSystem
	{
		public int MarbleCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			MarbleCount = tileCounts[TileID.Marble];
		}
	}
}
