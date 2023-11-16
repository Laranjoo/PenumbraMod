using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace PenumbraMod
{
    [LabelAttribute("$Mods.PenumbraMod.Config.PenumbraConfig.Label")]
    [BackgroundColor(20, 0, 38)]
    public class PenumbraConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        // The "$" character before a name means it should interpret the name as a translation key and use the loaded translation with the same key.
        // The things in brackets are known as "Attributes".
        #region Visuals
        [Header("$Mods.PenumbraMod.Config.Header")] // Headers are like titles in a config. You only need to declare a header on the item it should appear over, not every item in the category.
        [LabelKeyAttribute("$Mods.PenumbraMod.Config.HitEffect.Label")]// A label is the text displayed next to the option. This should usually be a short description of what it does./*/
        [BackgroundColor(255, 0, 0)]// color
        [TooltipKeyAttribute("$Mods.PenumbraMod.Config.HitEffect.Tooltip")] // A tooltip is a description showed when you hover your mouse over the option. It can be used as a more in-depth explanation of the option.
        [DefaultValue(true)] // This sets the configs default value.
        [ReloadRequired] // Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        public bool HitEffect;

        [LabelKeyAttribute("$Mods.PenumbraMod.Config.HitEffectCount.Label")] 
        [BackgroundColor(255, 0, 0)]
        [DefaultValue(typeof(int),"3")]
        [Range(1, 20)]
        public int HitEffectCount;

        [LabelKeyAttribute("$Mods.PenumbraMod.Config.HitEffectColor.Label")]
        [BackgroundColor(255, 0, 0)]
        [DefaultValue("255, 0, 0, 0")]
        [SliderColor(65, 19, 105)]
        public Color HitEffectColor;

        [LabelKeyAttribute("$Mods.PenumbraMod.Config.HitEffectVelocity.Label")]
        [BackgroundColor(255, 0, 0)]
        [SliderColor(65, 19, 105)]
        [DefaultValue(typeof(Vector2), "1, 1")]
        [Range(0f, 12f)]
        public Vector2 HitEffectVelocity { get; set; }

        [LabelKeyAttribute("$Mods.PenumbraMod.Config.UITEXT.Label")]// A label is the text displayed next to the option. This should usually be a short description of what it does./*/
        [BackgroundColor(255, 0, 0)]// color
        [DefaultValue(true)] // This sets the configs default value.
        public bool UITEXT { get; set; }

        [LabelKeyAttribute("$Mods.PenumbraMod.Config.ReaperBarModel.Label")]
        [BackgroundColor(255, 0, 0)]
        [DefaultValue(typeof(int), "1")]
        [Range(1, 3)]
        public int ReaperBarModel;

        #endregion

        #region Player
        [Header("$Mods.PenumbraMod.Config.Header2")]
        [LabelKeyAttribute("$Mods.PenumbraMod.Config.VanillaChanges.Label")]
        [BackgroundColor(130, 0, 155)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool VanillaChanges { get; set; }

        [LabelKeyAttribute("$Mods.PenumbraMod.Config.UseTurn.Label")]
        [TooltipKeyAttribute("$Mods.PenumbraMod.Config.UseTurn.Tooltip")]
        [BackgroundColor(130, 0, 155)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool UseTurn { get; set; }

        /* Removed since vanilla already adds auto use mechanic
        [LabelKeyAttribute("$Mods.PenumbraMod.Config.Autouse.Label")]
        [BackgroundColor(130, 0, 155)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool Autouse { get; set; } 
        */

        #endregion


    }

}
