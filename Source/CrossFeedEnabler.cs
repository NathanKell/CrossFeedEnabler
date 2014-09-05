using System;
using System.Collections.Generic;
using UnityEngine;
using KSP;

namespace CrossFeedEnabler
{
    public class ModuleCrossFeed : PartModule
    {
        // from ialdabaoth / DRE
        UIPartActionWindow _myWindow = null;
        UIPartActionWindow myWindow
        {
            get
            {
                if (_myWindow == null)
                {
                    foreach (UIPartActionWindow window in FindObjectsOfType(typeof(UIPartActionWindow)))
                    {
                        if (window.part == part) _myWindow = window;
                    }
                }
                return _myWindow;
            }
        }

        public Part parentPart = null;
        [KSPField(isPersistant = true)]
        public bool crossFeedOverride = true;

        [KSPField]
        public bool actionVisible = true;

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "Crossfeed is On")]
        public void ToggleCrossFeed()
        {
            crossFeedOverride = !crossFeedOverride;
            UpdateCrossFeed();
        }

        public void UpdateCrossFeed()
        {
            if (part.fuelLookupTargets != null && (object)(part.parent) != null && part.parent.fuelLookupTargets != null)
            {
                if (crossFeedOverride)
                {
                    if (!part.parent.fuelLookupTargets.Contains(part))
                        part.parent.fuelLookupTargets.Add(part);
                    if (!part.fuelLookupTargets.Contains(part.parent))
                        part.fuelLookupTargets.Add(part.parent);
                    Events["ToggleCrossFeed"].guiName = "Crossfeed is On";
                }
                else
                {
                    part.parent.fuelLookupTargets.Remove(part);
                    part.fuelLookupTargets.Remove(part.parent);
                    Events["ToggleCrossFeed"].guiName = "Crossfeed is Off";
                }
                if ((object)myWindow != null)
                    myWindow.displayDirty = true;
            }
        }
        // belt-and-suspenders: do this everywhere and everywhen.
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            UpdateCrossFeed();
            Events["ToggleCrossFeed"].guiActive = actionVisible;
            Events["ToggleCrossFeed"].guiActiveEditor = actionVisible;
        }

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
            UpdateCrossFeed();
            Events["ToggleCrossFeed"].guiActive = actionVisible;
            Events["ToggleCrossFeed"].guiActiveEditor = actionVisible;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            UpdateCrossFeed();
            Events["ToggleCrossFeed"].guiActive = actionVisible;
            Events["ToggleCrossFeed"].guiActiveEditor = actionVisible;
        }
        public virtual void Update()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                if (parentPart != part.parent)
                {
                    if ((object)parentPart != null && parentPart.fuelLookupTargets != null)
                    {
                        parentPart.fuelLookupTargets.Remove(part);
                        part.fuelLookupTargets.Remove(parentPart);
                    }
                    UpdateCrossFeed();
                    parentPart = part.parent;
                }
                else if(crossFeedOverride && !part.parent.fuelLookupTargets.Contains(part))
                    UpdateCrossFeed();
            }
        }

        public void OnDestroy()
        {
            if ((object)(part.parent) != null && part.parent.fuelLookupTargets != null)
            {
                part.parent.fuelLookupTargets.Remove(part);
            }
            if ((object)parentPart != null && parentPart.fuelLookupTargets != null)
            {
                parentPart.fuelLookupTargets.Remove(part);
            }
        }
    }
}
