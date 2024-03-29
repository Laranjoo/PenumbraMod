using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace PenumbraMod.Content.ExpertAccessorySlot
{
    public class UnlockedUI : UIState
    {
        private UIText text;
        private DraggableUI panel;
        private UIImage border;
        private HideUIButton closeButton;
        public override void OnInitialize()
        {
            panel = new DraggableUI();
            panel.SetPadding(0);
            SetRectangle(panel, left: Main.screenWidth / 2, top: Main.screenHeight / 2, width: 400f, height: 170f);
            panel.BackgroundColor = new Color(0, 0, 0) * 0f;
            panel.BorderColor = new Color(0, 0, 0) * 0f;

            border = new UIImage(Request<Texture2D>("PenumbraMod/Content/UI/UnlockedAccessorySlotBorder", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            border.SetPadding(0);
            SetRectangle(border, left: 0, top: 0, width: 400, height: 170);

            text = new UIText("", 0.92f); // text to show stat
            text.Width.Set(100, 0f);
            text.Height.Set(100, 0f);
            text.Top.Set(34, 0f);
            text.Left.Set(19, 0f);

            Asset<Texture2D> buttonDeleteTexture = Request<Texture2D>("PenumbraMod/Content/UI/UnlockedAccessorySlotBorderButton");
            closeButton = new HideUIButton(buttonDeleteTexture, Hideui.Text2); // Localized text for "Close"
            SetRectangle(closeButton, left: 370f, top: 8f, width: 26f, height: 28f);
            closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
            closeButton.SetVisibility(1f, 0.9f);

            panel.Append(border);
            panel.Append(closeButton);
            panel.Append(text);
            Append(panel);
        }
        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
            Hideui.hideui = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hideui.hideui)
                return;
            if (!Main.playerInventory)
                return;
            if (!Main.LocalPlayer.GetModPlayer<HibiscusPlayer>().hibiscusConsumed)
                return;
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            if (Hideui.hideui)
                return;
            if (!Main.LocalPlayer.GetModPlayer<HibiscusPlayer>().hibiscusConsumed)
                return;
            if (!Main.playerInventory)
                return;
            text.SetText(Hideui.Text);
            base.Update(gameTime);
        }
    }
    internal class HideUIButton : UIImageButton
    {
        // Tooltip text that will be shown on hover
        internal string hoverText;
        internal static LocalizedText text;
        public HideUIButton(Asset<Texture2D> texture, string hoverText) : base(texture)
        {
            this.hoverText = hoverText;
        }
        public HideUIButton(Asset<Texture2D> texture, LocalizedText Text) : base(texture)
        {
            text = Text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.DrawSelf(spriteBatch);

            // IsMouseHovering becomes true when the mouse hovers over the current UIElement
            if (IsMouseHovering)
            {
                Main.instance.MouseText(hoverText);
                Main.instance.MouseText((string)text);
            }
              
        }
    }
    public class Hideui : ModSystem
    {
        public static LocalizedText Text { get; private set; }
        public static LocalizedText Text2 { get; private set; }
        public override void Load()
        {
            string category = "UI";
            Text ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.UnlockedUI"));
            Text2 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.UnlockedUI.Close"));
        }
        public static bool hideui = false;
        public override void OnWorldLoad()
        {
            hideui = false;
        }

        public override void OnWorldUnload()
        {
            hideui = false;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (hideui)
            {
                tag["hideui"] = true;
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            hideui = tag.ContainsKey("hideui");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = hideui;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            hideui = flags[0];
        }
    }
}
