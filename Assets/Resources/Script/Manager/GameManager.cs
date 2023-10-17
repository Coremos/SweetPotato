using System.Collections;
using System.Collections.Generic;
using UnitSystem.PickupSystem;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 해당 클래스는 정적 영역으로부터 타 영역과 공유됩니다.
/// 단, 싱글톤 클래스는 아니니, 반드시 생성(및 활성화) => Awake 이후 사용해주세요.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Constant Values")]
    private readonly Vector3 WorldCenterPoint = new Vector3(512f, 0f, 512f);

    [Header("SingleTone")]
    public static GameManager Instance = null;

    [Header("Control")]
    public int MaxSeedCnt = 5;
    private int curSeedCnt = 0;
    public int DayCnt { get; private set; }
    public int MaxItemspawnMobCnt = 10;
    private int spawnMobCnt = 0;

    // 인게임 규칙
    //   몬스터를 스폰한다.
    //   씨앗을 스폰한다.
    //   플레이어가 씨앗을 습득한다.
    //   플레이어가 씨앗을 소모해 심는 액션을 취한다.(=> 채소 오브젝트 생성)
    //   채소 오브젝트가 점점 위로 올라온다. (특정 임계치까지)
    //   임계치까지 올라온 채소 오브젝트 근처에 플레이어가 다가오면 채소 오브젝트의 활동을 개시한다.

    //   채소가 몬스터를 공격한다.
    //   몬스터가 채소를 공격한다.
    //   몬스터가 플레이어를 공격한다.
    //   플레이어 체력 0시, 게임 Pause, 게임 오버 팝업 출력

    public Player Player;

    private List<Pickupable> SpawnedPickupables = new List<Pickupable>();
    [SerializeField] private Seed seed;
    Coroutine SeedSpawn;

    // Ally
    private List<GameObject> AllyOriginList = new List<GameObject>();
    private List<Ally> SpawnedAlly = new List<Ally>();
    [SerializeField] private GameObject Ally_Carrot;
    [SerializeField] private GameObject Ally_Potato;
    [SerializeField] private GameObject Ally_Radish;

    [SerializeField] private GameObject Ally_Jinseng;
    [SerializeField] private GameObject Ally_Onion;
    // Item
    private List<GameObject> ItemOriginList = new List<GameObject>();
    private List<Item> SpawnedItem = new List<Item>();
    [SerializeField] private GameObject Item_Heart;
    [SerializeField] private GameObject Item_Seed;

    // Monster 
    private List<GameObject> MonOriginList = new List<GameObject>();
    private List<Monster> SpawnedMonster = new List<Monster>();

    [SerializeField] private GameObject Monster_Mole;
    public Transform[] monSpawnPoints;
    Coroutine MonsterSpawn;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InitPrefabs();
        StartGame();
    }

    // Initalize and Start Games
    private void InitPrefabs()
    {
        Player = FindObjectOfType<Player>();

        AllyOriginList.Add(Ally_Radish);
        AllyOriginList.Add(Ally_Potato);
        AllyOriginList.Add(Ally_Carrot);
        AllyOriginList.Add(Ally_Onion);
        AllyOriginList.Add(Ally_Jinseng);

        ItemOriginList.Add(Item_Heart);
        ItemOriginList.Add(Item_Seed);

        MonOriginList.Add(Monster_Mole);
    }
    private void StartGame()
    {
        UIViewManager.Instance.GoView(View.StartView);

        StartSpawnSeeds();
        StartSpawnMonsters();

        PauseGame(true);
    }
    // Spawn Seeds
    private void StartSpawnSeeds()
    {
        // 초기 10개 드랍
        for (int i = 0; i < 5; i++)
            SpawnSeed();

        if (SeedSpawn != null)
            StopCoroutine(SeedSpawn);
        SeedSpawn = StartCoroutine(coSpawnSeed());
    }
    private void SpawnSeed()
    {
        Vector3 pos = RandomVectorCreator.GetStandWorldPosition(WorldCenterPoint);
        var prefab = ObjectPoolManager.Instance.doInstantiate(seed.gameObject, pos);
        prefab.transform.position = pos;

        Pickupable pickup = prefab.GetComponent<Pickupable>();
        if (pickup == null)
        {
            ObjectPoolManager.Instance.doDestroy(prefab);
            return;
        }

        SpawnedPickupables.Add(pickup);
        curSeedCnt++;
    }
    IEnumerator coSpawnSeed()
    {
        WaitForSeconds wait = new WaitForSeconds(25f);
        while (true)
        {
            if (isGamePaused())
            {
                yield return null;
                continue;
            }

            SpawnSeed();
            yield return wait;
        }
    }
    // Spawn Monsters
    private void StartSpawnMonsters()
    {
        if (MonsterSpawn != null)
            StopCoroutine(MonsterSpawn);
        MonsterSpawn = StartCoroutine(coSpawnMonster());
    }
    private void SpawnMonster()
    {
        var point = monSpawnPoints[Random.Range(0, monSpawnPoints.Length)];

        var pos = point.position;
        pos.y = RandomVectorCreator.GetPositionHeight(pos);

        var mob = ObjectPoolManager.Instance.doInstantiate(Monster_Mole, pos);
        mob.transform.position = pos;

        var mobComp = mob.GetComponent<Monster>();
        if (spawnMobCnt >= 1)
        {
            mobComp.SetSpecial(true);
            spawnMobCnt = 0;
            return;
        }

        SpawnedMonster.Add(mobComp);
        spawnMobCnt++;
    }

    IEnumerator coSpawnMonster()
    {
        WaitForSeconds wait = new WaitForSeconds(15f);
        while (true)
        {
            if (isGamePaused())
            {
                yield return null;
                continue;
            }

            SpawnMonster();
            SpawnMonster();
            yield return wait;
        }
    }

    // Spawn Allies
    public void SpawnAlly(Transform Origin)
    {
        GameObject randObj = AllyOriginList[Random.Range(0, AllyOriginList.Count)];
        var prefab = ObjectPoolManager.Instance.doInstantiate(randObj, Origin.position);
        prefab.transform.position = Origin.position;
        prefab.transform.rotation = Origin.rotation;
        SpawnedAlly.Add(prefab.GetComponent<Ally>());
    }

    // Spawn Item
    public void SpawnItem(Transform Origin)
    {
        GameObject randObj = ItemOriginList[Random.Range(0, ItemOriginList.Count)];
        var prefab = ObjectPoolManager.Instance.doInstantiate(randObj, Origin.position);
        prefab.transform.position = Origin.position;
        SpawnedItem.Add(prefab.GetComponent<Item>());
    }


    // Game Controll
    public void PauseGame(bool flag)
    {
        Time.timeScale = flag ? 0f : 1f;
    }
    public bool isGamePaused()
    {
        return Time.timeScale <= 0f;
    }
    public void EndGame()
    {
        PauseGame(true);
        PopupGameOver.SHOW(null);
    }
    public void GoToNextStage()
    {
        DayCnt++;

        Instance.PauseGame(true);
        if (DayCnt > 3)
        {
            PopupGameOver.SHOW(null);
        }
        else
        {
            StageDayPopup.Show(null, string.Format("Day {0}", DayCnt), () =>
            {
                Instance.PauseGame(false);
            });
        }
    }
    // Get,Set
    public int GetPlayerSeed()
    {
        return Player.hasSeedCnt;
    }
}