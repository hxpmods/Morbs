using RNG.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Morbs.Factories
{
    public static class ScenarioFactory
    {
        public static Dictionary<string, MapDataScenario> vanillaScenarios = new Dictionary<string, MapDataScenario>();

        public static List<MapDataScenario>  modScenarios = new List<MapDataScenario>();

        public static void RegisterScenarios(List<MapDataScenario> scenarios)
        {
            foreach (MapDataScenario mapDataScenario in scenarios)
            {
                ScenarioFactory.vanillaScenarios[mapDataScenario.scenarioName] = mapDataScenario;
            }
        }

        public static MapDataScenario CreateFromBaseGame(string name , string scenarioName = "SunnyClearing", bool clearPrereqs = true)
        {
            var scenario = ScriptableObject.Instantiate<MapDataScenario>(ScenarioFactory.vanillaScenarios[scenarioName]);
            scenario.name = name;
            scenario.scenarioName = name;

            if (clearPrereqs) scenario.scenarioPreReqs = new ScenarioPreReq[0];

            return scenario;
        }

    }
}
