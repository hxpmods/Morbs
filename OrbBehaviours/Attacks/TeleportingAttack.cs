using Battle.Enemies;
using Battle.StatusEffects;
using HarmonyLib;
using I2.Loc;
using PeglinUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Morbs;
using Morbs.OrbBehaviours.BattleAttackBehaviours;

namespace Morbs.OrbBehaviours.Attacks
{
    public class TeleportingAttack : TargetedAttack
    {
        public int shotsLeft = 1;

		public LocalizationParamsManager paramsManager
		{
			get
			{
				if (_paramsManager == null)
				{
					_paramsManager = GetComponent<LocalizationParamsManager>();
				}
				return _paramsManager;
			}
		}

		private LocalizationParamsManager _paramsManager;

		public override void Fire(AttackManager attackManager, Enemy target, float[] dmgValues, float dmgMult, int dmgBonus, int critCount = 0)
        {
            _target = target;
            _hitDamage = GetDamage(attackManager, dmgValues, dmgMult, dmgBonus, critCount);

			var t = Traverse.Create(this);
			TeleportingSpellBehavior _spell = (TeleportingSpellBehavior)t.Field("_spell").GetValue();

			if (!_spell.sfxAudioSource){
				_spell.sfxAudioSource = attackManager.SfxAudioSource;
			}

	

			SpawnSpell(target, critCount);
        }

		public override void UpdateBattleParameters()
		{
			base.UpdateBattleParameters();
			paramsManager.SetParameterValue("level", Level.ToString());

			var a = GetComponent<Teleporter>();
			paramsManager.SetParameterValue("portsLeft", a.portsLeft.ToString());
		}

		public override void ClearBattleParameters()
		{
			base.ClearBattleParameters();
			paramsManager.SetParameterValue("level", Level.ToString());

			var a = GetComponent<Teleporter>();
			paramsManager.SetParameterValue("portsLeft", a.portsLeft.ToString());
		}

		private new void SpawnSpell(Enemy target, int critCount)
        {
			Plugin.logger.LogMessage("Spawning spell");
			var t = Traverse.Create(this);

			SpellBehavior _critSpell = (SpellBehavior)t.Field("_critSpell").GetValue();
			SpellBehavior _spell = (SpellBehavior)t.Field("_spell").GetValue();


			SpellBehavior spellBehavior = _spell;//((critCount > 0 && _hitDamage > 0f) ? _critSpell : _spell);
			//_lastAttackCrit = critCount > 0;

			if (_target != null && !_target.gameObject.activeSelf && target != null && !target.gameObject.activeSelf)
			{
				_target = target;
			}

			if (target != null && target.gameObject.activeSelf)
			{
				_target = target;
			}
			if (_target == null || !_target.gameObject.activeSelf)
			{
				_attackManager.AttackAnimationEnded();
				return;
			}

			spellBehavior.gameObject.transform.position = target.GetComponentInChildren<TargetingUI>(includeInactive: true).transform.position;

			spellBehavior.gameObject.SetActive(value: false);
			spellBehavior.gameObject.SetActive(value: true);

			//Doing this is necassary to get any of it working
			//Without this the next attack does not strike
			spellBehavior.OnSpellHit = new SpellBehavior.SpellHit(HandleSpellHit);
			spellBehavior.OnSpellEnded = new SpellBehavior.SpellHit(HandleSpellEnded);
		}


		private Enemy GetRandomLivingTarget()
		{
			EnemyManager enemyManager = GameObject.FindObjectOfType<EnemyManager>();

			List<Enemy> enemies = new List<Enemy>(enemyManager.Enemies);
			List<Enemy> alive_enemies = new List<Enemy>();

			foreach (Enemy _enemy in enemies)
			{
				if (_enemy.CurrentHealth > 0 && _enemy.gameObject.activeSelf)
				{
					alive_enemies.Add(_enemy);
				}
			}

			if (alive_enemies.Count > 0)
			{
				var result = alive_enemies[UnityEngine.Random.Range(0, alive_enemies.Count)];
				return result;
			}
			else
			{
				return null;
			}


		}

		private void AttemptRetargetSpell()
        {
			if (shotsLeft > 0)
			{

				shotsLeft--;

				var target = GetRandomLivingTarget();
				if (target != null)
				{
					Plugin.logger.LogMessage("Shooting again");
					Plugin.logger.LogMessage("Target: " + target.name);

					_target = target;
					SpawnSpell(target, 0);
                }
                else
                {
					Plugin.logger.LogMessage("Cannot find target to shoot again");
					shouldEndSpell = true;
					HandleSpellEnded();
                }


			}
		}
		private bool shouldEndSpell = false;

		private void HandleSpellEnded()
        {
			if (shouldEndSpell)
			{
				//The SpellBehaviour needs to a TeleportingSpellBehaviour for this to work. Otherwise, handle spell ended destroys the object before it can be teleported
				_attackManager.AttackAnimationEnded();
				Plugin.logger.LogMessage("Spell ended");

				//We should destroy the prefab(s) here to clean up


				return;
			}

			AttemptRetargetSpell();

			if ( shotsLeft < 1)
            {
				shouldEndSpell = true;
            }




        }

		private void HandleSpellHit()
		{
			if (_target == null || !_target.gameObject.activeSelf)
			{
				return;
			}
			DealHitDamageToEnemy(_target);


		}

		private void DealHitDamageToEnemy(Enemy enemy)
		{
			enemy.DamageWithTypeMods(_hitDamage, enemy.spellAttackDamageMod);
			foreach (StatusEffect statusEffect in GetStatusEffects())
			{
				if (statusEffect.EffectType != 0)
				{
					enemy.ApplyStatusEffect(statusEffect);
				}
			}
			_attackManager.WaitForEnemyDamageAnimationToEnd(enemy);
		}

	}
}
