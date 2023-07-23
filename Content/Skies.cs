using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Common;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace PenumbraMod.Content
{
    /// <summary>
    /// This class contains all the custom skies of the mod!
    /// </summary>
    public class Skies
    {

    }

    public class StormSky : CustomSky
    {
        public bool Active;
        public float Intensity = 0f;
        float vel;
        public override void Update(GameTime gameTime)
        {
            const float alpha = 0.01f;
            if (Active)
            {
                Intensity += alpha;
                if (Intensity > 1f)
                {
                    Intensity = 1f;
                }
                vel -= 0.9f;
            }
            else
            {
                Intensity -= alpha;
                if (Intensity < 0f)
                {
                    Intensity = 0f;
                    Deactivate();
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D SkyTex = ModContent.Request<Texture2D>("PenumbraMod/Content/StormSky").Value;
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                Rectangle rect = new Rectangle(0, /*Math.Max(0, (int)((Main.worldSurface * 16.0 - Main.screenPosition.Y - 2400.0) * 0.10000000149011612))*/ 0, Main.screenWidth, Main.screenHeight);
                spriteBatch.Draw(SkyTex, rect, null, Main.ColorOfTheSkies * Intensity, 0f, new Vector2(vel % 1270, 0), SpriteEffects.None, 0f);
                spriteBatch.Draw(SkyTex, rect, null, Main.ColorOfTheSkies * Intensity, 0f, new Vector2(1270 + vel % 1270, 0), SpriteEffects.None, 0f);
            }
        }
        public override float GetCloudAlpha()
        {
            return 1f - Intensity;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            Active = true;
        }

        public override void Deactivate(params object[] args)
        {
            Active = false;
            vel = 0;
        }

        public override void Reset()
        {
            Active = false;
            vel = 0;
        }
        public override bool IsActive()
        {
            return Active;
        }
    }
    public class AbosluleSky : ModSceneEffect
    {
        public static SceneEffectPriority setPriority = SceneEffectPriority.BossLow;

        public override bool IsSceneEffectActive(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()))
            {
                return true;
            }
            return false;

        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Eyeofthestorm>()) && Main.npc[PenumbraGlobalNPC.eyeStorm].life < Main.npc[PenumbraGlobalNPC.eyeStorm].lifeMax / 2 && isActive)
            {
                Main.numClouds = 50;
                Filters.Scene.Activate("Graveyard", Main.npc[PenumbraGlobalNPC.eyeStorm].Center);

                SkyManager.Instance.Activate("PenumbraMod:StormSky");
            }
            else
            {
                SkyManager.Instance.Deactivate("PenumbraMod:StormSky");
                Filters.Scene.Deactivate("Graveyard");
            }
            if (isActive)
            {


            }
            else
            {

            }
        }
        public override SceneEffectPriority Priority => setPriority;

    }
}
