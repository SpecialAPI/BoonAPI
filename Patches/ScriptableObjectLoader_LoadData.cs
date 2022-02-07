using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using HarmonyLib;

namespace BoonAPI.Patches
{
    [HarmonyPatch(typeof(LoadingScreenManager), "LoadGameData")]
    public class LoadingScreenManager_LoadGameData
    {
        [HarmonyPostfix]
        public static void Prefix()
        {
            if(ScriptableObjectLoader<BoonData>.AllData != null)
            {
                foreach(NewBoon nb in NewBoon.boons)
                {
                    if(nb != null && nb.boon != null && !ScriptableObjectLoader<BoonData>.AllData.Contains(nb.boon))
                    {
                        ScriptableObjectLoader<BoonData>.AllData.Add(nb.boon);
                    }
                }
            }
        }
    }
}
