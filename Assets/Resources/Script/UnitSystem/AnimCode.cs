using UnityEngine;
using UnityEngine.Events;

public class AnimCode : MonoBehaviour
{
    public UnityAction AttackEvent;
    public UnityAction FollowAfterAttackEvent;

    public void Attack()
    {
        AttackEvent?.Invoke();
    }
    public void FollowAfterAttack()
    {
        FollowAfterAttackEvent?.Invoke();
    }
}