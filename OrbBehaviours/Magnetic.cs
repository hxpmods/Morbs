
using HarmonyLib;
using UnityEngine;

namespace Morbs
{
    public class Magnetic : MonoBehaviour
	{

		public Collider2D[] _nearbyPegs = new Collider2D[20];
		public bool toBombs = false;
		public bool toCrits = true;
		public bool toResets = false;

		public float strongStrength = 8f; // 6.5f
		public float weakStrength = 0.0f; // 0.9f long pegs or 1.1f

		public void DoMagnetAttraction(PhysicsScene2D physicsScene)
		{



            PachinkoBall ball =gameObject.GetComponent<PachinkoBall>();
			var traverse = Traverse.Create(ball);

			//Plugin.logger.LogMessage(ball);


			int state = (int)traverse.Field("_state").GetValue();

			var circleCastLayerMask = traverse.Field(" _circleCastLayerMask").GetValue();

			//Plugin.logger.LogMessage("Doing");
			//Plugin.logger.LogMessage(circleCastLayerMask);

			Rigidbody2D rigid = (Rigidbody2D)traverse.Field("_rigid").GetValue();

			Collider2D[] _nearbyPegs = (Collider2D[])traverse.Field("_nearbyPegs").GetValue();


			//FireballState.FIRING == 2
			
			physicsScene.OverlapCircle(base.transform.position, 2.5f, _nearbyPegs, (LayerMask)33792);
			Collider2D[] nearbyPegs = _nearbyPegs;


			foreach (Collider2D collider2D in nearbyPegs)
			{
				
				if (!(collider2D != null))
				{
					continue;
				}
				LongPeg component = collider2D.GetComponent<LongPeg>();
				if (component != null && !component.hit)
				{
					Vector2 vector = component.GetCenterOfPeg() - base.transform.position;
					float num = vector.magnitude / 2.5f;
					if (num > 0.05f)
					{
						float a = (((component.pegType == Peg.PegType.RESET && toResets == true) || (component.pegType == Peg.PegType.CRIT && toCrits == true)) ? strongStrength : weakStrength);
						//a = Mathf.Lerp(a, 0f, t);
						//_rigid.AddForce(vector.normalized * (a / (num * num)));
						rigid.AddForce(vector.normalized * (a / (num * num)));
					}
					continue;
				}
				Bomb component2 = collider2D.GetComponent<Bomb>();
				RegularPeg component3 = collider2D.GetComponent<RegularPeg>();
				if (component2 != null && !component2.detonated && toBombs == true)
				{
					Vector2 vector2 = component2.transform.position - base.transform.position;
					float num2 = vector2.magnitude / 2.5f;
					if (num2 > 0.05f)
					{
						float a2 = 1.1f;
						//a2 = Mathf.Lerp(a2, 0f, t);
						//_rigid.AddForce(vector2.normalized * (a2 / (num2 * num2)));

						rigid.AddForce(vector2.normalized * (a2 / (num2 * num2)));
					}
				}
				else if (component3 != null && !component3.IsDisabled() && component3.pegType != Peg.PegType.DULL)
				{
					Vector2 vector3 = component3.transform.position - base.transform.position;
					float num3 = vector3.magnitude / 2.5f;
					if (num3 > 0.05f)
					{
						float a3 = (((component3.pegType == Peg.PegType.RESET && toResets == true)|| (component3.pegType == Peg.PegType.CRIT && toCrits == true)) ? strongStrength : weakStrength);
						//a3 = Mathf.Lerp(a3, 0f, t);
						//_rigid.AddForce(vector3.normalized * (a3 / (num3 * num3)));
						rigid.AddForce(vector3.normalized * (a3 / (num3 * num3)));
					}
				}
			
			}
			/*
			*/
		}

	}
}
