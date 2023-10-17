using System.Collections.Generic;
using System.Linq;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour
    {
        #region Properties
        public static List<Unit> Units = new List<Unit>();
        public const float SIGHT = 200.0f;
        public const float SQUARED_SIGHT = SIGHT * SIGHT;

        [Header("Component")]
        protected NavMeshAgent navMeshAgent;
        protected Animator animator;
        protected int[] enemyList;

        [Header("Status")]
        [SerializeField] protected int seq;
        protected float hp;
        protected float maxHP;
        protected float atk;

        [Header("Prefab")]
        public GameObject HpBar;
        private StatusBar hpBar; 
        #endregion

        

        #region MonoBehaviour Methods
        protected virtual void Awake()
        {
            TryGetComponent(out navMeshAgent);
            animator = GetComponentInChildren<Animator>();
        }

        protected virtual void Start()
        {
            InstantiateHPBar();
        }
        protected virtual void OnEnable() { }
        protected virtual void Update() { }
        protected virtual void OnDisable() { }
        #endregion

        #region Methods
        protected void SetEnemyLayer(params int[] layer)
        {
            enemyList = layer;
        }

        public void TakeDamage(float damage = 0f)
        {
            //if (hp <= 0.0f) return;
            hp -= damage;
            hpBar.SetValue(hp, maxHP);

            if (hp <= 0.0f) Death();
        }
        #endregion

        protected void InstantiateHPBar()
        {
            if (HpBar != null)
            {
                var mainCamera = Camera.main;
                var canvas = mainCamera.GetComponentInChildren<Canvas>();

                var hpBarObj = Instantiate(HpBar, canvas.transform);
                hpBarObj.transform.SetAsFirstSibling();

                hpBar = hpBarObj.GetComponent<StatusBar>();
                if (hpBar != null)
                {
                    hpBar.Initialize(transform);
                    hpBar.SetValue(hp, maxHP);
                }
            }
        }


        protected abstract void Death();

        protected bool CanReachWithUnit(float reach, Unit unit)
        {
            float distance = transform.position.GetSquaredDistance(unit.transform.position);
            return distance < reach * reach;
        }

        protected bool IsEnemy(Unit target)
        {
            for (int index = 0; index < enemyList.Length; index++)
            {
                if (target.gameObject.layer == enemyList[index]) return true;
            }
            return false;
        }

        protected Unit FindNearestEnemy()
        {
            return Units.Where(unit => IsEnemy(unit))
                .OrderBy(unit => transform.position.GetSquaredDistance(unit.transform.position))
                .FirstOrDefault();
        }

        protected Unit FindNearestLayerTarget(string layer)
        {
            return Units.Where(unit => unit.gameObject.layer == LayerMask.NameToLayer(layer))
                .OrderBy(unit => transform.position.GetSquaredDistance(unit.transform.position))
                .FirstOrDefault();
        }
    }
}

public static class UnitExtension
{
    public static float GetSquaredDistance(this Unit unit, Unit target)
    {
        float xDifference = unit.transform.position.x - target.transform.position.x;
        float zDifference = unit.transform.position.z - target.transform.position.z;
        return xDifference * xDifference + zDifference * zDifference;
    }
}