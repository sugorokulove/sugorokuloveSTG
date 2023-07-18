using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; set; } = null;      // SpriteRendererコンポーネント参照用
    public Transform Transform { get; set; } = null;                // Transformのキャッシュ(少し速くなる)

    public Vector3 BoundSize { get; set; } = Vector3.zero;          // オブジェクトのサイズ
    public float Speed { get; set; } = 0.0f;                        // 移動速度

    public void Initialize(float speed)
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Transform = transform;

        BoundSize = SpriteRenderer.bounds.size;
        Speed = speed;
    }
}
