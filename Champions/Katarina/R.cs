using System;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;
using LeagueSandbox.GameServer.Logic.API;
using System.Linq;
using LeagueSandbox.GameServer;

namespace Spells
{
    public class KatarinaR : GameScript
    {
        public void OnActivate(Champion owner)
        {
        }

        public void OnDeactivate(Champion owner)
        {
        }

        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            spell.spellAnimation("Spell4", owner);
            ApiFunctionManager.AddParticle(owner, "katarina_deathLotus_tar.troy", owner.X, owner.Y);
            ApiFunctionManager.AddParticle(owner, "katarina_deathLotus_mis.troy", owner.X, owner.Y);
            ApiFunctionManager.AddParticle(owner, "Katarina_deathLotus_cas.troy", owner.X, owner.Y);

            foreach (var enemyTarget in ApiFunctionManager.GetUnitsInRange(target, 550, true))
            {
                if (enemyTarget != owner && owner.GetDistanceTo(enemyTarget) < 550 && !ApiFunctionManager.UnitIsTurret(enemyTarget) && !ApiFunctionManager.UnitIsChampion(enemyTarget))
                {
                    ApiFunctionManager.AddParticle(owner, "katarina_deathlotus_success.troy", owner.X, owner.Y);
                    ApiFunctionManager.AddParticle(owner, "katarina_bouncingBlades_mis.troy", enemyTarget.X, enemyTarget.Y);
                    ApiFunctionManager.CreateTimer(0.25f, () =>
                    {
                        ApiFunctionManager.AddParticle(owner, "katarina_deathlotus_success.troy", owner.X, owner.Y);
                        ApiFunctionManager.AddParticle(owner, "katarina_bouncingBlades_mis.troy", enemyTarget.X, enemyTarget.Y);
                    });
                }
            }

        }

        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            var damagePerDagger = new[] { 35, 55, 75 }[spell.Level - 1] + (owner.GetStats().AbilityPower.Total * 0.25f) + (owner.GetStats().AttackDamage.Total * 0.375f);

            foreach (var enemyTarget in ApiFunctionManager.GetUnitsInRange(target, 550, true))
            {
                if (enemyTarget != owner && owner.GetDistanceTo(enemyTarget) < 550 && !ApiFunctionManager.UnitIsTurret(enemyTarget) && !ApiFunctionManager.UnitIsChampion(enemyTarget))
                {
                    enemyTarget.TakeDamage(owner, damagePerDagger, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    ApiFunctionManager.CreateTimer(0.25f, ()=>
                    {
                        enemyTarget.TakeDamage(owner, damagePerDagger, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    });
                }
            }
        }

        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {
            projectile.setToRemove();
        }

        public void OnUpdate(double diff)
        {

        }
    }
}

