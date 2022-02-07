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
			yield return result;
			BoonBehavior.DestroyAllInstances();
			yield break;
		}
	}
}
