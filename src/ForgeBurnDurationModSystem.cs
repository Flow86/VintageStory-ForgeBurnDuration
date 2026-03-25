using HarmonyLib;
using Vintagestory.API.Common;

[assembly: ModInfo("Forge Burn Duration", "forgeburnduration",
                    Authors = new string[] { "Flow86" },
                    Description = "Changes the Forge to take the burn duration of the fuel into account",
                    Version = "1.0.0")]

namespace Flow86.ForgeBurnDuration
{
    public class ForgeBurnDurationModSystem : ModSystem
    {
        private Harmony? harmony = null;

        public override void Start(ICoreAPI api)
        {
            harmony = new Harmony(Mod.Info.ModID);
            harmony.PatchAll();

            api.Logger.Notification("[ForgeBurnDuration] Started " + api.Side);
        }

        public override void Dispose()
        {
            harmony?.UnpatchAll(Mod.Info.ModID);

            base.Dispose();
        }
    }
}
