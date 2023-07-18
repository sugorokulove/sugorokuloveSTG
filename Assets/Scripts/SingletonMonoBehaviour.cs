using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    protected static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance != null)
            {
                return m_instance;
            }

            m_instance = (T)FindObjectOfType<T>(true);

            if (m_instance == null)
            {
                Debug.LogWarning($"SingletonMonoBehaviour:cannot find {typeof(T)}.");
            }

            return m_instance;
        }
    }

    public static T TryInstance => m_instance;

    private void Awake()
    {
        if (CheckInstance())
        {
            Initialize();
        }
    }

    protected abstract void Initialize();

    protected bool CheckInstance()
    {
        if (m_instance == null)
        {
            m_instance = (T)this;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }

    protected virtual void OnDestroy()
    {
        OnDestroyChild();

        if (m_instance == this)
        {
            m_instance = null;
        }
    }

    protected virtual void OnDestroyChild()
    {
    }
}
