using UnityEngine;

/// <summary>
/// Singleton pattern.
/// This class can be called from anywhere and only exists one instance at a time.
/// </summary>
/// <typeparam name="T"></typeparam>

public class SingletonMonobehaviourDestroyOnLoad<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static T Instance
    {
        private set { }
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}