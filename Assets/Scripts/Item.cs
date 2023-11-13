using UnityEngine;

public class Item : ObjectBase, IPoolable
{
    [SerializeField] private int m_score;
    private Vector3 m_move = Vector3.zero;

    public ObjectType BaseObjectType { get; set; } = ObjectType.Item;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    public void Init(Vector3 position)
    {
        ObjectBaseInitialize();

        Transform.position = position;

        m_move = new Vector3(0.0f, Speed, 0.0f);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Transform.position += m_move;

        if (JudgeOutOfScreenBottom())
        {
            Remove();
        }
    }

    /// <summary>
    /// 自機に接触した際に自身を破壊する
    /// </summary>
    public void Remove()
    {
        UIManager.Instance.UpdateScore(m_score);
        ObjectPoolManager.Instance.Return(this);
    }
}
