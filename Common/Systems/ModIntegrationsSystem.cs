using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items.Placeable.Furniture;
using PenumbraMod.Content.NPCs.Bosses;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Common.Systems
{
    internal class ModIntegrationsSystem
    {
        public static void PerformModSupport()
        {
            PerformBossChecklistSupport();
        }
        private static void PerformBossChecklistSupport()
        {
            PenumbraMod mod = PenumbraMod.Instance;
            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                #region Eyeofstorm
                bossChecklistMod.Call("AddBoss", mod, "Eye of the Storm", ModContent.NPCType<Eyeofthestorm>(), 5.5f, () => DownedBossSystem.downedEyestormBoss, () => true,
                    new List<int>
                    {
                ItemID.GoldBar,
                ItemID.SandBoots,
                ModContent.ItemType<ShockWave>(),
                ModContent.ItemType<SparkBow>(),
                ModContent.ItemType<StaffofEnergy>(),
                ModContent.ItemType<ChargeGun>(),
                ModContent.ItemType<OmniStaff>(),
                ModContent.ItemType<EyestormBag>(),
                ModContent.ItemType<EyestormRelic>(),
                    },
                   ModContent.ItemType<Content.Items.Consumables.EyestormSummon>(), "Use a [i:" + ModContent.ItemType<Content.Items.Consumables.EyestormSummon>() + "] to rage the wind and wake the ancient Eye of the Storm.",
                    "The Eye of the Storm flew away for the storms...",
                    (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Eyeofthestorm_Preview").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }, null);

                #endregion
            }

        }
    }
}
