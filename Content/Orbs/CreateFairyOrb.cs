using Morbs.Factories;
using Morbs.OrbBehaviours;
using UnityEngine;

namespace Morbs.Content.Orbs
{
    [Content]
    static class CreateFairyOrb
    {
        public static void OnRegister()
        {
            //Registering here ensures we have access to all prefabs
            MainMenuOrbsPatch.OnRegisterOrbs += RegisterForbe;
        }

        static void RegisterForbe()
        {
            var orb = createOrb();

            //Add twice in case vanilla relics are turned off, the game does not like having only 2 orbs. 1 works, 4 does, I haven't tested 3.
            //This adds your orb to the available orb pool
            OrbFactory.modOrbs.Add(orb);
            OrbFactory.modOrbs.Add(orb);
        }

        public static GameObject createOrb()
        {
            var orb1 = OrbFactory.createOrbFromName();

            orb1.name = "FairyOrb-Lvl1";
            FireballAttack a = orb1.GetComponent<FireballAttack>();

            a.locNameString = "fairyorb";
            a.locDescStrings = new string[] { };
            a.DamagePerPeg = 2;
            a.CritDamagePerPeg = 3;
            a.Level = 1;

            orb1.AddComponent<FairyDustSpawner>();

            var s = orb1.GetComponentInChildren<SpriteRenderer>();
            var size = s.size;
            s.sprite = Plugin.LoadSpriteFromFile("Crystorb.png", pixelsPerUnit: 8);

            s.size = size;

            var rb = orb1.GetComponent<Rigidbody2D>();

            rb.mass *= 1.2f;

            GameObject[] prefabs = OrbFactory.createModFireballAttackPrefabs();

            foreach (GameObject prefab in prefabs)
            {
                prefab.GetComponent<SpriteRenderer>().sprite = s.sprite;
            }

            var trail = orb1.GetComponentInChildren<TrailRenderer>();

            OrbFactory.setFireballAttackPrefabs(a, prefabs);



            a.NextLevelPrefab = null;

            return orb1;
        }

       

    }
}
