using UnityEngine;

public class Core : ObjectBase
{
    enum State
    {
        Grey = 0,
        Blue,
        Yellow,
        Red
    }

    [SerializeField] private int m_hp;              // HP設定用
    [SerializeField] private int m_score;           // スコア
    [SerializeField] private SpriteFlash m_flash;   // 点滅用
    [SerializeField] private Boss m_boss;           // ボス
    [SerializeField] private Cannon m_cannon;       // 砲台
    [SerializeField] private Sprite[] m_costumes;   // 色変化

    private State m_state;                          // 色
    private int m_hpNow = 0;                        // HP現在値
    private int m_hpMax;                            // HP最大値

    public int Hp => m_hpNow;

    public void Init()
    {
        ObjectBaseInitialize();

        m_flash.Reset();

        m_state = State.Grey;
        m_hpMax = m_hp;
        m_hpNow = m_hpMax;

        SpriteRenderer.sprite = m_costumes[(int)State.Grey];

        gameObject.SetActive(true);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="power">攻撃力(ダメージ値)</param>
    public void Damage(int power)
    {
        if (m_state != State.Grey)
        {
            m_flash.FlashLoop(3);

            m_hpNow -= power;
            if (m_hpNow <= 0)
            {
                m_hpNow = 0;
                m_boss.IsDestroyed();
                m_cannon.State = Cannon.StateType.Stop;
                UIManager.Instance.UpdateScore(m_score);
                ResourceGenerator.GenerateEnemyExplosion(Transform.position);
                gameObject.SetActive(false);
            }

            SetImageFromHP();
        }
    }

    /// <summary>
    /// 体力によりコアの色を変更する
    /// </summary>
    public void SetImageFromHP()
    {
        m_state = State.Blue;
        if (m_hpNow < m_hpMax * 0.4f)
        {
            m_state = State.Red;
        }
        else if (m_hpNow < m_hpMax * 0.7f)
        {
            m_state = State.Yellow;
        }
        SpriteRenderer.sprite = m_costumes[(int)m_state];
    }
}
