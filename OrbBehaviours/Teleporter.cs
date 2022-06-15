using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Morbs.OrbBehaviours
{
    public class Teleporter : MonoBehaviour
    {

        public int maxPorts = 1;
        public int portCount = 0;

        public delegate void Teleported();
        public Teleported OnTeleported = delegate { };

        public bool CanPort => maxPorts > portCount;
        public int portsLeft => maxPorts - portCount;

        private TrailRenderer trail;

        public Vector3? lastFramePos = null;

        public void Start()
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }

        public void Update()
        {
            if (lastFramePos != null)
            {
                bool hasTeleported = (lastFramePos.Value - gameObject.transform.position).magnitude > 1.0f;
                
                trail.enabled = !hasTeleported;
                if (hasTeleported) trail.Clear();
            }


            lastFramePos = gameObject.transform.position;
        }

        public bool doTeleport()
        {
            if (CanPort)
            {

                OnTeleported.Invoke();

                var rigidBody = gameObject.GetComponent<Rigidbody2D>();
                rigidBody.isKinematic = true;
                rigidBody.GetComponent<Collider2D>().enabled = false;

                /*
                //Disable the trail
                var trail = rigidBody.GetComponentInChildren<TrailRenderer>();
                trail.emitting = false;
                trail.enabled = false;
                trail.Clear();
                */

                var teleportTo = rigidBody.position;

                teleportTo.y = 4.0f; //Top of field.

                rigidBody.position = teleportTo;

                rigidBody.GetComponent<Collider2D>().enabled = true;
                rigidBody.isKinematic = false;


                /*
                Plugin.logger.LogMessage("Resetting trail:");
                Plugin.logger.LogMessage(trail);

                //Reanable trail
                trail.enabled = true;
                trail.emitting = true;
                trail.Clear();
                */

               
                portCount++;


                return true;
            }

            return false;
        }
    }
}
