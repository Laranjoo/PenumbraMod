using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace PenumbraMod.Content.ExpertAccessorySlot
{
    public class ExpertSlot : ModAccessorySlot
    {
        bool unlockedEffect = true; // Glow effect behind slot when first unlocking it
        bool unlockedEffect2 = true; // Same
        bool unlockedEffect3 = true; // Same
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
           // Player player = Main.player[Main.myPlayer];
            if (ModSlotPlayer.Player.GetModPlayer<HibiscusPlayer>().hibiscusConsumed && checkItem.expert) // If player has consumed the bloody hibiscus AND if the item is expert, the slot can have an accessory equipped
            {

                    return true;
            }
            return false; // Otherwise nothing in slot
        }

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
        {
            if (ModSlotPlayer.Player.GetModPlayer<HibiscusPlayer>().hibiscusConsumed && item.expert) // If player has consumed the bloody hibiscus AND if the item is expert, the slot can have an accessory equipped
            {

                    return true;
            }
            return false;
        }
        public override bool IsEnabled()
        {
            
            if (ModSlotPlayer.Player.GetModPlayer<HibiscusPlayer>().hibiscusConsumed && Main.expertMode) // If player has consumed the bloody hibiscus, show the slot.
                return true;
            return false; // Otherwise, don't show.
        }
        // Overrides the default behaviour where a disabled accessory slot will allow retrieve items if it contains items
        public override bool IsVisibleWhenNotEnabled()
        {
            return false; // We set to false to just not display if not Enabled. NOTE: this does not affect behavour when mod is unloaded!
        }
        float rot;
        public override string VanityBackgroundTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotVanity";
        public override string DyeBackgroundTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotDye";
        public override string FunctionalBackgroundTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlot";

        public override bool PreDraw(AccessorySlotType context, Item item, Vector2 position, bool isHovered)
        {
            //
            // Used to draw a glow behind the slots when the player just unlocked them (they reappear when reloading the mod, only when the slots aren´t being used)
            //
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D tex = ModContent.Request<Texture2D>("PenumbraMod/Content/ExpertAccessorySlot/LightUnlocked").Value;
            const float TwoPi = (float)Math.PI * 2f;
            float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi) * 2.0f + 0.7f;
            rot -= 0.05f;
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Color baseColor = Color.Red;
                    Color lightColor = item.GetAlpha(baseColor);
                    baseColor.A = 0;
                    float k = (lightColor.R / 255f + lightColor.G / 255f + lightColor.B / 255f) / 2f;
                    if (item.type != ItemID.None)
                        unlockedEffect = false; // When the slot is being used, hide the glow
                    if (unlockedEffect) // Otherwise, make the glow appear
                        spriteBatch.Draw(tex, position + new Vector2(20, 20), null, baseColor * k * 2.2f, rot, tex.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    
                    break;

                case AccessorySlotType.VanitySlot:
                    Color baseColor2 = Color.Green;
                    Color lightColor2 = item.GetAlpha(baseColor2);
                    baseColor2.A = 0;
                    float k2 = (lightColor2.R / 255f + lightColor2.G / 255f + lightColor2.B / 255f) / 2f;
                    if (item.type != ItemID.None)
                        unlockedEffect2 = false; // When the slot is being used, hide the glow
                    if (unlockedEffect2) // Otherwise, make the glow appear
                        spriteBatch.Draw(tex, position + new Vector2(20, 20), null, baseColor2 * k2 * 2.2f, 23 + rot, tex.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    break;

                case AccessorySlotType.DyeSlot:
                    Color baseColor3 = Color.Blue;
                    Color lightColor3 = item.GetAlpha(baseColor3);
                    baseColor3.A = 0;
                    float k3 = (lightColor3.R / 255f + lightColor3.G / 255f + lightColor3.B / 255f) / 2f;
                    if (item.type != ItemID.None)
                        unlockedEffect3 = false; // When the slot is being used, hide the glow
                    if (unlockedEffect3) // Otherwise, make the glow appear
                        spriteBatch.Draw(tex, position + new Vector2(20, 20), null, baseColor3 * k3 * 2.2f, 72 + rot, tex.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                    break;
            }

            return true;
        }
        public override void PostDraw(AccessorySlotType context, Item item, Vector2 position, bool isHovered)
        {
            SpriteBatch sb = Main.spriteBatch;
            if (item.type != ItemID.None)
            {
                // Rainbow effect on the line when an accessory is equipped
                sb.Draw(ModContent.Request<Texture2D>("PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLine").Value, position, null, Main.DiscoColor, 0, item.position, 1, SpriteEffects.None, 0);
            }

            else
            {
                // If not, show just a white line
                sb.Draw(ModContent.Request<Texture2D>("PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLine").Value, position, null, Color.White, 0, item.position, 1, SpriteEffects.None, 0);
            }

        }
        // Can be used to modify stuff while the Mouse is hovering over the slot.
        public override void OnMouseHover(AccessorySlotType context)
        {
            // We will modify the hover text while an item is not in the slot, so that it says "AUUGH".
            var item = Main.mouseItem;
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = (string)NormalSlotsPrevention.Text;
                    item.accessory = true; // When hovering into the expert-only slot, make the accessory equippable (read line 214)
                    break;
                case AccessorySlotType.VanitySlot:
                    Main.hoverItemName = (string)NormalSlotsPrevention.Text2; // When hovering into the expert-only slot, make the accessory equippable (read line 214)
                    item.accessory = true;
                    break;
                case AccessorySlotType.DyeSlot:
                    Main.hoverItemName = (string)NormalSlotsPrevention.Text3; // When hovering into the expert-only slot, make the accessory equippable (read line 214)
                    item.accessory = true;
                    break;
            }
        }
    }
    public class ExpertSlotLocked : ModAccessorySlot
    {
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return false; // Nothing enters the slot
        }
        // Designates our slot to be a priority for putting wings in to. NOTE: use ItemLoader.CanEquipAccessory if aiming for restricting other slots from having wings!
        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
        {
            return false;
        }
        public override bool IsEnabled()
        {
           // Player player = Main.player[Main.myPlayer];
            if (!ModSlotPlayer.Player.GetModPlayer<HibiscusPlayer>().hibiscusConsumed && Main.expertMode) // When the player has'nt consumed the bloody hibiscus that unlocks the normal slot
                return true;
            return false; // If player has consumed, hide this slot
        }
        // Overrides the default behaviour where a disabled accessory slot will allow retrieve items if it contains items
        public override bool IsVisibleWhenNotEnabled()
        {
            return false; // We set to false to just not display if not Enabled. NOTE: this does not affect behavour when mod is unloaded!
        }
        public override string VanityTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLocketIcon";
        public override string VanityBackgroundTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLocked";
        public override string DyeTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLocketIcon";
        public override string DyeBackgroundTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLocked";
        public override string FunctionalTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLocketIcon";
        public override string FunctionalBackgroundTexture => "PenumbraMod/Content/ExpertAccessorySlot/ExpertSlotLocked";
        // Can be used to modify stuff while the Mouse is hovering over the slot.
        public override void OnMouseHover(AccessorySlotType context)
        {

            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = (string)NormalSlotsPrevention.Text4;
                    break;
                case AccessorySlotType.VanitySlot:
                    Main.hoverItemName = (string)NormalSlotsPrevention.Text5;
                    break;
                case AccessorySlotType.DyeSlot:
                    Main.hoverItemName = (string)NormalSlotsPrevention.Text6;
                    break;
            }
        }
    }
    //
    // Prevents expert items from being placed on normal accessory slots
    //
    class NormalSlotsPrevention : ModSystem
    {
        public static LocalizedText Text { get; private set; }
        public static LocalizedText Text2 { get; private set; }
        public static LocalizedText Text3 { get; private set; }
        public static LocalizedText Text4 { get; private set; }
        public static LocalizedText Text5 { get; private set; }
        public static LocalizedText Text6 { get; private set; }
        public override void Load()
        {
            string category = "UI";
            Text ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ExpertAccessoryHoverName1"));
            Text2 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ExpertAccessoryHoverName2"));
            Text3 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ExpertAccessoryHoverName3"));
            Text4 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ExpertAccessoryHoverName4"));
            Text5 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ExpertAccessoryHoverName5"));
            Text6 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ExpertAccessoryHoverName6"));
            On_ItemSlot.MouseHover_ItemArray_int_int += On_ItemSlot_MouseHover_ItemArray_int_int;
        }
        public override void Unload()
        {
            On_ItemSlot.MouseHover_ItemArray_int_int -= On_ItemSlot_MouseHover_ItemArray_int_int;
        }
        private void On_ItemSlot_MouseHover_ItemArray_int_int(On_ItemSlot.orig_MouseHover_ItemArray_int_int orig, Item[] inv, int context, int slot)
        {
            // Basically, when the player has an expert accessory on the mouse, make the accessory "not an accessory"
            // So when hovering the expert slot, the accessory can "be an accessory" again. (Meaning it can be equippied again, and ONLY the expert slot)
            // Lines 121 to 140.
            orig(inv, context, slot);
            
            var item = Main.mouseItem;
            if (item.expert)
            {
                switch (context)
                {
                    case ItemSlot.Context.EquipAccessory:
                        item.accessory = false;
                        return;
                }
            }
        }
       
    }
}
