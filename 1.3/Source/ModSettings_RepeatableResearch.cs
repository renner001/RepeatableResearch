using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DanielRenner.RepeatableResearch
{
    class ModSettings_RepeatableResearch : ModSettings
    {
        public bool enabledResearchGlobalWorkSpeed = true;

        public override void ExposeData()
        {
            base.ExposeData();

            // all scribes...

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                Log.Debug("ModSettings_RepeatableResearch.ExposeData() post load init called")
                if (DefDatabase<ResearchProjectDef>.AllDefs.Contains(DefOfs_RepeatableResearch.RepeatablePermanentGlobalWorkSpeed) && !this.enabledResearchGlobalWorkSpeed)
                {
                    Log.Debug("removing research RepeatablePermanentGlobalWorkSpeed from research database");
                    RemovePublic(DefOfs_RepeatableResearch.RepeatablePermanentGlobalWorkSpeed);
                }
                else if (!DefDatabase<ResearchProjectDef>.AllDefs.Contains(DefOfs_RepeatableResearch.RepeatablePermanentGlobalWorkSpeed) && this.enabledResearchGlobalWorkSpeed)
                {
                    Log.Debug("pushing research RepeatablePermanentGlobalWorkSpeed to research database");
                    DefDatabase<ResearchProjectDef>.Add(DefOfs_RepeatableResearch.RepeatablePermanentGlobalWorkSpeed);
                }
            }
                
        }

        public static void RemovePublic(ResearchProjectDef def)
        {
            // get the private method Remove
            Type defDatabaseType = typeof(DefDatabase<ResearchProjectDef>);
            var removeMethod = defDatabaseType.GetMethod("Remove", BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | BindingFlags.InvokeMethod);
            // invoke it!
            removeMethod.Invoke(null, new[] { def });
        }
    }
}
