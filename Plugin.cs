using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Battle;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PeglinRelicLib.Model;
using PeglinRelicLib.Register;
using PeglinRelicLib.Utility;
using PixelCrushers.DialogueSystem;
using Relics;
using UnityEngine;
using Morbs.Utility;
using Morbs.Factories;


namespace Morbs
{
    [BepInPlugin(GUID, Name, Version)]
    [BepInDependency("io.github.crazyjackel.RelicLib")]

    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "hxpmods.morbs";
        public const string Name = "Morbs";
        public const string Version = "0.0.0";

        public static ManualLogSource logger;

        public static List<string> customRelics = new List<string>();

        public static RelicEffect testRelicEffect;

        public static PegManager pm;

        public static GameObject portorb;

        public static bool debug = false;

        void Awake()
        {
            Harmony patcher = new Harmony(GUID);
            patcher.PatchAll();

            logger = Logger;
            logger.LogMessage(Name +" loaded");

            //This collects all the prefabs and data from the vanilla game for use in OrbFactory
            MainMenuOrbsPatch.OnPreOrbPoolSpawned += OrbFactory.CollectOrbPool;

            MainMenuOrbsPatch.OnPreOrbPoolSpawned += ContentRegister.RegisterAll;

            //Do it post orb pool spawn to ensure all content has been registed
            MainMenuOrbsPatch.OnPostOrbPoolSpawned += InjectOrbPool;


            GameInitEvent.OnGameStarted += OrbFactory.SetOrbPoolForGame;


            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/01/Dialogue Text", "From the corner of your eye you spot a pure white plinth, simple and elegant. You can't quite catch a glimpse of whatever is on top.");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/02/Dialogue Text", "Approach the plinth");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/03/Dialogue Text", "Leave");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/04/Dialogue Text", "...   \n...   \n...   \nIt's cake.");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/05/Dialogue Text", "Approach the cake");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/06/Dialogue Text", "I see where this is going...");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/07/Dialogue Text", "In the walk up there you start to wonder how you ever thought it looked like cake at all. This is clearly a <color=\"purple\">Portorb</color>, and you clearly need Glasses.");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/08/Dialogue Text", "Grab the <color=\"purple\">Portorb</color>");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/09/Dialogue Text", "Leave");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/10/Dialogue Text", "You pick up the <color=\"purple\">Portorb</color>. It faintly hums in your hand. By the time you look up, the plinth is gone completely.");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/11/Dialogue Text", "...  \n <color=\"orange\">\"No you dont.\"</color>, a strangely metallic voice calls out from the plinth.");
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/12/Dialogue Text", "Yes I do");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/13/Dialogue Text", "There was an awkward pause. <color=\"orange\">\"Fine. Where is this going then?\"</color>");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/14/Dialogue Text", "There is no cake");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/15/Dialogue Text", "<color=\"orange\">\"'ThErE iS nO cAkE'\"\n\"Wow, you know it all huh?. A real renaissance Peglin.\"\n\"If you're so confident why don't you come here and prove it?\"</color>");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/16/Dialogue Text", "You're going to eat me");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/17/Dialogue Text", "<color=\"orange\">\"Alright, alright, you got me, the cake is a l--\"</color>\n<color=\"orange\">\"Wait, did you say EAT you?... That's.. That's not what's happening here at all. Frankly, that's just morbid...\"</color>");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/18/Dialogue Text", "It's a tough life");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/19/Dialogue Text", "<color=\"orange\">\"You're telling me...\"</color>\n<color=\"orange\">\"Hey, you're clearly going on some grand adventure. How would you like to take this <color=\"purple\">Portorb</color> off my ha-- display?\"</color>");
            
            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/20/Dialogue Text", "So where is this going then?");

            ModdedLocManager.AddTerm("Dialogue System/Conversation/Scenario.PeglinMod.TestScenario/Entry/21/Dialogue Text", "<color=\"orange\">\"Only one way to find out.\"</color>");

            ModdedLocManager.AddTermsFromCSV();

            

            //SuggestionOrbsPatch.OnOrbPoolSpawned += InjectOrbPool;

            RelicDataModel model = new RelicDataModel("hxpmods.morbs.testRelic")
            {
                Rarity = RelicRarity.COMMON,
                BundlePath = "relic",
                //SpriteName = "knife",
                RelicSprite = LoadSpriteFromFile("bombdoll.png", pixelsPerUnit: 8.0f),
                LocalKey = "boomdoll",
                DescriptionKey = "boomdollRelic"
            };

            customRelics.Add("hxpmods.morbs.testRelic");

            model.SetAssemblyPath(this);
            bool success = RelicRegister.RegisterRelic(model, out RelicEffect myEffect);
            
            LocalizationHelper.ImportTerm(new TermDataModel(model.NameTerm)
            {
                English = "Baboomshka"
            });

            LocalizationHelper.ImportTerm(new TermDataModel(model.DescriptionTerm)
            {
                English = "Your orbs Multiball when they detonate <sprite name=\"BOMB\"> "
            });
        }

        public static void LogDialogue()
        {

           Plugin.logger.LogMessage("Logging dialogue");
           
         var so = ScriptableObject.FindObjectsOfType<DialogueDatabase>();
         foreach (DialogueDatabase db in so)
         {
             foreach (Conversation convo in db.conversations)
             {
                 Plugin.logger.LogMessage(convo.Name);
             }
         }

         var dialogueDatabases = Resources.FindObjectsOfTypeAll<DialogueDatabase>();

         foreach (DialogueDatabase database in dialogueDatabases)
         {
             foreach (Conversation convo in database.conversations)
             {
                 Plugin.logger.LogMessage(convo.Name);
             }
         }
           
            foreach (Conversation convo in DialogueManager.masterDatabase.conversations)
            {
                Plugin.logger.LogMessage(convo.Name);
            }
           
        }
  
        void InjectOrbPool()
        {
           
             OrbPool[] orbPools = Resources.FindObjectsOfTypeAll<OrbPool>();

             var orbs = orbPools[0].AvailableOrbs.ToList();
             orbs.AddRange(OrbFactory.modOrbs);
             //And back again
             orbPools[0].AvailableOrbs = orbs.ToArray();


            //Save a reference to avoid Shrodinger's OrbPool (the next time the orb is loaded it will have all our orbs removed if we don't)
            OrbFactory.availableOrbPool = orbPools[0] as OrbPool;

        }

        public static Sprite LoadSpriteFromFile(string filePath, string directory = null, float pixelsPerUnit = 1.0f)
        {

            directory = "BepinEx/plugins/assets/";

            var data = File.ReadAllBytes(directory + filePath);
            var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false)
            {
                filterMode = FilterMode.Point
            };

            //tex.ClearRequestedMipmapLevel();
            //tex.filterMode = FilterMode.Point;

            if (!tex.LoadImage(data))
            {
                logger.LogMessage("Failed to load image from file: " + filePath);
                throw new Exception("Failed to load image from file: " + filePath);
            }
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            return sprite;
        }
        public static Texture LoadTextureFromFile(string filePath, string directory = null)
        {

            directory = "BepinEx/plugins/assets/";

            var data = File.ReadAllBytes(directory + filePath);
            var tex = new Texture2D(2, 2);

            //tex.ClearRequestedMipmapLevel();
            //tex.filterMode = FilterMode.Point;

            if (!tex.LoadImage(data))
            {
                logger.LogMessage("Failed to load image from file: " + filePath);
                throw new Exception("Failed to load image from file: " + filePath);
            }
            return tex;
        }

    }

}
