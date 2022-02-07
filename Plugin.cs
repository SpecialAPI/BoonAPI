using BepInEx;
using HarmonyLib;
using System;
using DiskCardGame;
using System.Collections.Generic;
using System.Text;

namespace BoonAPI
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "spapi.inscryption.boonapi";
        public const string NAME = "BoonAPI";
        public const string VERSION = "1.0.0";

        public void Awake()
        {
            Harmony harm = new Harmony(GUID);
            harm.PatchAll();
        }

        public void Start()
        {
            if (ScriptableObjectLoader<BoonData>.AllData != null)
            {
                foreach (NewBoon nb in NewBoon.boons)
                {
                    if (nb != null && nb.boon != null && !ScriptableObjectLoader<BoonData>.AllData.Contains(nb.boon))
                    {
                        ScriptableObjectLoader<BoonData>.AllData.Add(nb.boon);
                    }
                }
            }
        }
    }
}
