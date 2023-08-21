using UnityEngine;

public class Background : ObjectBase
{
    public int Index { get; set; } = 0;

    private Vector3 m_position;
    public Vector3 Position { get => m_position; set => m_position = value; }
    
    void Start()
    {
        Initialize(0.0f);

        m_position = Vector3.zero;
    }

    void Update()
    {
        m_position.y += Speed;
        Transform.position = m_position;
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sprite">Sprite</param>
    /// <param name="position">初期位置</param>
    /// <param name="speed">速度</param>
    public void Initialize(int index, Vector3 position, float speed)
    {
        Index = index;
        m_position = position;
        Speed = speed;
    }

    /// <summary>
    /// スプライト設定
    /// </summary>
    /// <param name="sprite">Speite</param>
    public void SetBackgroundSprite(Sprite sprite)
    {
        SpriteRenderer.sprite = sprite;
    }
}
