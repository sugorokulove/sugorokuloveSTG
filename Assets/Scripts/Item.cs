using UnityEngine;

public class Item : ObjectBase
{
    private Vector3 m_position = Vector3.zero;      // アイテムの位置・座標

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    public void Initialize(Vector3 position)
    {
        Initialize(-0.1f);

        m_position = position;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        m_position.y += Speed;

        if (transform.position.y <= -(GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            Remove();
        }

        Transform.position = m_position;
    }

    /// <summary>
    /// 自機に接触した際に自身を破壊する
    /// </summary>
    public void Remove()
    {
        Destroy(gameObject);
    }
}
