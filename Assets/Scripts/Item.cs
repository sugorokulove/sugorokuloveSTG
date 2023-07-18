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
            Destroy(gameObject);
        }

        Transform.position = m_position;
    }
}
