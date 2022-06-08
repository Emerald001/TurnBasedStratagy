using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationManager : MonoBehaviour
{
    public UnitManager Owner;

    //In order of; Throw, Summon, Call
    public Dictionary<string, float> AnimationTimes = new Dictionary<string, float>();

    private Animator animator;
    [HideInInspector] public GameObject WeaponHolder;

    public void Init() {
        AnimationTimes.Add("Throw", 2.3f);
        AnimationTimes.Add("Summon", 1.767f);
        AnimationTimes.Add("Call", 2.67f);

        animator = Owner.GetComponentInChildren<Animator>();
        WeaponHolder = Owner.GetComponentInChildren<WeaponHolderReference>().gameObject;
    }

    public void HitEnemy(UnitManager unit) => StartCoroutine(HitDelay(unit));
    public void WalkAnim(bool startOrStop) => animator.SetBool("Walking", startOrStop);
    public void AttackAnim() => animator.SetTrigger("Attack");
    public void HitAnim() => animator.SetTrigger("Hit");
    public void DeathAnim() => StartCoroutine(DeathDelay());

    public void AbilityAnim(string Trigger) {
        animator.SetTrigger(Trigger);
        if(AnimationTimes.ContainsKey(Trigger))
            StartCoroutine(HideWeapon(AnimationTimes[Trigger]));
    }

    public IEnumerator HideWeapon(float time) {
        WeaponHolder.SetActive(false);
        yield return new WaitForSeconds(time);
        WeaponHolder.SetActive(true);
    }

    public IEnumerator HitDelay(UnitManager Unit) {
        yield return new WaitForSeconds(.7f);
        Unit.UnitAnimator.HitAnim();
    }
    public IEnumerator DeathDelay() {
        yield return new WaitForSeconds(.5f);
        animator.SetTrigger("Death");
    }
}