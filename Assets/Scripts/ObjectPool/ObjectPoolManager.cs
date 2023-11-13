using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インスペクタ上で指定するデータ
/// </summary>
[Serializable]
public class ObjectPoolData
{
    [SerializeField]
    ObjectType m_objectType = ObjectType.None;
    public ObjectType PoolObjectType => m_objectType;

    [SerializeField]
    GameObject m_prefab = null;
    public GameObject Prefab
    {
        get { return m_prefab; }
        set { m_prefab = value; }
    }
}

/// <summary>
/// オブジェクトプール一括管理（IPoolableが付与されているオブジェクトが対象）
/// </summary>
public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
{
    // 返却場所
    [SerializeField] Transform m_PoolRoot;

    // 読み込み対象設定
    [SerializeField] List<ObjectPoolData> m_objectPoolDatas = null;

    // オブジェクトプールリスト(ランタイム蓄積)
    Dictionary<ObjectType, ShootingObjectPool> m_pools = new Dictionary<ObjectType, ShootingObjectPool>();

    protected override void Initialize() { }

    /// <summary>
    /// 破棄
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var pool in m_pools.Values)
        {
            pool.Clear();
            pool.Dispose();
        }
        m_pools.Clear();
        m_pools = null;
    }

    /// <summary>
    /// ObjectPool作成
    /// </summary>
    public void CreatePoolTask()
    {
        foreach (var pool in m_objectPoolDatas)
        {
            if ((pool.PoolObjectType != ObjectType.None) &&
                (pool.Prefab != null))
            {
                m_pools[pool.PoolObjectType] = new ShootingObjectPool(pool.Prefab);
            }
        }
    }

    /// <summary>
    /// オブジェクト生成
    /// </summary>
    /// <param name="objectType">生成するオブジェクトタイプ</param>
    /// <returns></returns>
    public MonoBehaviour Rent(ObjectType objectType)
    {
        MonoBehaviour newObject = null;

        if (m_pools.TryGetValue(objectType, out var pool))
        {
            //既にプールがある
            newObject = pool.Rent();
        }
        else
        {
            //まだプールがない
            Debug.Assert(false, "プールがない:" + objectType);
        }

        newObject.transform.SetParent(null);

        return newObject;
    }

    /// <summary>
    /// オブジェクト返却
    /// </summary>
    /// <param name="obj">返却するオブジェクト</param>
    public void Return(MonoBehaviour obj)
    {
        if (!obj) { return; }

        if (obj is IPoolable pool)
        {
            obj.transform.SetParent(m_PoolRoot);
            obj.transform.localPosition = Vector3.zero;
            m_pools[pool.BaseObjectType].Return(obj);
        }
        else
        {
            Debug.Assert(false, "IPoolableでないオブジェクトを返却しようとしました:" + obj);
        }
    }
}
