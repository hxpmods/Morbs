using HarmonyLib;
using I2.Loc;
using Morbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Morbs.Factories
{
    public static class OrbFactory
    {
        public static Dictionary<string, GameObject> orbPrefabs = new Dictionary<string, GameObject>();
        public static OrbPool availableOrbPool;

        public static bool doClear = false;

        private static GameObject orbHolder;
        private static GameObject attackPrefabHolder;

        public static List<GameObject> modOrbs = new List<GameObject>();
        public static List<GameObject> allModOrbs = new List<GameObject>();

        public static void logPrefabData()
        {
            foreach (KeyValuePair<string, GameObject> pair in orbPrefabs)
            {
                Plugin.logger.LogInfo(pair);
            }
        }

        public static void ClearBaseGameOrbPool(bool doclear)
        {
            doClear = doclear;
        }

        public static void CollectOrbPool()
        {
            OrbPool[] orbPools = Resources.FindObjectsOfTypeAll<OrbPool>();

            foreach (OrbPool orbPool in orbPools)
            {
                if (Plugin.debug)
                {
                    Plugin.logger.LogMessage(orbPool.name);
                }
                foreach (GameObject orb in orbPool.AvailableOrbs)
                {
                    OrbFactory.orbPrefabs.Add(orb.name, orb);

                }
            }
        }

        public static void SetOrbPoolForGame()
        {
            List<GameObject> base_game = orbPrefabs.Values.ToList();
            List<GameObject> modded = modOrbs;

            var result = new List<GameObject>();

            if (!doClear)
            {
                result.AddRange(base_game);
            }

            result.AddRange(modded);

            availableOrbPool.AvailableOrbs = result.ToArray();

            foreach (GameObject orb in availableOrbPool.AvailableOrbs)
            {
                Plugin.logger.LogMessage(orb);
            }

        }


        public static GameObject[] createModTargetedAttackPrefabs(string fromOrb = "LightningBall-Lvl1")
        {
            if (!attackPrefabHolder)
            {
                attackPrefabHolder = new GameObject("ModAttackPrefabHolder");
                UnityEngine.Object.DontDestroyOnLoad(attackPrefabHolder);
                attackPrefabHolder.SetActive(false);
            }

            var orb = orbPrefabs.GetValueSafe(fromOrb);
            var attack = orb.GetComponent<TargetedAttack>();

            var t = Traverse.Create(attack);
            GameObject cs = (GameObject)t.Field("_thunderPrefab").GetValue();
            GameObject s = (GameObject)t.Field("_criticalThunderPrefab").GetValue();

            GameObject critPrefab = UnityEngine.Object.Instantiate(cs, attackPrefabHolder.transform);
            GameObject prefab = UnityEngine.Object.Instantiate(s, attackPrefabHolder.transform);

            return new GameObject[] { prefab, critPrefab };

        }

        public static GameObject[] createModFireballAttackPrefabs(string fromOrb = "StoneOrb-Lvl1")
        {
            if (!attackPrefabHolder)
            {
                attackPrefabHolder = new GameObject("ModAttackPrefabHolder");
                UnityEngine.Object.DontDestroyOnLoad(attackPrefabHolder);
                attackPrefabHolder.SetActive(false);
            }

            var orb = orbPrefabs.GetValueSafe(fromOrb);
            var attack = orb.GetComponent<FireballAttack>();

            var t = Traverse.Create(attack);
            GameObject cs = (GameObject)t.Field("_criticalShotPrefab").GetValue();
            GameObject s = (GameObject)t.Field("_shotPrefab").GetValue();

            GameObject critPrefab = UnityEngine.Object.Instantiate(cs, attackPrefabHolder.transform);
            GameObject prefab = UnityEngine.Object.Instantiate(s, attackPrefabHolder.transform);

            return new GameObject[] { prefab, critPrefab };

        }

        public static GameObject createOrbFromName(string fromOrb = "StoneOrb-Lvl1")
        {
            if (!orbHolder)
            {
                orbHolder = new GameObject("ModOrbHolder");
                UnityEngine.Object.DontDestroyOnLoad(orbHolder);
                orbHolder.SetActive(false);
            }

            GameObject orb = UnityEngine.Object.Instantiate(orbPrefabs.GetValueSafe(fromOrb), orbHolder.transform);

            allModOrbs.Add(orb);

            return orb;
        }

        public static GameObject createOrbFromPrefab(GameObject prefab)
        {
            if (!orbHolder)
            {
                orbHolder = new GameObject("ModOrbHolder");
                UnityEngine.Object.DontDestroyOnLoad(orbHolder);
                orbHolder.SetActive(false);
            }

            GameObject orb = UnityEngine.Object.Instantiate(prefab, orbHolder.transform);

            return orb;
        }


        public static void setFireballAttackPrefabs(FireballAttack attackComp, GameObject[] prefabs)
        {

            var t = Traverse.Create(attackComp);
            t.Field("_shotPrefab").SetValue(prefabs[0]);
            t.Field("_criticalShotPrefab").SetValue(prefabs[1]);

        }

        public static void setTargetedAttackPrefabs(TargetedAttack attackComp, GameObject[] prefabs)
        {

            var t = Traverse.Create(attackComp);
            t.Field("_thunderPrefab").SetValue(prefabs[0]);
            t.Field("_criticalThunderPrefab").SetValue(prefabs[1]);

        }

    }


}
