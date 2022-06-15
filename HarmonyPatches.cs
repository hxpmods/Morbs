using HarmonyLib;
using PeglinUI.MainMenu;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using PeglinUI;
using I2.Loc;
using Relics;
using PeglinRelicLib.Register;
using PeglinUI.OrbDisplay;
using PixelCrushers.DialogueSystem;
using RNG.Scenarios;
using PixelCrushers.DialogueSystem.I2Support;
using Worldmap;
using Morbs.OrbBehaviours;
using Morbs.Factories;
using Morbs.Utility;

namespace Morbs
{
    [HarmonyPatch(typeof(MainMenuRandomOrbDrop), "Awake")]
    public static class MainMenuOrbsPatch
    {
        private static bool hasInjected = false;
        //It seems we can inject orbs into the pool whenever it exists, so we do it at the earliest convenience here

        public delegate void OrbPoolSpawned();
        public static OrbPoolSpawned OnPostOrbPoolSpawned = delegate { };
        public static OrbPoolSpawned OnPreOrbPoolSpawned = delegate { };
        public static OrbPoolSpawned OnRegisterOrbs = delegate { };

        static bool Prefix()
        {
            if (!hasInjected)
            {
                //Register content is called here
                OnPreOrbPoolSpawned.Invoke();
                //Here exists so that Content can trigger here, as OnPreOrbPoolSpawned has already passed when the content is registered.
                OnRegisterOrbs.Invoke();

            }
            return true;
        }

        static void Postfix()
        {
            if (!hasInjected)
            {
                OnPostOrbPoolSpawned.Invoke();
                hasInjected = true;
            }
        }
    }

    [HarmonyPatch(typeof(DialogueSystemSceneEvents), "Awake")]
    public static class LogDialogueSystemSceneEventsPatch
    {
        static void Postfix(DialogueSystemSceneEvents __instance)
        {
            if (Plugin.debug)
            {
                foreach (DialogueEntrySceneEvent e in __instance.dialogueEntrySceneEvents)
                {
                    Plugin.logger.LogMessage("Logging dialogueEntrySceneEvent: " + e.guid);

                    for (int i = 0; i < e.onExecute.GetPersistentEventCount(); i++)
                    {
                        string methodName = e.onExecute.GetPersistentMethodName(i);
                        string targetName = e.onExecute.GetPersistentTarget(i).GetType().ToString();
                        Plugin.logger.LogMessage("Executes method: " + targetName + "." + methodName);
                    }
                    Plugin.logger.LogMessage("\n");
                }
            }
        }
    }

[HarmonyPatch(typeof(DialogueDebug),"logInfo",methodType: MethodType.Getter)]
public static class logInfoClass
    {
        static bool Prefix(ref bool __result)
        {
            if (!Plugin.debug) return true;

            __result = true;
            return false;
        }
    }

[HarmonyPatch(typeof(DialogueDebug), "logWarnings", methodType: MethodType.Getter)]
    public static class logWarningsClass
    {
        static bool Prefix(ref bool __result)
        {
            if (!Plugin.debug) return true;

            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(DialogueDebug), "logErrors", methodType: MethodType.Getter)]
    public static class logErrorClass
    {
        static bool Prefix(ref bool __result)
        {
            if (!Plugin.debug) return true;

            __result = true;
            return false;
        }
    }

[HarmonyPatch(typeof(DialogueSystemUseI2Language),  "OnConversationLine")]
public static class PrintDialogueEntryHeadersInConversationPatch
    {
        static void Postfix(Subtitle subtitle, DialogueSystemUseI2Language __instance)
        {
            if (Plugin.debug)
            {
                var entry = subtitle.dialogueEntry;

                var traverse = Traverse.Create(__instance);

                string dialogueEntryHeader = traverse.Method("GetDialogueEntryHeader", entry).GetValue<string>();
                Plugin.logger.LogMessage("Dialogue entry header: " + dialogueEntryHeader);

                //Plugin.logger.LogMessage("Patch called");
                //throw new Exception("OnConversationLineException");
            }
        }
    }

[HarmonyPatch(typeof(DialogueManager), nameof(DialogueManager.StartConversation), argumentTypes: new Type[]{typeof(string)})]

public static class ForceConversationPatch
{
    static string convoTitle = "";
    static bool Prefix(ref string title)
        {
            //Useful for testing new conversations before setting up an event
            if (convoTitle != ""){
                title = convoTitle;
            }
            //title = "Scenario/PeglinMod/TestScenario";
            //title = "Scenario/General/SecretTunnel";
            return true;
        }
}

[HarmonyPatch(typeof(DialogueSystemController), "Awake")]
public static class DialogueSystemControllerAwake
{
        public delegate void Awake();
        public static Awake OnAwake = delegate { };



