using UnityEngine;

public class Item : ObjectBase
{
    private Vector3 m_move = Vector3.zero;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    public void Init(Vector3 position)
    {
        Initialize();

        Transform.position = position;

        m_move = new Vector3(0.0f, Speed, 0.0f);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Transform.position += m_move;

        if (Transform.position.y <= -(GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            Remove();
        }
    }

    /// <summary>
    /// 自機に接触した際に自身を破壊する
    /// </summary>
    public void Remove()
    {
        Destroy(gameObject);
    }
}
