using Terraria;
using PenumbraMod.Content.Buffs;
using Microsoft.Xna.Framework;

namespace PenumbraMod.Common.Base
{
	
	public static class BaseExtension
	{
        /// <summary>This references a instance of the buff GlobalNPC.</summary>
        public static BuffNPC PenumbraNPCBuff(this NPC npc) => npc.GetGlobalNPC<BuffNPC>();
        /// <summary>This references a instance of the buff ModPlayer.</summary>
        public static BuffPlayer PenumbraPlayerBuff(this Player player) => player.GetModPlayer<BuffPlayer>();
        /// <summary>
        /// Used for lerping colors
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Color MultiLerpColor(float percent, params Color[] colors)
        {
            float per = 1f / ((float)colors.Length - 1);
            float total = per;
            int currentID = 0;
            while (percent / total > 1f && currentID < colors.Length - 2) { total += per; currentID++; }
            return Color.Lerp(colors[currentID], colors[currentID + 1], (percent - per * currentID) / per);
        }
       
    }
}
