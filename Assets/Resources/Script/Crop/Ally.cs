using System.Collections;
using UnitSystem;
using UnityEngine;

public class Ally : Unit
{
    #region Properties
    private Unit player;
    public enum CharacterState { Born, Grow, Activated };
    protected CharacterState state;
    protected BehaviourStateType behaviourState;

    protected CropScript script;
    protected Unit target;

    private AnimCode animCode;
    #endregion

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Initialize();
    }

    protected override void Update()
    {
        Debug.Log(behaviourState);
    }
    #endregion

    #region Methods
    private void Initialize()
    {
        if (seq > 0)
        {
            script = CropScript.Get(seq);
        }
        hp = maxHP = script.hp;
        script.reach = 30.0f;
        state = CharacterState.Born;
        player = FindNearestLayerTarget("Player");
        StartCoroutine(UpdateGrow());
        animCode = GetComponentInChildren<AllyAnimCode>();
        animCode.AttackEvent = Attack;
        animCode.FollowAfterAttackEvent = FollowAfterAttack;
        SetEnemyLayer(LayerMask.NameToLayer("Monster"));
    }

    private void FollowAfterAttack()
    {
        animator.SetBool("Attack", false);
        behaviourState = BehaviourStateType.DETECT;
    }

    public void Attack()
    {
        if (target == null)
        {
            behaviourState = BehaviourStateType.DETECT;
            return;
        }
        target.TakeDamage(script.atk);
    }

    private void FixedUpdate()
    {
        if (state != CharacterState.Activated) return;

        if (behaviourState == BehaviourStateType.WAIT)
        {
            animator.SetBool("Attack", false);
            target = FindNearestEnemy();

            if (target != null && this.GetSquaredDistance(target) > SQUARED_SIGHT)
            {
                target = null;
            }

            if (target == null)
            {
                if (CanReachWithUnit(script.reach, player))
                {
                    navMeshAgent.SetDestination(transform.position);
                    navMeshAgent.isStopped = true;
                    animator.SetBool("isWalk", false);
                }
                else
                {
                    animator.SetBool("isWalk", true);
                    navMeshAgent.SetDestination(player.transform.position);
                    navMeshAgent.isStopped = false;
                }
            }
            else
            {
                behaviourState = BehaviourStateType.DETECT;
            }
        }
        else if (behaviourState == BehaviourStateType.DETECT)
        {
            animator.SetBool("Attack", false);
            if (target == null)
            {
                behaviourState = BehaviourStateType.WAIT;
                return;
            }

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
            //if(target == null)
            //{
            //    behaviourState = BehaviourStateType.WAIT;
            //}
            animator.SetBool("isWalk", false);
            animator.SetBool("Attack", true);
            navMeshAgent.isStopped = true;
        }
    }

    private IEnumerator UpdateGrow()
    {
        float scale = 0.1f;
        while (scale < 2.0f)
        {
            scale += Time.deltaTime;
            transform.localScale = Vector3.one * scale;
            yield return null;
        }
        state = CharacterState.Grow;
    }

    public bool SetActivate()
    {
        if (state != CharacterState.Grow) return false;
        state = CharacterState.Activated;
        Units.Remove(this);
        Units.Add(this);
        return true;
    }

    protected override void Death()
    {
        ObjectPoolManager.Instance.doDestroy(gameObject);
        Units.Remove(this);
    }
    #endregion
}