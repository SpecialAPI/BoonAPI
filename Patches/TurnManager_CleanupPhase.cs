using System.Text;
using HarmonyLib;
using DiskCardGame;
using UnityEngine;
using System.Collections;

namespace BoonAPI.Patches
{
    [HarmonyPatch(typeof(TurnManager), "CleanupPhase")]
    public class TurnManager_CleanupPhase
    {
		public static IEnumerator Postfix(IEnumerator result)
		{
			foreach (BoonBehaviour bb in BoonBehaviour.Instances)
			{
				if (bb != null && bb.RespondToPreBattleCleanup())
				{
					yield return bb.OnPreBattleCleanup();
				}
			}
			yield return result;
			foreach (BoonBehaviour bb in BoonBehaviour.Instances)
			{
				if (bb != null && bb.RespondToPostBattleCleanup())
				{
					yield return bb.OnPostBattleCleanup();
				}
			}
			BoonBehaviour.DestroyAllInstances();
			yield break;
		}
	}
}
