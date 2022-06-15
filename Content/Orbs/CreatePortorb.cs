using I2.Loc;
using Morbs.Factories;
using Morbs.OrbBehaviours;
using Morbs.OrbBehaviours.Attacks;
using Morbs.OrbBehaviours.BattleAttackBehaviours;
using Morbs.Utility;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;
using static UnityEngine.Object;

namespace Morbs.Content.Orbs
{
    //Static classes tagged with "Content" will have their OnRegister called automatically
    [Content]
    static class CreatePortorb
    {
        public static void OnRegister()
        {
            //Registering here ensures we have access to all prefabs
            MainMenuOrbsPatch.OnRegisterOrbs += RegisterPortorb;
        }

        static void RegisterPortorb()
        {
            var portorb = createPortorb();
            var portorb2 = createPortorb2(portorb);
            var portorb3 = createPortorb3(portorb2);

            Plugin.portorb = portorb; 

            //Not getting the teleporting attack specifically results in getting the base attack, as they are Object.Destroy() works at the end of the frame.
            portorb.GetComponent<TeleportingAttack>().NextLevelPrefab = portorb2;
            portorb2.GetComponent<TeleportingAttack>().NextLevelPrefab = portorb3;

            //Add twice in case vanilla relics are turned off, the game does not like having only 2 orbs. 1 works, 4 does, I haven't tested 3.
            //This adds your orb to the available orb pool
            OrbFactory.modOrbs.Add(portorb);
            OrbFactory.modOrbs.Add(portorb);

        }

        public static GameObject createPortorb()
        {
            var orb = OrbFactory.createOrbFromName("LightningBall-Lvl1");

            orb.name = "Portorb-Lvl1";


            /*Battle still catches after attack with shots left*/


            TargetedAttack a = orb.AddComponent<TeleportingAttack>();
            a.locNameString = "portorb";
            a.locDescStrings = a.locDescStrings = new string[] { "teleport", "portorb_attack" };
            a.Level = 1;

            a.CritDamagePerPeg = 2;
            a.DamagePerPeg = 1;

            var s = orb.GetComponentInChildren<SpriteRenderer>();

            s.sprite = Plugin.LoadSpriteFromFile("Portorb_1.png", pixelsPerUnit: 8.0f);
            s.gameObject.transform.localScale = Vector3.one * 1.1f;

            GameObject sprite = orb.transform.Find("Sprite").gameObject;


       

            ReplaceAnimationComp replaceAnim = sprite.AddComponent<ReplaceAnimationComp>();
            replaceAnim.animation_set = "portorb";

            ReplaceAnimationComp.sprite_sheets.Add("portorb", new List<Sprite>());

            ReplaceAnimationComp.sprite_sheets["portorb"].Add(s.sprite);
            ReplaceAnimationComp.sprite_sheets["portorb"].Add(Plugin.LoadSpriteFromFile("Portorb_2.png", pixelsPerUnit: 8.0f));
            ReplaceAnimationComp.sprite_sheets["portorb"].Add(Plugin.LoadSpriteFromFile("Portorb_3.png", pixelsPerUnit: 8.0f));
            ReplaceAnimationComp.sprite_sheets["portorb"].Add(Plugin.LoadSpriteFromFile("Portorb_4.png", pixelsPerUnit: 8.0f));
            ReplaceAnimationComp.sprite_sheets["portorb"].Add(Plugin.LoadSpriteFromFile("Portorb_5.png", pixelsPerUnit: 8.0f));
            ReplaceAnimationComp.sprite_sheets["portorb"].Add(Plugin.LoadSpriteFromFile("Portorb_6.png", pixelsPerUnit: 8.0f));
            ReplaceAnimationComp.sprite_sheets["portorb"].Add(Plugin.LoadSpriteFromFile("Portorb_7.png", pixelsPerUnit: 8.0f));
            ReplaceAnimationComp.sprite_sheets["portorb"].Add(Plugin.LoadSpriteFromFile("Portorb_8.png", pixelsPerUnit: 8.0f));


            ReplaceAnimationComp.sprite_sheets.Add("PortorbAttack", new List<Sprite>());

            float ppu = 32.0f;

            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_1.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_2.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_3.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_4.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_5.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_6.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_7.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_8.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_9.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_10.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_11.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_12.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_13.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_14.png", pixelsPerUnit: ppu));
            ReplaceAnimationComp.sprite_sheets["PortorbAttack"].Add(Plugin.LoadSpriteFromFile("PortorbAttack_15.png", pixelsPerUnit: ppu));

            var t = orb.AddComponent<Teleporter>();

            var cs = orb.AddComponent<PortorbColorSwitcher>();


            LocalizationParamsManager loc = orb.AddComponent<LocalizationParamsManager>();

            loc.SetParameterValue("level", a.Level.ToString());
            loc.SetParameterValue("portsLeft", a.Level.ToString());

            ///orb.AddComponent<TeleportingAttack>();

            Destroy(orb.GetComponent<ThunderOrbPachinko>());
            Destroy(orb.GetComponent<TutorialElement>());
            Destroy(orb.GetComponent<TargetedAttack>());


            GameObject[] prefabs = OrbFactory.createModTargetedAttackPrefabs();

            foreach (GameObject prefab in prefabs)
            {
                var sb = prefab.GetComponent<SpellBehavior>();

                var tsb = prefab.AddComponent<TeleportingSpellBehavior>();

                tsb.AttackSfx = sb.AttackSfx;

                var rac = prefab.AddComponent<ReplaceAnimationComp>();
                rac.animation_set = "PortorbAttack";


                Destroy(sb);
            }

            OrbFactory.setTargetedAttackPrefabs(a, prefabs);

            // a.NextLevelPrefab = createPortorb2(orb);

            return orb;

        }

        public static GameObject createPortorb2(GameObject fromPortorb)
        {
            var orb1 = OrbFactory.createOrbFromPrefab(fromPortorb);

            orb1.name = "Portorb-Lvl2";
            TeleportingAttack a = orb1.GetComponent<TeleportingAttack>();
            a.Level = 2;

            a.shotsLeft = 2;

            var t = orb1.GetComponent<Teleporter>();
            t.maxPorts = 2;


            Destroy(orb1.GetComponent<ThunderOrbPachinko>());
            Destroy(orb1.GetComponent<TutorialElement>());
            Destroy(orb1.GetComponent<TargetedAttack>());

            //a.NextLevelPrefab = createPortorb3(orb1);
            return orb1;
        }

        public static GameObject createPortorb3(GameObject fromPortorb)
        {
            var orb1 = OrbFactory.createOrbFromPrefab(fromPortorb);

            orb1.name = "Portorb-Lvl3";
            TeleportingAttack a = orb1.GetComponent<TeleportingAttack>();

            a.Level = 3;

            a.shotsLeft = 3;

            var t = orb1.GetComponent<Teleporter>();
            t.maxPorts = 3;


            //We destroy the excess components each time as they are not destroyed by the time they are cloned.
            Destroy(orb1.GetComponent<ThunderOrbPachinko>());
            Destroy(orb1.GetComponent<TutorialElement>());
            Destroy(orb1.GetComponent<TargetedAttack>());

            a.NextLevelPrefab = null;
            return orb1;
        }

    }
}
