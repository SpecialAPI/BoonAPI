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
			BoonBehaviour.DestroyAllInstances();
			if (__instance.BoonsEnabled && RunState.Run != null && RunState.Run.playerDeck != null && RunState.Run.playerDeck.Boons != null && NewBoon.boons != null)
			{
				foreach (BoonData boon in RunState.Run.playerDeck.Boons)
				{
					if (boon != null)
					{
						NewBoon nb = NewBoon.boons.Find((x) => x.boon.type == boon.type);
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
								if (behav.RespondToPreBoonActivation())
								{
									yield return behav.OnPreBoonActivation();
								}
							}
						}
					}
				}
			}
			yield return result;
			foreach(BoonBehaviour bb in BoonBehaviour.Instances)
            {
				if(bb != null && bb.RespondToPostBoonActivation())
                {
					yield return bb.OnPostBoonActivation();
                }
            }
			yield break;
		}
	}
}
