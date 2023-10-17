using System;
using UnitSystem;
using UnityEngine;

public class Monster : Unit
{
    protected MonsterScript script;
    protected BehaviourStateType behaviourState;

    [Header("Component")]
    private MonsterAnimCode monsterAnimCode;

    private Unit target;
    private bool isSpecial = false;

    protected override void Awake()
    {
        base.Awake();

        monsterAnimCode = GetComponentInChildren<MonsterAnimCode>();
        monsterAnimCode.AttackEvent = Attack;
        monsterAnimCode.FollowAfterAttackEvent = FollowAfterAttack;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Initialize();
    }

    private void Initialize()
    {
        Units.Remove(this);
        Units.Add(this);
        if (seq > 0)
        {
            script = MonsterScript.Get(seq);
        }
        script.reach = 25.0f;
        hp = maxHP = script.hp;
        SetEnemyLayer(LayerMask.NameToLayer("Crop"), LayerMask.NameToLayer("Player"));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Units.Remove(this);
    }

    private void FollowAfterAttack()
    {
        CancleAttack();
    }

    public void Attack()
    {
        if (target == null)
        {
            CancleAttack();
            return;
        }
        target.TakeDamage(script.atk);
    }

    public void CancleAttack()
    {
        animator.SetBool("Attack", false);
        behaviourState = BehaviourStateType.DETECT;
    }

    private void FixedUpdate()
    {
        if (behaviourState == BehaviourStateType.WAIT)
        {
                behaviourState = BehaviourStateType.DETECT;
        }
        else if (behaviourState == BehaviourStateType.DETECT)
        {
            animator.SetBool("Attack", false);
            target = FindNearestEnemy();
            if (target == null) return;

            if (CanReachWithUnit(script.reach, target))
            {
                behaviourState = BehaviourStateType.ATTACK;
            }
            else
            {
                animator.SetBool("isWalk", true);
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(target.transform.position);
            }
        }
        else if (behaviourState == BehaviourStateType.ATTACK)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("Attack", true);
            navMeshAgent.isStopped = true;
        }
    }

    public void SetSpecial(bool flag)
    {
        isSpecial = flag;
    }

    private bool CanAttack()
    {
        float distance = this.GetSquaredDistance(target);
        return distance < script.reach * script.reach;
    }

    protected override void Death()
    {
        // 아이템 드롭
        if (isSpecial)
        {
            GameManager.Instance.SpawnItem(transform);
        }
        ObjectPoolManager.Instance.doDestroy(gameObject);
    }
}