        static void Postfix(DialogueSystemController __instance)
        {
            OnAwake.Invoke();
        }

        /*

            Plugin.logger.LogMessage("Dumping conversations:");
        foreach(Conversation convo in __instance.MasterDatabase.conversations)
        {
                Plugin.logger.LogMessage("Logging conversation:" + convo.Title);
                //convo.GetDialogueEntry
                var i2Lan = UnityEngine.Object.FindObjectOfType<DialogueSystemUseI2Language>();
                var traverse = Traverse.Create(i2Lan);


            foreach (DialogueEntry entry in convo.dialogueEntries)
            {
                    Plugin.logger.LogMessage("\tLogging entry: " + entry.id);
                    Plugin.logger.LogMessage("\tTitle: " + entry.Title);
                    Plugin.logger.LogMessage("\tIs root: "+entry.isRoot);
                    var entryHeader = traverse.Method("GetDialogueEntryHeader", entry).GetValue<string>();
                    Plugin.logger.LogMessage("\tDialogue entry header (Localization term key): " + entryHeader);

                    entryHeader += "/Dialogue Text";

                    var translated_text = LocalizationManager.GetTermTranslation(entryHeader, FixForRTL: true, 0, ignoreRTLnumbers: true, applyParameters: false, null, LocalizationManager.CurrentLanguage);

                    Plugin.logger.LogMessage("\tLocalized entry text: " + translated_text);

                    Plugin.logger.LogMessage("\tCurrent dialogue text: " + entry.currentDialogueText);
                    Plugin.logger.LogMessage("\tScene event guid: " + entry.sceneEventGuid);


                    Plugin.logger.LogMessage("\t ActorID: " + entry.ActorID);
                    Plugin.logger.LogMessage("\t Sequence: " + entry.Sequence);

                    Plugin.logger.LogMessage("\tDialogue conditions: " + entry.conditionsString);
                    Plugin.logger.LogMessage("\tDialogue Links: ");

                    foreach(Link link in entry.outgoingLinks)
                    {
                        Plugin.logger.LogMessage("\t\tLinks to: " + link.destinationDialogueID);
                    }

                    for (int i = 0; i < entry.onExecute.GetPersistentEventCount(); i++)
                    {
                        string methodName = entry.onExecute.GetPersistentMethodName(i);
                        string targetName = entry.onExecute.GetPersistentTarget(i).GetType().ToString();
                        Plugin.logger.LogMessage("\tExecutes method: "+ targetName+"."+methodName );
                    }

                    Plugin.logger.LogMessage("\n");
            }

            foreach (Field field in convo.fields)
                {
                    Plugin.logger.LogMessage("\tLogging Field: "+ field.title +", Value: " + field.value);
                }

                Plugin.logger.LogMessage("\n");
            }*/


}

    [HarmonyPatch(typeof(MapController), "Awake")]
    public static class OnMapControllerAwake
    {

        public delegate void RegisterScenarios();
        public static RegisterScenarios OnRegisterScenarios = delegate { };

        static bool firstTime = true;
        static bool Prefix(ref List<MapDataScenario>  ____potentialRandomScenarios)
        {
            if (firstTime)
            {
                //Grab base game scenarios
                ScenarioFactory.RegisterScenarios(____potentialRandomScenarios);
                //Call register scenario event for mods to latch on to
                OnRegisterScenarios.Invoke();


                ____potentialRandomScenarios = ____potentialRandomScenarios.GetRange(0, 2);

                //Add our scenarios to potential random scenario list
                ____potentialRandomScenarios.AddRange(ScenarioFactory.modScenarios);

                firstTime = false;

            }

            /*
            Plugin.logger.LogMessage("Logging scenarios:");
            foreach (MapDataScenario mapDataScenario in ____potentialRandomScenarios)
            {
                Plugin.logger.LogMessage("\tName: " + mapDataScenario.scenarioName);
                Plugin.logger.LogMessage("\t\tDoodads: ");
                foreach( GameObject doodad in mapDataScenario.scenarioDoodads)
                {
                    Plugin.logger.LogMessage("\t\t\t"+ doodad.name);
                }
                Plugin.logger.LogMessage("\t\tScenario prereqs: ");
                foreach (ScenarioPreReq preReq in mapDataScenario.scenarioPreReqs)
                {
                    Plugin.logger.LogMessage("\t\t\t" + preReq.ToString());
                }

            }*/
            return true;
        }
    }

