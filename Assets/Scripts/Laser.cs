using UnityEngine;

public class Laser : ObjectBase, IPoolable
{
    public int CannonIndex { get; set; } = 0;

    public ObjectType BaseObjectType { get; set; } = ObjectType.Laser;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    public void Init(Vector3 position)
    {
        ObjectBaseInitialize();

        Transform.position = new Vector3(position.x, position.y + BoundSize.y, 0.0f);
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        var position = Transform.position;

        position.x = Cannon.CannonPosition[CannonIndex].x;
        position.y += Speed;

        Transform.position = position;

        if (JudgeOutOfScreenBottom())
        {
            ObjectPoolManager.Instance.Return(this);
        }
    }

    /// <summary>
    /// 弾と何かの当たり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 自機
        if (collision.TryGetComponent<Player>(out var player))
        {
            player.Damage(100);
        }
    }
}
