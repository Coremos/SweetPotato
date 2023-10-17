using UnityEngine;

public enum BehaviourStateType { WAIT = 0, DETECT, ATTACK }

public abstract class Behaviour : MonoBehaviour
{
}

public class AllyBehaviour : Behaviour
{
}

public class NeutralBehaviour
{
}