    [HarmonyPatch(typeof(PachinkoBall), "OnCollisionEnter2D")]
    public static class OnCollisionEnterPatch
    {
        static bool Prefix(PachinkoBall __instance, Collision2D collision, RelicManager ____relicManager)
        {
            if (collision.collider.CompareTag("Peg"))
            {
                Peg component = collision.collider.GetComponent<Peg>();
                if (component != null)
                {
                    if (component.pegType == Peg.PegType.CRIT)
                    {
                        if (__instance.GetComponent<CritSplit>())
                        {
                            __instance.multiballLevel++;
                        }
                    }
                }
            }


            //Bomb relic test

            if (collision.collider.CompareTag("Bomb"))
            {
                Bomb component = collision.collider.GetComponent<Bomb>();
                if (component != null)
                {
                    {
                        RelicEffect re;
                        RelicRegister.TryGetCustomRelicEffect("hxpmods.morbs.testRelic", out re);

                        if (component.HitCount > 0)
                        {
                            if (____relicManager.AttemptUseRelic(re))
                            {
                                //If HitCount > 0 and it has been hit, it is about to explode.
                                __instance.multiballLevel++;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }


    [HarmonyPatch(typeof(GameInit), "Start")]
    public static class GameInitEvent
    {

        public delegate void GameStarted();
        public static GameStarted OnGameStarted = delegate { };

        public static void Prefix()
        {
            if (Plugin.debug)
            {
                Plugin.LogDialogue();
            }

            OnGameStarted.Invoke();

            if (OrbFactory.doClear)
            {
                RelicSet[] relicSets = Resources.FindObjectsOfTypeAll<RelicSet>();

                foreach (RelicSet relicSet in relicSets)
                {
                    Plugin.logger.LogMessage(relicSet.name);

                    var modRelics = new List<Relic>();

                    foreach (string guid in Plugin.customRelics)
                    {
                        RelicEffect re;
                        Relic relic;
                        RelicRegister.TryGetCustomRelicEffect(guid, out re);
                        RelicRegister.TryGetCustomRelic(re, out relic);
                        modRelics.Add(relic);
                        Plugin.logger.LogMessage(relic);
                    }

                    Traverse.Create(relicSet).Field("_relics").SetValue(modRelics);
                }

            }

        }
    }

    [HarmonyPatch(typeof(PlayButton), "Awake")]
    public static class PlayButtonAwake
    {
        //This is effectively our OnApplicationStarted along with MainMenuOrbsPatch

        static void Postfix()
        {

            //Register terms here. Due to google docs update feature, they also need to be registered again later

            ModdedLocManager.RegisterTerms();


            //This adds the "Disable Vanilla Orb and Relics" to the game start menu
            var o = Resources.FindObjectsOfTypeAll<OptionsPanel>()[0];
            var p = o.gameObject.transform.Find("Settings/GeneralSettings/ClickToAimRow");

            var menu = o.transform.parent.parent.Find("Character+CruciballCanvas");

            Transform r = GameObject.Instantiate(p, menu);

            //r.position = new Vector3(-15f, 4.0999f, 90f);
            var rect = r.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.05f, 0.2f);
            rect.anchorMax = new Vector2(0.05f, 0.2f);

            var rect2 = r.Find("ClickToAimToggle").GetComponent<RectTransform>();
            rect2.anchorMin = new Vector2(0.8f, 0.5f);
            rect2.anchorMax = new Vector2(0.8f, 0.5f);

            var t = r.GetComponentInChildren<UnityEngine.UI.Toggle>();
            t.onValueChanged.RemoveAllListeners();

            t.onValueChanged.AddListener(OrbFactory.ClearBaseGameOrbPool);
            t.isOn = OrbFactory.doClear;

            UnityEngine.Object.Destroy(r.GetComponentInChildren<ClickToAimToggle>());

            //r.GetComponentInChildren<Localize>().SetTerm("Menu/devmode");
            r.GetComponent<Localize>().Term = "Menu/devmode";
            r.Find("Description").gameObject.SetActive(false);

            //Plugin.logger.LogMessage(o);

            //__instance.onValueChanged.AddListener(TestBool);
        }
    }


    [HarmonyPatch(typeof(DeckManager),nameof(DeckManager.LoadDeckData))]
    public static class LoadDeckDataPatch
    {

        //Compatability issues arise using this method

        static bool Prefix( DeckManager __instance)
        {
            if (DeckManager.completeDeck == null)
            {
                Debug.LogError("DeckManager::LoadDeckData(): Attempted to load a deck, but completeDeck was null. Bailing.");
            }
            var obj = ToolBox.Serialization.DataSerializer.Load<SaveObjectData>("DeckManager");

            Plugin.logger.LogMessage(obj);

            List<GameObject> list = new List<GameObject>();
            string[] prefabNames = (string[])Traverse.Create(obj).Field("_prefabNames").GetValue();



            foreach (string text in prefabNames)
            {
                //If the prefab exists in our modded orbs, it will be added to this list
                IEnumerable<GameObject> prefab = from _prefab in OrbFactory.allModOrbs where _prefab.name == text select _prefab;

                GameObject gameObject;

                if (prefab.Count() > 0)
                {
                    Plugin.logger.LogMessage("Loading modded orb");
                    gameObject = prefab.First();
                }
                else
                {
                    //If it is not in our list, load it from base game instead
                    gameObject = Resources.Load<GameObject>("Prefabs/Orbs/" + text);

                }
                if (gameObject == null)
                {
                    Debug.LogError("Prefab/Orbs/" + text + ".prefab not found when loading deck!");
                }

                list.Add(gameObject);

                Plugin.logger.LogMessage(text);
            }
            __instance.InstantiateDeck(list);
            return false;
        }
    }

    [HarmonyPatch(typeof(PopulateSuggestionOrbs), "Start")]
    public static class SuggestionOrbsPatch
    {
        private static bool hasInjected = false;
        //It seems we can inject orbs into the pool whenever it exists, so we do it at the earliest convenience here

        public delegate void OrbPoolSpawned();
        public static OrbPoolSpawned OnOrbPoolSpawned = delegate { };

        static void Postfix()
        {
            if (!hasInjected)
            {
                Plugin.logger.LogMessage("Suggestion Orb Pool Spawned");
                OnOrbPoolSpawned.Invoke();
                //hasInjected = true;
            }
        }
    }

    [HarmonyPatch(typeof(PachinkoBall), "FixedUpdate")]
    public static class MagneticFixedUpdate
    {
        static void Postfix(PachinkoBall __instance)
        {
            if (!__instance.IsDummy)
            {
                var m = __instance.GetComponent<Magnetic>();
                if (m)
                {
                    m.DoMagnetAttraction(Physics2D.defaultPhysicsScene);
                }
            }
        }
    }

    [HarmonyPatch(typeof(PachinkoBall), "DoUpdate")]
    public static class MagneticDoUpdate
    {
        static void Postfix(PachinkoBall __instance, PhysicsScene2D physicsScene2D)
        {
            var m = __instance.GetComponent<Magnetic>();
            if (m)
            {
                m.DoMagnetAttraction(physicsScene2D);
            }
        }
    }

    [HarmonyPatch(typeof(PachinkoBall), "StartDestroy")]
    public static class BallStartDestroy
    {
        static bool Prefix(PachinkoBall __instance)
        {
            var obj = __instance.gameObject;
            var t = obj.GetComponent<Teleporter>();
            if (t)
            {
                //If we have have a teleporting component, ask it to teleport the ball.
                bool didTeleport = t.doTeleport();
                return !didTeleport; //If we did not teleport, allow the ball to be destroyed by continuing this function.
            }
            return true;
        }
    }


    [HarmonyPatch(typeof(UpcomingOrbDisplay), "Initialize")]
    public static class FixOrbDisplay
    {
        static void Postfix(UpcomingOrbDisplay __instance, SpriteRenderer ____renderer, GameObject orbPrefab)
        {
            var r = orbPrefab.GetComponentInChildren<ReplaceAnimationComp>();

            if(r)
            {
                var _r = ____renderer.gameObject.AddComponent<ReplaceAnimationComp>();
                _r.animation_set = r.animation_set;
            }
        }
    }

    [HarmonyPatch(typeof(UpgradeOption), "SpecifiedOrb",MethodType.Setter)]
    public static class FixUpgradeOption
    {
        static void Postfix( GameObject value, Image ___orbDisplayImage)
        {

            Plugin.logger.LogMessage(value);
            Plugin.logger.LogMessage(___orbDisplayImage);

            var sr = value.GetComponentInChildren<SpriteRenderer>();
            var rac = sr.GetComponent<ReplaceAnimationComp>();
            if (rac)
            {
                if (___orbDisplayImage)
                {
                    var _rac = ___orbDisplayImage.gameObject.AddComponent<ReplaceAnimationComp>();
                    _rac.animation_set = rac.animation_set;
                }

            }



        }
    }

}