using System;
using System.Collections.Generic;
using UnityEngine;
using KSP;

namespace CrossFeedEnabler
{
    public class ModuleCrossFeed : PartModule
    {
        // belt-and-suspenders: do this everywhere and everywhen.
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            if (part.parent != null && part.parent.fuelLookupTargets != null)
            {
                if (!part.parent.fuelLookupTargets.Contains(this.part))
                    part.parent.fuelLookupTargets.Add(this.part);
                if (!this.part.fuelLookupTargets.Contains(part.parent))
                    part.fuelLookupTargets.Add(part.parent);
            }
        }

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
            if (part.parent != null && part.parent.fuelLookupTargets != null)
            {
                if (!part.parent.fuelLookupTargets.Contains(this.part))
                    part.parent.fuelLookupTargets.Add(this.part);
                if (!this.part.fuelLookupTargets.Contains(part.parent))
                    part.fuelLookupTargets.Add(part.parent);
            }
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            if (part.parent != null && part.parent.fuelLookupTargets != null)
            {
                if (!part.parent.fuelLookupTargets.Contains(this.part))
                    part.parent.fuelLookupTargets.Add(this.part);
                if (!this.part.fuelLookupTargets.Contains(part.parent))
                    part.fuelLookupTargets.Add(part.parent);
            }
        }

        public void OnDestroy()
        {
            if (part.parent != null && part.parent.fuelLookupTargets != null)
            {
                if (part.parent.fuelLookupTargets.Contains(this.part))
                    part.parent.fuelLookupTargets.Remove(this.part);
            }
        }
    }
}
