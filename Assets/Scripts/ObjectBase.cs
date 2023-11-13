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
    public void ObjectBaseInitialize()
    {
        if (Transform != null)
        {
            return;
        }

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

    /// <summary>
    /// 画面外判定
    /// </summary>
    /// <returns>true:画面外／false:画面内</returns>
    public bool JudgeOutOfScreenLeft()
    {
        return Transform.position.x <= -(GameInfo.Instance.ScreenBound.x + BoundSize.x);
    }
    public bool JudgeOutOfScreenRight()
    {
        return Transform.position.x >= (GameInfo.Instance.ScreenBound.x + BoundSize.x);
    }
    public bool JudgeOutOfScreenTop()
    {
        return Transform.position.y >= (GameInfo.Instance.ScreenBound.y + BoundSize.y);
    }
    public bool JudgeOutOfScreenBottom()
    {
        return Transform.position.y <= -(GameInfo.Instance.ScreenBound.y + BoundSize.y);
    }
}
