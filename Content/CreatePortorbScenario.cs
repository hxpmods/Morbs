using Morbs.Factories;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morbs.Content
{
    //Static classes tagged with "Content" will have their OnRegister called automatically
    [Content]
    static class CreatePortorbScenario
    {
        public static void OnRegister()
        {
            OnMapControllerAwake.OnRegisterScenarios +=CreateScenario;
            DialogueSystemControllerAwake.OnAwake += CreateConversation;
        }

        public static void CreateScenario()
        {
            var scenario = ScenarioFactory.CreateFromBaseGame("PeglinMod/TestScenario");
            ScenarioFactory.modScenarios.Add(scenario);
            //scenario.ignoreRemove = true;
        }
        
        public static void CreateConversation()
        {
            var t = Template.FromDefault();
            var c = t.CreateConversation(69, "Scenario/PeglinMod/TestScenario");
            c.fields[2].value = "1";
            c.fields[3].value = "2";

            var d0 = t.CreateDialogueEntry(0, 69, "START");
            d0.Sequence = "None()";
            d0.ActorID = 1;


            var d1 = t.CreateDialogueEntry(1, 69, "");
            d1.ActorID = 2;
            d1.DialogueText = "plinth"; //We use a key generated via the Entry to actually display texts, however, if DialogueText is empty this will not happen. 

            var d2 = t.CreateDialogueEntry(2, 69, "");
            d2.ActorID = 1;
            d2.DialogueText = "approach";

            var d3 = t.CreateDialogueEntry(3, 69, "");
            d3.ActorID = 1;
            d3.DialogueText = "leave";

            var d4 = t.CreateDialogueEntry(4, 69, "");
            d4.ActorID = 2;
            d4.DialogueText = "cake";

            var d5 = t.CreateDialogueEntry(5, 69, "");
            d5.ActorID = 1;
            d5.DialogueText = "approachcake";

            var d6 = t.CreateDialogueEntry(6, 69, "");
            d6.ActorID = 1;
            d6.DialogueText = "iseewhere";

            var d7 = t.CreateDialogueEntry(7, 69, "");
            d7.ActorID = 2;
            d7.DialogueText = "notcake";

            var d8 = t.CreateDialogueEntry(8, 69, "");
            d8.ActorID = 1;
            d8.DialogueText = "takeOrb";

            var dm = UnityEngine.Resources.FindObjectsOfTypeAll<DeckManager>()[0];

            Plugin.logger.LogMessage("Deckmanager:" + dm);

            d8.onExecute = new UnityEngine.Events.UnityEvent();

            d8.onExecute.AddListener(delegate { dm.AddOrbToDeck(Plugin.portorb); });

            var d9 = t.CreateDialogueEntry(9, 69, "");
            d9.ActorID = 1;
            d9.DialogueText = "leave";

            var d10 = t.CreateDialogueEntry(10, 69, "");
            d10.ActorID = 2;
            d10.DialogueText = "exitwithportorb";

            var d11 = t.CreateDialogueEntry(11, 69, "");
            d11.ActorID = 2;
            d11.DialogueText = "noyoudont";

            var d12 = t.CreateDialogueEntry(12, 69, "");
            d12.ActorID = 1;
            d12.DialogueText = "yesido";

            var d13 = t.CreateDialogueEntry(13, 69, "");
            d13.ActorID = 2;
            d13.DialogueText = "finethen";

            var d14 = t.CreateDialogueEntry(14, 69, "");
            d14.ActorID = 1;
            d14.DialogueText = "cakeislie";

            var d15 = t.CreateDialogueEntry(15, 69, "");
            d15.ActorID = 2;
            d15.DialogueText = "knowitall";

            var d16 = t.CreateDialogueEntry(16, 69, "");
            d16.ActorID = 1;
            d16.DialogueText = "eat";

            var d17 = t.CreateDialogueEntry(17, 69, "");
            d17.ActorID = 2;
            d17.DialogueText = "whatthe";

            var d18 = t.CreateDialogueEntry(18, 69, "");
            d18.ActorID = 1;
            d18.DialogueText = "toughLife";

            var d19 = t.CreateDialogueEntry(19, 69, "");
            d19.ActorID = 2;
            d19.DialogueText = "tellingMe";

            var d20 = t.CreateDialogueEntry(20, 69, "");
            d20.ActorID = 1;
            d20.DialogueText = "whatsHappening";

            var d21 = t.CreateDialogueEntry(21, 69, "");
            d21.ActorID = 2;
            d21.DialogueText = "findOut";


            d0.outgoingLinks.Add(new Link(69, 0, 69, 1) { isConnector = true });

            d1.outgoingLinks.Add(new Link(69, 1, 69, 2) { isConnector = true });
            d1.outgoingLinks.Add(new Link(69, 1, 69, 3) { isConnector = true });

            d2.outgoingLinks.Add(new Link(69, 2, 69, 4) { isConnector = true });

            d4.outgoingLinks.Add(new Link(69, 4, 69, 5) { isConnector = true });
            d4.outgoingLinks.Add(new Link(69, 4, 69, 6) { isConnector = true });

            d5.outgoingLinks.Add(new Link(69, 5, 69, 7) { isConnector = true });

            d6.outgoingLinks.Add(new Link(69, 6, 69, 11) { isConnector = true });

            d7.outgoingLinks.Add(new Link(69, 7, 69, 8) { isConnector = true });
            d7.outgoingLinks.Add(new Link(69, 7, 69, 9) { isConnector = true });

            d8.outgoingLinks.Add(new Link(69, 8, 69, 10) { isConnector = true });

            d11.outgoingLinks.Add(new Link(69, 11, 69, 12) { isConnector = true });
            d12.outgoingLinks.Add(new Link(69, 12, 69, 13) { isConnector = true });

            d13.outgoingLinks.Add(new Link(69, 13, 69, 14) { isConnector = true });
            d13.outgoingLinks.Add(new Link(69, 13, 69, 16) { isConnector = true });

            d14.outgoingLinks.Add(new Link(69, 14, 69, 15) { isConnector = true });

            d15.outgoingLinks.Add(new Link(69, 15, 69, 5) { isConnector = true });
            d15.outgoingLinks.Add(new Link(69, 15, 69, 3) { isConnector = true });

            d16.outgoingLinks.Add(new Link(69, 16, 69, 17) { isConnector = true });

            d17.outgoingLinks.Add(new Link(69, 17, 69, 18) { isConnector = true });
            d17.outgoingLinks.Add(new Link(69, 17, 69, 20) { isConnector = true });

            d18.outgoingLinks.Add(new Link(69, 18, 69, 19) { isConnector = true });

            d19.outgoingLinks.Add(new Link(69, 19, 69, 8) { isConnector = true });
            d19.outgoingLinks.Add(new Link(69, 19, 69, 9) { isConnector = true });

            d20.outgoingLinks.Add(new Link(69, 20, 69, 21) { isConnector = true });

            d21.outgoingLinks.Add(new Link(69, 21, 69, 5) { isConnector = true });
            d21.outgoingLinks.Add(new Link(69, 21, 69, 3) { isConnector = true });

            c.dialogueEntries.Add(d0);
            c.dialogueEntries.Add(d1);
            c.dialogueEntries.Add(d2);
            c.dialogueEntries.Add(d3);
            c.dialogueEntries.Add(d4);
            c.dialogueEntries.Add(d5);
            c.dialogueEntries.Add(d6);
            c.dialogueEntries.Add(d7);
            c.dialogueEntries.Add(d8);
            c.dialogueEntries.Add(d9);
            c.dialogueEntries.Add(d10);
            c.dialogueEntries.Add(d11);
            c.dialogueEntries.Add(d12);
            c.dialogueEntries.Add(d13);
            c.dialogueEntries.Add(d14);
            c.dialogueEntries.Add(d15);
            c.dialogueEntries.Add(d16);
            c.dialogueEntries.Add(d17);
            c.dialogueEntries.Add(d18);
            c.dialogueEntries.Add(d19);
            c.dialogueEntries.Add(d20);
            c.dialogueEntries.Add(d21);

            DialogueManager.MasterDatabase.AddConversation(c);
        }

    }
}
