using UnityEngine;

public class SingleTone<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    /// <summary>
    /// Returns the instance of this singleton.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 기존 씬에 있는지 없는지 탐색
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    // 오브젝트 없으면 그떄 새로 생성
                    GameObject obj = new GameObject(typeof(T).ToString());
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
}