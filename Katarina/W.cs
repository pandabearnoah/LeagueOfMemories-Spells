using System;
using LeagueSandbox.GameServer.Logic.GameObjects;
using LeagueSandbox.GameServer.Logic.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Logic.Scripting.CSharp;
using LeagueSandbox.GameServer.Logic.API;
using System.Linq;
using LeagueSandbox.GameServer;

namespace Spells
{
    public class KatarinaW : GameScript
    {
        public void OnActivate(Champion owner)
        {
        }

        public void OnDeactivate(Champion owner)
        {
        }

        public void OnStartCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            spell.spellAnimation("Spell2", owner);
            ApiFunctionManager.AddParticleTarget(owner, "katarina_W_cas.troy", owner);
            ApiFunctionManager.AddParticleTarget(owner, "katarina_w_mis.troy", owner);
            ApiFunctionManager.AddParticleTarget(owner, "katarina_w_tar.troy", owner);
            ApiFunctionManager.AddParticleTarget(owner, "katarina_W_Dagger", owner);

            //most of the particles have been found, still dont know how to get the spinning red and black tho.

        }

        public void OnFinishCasting(Champion owner, Spell spell, AttackableUnit target)
        {
            var damage = new[] { 40, 75, 110, 145, 180 }[spell.Level - 1] + (owner.GetStats().AbilityPower.Total * 0.6f) + (owner.GetStats().AttackDamage.Total * 0.25f);

            foreach (var enemyTarget in ApiFunctionManager.GetUnitsInRange(target, 375, true))
            {
                if (enemyTarget != owner && owner.GetDistanceTo(enemyTarget) < 375 && !ApiFunctionManager.UnitIsTurret(enemyTarget) && !ApiFunctionManager.UnitIsChampion(enemyTarget))
                {
                    enemyTarget.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                }
                else if(ApiFunctionManager.UnitIsChampion(enemyTarget) && enemyTarget != owner && owner.GetDistanceTo(enemyTarget) < 375 && !ApiFunctionManager.UnitIsTurret(enemyTarget))
                {
                    enemyTarget.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    var buff = ((ObjAIBase)target).AddBuffGameScript("KatarinaWBuff", "KatarinaWBuff", spell, -1, true);
                    ApiFunctionManager.CreateTimer(1.0f, () =>
                    {
                        owner.RemoveBuffGameScript(buff);
                    });
                }
            }
        }

        public void ApplyEffects(Champion owner, AttackableUnit target, Spell spell, Projectile projectile)
        {

        }

        public void OnUpdate(double diff)
        {
        }
    }
}
