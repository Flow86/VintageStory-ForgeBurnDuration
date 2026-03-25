using HarmonyLib;
using Microsoft.VisualBasic;
using System;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Flow86.ForgeBurnDuration
{
    [HarmonyPatch(typeof(BlockEntityForge), "OnCommonTick")]
    public class OnCommonTickPatch
    {
        static bool Prefix(BlockEntityForge __instance, float dt, ref bool ___burning, ref float ___fuelLevel, ref double ___lastTickTotalHours)
        {
            if (___burning)
            {
                double num = __instance.Api.World.Calendar.TotalHours - ___lastTickTotalHours;
                if (___fuelLevel > 0f)
                {
                    ___fuelLevel = Math.Max(0f, ___fuelLevel - (float)(5.0 / (4.0 * 48.0) * num)); // longer * 4.0
                }

                if (___fuelLevel <= 0f)
                {
                    ___burning = false;
                }

                if (__instance.Contents != null)
                {
                    float temperature = __instance.Contents.Collectible.GetTemperature(__instance.Api.World, __instance.Contents);
                    if (temperature < 1100f)
                    {
                        float num2 = (float)(num * 1500.0 * 2.0); // faster * 2.0
                        __instance.Contents.Collectible.SetTemperature(__instance.Api.World, __instance.Contents, Math.Min(1100f, temperature + num2));
                    }
                }
            }

            ___lastTickTotalHours = __instance.Api.World.Calendar.TotalHours;

            // do not run original method
            return false;
        }
    }

    //[HarmonyPatch(typeof(BlockEntityForge), "OnPlayerInteract")]
    //public class BlockEntityForgePatch
    //{
    //    static readonly FieldInfo fuelLevelField = AccessTools.Field(typeof(BlockEntityForge), "fuelLevel");
    //
    //    static bool Prefix(BlockEntityForge __instance, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
    //    {
    //        ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
    //
    //        // Nur SHIFT + Brennstoff → Fuel hinzufügen
    //        if (!byPlayer.Entity.Controls.ShiftKey)
    //            return true;
    //
    //        if (slot.Itemstack == null)
    //            return false;
    //
    //        var comb = slot.Itemstack.Collectible.CombustibleProps;
    //        if (comb == null || comb.BurnTemperature <= 1000)
    //            return true;
    //
    //        // Basiswerte: Vanilla-Charcoal
    //        float baseFuel = 0.0625f;
    //        float baseBurn = 40f;
    //
    //        float factor = comb.BurnDuration / baseBurn;
    //        float fuelGain = baseFuel * factor;
    //
    //        // Basiswerte: Vanilla Maximum * 8
    //        float maxFuelLevel = 0.3125f * 8;
    //
    //        float currentFuel = (float)fuelLevelField.GetValue(__instance);
    //        if (currentFuel + fuelGain > maxFuelLevel)
    //            return false;
    //
    //        // FuelLevel setzen
    //        fuelLevelField.SetValue(__instance, currentFuel + fuelGain);
    //
    //        // Vanilla-Sound
    //        if (slot.Itemstack.Collectible is ItemCoal || slot.Itemstack.Collectible is ItemOre)
    //        {
    //            world.PlaySoundAt(new AssetLocation("sounds/block/charcoal"), byPlayer, byPlayer, true, 16f, 1f);
    //        }
    //
    //        // Item verbrauchen
    //        if (byPlayer.WorldData.CurrentGameMode == EnumGameMode.Survival)
    //        {
    //            slot.TakeOut(1);
    //            slot.MarkDirty();
    //        }
    //
    //        __instance.MarkDirty(false, null);
    //
    //        return false;
    //    }
    //}
}
