﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace DanielRenner.RepeatableResearch
{
    [StaticConstructorOnStartup]
    public class Mod_RepeatableResearch : Mod
    {
        static Mod_RepeatableResearch()
        {
            Verse.Log.Message("Mod 'Repeatable Research': loaded");
#if DEBUG
            Harmony.DEBUG = true;
#endif
            Harmony harmony = new Harmony("DanielRenner.RepeatableResearch");
            harmony.PatchAll();
        }

        public Mod_RepeatableResearch(ModContentPack mcp) : base(mcp)
        {
            LongEventHandler.ExecuteWhenFinished(() => { 
				base.GetSettings<ModSettings_RepeatableResearch>(); 
			});
        }


		public override void WriteSettings()
		{
			base.WriteSettings();
		}


		public override string SettingsCategory()
		{
			return Translations_RepeatableResearch.SettingsPanelName;
		}


		public override void DoSettingsWindowContents(Rect rect)
		{
			Listing_Standard list = new Listing_Standard()
			{
				ColumnWidth = rect.width
			};

			Rect topRect = rect.TopPart(0.25f);
			Rect bottomRect = rect.BottomPart(0.75f);
			Rect fullRect = list.GetRect(Text.LineHeight).ContractedBy(4);
			Rect leftRect = fullRect.LeftHalf().Rounded();
			Rect rightRect = fullRect.RightHalf().Rounded();

			list.Begin(rect);

			/*
			list.QuarryHealthSetting();
			Listing_Standard listtop = list.BeginSection(topRect.height, true);
			{
				listtop.ColumnWidth = rect.width / 2;
				listtop.CheckboxLabeled(Static.LetterSent, ref QuarrySettings.letterSent, Static.ToolTipLetter);
				listtop.CheckboxLabeled(Static.AllowRottable, ref QuarrySettings.allowRottable, Static.ToolTipAllowRottable);
				listtop.NewColumn();
				listtop.LabeledScrollbarSetting("QRY_SettingsJunkChance".Translate(QuarrySettings.junkChance), ref QuarrySettings.junkChance, Static.ToolTipJunkChance);
				listtop.LabeledScrollbarSetting("QRY_SettingsChunkChance".Translate(QuarrySettings.chunkChance), ref QuarrySettings.chunkChance, Static.ToolTipChunkChance);
				listtop.LabeledScrollbarSetting("QRY_SettingsResourceModifier".Translate(QuarrySettings.resourceModifer * 100), ref QuarrySettings.resourceModifer, Static.ToolTipResourceModifier);
			}
			list.EndSection(listtop);
			*/
			list.End();
			/*
			{

				Rect DictInterfaceRect = bottomRect.TopPart(0.25f).Rounded();
				{
					Rect labelRect = DictInterfaceRect.TopPart(0.75f).Rounded();
					Text.Font = GameFont.Medium;
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(labelRect, Static.LabelDictionary);
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
				}
				{
					Rect buttonsRect = DictInterfaceRect.BottomPart(0.25f).Rounded();
					Rect addRect = buttonsRect.LeftThird();
					Rect rmvRect = buttonsRect.MiddleThird();
					Rect resRect = buttonsRect.RightThird();

					// Add an entry to the dictionary
					if (Widgets.ButtonText(addRect, Static.LabelAddThing))
					{
						List<FloatMenuOption> thingList = new List<FloatMenuOption>();
						foreach (ThingDef current in from t in QuarryUtility.PossibleThingDefs()
													 orderby t.label
													 select t)
						{

							bool skip = false;
							for (int i = 0; i < QuarrySettings.oreDictionary.Count; i++)
							{
								if (QuarrySettings.oreDictionary[i].thingDef == current)
								{
									skip = true;
									break;
								}
							};
							if (skip) continue;

							thingList.Add(new FloatMenuOption(current.LabelCap, delegate {
								QuarrySettings.oreDictionary.Add(new ThingCountExposable(current, 1));
							}));
						}
						FloatMenu menu = new FloatMenu(thingList);
						Find.WindowStack.Add(menu);
					}

					// Remove an entry from the dictionary
					if (Widgets.ButtonText(rmvRect, Static.LabelRemoveThing) && QuarrySettings.oreDictionary.Count >= 2)
					{
						List<FloatMenuOption> thingList = new List<FloatMenuOption>();
						foreach (ThingCountExposable current in from t in QuarrySettings.oreDictionary
																orderby t.thingDef.label
																select t)
						{
							ThingDef localTd = current.thingDef;
							thingList.Add(new FloatMenuOption(localTd.LabelCap, delegate {
								for (int i = 0; i < QuarrySettings.oreDictionary.Count; i++)
								{
									if (QuarrySettings.oreDictionary[i].thingDef == localTd)
									{
										QuarrySettings.oreDictionary.Remove(QuarrySettings.oreDictionary[i]);
										break;
									}
								};
							}));
						}
						FloatMenu menu = new FloatMenu(thingList);
						Find.WindowStack.Add(menu);
					}

					// Reset the dictionary
					if (Widgets.ButtonText(resRect, Static.LabelResetList))
					{
						OreDictionary.Build();
					}
				}

				{
					Rect listRect = bottomRect.BottomPart(0.75f).Rounded();
					Rect cRect = listRect.ContractedBy(10f);
					Rect position = listRect;
					Rect outRect = new Rect(0f, 0f, position.width, position.height);
					Rect viewRect = new Rect(0f, 0f, position.width - 16f, scrollViewHeight);

					float num = 0f;
					List<ThingCountExposable> dict = new List<ThingCountExposable>(QuarrySettings.oreDictionary);

					GUI.BeginGroup(position);
					Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);

					foreach (var tc in dict.Select((value, index) => new { index, value }))
					{
						Rect entryRect = new Rect(0f, num, viewRect.width, 32);
						Rect iconRect = entryRect.LeftPartPixels(32);
						Rect labelRect = new Rect(entryRect.LeftThird().x + 33f, entryRect.y, entryRect.LeftThird().width - 33f, entryRect.height);
						Rect texEntryRect = new Rect(entryRect.LeftHalf().RightPartPixels(103).x, entryRect.y, 60f, entryRect.height);
						Rect pctRect = new Rect(entryRect.LeftHalf().RightPartPixels(41).x, entryRect.y, 40f, entryRect.height);
						Rect sliderRect = new Rect(entryRect.RightHalf().x, entryRect.y, entryRect.RightHalf().width, entryRect.height);

						Widgets.ThingIcon(iconRect, tc.value.thingDef);
						Widgets.Label(labelRect, tc.value.thingDef.LabelCap);
						Widgets.Label(pctRect, $"{QuarrySettings.oreDictionary.WeightAsPercentageOf(tc.value.count).ToStringDecimal()}%");
						int val = tc.value.count;
						string nullString = null;
						Widgets.TextFieldNumeric(
							texEntryRect,
							ref QuarrySettings.oreDictionary[tc.index].count,
							ref nullString,
							0, OreDictionary.MaxWeight);
						val = Widgets.HorizontalSlider(
							sliderRect,
							QuarrySettings.oreDictionary[tc.index].count, 0f, OreDictionary.MaxWeight, true
						).RoundToAsInt(1);
						if (val != QuarrySettings.oreDictionary[tc.index].count)
						{
							QuarrySettings.oreDictionary[tc.index].count = val;
						}

						if (Mouse.IsOver(entryRect))
						{
							Widgets.DrawHighlight(entryRect);
						}
						TooltipHandler.TipRegion(entryRect.LeftThird(), tc.value.thingDef.description);

						num += 32f;
						scrollViewHeight = num;
					}

					Widgets.EndScrollView();
					GUI.EndGroup();
				}
			
		}*/
		}
	}
}
