using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using DiskCardGame;
using UnityEngine;
using System.Collections;

namespace BoonAPI.Patches
{
	[HarmonyPatch(typeof(BoonsHandler), "ActivatePreCombatBoons")]
	public class BoonsHandler_ActivatePreCombatBoons
	{
		public static IEnumerator Postfix(IEnumerator result, BoonsHandler __instance)
        {
            yield return result;
			BoonBehavior.DestroyAllInstances();
            if (__instance.BoonsEnabled && RunState.Run != null && RunState.Run.playerDeck != null && RunState.Run.playerDeck.Boons != null && NewBoon.boons != null)
			{
				foreach (BoonData boon in RunState.Run.playerDeck.Boons)
				{
					if (boon != null)
					{
						NewBoon nb = NewBoon.boons.Find((x) => x.boon.type == boon.type);
						if(nb != null && nb.boonHandlerType != null && (nb.stacks || BoonBehavior.CountInstancesOfType(nb.boon.type) < 1))
                        {
							GameObject boonhandler = new GameObject(nb.boon.name + " Boon Handler");
							BoonBehavior behav = boonhandler.AddComponent(nb.boonHandlerType) as BoonBehavior;
							if(behav != null)
                            {
								GlobalTriggerHandler.Instance?.RegisterNonCardReceiver(behav);
                                behav.boon = nb;
								BoonBehavior.Instances.Add(behav);
                                if (behav.RespondToBattleStart())
                                {
									yield return behav.OnBattleStart();
                                }
                            }
                        }
					}
				}
			}
			yield break;
		}
	}
}
