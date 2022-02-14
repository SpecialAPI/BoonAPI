using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DiskCardGame;
using HarmonyLib;

namespace BoonAPI.Patches
{
    [HarmonyPatch(typeof(DeckInfo), "AddBoon")]
    public class DeckInfo_AddBoon
    {
        [HarmonyPostfix]
        public static void Postfix(BoonData.Type boonType)
        {
            if(TurnManager.Instance != null && !TurnManager.Instance.GameEnded && !TurnManager.Instance.GameEnding && !TurnManager.Instance.IsSetupPhase && TurnManager.Instance.Opponent != null)
            {
                NewBoon nb = NewBoon.boons.Find((x) => x.boon.type == boonType);
                if (nb != null && nb.boonHandlerType != null && (nb.stacks || BoonBehaviour.CountInstancesOfType(nb.boon.type) < 1))
                {
                    int instances = BoonBehaviour.CountInstancesOfType(nb.boon.type);
                    GameObject boonhandler = new GameObject(nb.boon.name + " Boon Handler");
                    BoonBehaviour behav = boonhandler.AddComponent(nb.boonHandlerType) as BoonBehaviour;
                    if (behav != null)
                    {
                        GlobalTriggerHandler.Instance?.RegisterNonCardReceiver(behav);
                        behav.boon = nb;
                        behav.instanceNumber = instances + 1;
                        BoonBehaviour.Instances.Add(behav);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(DeckInfo), "ClearBoons")]
    public class DeckInfo_ClearBoons
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            BoonBehaviour.DestroyAllInstances();
        }
    }

    [HarmonyPatch(typeof(DeckInfo), "get_Boons")]
    public class DeckInfo_get_Boons
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<BoonData> __result, DeckInfo __instance)
        {
            if (__instance.boons != null && __instance.boonIds != null && __instance.boons.Count != __instance.boonIds.Count)
            {
                __instance.LoadBoons();
            }
            __result = __instance.boons;
        }
    }

    [HarmonyPatch(typeof(DeckInfo), "LoadBoons")]
    public class DeckInfo_LoadBoons
    {
        [HarmonyPostfix]
        public static void Postfix(DeckInfo __instance)
        {
            __instance.boons.RemoveAll((x) => x == null);
        }
    }
}
