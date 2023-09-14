using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer = null;
    private Transform m_transform = null;
    private Vector3 m_move = Vector3.zero;

    public int Index { get; set; } = 0;

    public Vector3 Position
    {
        get => m_transform.position;
        set => m_transform.position = value;
    }
    
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_transform = transform;
        m_transform.position = Vector3.zero;
    }

    void Update()
    {
        m_transform.position += m_move;
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="index">背景画像の番号</param>
    /// <param name="position">初期位置</param>
    /// <param name="speed">速度</param>
    public void Initialize(int index, Vector3 position, float speed)
    {
        Index = index;
        m_transform.position = position;
        m_move = new Vector3(0.0f, speed, 0.0f);
    }

    /// <summary>
    /// スプライト設定
    /// </summary>
    /// <param name="sprite">Speite</param>
    public void SetBackgroundSprite(Sprite sprite)
    {
        m_spriteRenderer.sprite = sprite;
    }
}
