using UnityEngine;

public class Enemy001 : EnemyBase, IPoolable
{
    [SerializeField] private float m_angleSpeed;    // 角度の変化量
    [SerializeField] private float m_radius;        // 半径(移動幅)

    private float m_angle = 0.0f;                   // 角度

    public ObjectType BaseObjectType { get; set; } = ObjectType.Enemy001;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        EnemyBaseInitialize();

        m_angle = 0.0f;
        
        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        var position = Transform.position;

        position.x = Mathf.Sin(m_angle * Mathf.PI) * m_radius;
        position.y += Speed;

        m_angle += m_angleSpeed;

        Transform.position = position;
    }
}
