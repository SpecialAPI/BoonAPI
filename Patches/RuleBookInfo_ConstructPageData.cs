using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace BoonAPI.Patches
{
    [HarmonyPatch(typeof(RuleBookInfo), "ConstructPageData")]
    public class RuleBookInfo_ConstructPageData
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<RuleBookPageInfo> __result, RuleBookInfo __instance, AbilityMetaCategory metaCategory)
        {
            if(NewBoon.boons.Count > 0)
            {
                foreach (PageRangeInfo info in __instance.pageRanges)
                {
                    if(info.type == PageRangeType.Boons)
                    {
                        List<int> customBoons = NewBoon.boons.Select(x => (int)x.boon.type).ToList();
                        int min = customBoons.AsQueryable().Min();
                        int max = customBoons.AsQueryable().Max();
                        List<RuleBookPageInfo> infos = __instance.ConstructPages(info, max + 1, min, (int index) => metaCategory == AbilityMetaCategory.Part1Rulebook && BoonsUtil.GetData((BoonData.Type)index).icon != null && customBoons.Contains(index) &&
                            (NewBoon.boons.Find((x) => (int)x.boon.type == index)?.appearInRulebook).GetValueOrDefault(),
                            new Action<RuleBookPageInfo, PageRangeInfo, int>(__instance.FillBoonPage), Localization.Translate("APPENDIX XII, SUBSECTION VIII - BOONS {0}"));
                        __result.AddRange(infos);
                    }
                }
            }
        }
    }
}
