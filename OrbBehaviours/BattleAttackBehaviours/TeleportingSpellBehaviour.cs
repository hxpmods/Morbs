using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Morbs.OrbBehaviours.BattleAttackBehaviours
{
    public class TeleportingSpellBehavior : SpellBehavior
    {
		public AudioSource sfxAudioSource;
		public new void AnimationEnd()
		{
			if (Plugin.debug)
			{
				Plugin.logger.LogMessage("TSB Animation ended");
			}
			OnSpellEnded?.Invoke();
		}
		private void OnEnable()
		{

		//sfxAudioSource = (AudioSource)Traverse.Create(this).Field("_sfxAudioSource").GetValue();
			sfxAudioSource?.PlayOneShot(AttackSfx.SlowAttackClip);
		}
		public new void AnimationHit()
		{
			if (Plugin.debug)
			{
				Plugin.logger.LogMessage("TSB Animation hit");
			}
			OnSpellHit?.Invoke();
			sfxAudioSource?.PlayOneShot(AttackSfx.OnHitClip);
		}

	}
}
