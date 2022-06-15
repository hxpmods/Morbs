using Morbs.Factories;
using Morbs.OrbBehaviours;
using UnityEngine;

namespace Morbs.Content.Orbs
{
    [Content]
    static class CreateForbe
    {
        public static void OnRegister()
        {
            //Registering here ensures we have access to all prefabs
            MainMenuOrbsPatch.OnRegisterOrbs += RegisterForbe;
        }

        static void RegisterForbe()
        {
            var forbe = createForbe();

            //Add twice in case vanilla relics are turned off, the game does not like having only 2 orbs. 1 works, 4 does, I haven't tested 3.
            //This adds your orb to the available orb pool
            OrbFactory.modOrbs.Add(forbe);
            OrbFactory.modOrbs.Add(forbe);
        }

        public static GameObject createForbe()
        {
            var orb1 = OrbFactory.createOrbFromName();

            orb1.name = "Forbe-Lvl1";
            FireballAttack a = orb1.GetComponent<FireballAttack>();

            a.locNameString = "luckorb";
            a.locDescStrings = new string[] { "splitcrit" };
            a.DamagePerPeg = 1;
            a.CritDamagePerPeg = 5;
            a.Level = 1;

            orb1.AddComponent<CritSplit>();

            var s = orb1.GetComponentInChildren<SpriteRenderer>();
            var size = s.size;
            s.sprite = Plugin.LoadSpriteFromFile("Luckorb.png", pixelsPerUnit: 8);
            s.size = size;

            GameObject[] prefabs = OrbFactory.createModFireballAttackPrefabs();

            foreach (GameObject prefab in prefabs)
            {
                //Color col = new Color(245, 140, 40, 255) / 255.0f * 1.25f;
                prefab.GetComponent<SpriteRenderer>().sprite = s.sprite;
            }

            var trail = orb1.GetComponentInChildren<TrailRenderer>();
            trail.endColor = new Color(0.9608f, 0.7294f, 0.302f, 0);

            OrbFactory.setFireballAttackPrefabs(a, prefabs);


            //Make lighter
            PachinkoBall b = orb1.GetComponent<PachinkoBall>();
            b.GravityScale *= 0.9f;


            a.NextLevelPrefab = createForbe2(orb1);

            return orb1;
        }

        public static GameObject createForbe2(GameObject lastLevel)
        {
            var orb1 = OrbFactory.createOrbFromPrefab(lastLevel);

            orb1.name = "Forbe-Lvl2";
            Attack a = orb1.GetComponent<Attack>();

            a.locDescStrings = new string[] { "splitcrit", "magnetic_to_crit" };
            a.CritDamagePerPeg = 6;
            a.Level = 2;

            orb1.AddComponent<Magnetic>();
            a.NextLevelPrefab = createForbe3(orb1);



            return orb1;
        }

        public static GameObject createForbe3(GameObject lastLevel)
        {
            var orb1 = OrbFactory.createOrbFromPrefab(lastLevel);

            orb1.name = "Forbe-Lvl3";
            Attack a = orb1.GetComponent<Attack>();
            a.CritDamagePerPeg = 7;
            a.Level = 3;

            a.NextLevelPrefab = null;

            return orb1;
        }

    }
}
