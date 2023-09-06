using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    [SerializeField] float m_speed;
    public float Speed { get => m_speed; set => m_speed = value; }  // 速度

    public SpriteRenderer SpriteRenderer { get; set; } = null;      // SpriteRendererコンポーネント参照用
    public BoxCollider2D BoxCollider { get; set; } = null;          // BoxCollider2Dコンポーネント参照用
    public Transform Transform { get; set; } = null;                // Transformのキャッシュ(少し速くなる)
    public Vector3 BoundSize { get; set; } = Vector3.zero;          // オブジェクトのサイズ

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="speed">移動速度</param>
    public void Initialize()
    {
        if (TryGetComponent<SpriteRenderer>(out var renderer))
        {
            SpriteRenderer = renderer;
        }
        if (TryGetComponent<BoxCollider2D>(out var collider))
        {
            BoxCollider = collider;
        }
        Transform = transform;

        SetSize();
    }

    /// <summary>
    /// サイズ調整
    /// </summary>
    /// <param name="rate"></param>
    public void SetSize(float rate = 1.0f)
    {
        BoundSize = SpriteRenderer.bounds.size * rate;
        if (BoxCollider != null)
        {
            BoxCollider.size = SpriteRenderer.bounds.size;
        }
    }
}
