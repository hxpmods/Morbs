using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Morbs.PegBehaviours
{
    public class FairyDustPegBehaviour : MonoBehaviour
    {
        public int strength = 1;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("PachinkoBall"))
            {
                Vector2 vector = collision.collider.transform.position - base.transform.position;
                collision.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(vector.normalized * 100f * strength);
                Plugin.logger.LogMessage("Colliding");
            }

        }
        public void OnEnable()
        {
            var p = GetComponent<Peg>();
            Traverse.Create(p).Method("UpdateBuff", strength).GetValue();
        }

        public void OnDisable()
        {
            var p = GetComponent<Peg>();
            Traverse.Create(p).Method("UpdateBuff", -strength).GetValue();
        }

    }
}
