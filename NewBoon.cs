using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BoonAPI
{
    public class NewBoon
    {
        public static List<NewBoon> boons = new List<NewBoon>();
        public BoonData boon;
        public Type boonHandlerType;
        public bool appearInRulebook;
        public bool stacks;

        public NewBoon(string name, Type boonHandlerType, string rulebookDescription, Texture icon, Texture cardArt, bool stackable = true, bool appearInLeshyTrials = true, bool appearInRulebook = true)
        {
            BoonData data = ScriptableObject.CreateInstance<BoonData>();
            data.name = name;
            data.displayedName = name;
            data.description = rulebookDescription;
            data.icon = icon;
            data.cardArt = cardArt;
            data.minorEffect = !appearInLeshyTrials;
            data.type = BoonData.Type.NUM_TYPES + boons.Count + 1;
            this.appearInRulebook = appearInRulebook;
            boon = data;
            if(boonHandlerType != null)
            {
                if (!boonHandlerType.IsSubclassOf(typeof(BoonBehavior)))
                {
                    boonHandlerType = typeof(BoonBehavior);
                }
                this.boonHandlerType = boonHandlerType;
            }
            stacks = stackable;
            boons.Add(this);
        }
    }
}
