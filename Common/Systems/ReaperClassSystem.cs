using PenumbraMod.Common.Players;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Common.Systems
{
    // Acts as a container for keybinds registered by this mod.
    // See Common/Players/ExampleKeybindPlayer for usage.
    public class ReaperClassSystem : ModSystem
    {
        public static ModKeybind ReaperClassKeybind { get; private set; }

        public override void Load()
        {
            // Registers a new keybind
            ReaperClassKeybind = KeybindLoader.RegisterKeybind(Mod, "Reaper energy hability", "F");
        }

        // Please see ExampleMod.cs' Unload() method for a detailed explanation of the unloading process.
        public override void Unload()
        {
            // Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
            ReaperClassKeybind = null;
        }
    }
}
