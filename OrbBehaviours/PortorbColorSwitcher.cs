using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Morbs.OrbBehaviours
{
    class PortorbColorSwitcher : MonoBehaviour
    {
        bool isOrange = false;
        TrailRenderer orbTrail;
        Color blue = new Color(0.45f, 0.83f, 0.92f, 0.0f);
        Color orange = new Color(0.88f, 0.45f, 0.07f, 0.0f);

        public void Awake()
        {
            orbTrail = GetComponentInChildren<TrailRenderer>();
            orbTrail.endColor = blue;

            var teleporter = GetComponentInChildren<Teleporter>();

            teleporter.OnTeleported += SwitchColours;
        }

        public void SwitchColours()
        {
            if (!isOrange)
            {
                orbTrail.endColor = orange;
                isOrange = true;
            }
            else
            {
                orbTrail.endColor = blue;
            }
        }
    }
}
