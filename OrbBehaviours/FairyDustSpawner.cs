using HarmonyLib;
using Morbs.PegBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Morbs.OrbBehaviours
{
    class FairyDustSpawner : MonoBehaviour
    {

        class FairyDust : MonoBehaviour
        {
            void OnCollisionEnter2D(Collision2D collision)
            {
               

                if (collision.collider.CompareTag("Peg"))
                {
                    collision.collider.gameObject.AddComponent<FairyDustPegBehaviour>();
                    Destroy(gameObject);
                }


            }

            private float time = 0.0f;
            public float lifeTime = 6.0f;
            void Update()
            {
 
                    time += Time.deltaTime;

                    if (time >= lifeTime)
                    {
                        Destroy(gameObject);
                    // execute block of code here
                    }
            }

        }

        Sprite dustSprite;

        public void Awake()
        {
            dustSprite = Plugin.LoadSpriteFromFile("dust.png", pixelsPerUnit: 8);
            ball = GetComponent<PachinkoBall>();
        }

        public void SpawnDust()
        {
            var fairyDust= new GameObject("FairyDust");

            fairyDust.transform.position = gameObject.transform.position;

            var sr = fairyDust.AddComponent<SpriteRenderer>();

            var rb = fairyDust.AddComponent<Rigidbody2D>();

            var cc =fairyDust.AddComponent<CircleCollider2D>();

            fairyDust.AddComponent<FairyDust>();

            sr.sprite = dustSprite;
            sr.transform.localScale = Vector2.one* 0.4f;

            rb.gravityScale = 0.4f;
            rb.velocity = -ball.GetComponent<Rigidbody2D>().velocity;
            rb.velocity *= new Vector2(0.2f,0.05f);

            cc.radius = 1.0f / 8.0f;

            fairyDust.layer = 21;
            Physics2D.IgnoreLayerCollision(21,13);
            Physics2D.IgnoreLayerCollision(21,21);
        }

        private float time = 0.0f;
        public float interpolationPeriod = 0.4f;

        PachinkoBall ball;



        void Update()
        {
            if (ball.shotTime > 0.0f)
            {
                time += Time.deltaTime;

                if (time >= interpolationPeriod)
                {
                    time = time - interpolationPeriod;
                    SpawnDust();
                    // execute block of code here
                }
            }
        }

    }
}
