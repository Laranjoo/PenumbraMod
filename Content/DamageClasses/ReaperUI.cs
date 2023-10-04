using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.ExpertAccessorySlot;
using PenumbraMod.Content.Items.ReaperJewels;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace PenumbraMod.Content.DamageClasses
{
    internal class ReaperUI : UIState
    {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the Mod class.
        public static bool clickedagain = false;
        private UIText text;
        private UIImage barFrame;
        private UIImage barFrameHover;
        #region Crystals
        // UI
        private UIImage AmethystCrystal;
        private UIImage AmethystCrystalSlot2;

        private UIImage TopazCrystal;
        private UIImage TopazCrystalSlot2;

        private UIImage SapphireCrystal;
        private UIImage SapphireCrystalSlot2;

        private UIImage EmeraldCrystal;
        private UIImage EmeraldCrystalSlot2;

        private UIImage RubyCrystal;
        private UIImage RubyCrystalSlot2;

        private UIImage DiamondCrystal;
        private UIImage DiamondCrystalSlot2;

        private UIImage MagicCrystal;
        private UIImage MagicCrystalSlot2;

        private UIImage AzuriteCrystal;
        private UIImage AzuriteCrystalSlot2;

        private UIImage PrimeyeCrystal;
        private UIImage PrimeyeCrystalSlot2;

        private UIImage Ablazed;
        private UIImage AblazedSlot2;

        // simpler

        private UIImage Slimy;
        private UIImage SlimySlot2;

        private UIImage Terra;
        private UIImage TerraSlot2;

        private UIImage Bloodstained;
        private UIImage BloodstainedSlot2;

        private UIImage Darkened;
        private UIImage DarkenedSlot2;

        private UIImage Spectre;
        private UIImage SpectreSlot2;

        // way simpler

        private UIImage Corr;
        private UIImage Corr2;

        private UIImage Ony;
        private UIImage Ony2;

        #endregion 
        private UIImage barSlots;
        private UIImage barSlots2;
        private UIImage barSlots3;
        private UIImage barSlots4;
        private Color gradientA;
        private Color gradientB;
        private UIImage barDrawing;
        private DraggableUI panel;
        private CrystalSlots item;
        private CrystalSlots2 item2;
        public static float position;
        public static float position2;
        public override void OnInitialize()
        {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding. You can use a UIPanel if you wish for a background.

            // This is one of the most painful thing to code i ever made (Laranjoo)


             panel = new DraggableUI();
            panel.SetPadding(0);
            SetRectangle(panel, left: 800, top: 30, width: 92, height: 23);
            panel.BackgroundColor = new Color(0, 0, 0, 255) * 0f;
            panel.BorderColor = new Color(0, 0, 0, 255) * 0f;

            barFrame = new UIImage(Request<Texture2D>("PenumbraMod/Content/DamageClasses/ReaperClassBar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame.SetPadding(0);
            SetRectangle(barFrame, left: 0, top: 0, width: 92, height: 23);

            barFrameHover = new UIImage(Request<Texture2D>("PenumbraMod/Content/DamageClasses/ReaperClassBarHover", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrameHover.SetPadding(0);
            SetRectangle(barFrameHover, left: 0, top: 0, width: 96, height: 24);

            barSlots = new UIImage(Request<Texture2D>("PenumbraMod/Content/DamageClasses/ReaperClassBarSlots", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots.SetPadding(0);
            SetRectangle(barSlots, left: 20, top: 39, width: 0, height: 0);

            barSlots2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/DamageClasses/ReaperClassBarSlotsUsed1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots2.SetPadding(0);
            SetRectangle(barSlots2, left: 20, top: 39, width: 0, height: 0);

            barSlots3 = new UIImage(Request<Texture2D>("PenumbraMod/Content/DamageClasses/ReaperClassBarSlotsUsed2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots3.SetPadding(0);
            SetRectangle(barSlots3, left: 20, top: 39, width: 0, height: 0);

            barSlots4 = new UIImage(Request<Texture2D>("PenumbraMod/Content/DamageClasses/ReaperClassBarSlotsUsed3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots4.SetPadding(0);
            SetRectangle(barSlots4, left: 20, top: 39, width: 0, height: 0);

            barDrawing = new UIImage(Request<Texture2D>("PenumbraMod/EMPTY", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barDrawing.SetPadding(0);
            barDrawing.Color = new Color(0, 0, 0, 255) * 0f;
            SetRectangle(barDrawing, left: 0, top: 0, width: 86, height: 12);

            #region Crystalss
            AmethystCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AmethystCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AmethystCrystal.SetPadding(0);
            SetRectangle(AmethystCrystal, left: 22, top: 9, width: 10, height: 12);

            AmethystCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AmethystCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AmethystCrystalSlot2.SetPadding(0);
            SetRectangle(AmethystCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            TopazCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TopazCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            TopazCrystal.SetPadding(0);
            SetRectangle(TopazCrystal, left: 22, top: 9, width: 10, height: 12);

            TopazCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TopazCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            TopazCrystalSlot2.SetPadding(0);
            SetRectangle(TopazCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            SapphireCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SapphireCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SapphireCrystal.SetPadding(0);
            SetRectangle(SapphireCrystal, left: 22, top: 9, width: 10, height: 12);

            SapphireCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SapphireCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SapphireCrystalSlot2.SetPadding(0);
            SetRectangle(SapphireCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            EmeraldCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/EmeraldCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            EmeraldCrystal.SetPadding(0);
            SetRectangle(EmeraldCrystal, left: 22, top: 9, width: 10, height: 12);

            EmeraldCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/EmeraldCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            EmeraldCrystalSlot2.SetPadding(0);
            SetRectangle(EmeraldCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            RubyCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/RubyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            EmeraldCrystal.SetPadding(0);
            SetRectangle(EmeraldCrystal, left: 22, top: 9, width: 10, height: 12);

            RubyCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/RubyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            RubyCrystalSlot2.SetPadding(0);
            SetRectangle(RubyCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            DiamondCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DiamondCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            DiamondCrystal.SetPadding(0);
            SetRectangle(DiamondCrystal, left: 22, top: 9, width: 10, height: 12);

            DiamondCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DiamondCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            DiamondCrystalSlot2.SetPadding(0);
            SetRectangle(DiamondCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            MagicCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/MagicCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MagicCrystal.SetPadding(0);
            SetRectangle(MagicCrystal, left: 22, top: 9, width: 10, height: 12);

            MagicCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/MagicCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MagicCrystalSlot2.SetPadding(0);
            SetRectangle(MagicCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            AzuriteCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AzuriteCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AzuriteCrystal.SetPadding(0);
            SetRectangle(AzuriteCrystal, left: 22, top: 9, width: 10, height: 12);

            AzuriteCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AzuriteCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AzuriteCrystalSlot2.SetPadding(0);
            SetRectangle(AzuriteCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            PrimeyeCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/ThePrimeyeCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            PrimeyeCrystal.SetPadding(0);
            SetRectangle(PrimeyeCrystal, left: 22, top: 9, width: 10, height: 12);

            PrimeyeCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/ThePrimeyeCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            PrimeyeCrystalSlot2.SetPadding(0);
            SetRectangle(PrimeyeCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            Ablazed = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AblazedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Ablazed.SetPadding(0);
            SetRectangle(Ablazed, left: 22, top: 9, width: 10, height: 12);

            AblazedSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AblazedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AblazedSlot2.SetPadding(0);
            SetRectangle(AblazedSlot2, left: 61, top: 9, width: 10, height: 12);

            Slimy = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SlimyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Slimy.SetPadding(0);
            SetRectangle(Slimy, left: 22, top: 9, width: 10, height: 12);

            SlimySlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SlimyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SlimySlot2.SetPadding(0);
            SetRectangle(SlimySlot2, left: 61, top: 9, width: 10, height: 12);

            Terra = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TerraCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Terra.SetPadding(0);
            SetRectangle(Terra, left: 22, top: 9, width: 10, height: 12);

            TerraSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TerraCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            TerraSlot2.SetPadding(0);
            SetRectangle(TerraSlot2, left: 61, top: 9, width: 10, height: 12);

            Bloodstained = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/BloodstainedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Bloodstained.SetPadding(0);
            SetRectangle(Bloodstained, left: 22, top: 9, width: 10, height: 12);

            BloodstainedSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/BloodstainedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            BloodstainedSlot2.SetPadding(0);
            SetRectangle(BloodstainedSlot2, left: 61, top: 9, width: 10, height: 12);

            Darkened = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DarkenedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Darkened.SetPadding(0);
            SetRectangle(Darkened, left: 22, top: 9, width: 10, height: 12);

            DarkenedSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DarkenedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            DarkenedSlot2.SetPadding(0);
            SetRectangle(DarkenedSlot2, left: 61, top: 9, width: 10, height: 12);

            Spectre = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SpectreCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Spectre.SetPadding(0);
            SetRectangle(Spectre, left: 22, top: 9, width: 10, height: 12);

            SpectreSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SpectreCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SpectreSlot2.SetPadding(0);
            SetRectangle(SpectreSlot2, left: 61, top: 9, width: 10, height: 12);

            Corr = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/CorrosiveJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Corr.SetPadding(0);
            SetRectangle(Corr, left: 22, top: 9, width: 10, height: 12);

            Corr2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/CorrosiveJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Corr2.SetPadding(0);
            SetRectangle(Corr2, left: 61, top: 9, width: 10, height: 12);

            Ony = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/OnyxJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Ony.SetPadding(0);
            SetRectangle(Ony, left: 22, top: 9, width: 10, height: 12);

            Ony2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/OnyxJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Ony2.SetPadding(0);
            SetRectangle(Ony2, left: 61, top: 9, width: 10, height: 12);
            #endregion

            text = new UIText("", 0.8f); // text to show stat
            text.Width.Set(46, 0f);
            text.Height.Set(22, 0f);
            text.Top.Set(22, 0f);
            text.Left.Set(-38, 0f);

            item = new CrystalSlots();
            SetRectangle(item, left: 1, top: 50, width: 52, height: 150);

            item2 = new CrystalSlots2();
            SetRectangle(item2, left: 39, top: 50, width: 52, height: 150);

            Asset<Texture2D> CrystalButtonn = ModContent.Request<Texture2D>("PenumbraMod/Content/DamageClasses/ReaperClassBarButton");
            ExampleUIHoverImageButton CrystalButton = new ExampleUIHoverImageButton(CrystalButtonn, "Jewels");
            SetRectangle(CrystalButton, left: 42, top: 7.5f, width: 10f, height: 13f);
            CrystalButton.OnLeftClick += new MouseEvent(ButtonClicked);

            gradientA = new Color(199, 60, 10); // A orange
            gradientB = new Color(134, 15, 46); // A dark pink

            panel.Append(item);
            panel.Append(item2);
            panel.Append(text);
            panel.Append(barFrame);
            panel.Append(barFrameHover);
            panel.Append(barSlots);
            panel.Append(barSlots2);
            panel.Append(barSlots3);
            panel.Append(barSlots4);
            panel.Append(barDrawing);
            panel.Append(CrystalButton);
            
            #region Crystalsss
            panel.Append(AmethystCrystal);
            panel.Append(AmethystCrystalSlot2);
            panel.Append(TopazCrystal);
            panel.Append(TopazCrystalSlot2);
            panel.Append(SapphireCrystal);
            panel.Append(SapphireCrystalSlot2);
            panel.Append(EmeraldCrystal);
            panel.Append(EmeraldCrystalSlot2);
            panel.Append(RubyCrystal);
            panel.Append(RubyCrystalSlot2);
            panel.Append(DiamondCrystal);
            panel.Append(DiamondCrystalSlot2);
            panel.Append(MagicCrystal);
            panel.Append(MagicCrystalSlot2);
            panel.Append(AzuriteCrystal);
            panel.Append(AzuriteCrystalSlot2);
            panel.Append(PrimeyeCrystal);
            panel.Append(PrimeyeCrystalSlot2);
            panel.Append(Spectre);
            panel.Append(SpectreSlot2);
            panel.Append(Slimy);
            panel.Append(SlimySlot2);
            panel.Append(Bloodstained);
            panel.Append(BloodstainedSlot2);
            panel.Append(Darkened);
            panel.Append(DarkenedSlot2);
            panel.Append(Terra);
            panel.Append(TerraSlot2);
            panel.Append(Ablazed);
            panel.Append(AblazedSlot2);
            panel.Append(Corr);
            panel.Append(Corr2);
            panel.Append(Ony);
            panel.Append(Ony2);
            #endregion
            Append(panel);
        }
        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
        private void ButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (clickedagain)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                clickedagain = false;
                Main.playerInventory = false;
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                clickedagain = true;
                Main.playerInventory = true;
            }
        }
        #region Crystals
        // 1st slot
        public bool amycryst = false;
        public bool emecryst = false;
        public bool magcryst = false;
        public bool rubycryst = false;
        public bool saphcryst = false;
        public bool diamcryst = false;
        public bool topcryst = false;
        public bool azucryst = false;
        public bool pricryst = false;
        public bool blood = false;
        public bool abla = false;
        public bool terra = false;
        public bool spec = false;
        public bool darke = false;
        public bool slim = false;
        public bool corr = false;
        public bool ony = false;

        //2nd slot
        public bool amycryst2 = false;
        public bool emecryst2 = false;
        public bool magcryst2 = false;
        public bool rubycryst2 = false;
        public bool saphcryst2 = false;
        public bool diamcryst2 = false;
        public bool topcryst2 = false;
        public bool azucryst2 = false;
        public bool pricryst2 = false;
        public bool blood2 = false;
        public bool abla2 = false;
        public bool terra2 = false;
        public bool spec2 = false;
        public bool darke2 = false;
        public bool slim2 = false;
        public bool corr2 = false;
        public bool ony2 = false;
        #endregion
        public override void Draw(SpriteBatch spriteBatch)
        {

            // Another painful part

            if (Main.LocalPlayer.HeldItem.DamageType != GetInstance<ReaperClass>())
                return;
            #region Bar
            if (panel.IsMouseHovering)
            {
                SetRectangle(barFrameHover, left: -1, top: -1, width: 92, height: 23);
            }
            else
            {
                SetRectangle(barFrameHover, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            if (clickedagain && Main.playerInventory && item.Item.type == ItemID.None && item2.Item.type == ItemID.None)
            {
                SetRectangle(barSlots, left: 20, top: 39, width: 52, height: 42);
            }
            else
            {
                SetRectangle(barSlots, left: 2145623, top: 1613446, width: 0, height: 0);
            }
            if (clickedagain && Main.playerInventory && item.Item.type != ItemID.None && item2.Item.type == ItemID.None)
            {
                SetRectangle(barSlots2, left: 20, top: 39, width: 52, height: 42);
            }
            else
            {
                SetRectangle(barSlots2, left: 2145623, top: 1613446, width: 0, height: 0);
            }
            if (clickedagain && Main.playerInventory && item.Item.type == ItemID.None && item2.Item.type != ItemID.None)
            {
                SetRectangle(barSlots3, left: 20, top: 39, width: 52, height: 42);
            }
            else
            {

                SetRectangle(barSlots3, left: 2145623, top: 1613446, width: 0, height: 0);
            }
            if (clickedagain && Main.playerInventory && item.Item.type != ItemID.None && item2.Item.type != ItemID.None)
            {
                SetRectangle(barSlots4, left: 20, top: 39, width: 52, height: 42);
            }
            else
            {
                SetRectangle(barSlots4, left: 2145623, top: 1613446, width: 0, height: 0);
            }
            #endregion
            #region Crystals
            if (item.Item.type == ItemType<AmythestCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst = true;
                amycryst = true;
                SetRectangle(AmethystCrystal, left: 22, top: 9, width: 10, height: 12);
            }
            else
            {
                //  Main.LocalPlayer.ClearBuff(BuffType<AmethystForce>());
                amycryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst = false;
                SetRectangle(AmethystCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<AmythestCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst2 = true;
                amycryst2 = true;
                SetRectangle(AmethystCrystalSlot2, left: 60, top: 9, width: 10, height: 12);
            }
            else
            {
                //  Main.LocalPlayer.ClearBuff(BuffType<AmethystForce>());
                amycryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst2 = false;
                SetRectangle(AmethystCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<TopazCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst = true;
                topcryst = true;
                SetRectangle(TopazCrystal, left: 22, top: 9, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<TopazForce>());
                topcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst = false;
                SetRectangle(TopazCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<TopazCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst2 = true;
                topcryst2 = true;
                SetRectangle(TopazCrystalSlot2, left: 60, top: 9, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<TopazForce>());
                topcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst2 = false;
                SetRectangle(TopazCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<SapphireCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst = true;
                saphcryst = true;
                SetRectangle(SapphireCrystal, left: 22, top: 9, width: 10, height: 12);
            }
            else
            {
                //  Main.LocalPlayer.ClearBuff(BuffType<SapphireForce>());
                saphcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst = false;
                SetRectangle(SapphireCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<SapphireCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst2 = true;
                saphcryst2 = true;
                SetRectangle(SapphireCrystalSlot2, left: 60, top: 9, width: 10, height: 12);
            }
            else
            {
                //   Main.LocalPlayer.ClearBuff(BuffType<SapphireForce>());
                saphcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst2 = false;
                SetRectangle(SapphireCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<EmeraldCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst = true;
                emecryst = true;
                SetRectangle(EmeraldCrystal, left: 22, top: 9, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<EmeraldForce>());
                emecryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst = false;
                SetRectangle(EmeraldCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<EmeraldCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst2 = true;
                emecryst2 = true;
                SetRectangle(EmeraldCrystalSlot2, left: 60, top: 9, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<EmeraldForce>());
                emecryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst2 = false;
                SetRectangle(EmeraldCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<RubyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst = true;
                rubycryst = true;
                SetRectangle(RubyCrystal, left: 22, top: 9, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<RubyForce>());
                rubycryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst = false;
                SetRectangle(RubyCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<RubyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst2 = true;
                rubycryst2 = true;
                SetRectangle(RubyCrystalSlot2, left: 60, top: 9, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<RubyForce>());
                rubycryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst2 = false;
                SetRectangle(RubyCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<DiamondCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst = true;
                diamcryst = true;
                SetRectangle(DiamondCrystal, left: 22, top: 9, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<DiamondForce>());
                diamcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst = false;
                SetRectangle(DiamondCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<DiamondCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst2 = true;
                diamcryst2 = true;
                SetRectangle(DiamondCrystalSlot2, left: 60, top: 9, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<DiamondForce>());
                diamcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst2 = false;
                SetRectangle(DiamondCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<MagicCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst = true;
                magcryst = true;
                SetRectangle(MagicCrystal, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<MagicForce>());
                magcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst = false;
                SetRectangle(MagicCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<MagicCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst2 = true;
                magcryst2 = true;
                SetRectangle(MagicCrystalSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<MagicForce>());
                magcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst2 = false;
                SetRectangle(MagicCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<AzuriteCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst = true;
                azucryst = true;
                SetRectangle(AzuriteCrystal, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<AzuriteForce>());
                azucryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst = false;
                SetRectangle(AzuriteCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<AzuriteCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst2 = true;
                azucryst2 = true;
                SetRectangle(AzuriteCrystalSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<AzuriteForce>());
                azucryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst2 = false;
                SetRectangle(AzuriteCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<ThePrimeyeCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst = true;
                pricryst = true;
                SetRectangle(PrimeyeCrystal, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<PrimeyeForce>());
                pricryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst = false;
                SetRectangle(PrimeyeCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<ThePrimeyeCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst2 = true;
                pricryst2 = true;
                SetRectangle(PrimeyeCrystalSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<PrimeyeForce>());
                pricryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst2 = false;
                SetRectangle(PrimeyeCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<AblazedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla = true;
                abla = true;
                SetRectangle(Ablazed, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                abla = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla = false;
                SetRectangle(Ablazed, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<AblazedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla2 = true;
                abla2 = true;
                SetRectangle(AblazedSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                abla2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla2 = false;
                SetRectangle(AblazedSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<SlimyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim = true;
                slim = true;
                SetRectangle(Slimy, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                slim = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim = false;
                SetRectangle(Slimy, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<SlimyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim2 = true;
                slim2 = true;
                SetRectangle(SlimySlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                slim2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim2 = false;
                SetRectangle(SlimySlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<TerraCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra = true;
                terra = true;
                SetRectangle(Terra, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                terra = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra = false;
                SetRectangle(Terra, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<TerraCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra2 = true;
                terra2 = true;
                SetRectangle(TerraSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                terra2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra2 = false;
                SetRectangle(TerraSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<BloodstainedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood = true;
                blood = true;
                SetRectangle(Bloodstained, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                blood = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood = false;
                SetRectangle(Bloodstained, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<BloodstainedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood2 = true;
                blood2 = true;
                SetRectangle(BloodstainedSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                blood2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood2 = false;
                SetRectangle(BloodstainedSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<DarkenedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke = true;
                darke = true;
                SetRectangle(Darkened, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                darke = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke = false;
                SetRectangle(Darkened, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<DarkenedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke = true;
                darke2 = true;
                SetRectangle(DarkenedSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                darke2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke2 = false;
                SetRectangle(DarkenedSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<SpectreCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec = true;
                spec = true;
                SetRectangle(Spectre, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                spec = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec = false;
                SetRectangle(Spectre, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<SpectreCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec2 = true;
                spec2 = true;
                SetRectangle(SpectreSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                spec2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec2 = false;
                SetRectangle(SpectreSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<CorrosiveJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr = true;
                corr = true;
                SetRectangle(Corr, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                corr = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr = false;
                SetRectangle(Corr, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<CorrosiveJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr2 = true;
                corr2 = true;
                SetRectangle(Corr2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                corr2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr2 = false;
                SetRectangle(Corr2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<OnyxJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony = true;
                ony = true;
                SetRectangle(Ony, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                ony = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony = false;
                SetRectangle(Ony, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item2.Item.type == ItemType<OnyxJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony2 = true;
                ony2 = true;
                SetRectangle(Ony2, left: 20, top: 8, width: 10, height: 12);
            }
            else
            {
                ony2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony2 = false;
                SetRectangle(Ony2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            #endregion
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var ReaperClassPlayer = Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>();
            // Calculate quotient
            float quotient = (float)ReaperClassPlayer.ReaperEnergy / ReaperClassPlayer.ReaperEnergyMax; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
            quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = barDrawing.GetInnerDimensions().ToRectangle();
            hitbox.X += 1;
            hitbox.Width += 1;
            hitbox.Y += 1;
            hitbox.Height += 1;

            // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                //float percent = (float)i / steps; // Alternate Gradient Approach
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i + 2, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.HeldItem.DamageType != GetInstance<ReaperClass>())
                return;
            var ReaperClassPlayer = Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>();
            // Setting the text per tick to update and show our resource values.
            float b = (float)ReaperClassPlayer.ReaperEnergy / 20f;
            if (GetInstance<PenumbraConfig>().UITEXT)
                text.SetText($"Reaper Energy {ReaperClassPlayer.ReaperEnergy} / {ReaperClassPlayer.ReaperEnergyMax}");
            base.Update(gameTime);
        }
    }
    internal class CrystalSlots : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public CrystalSlots(int context = ItemSlot.Context.BankItem, float scale = 0f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults();
            Width.Set(52, 0f);
            Height.Set(150, 0f);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            if (ReaperUI.clickedagain && Main.playerInventory)
            {
                ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
                spriteBatch.Draw(TextureAssets.Item[Item.type].Value, rectangle.TopLeft() + new Vector2(17, 6), Color.White);
            }

            Main.inventoryScale = oldScale;

        }
    }
    internal class CrystalSlots2 : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public CrystalSlots2(int context = ItemSlot.Context.BankItem, float scale = 0f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(0);
            Width.Set(52, 0f);
            Height.Set(150, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            if (ReaperUI.clickedagain && Main.playerInventory)
            {
                ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
                spriteBatch.Draw(TextureAssets.Item[Item.type].Value, rectangle.TopLeft() + new Vector2(20, 6), Color.White);
            }

            Main.inventoryScale = oldScale;

        }
    }
    public class DraggableUI : UIPanel
    {
        // Stores the offset from the top left of the UIPanel while dragging
        private Vector2 offset;
        // A flag that checks if the panel is currently being dragged
        private bool dragging;

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.LeftMouseDown(evt);
            // When the mouse button is down, then we start dragging
            DragStart(evt);
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            // When the mouse button is up, then we stop dragging
            DragEnd(evt);
        }

        private void DragStart(UIMouseEvent evt)
        {
            // The offset variable helps to remember the position of the panel relative to the mouse position
            // So no matter where you start dragging the panel, it will move smoothly
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 endMousePosition = evt.MousePosition;
            dragging = false;

            Left.Set(endMousePosition.X - offset.X, 0f);
            Top.Set(endMousePosition.Y - offset.Y, 0f);

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Checking ContainsPoint and then setting mouseInterface to true is very common
            // This causes clicks on this UIElement to not cause the player to use current items
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            // Here we check if the DragableUIPanel is outside the Parent UIElement rectangle
            // (In our example, the parent would be ExampleCoinsUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
            // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                // Recalculate forces the UI system to do the positioning math again.
                Recalculate();
            }
        }
    }

}
