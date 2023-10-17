using UnitSystem;
using UnityEngine;

public class Player : Unit
{
    public enum PlayerAnimState { Idle, Move, Planting, Dead };

    [Header("Status")]
    private PlayerAnimState state;                     // 현재 플레이어 상태
    public int hasSeedCnt { get; private set; }        // 소유 씨앗 수

    [Header("Component")]
    private PlayerAnimCode playerAnimCode;

    protected override void Start()
    {
        hp = maxHP = 15;

        base.Start();

        Units.Add(this);
        animator = GetComponentInChildren<Animator>();
        playerAnimCode = GetComponentInChildren<PlayerAnimCode>();
        playerAnimCode.PlantEndEvent = PlantEnd;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        ClickToMove();
        ClickToPlant();
        Animate();
    }

    private void ClickToMove()
    {
        switch (state)
        {
            case PlayerAnimState.Planting:
            case PlayerAnimState.Dead:
                return;
        }
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                navMeshAgent.SetDestination(hit.point);
            }
        }
    }
    private void ClickToPlant()
    {
        if (hasSeedCnt <= 0 || state == PlayerAnimState.Planting)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.SetDestination(transform.position);
            state = PlayerAnimState.Planting;
            animator.SetBool("IsPlant", true);
        }
    }

    private void Animate()
    {
        animator.SetBool("IsWalk", navMeshAgent.velocity.magnitude > 0);
    }

    private void PlantEnd()
    {
        animator.SetBool("IsPlant", false);
        state = PlayerAnimState.Idle;

        // 채소 생성
        AddSeed(-1);
        GameManager.Instance.SpawnAlly(transform);
    }

    public void AddSeed(int value = 1)
    {
        if (hasSeedCnt > 5 || hasSeedCnt < 0)
            return;

        hasSeedCnt += value;
        hasSeedCnt = Mathf.Clamp(hasSeedCnt, 0, GameManager.Instance.MaxSeedCnt);
    }

    public void AddMaxSeed(int value = 1)
    {
        GameManager.Instance.MaxSeedCnt += value;
    }
    public void Heal(float value)
    {
        hp += value;
        hp = Mathf.Clamp(hp, 0f, maxHP);
    }

    protected override void Death()
    {
        PopupGameOver.SHOW(null);
    }
}