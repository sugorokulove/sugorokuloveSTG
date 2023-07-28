using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    [SerializeField] BoxCollider2D m_collider;                      // 当たり判定用コライダー
    public BoxCollider2D Collider => m_collider;

    public SpriteRenderer SpriteRenderer { get; set; } = null;      // SpriteRendererコンポーネント参照用
    public Transform Transform { get; set; } = null;                // Transformのキャッシュ(少し速くなる)

    public Vector3 BoundSize { get; set; } = Vector3.zero;          // オブジェクトのサイズ
    public float Speed { get; set; } = 0.0f;                        // 移動速度

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="speed">移動速度</param>
    public void Initialize(float speed)
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Transform = transform;

        Speed = speed;

        SetSize();
    }

    /// <summary>
    /// サイズ調整
    /// </summary>
    /// <param name="rate"></param>
    public void SetSize(float rate = 1.0f)
    {
        BoundSize = SpriteRenderer.bounds.size * rate;
        Collider.size = SpriteRenderer.bounds.size;
    }
}
