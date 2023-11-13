using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Boss : EnemyBase, IPoolable
{
    // 移動方向設定
    private static readonly Vector3[] NextMove = new Vector3[8]
    {
            new (0.0f,  0.0f, 0.0f),        // 00 : なし
            new (0.0f, -1.0f, 0.0f),        // 01 : 登場
            new (0.0f,  1.0f, 0.0f),        // 02 : 上
            new (0.0f, -1.0f, 0.0f),        // 03 : 下
            new (-1.0f, 0.0f, 0.0f),        // 04 : 左
            new ( 1.0f, 0.0f, 0.0f),        // 05 : 右
            new ( 1.0f, 0.0f, 0.0f),        // 06 : 右
            new (-1.0f, 0.0f, 0.0f)         // 07 : 左
    };

    [SerializeField] private Cannon[] m_cannons;

    private int m_state = 0;                        // 動作管理
    private int m_count = 0;                        // 生成数カウント
    private int m_timer = 0;                        // 生成間隔
    private Vector3 m_move = Vector3.zero;          // 移動量

    public ObjectType BaseObjectType { get; set; } = ObjectType.Boss;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        EnemyBaseInitialize();
        SetSize(0.5f);

        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material.DOFade(1, 0.0f);
        }

        foreach (var c in m_cannons)
        {
            c.Init();
        }

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);

        m_state = 1;
        m_count = 0;
        m_timer = 0;
        m_move = Vector3.down * Speed;
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        switch (m_state)
        {
            case 1:     // 登場
                if (Transform.position.y <= 40.0f)
                {
                    foreach (var cannon in m_cannons)
                    {
                        cannon.State = Cannon.StateType.SelectAction;
                        cannon.Core.SetImageFromHP();
                    }

                    Speed = 0.5f;
                    CreateDirection(0.0f, 40.0f);
                }
                break;
            case 2:     // 上下の上昇
                if (Transform.position.y >= 100.0f)
                {
                    CreateDirection(0.0f, 100.0f);
                }
                break;
            case 3:     // 上下の下降
                if (Transform.position.y <= 40.0f)
                {
                    CreateDirection(0.0f, 40.0f);
                }
                break;
            case 4:     // 左側の往路
                if (Transform.position.x <= -200.0f)
                {
                    CreateDirection(-200.0f, 40.0f);
                }
                break;
            case 5:     // 左側の復路
                if (Transform.position.x >= 0.0f)
                {
                    CreateDirection(0.0f, 40.0f);
                }
                break;
            case 6:     // 右側の往路
                if (Transform.position.x >= 200.0f)
                {
                    CreateDirection(200.0f, 40.0f);
                }
                break;
            case 7:     // 右側の復路
                if (Transform.position.x <= 0.0f)
                {
                    CreateDirection(0.0f, 40.0f);
                }
                break;
            case 8:     // 爆発
                DestroyExplosion();
                break;
            case 9:     // フェード待ち
                break;
        }

        Transform.position += m_move;
    }

    /// <summary>
    /// 移動方向作成
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CreateDirection(float x, float y)
    {
        if ((m_state % 2) == 0)
        {
            m_state++;
        }
        else
        {
            m_state = Random.Range(1, 4) * 2;
        }

        m_move = NextMove[m_state] * Speed;
        Transform.position = new Vector3(x, y, 0.0f);
    }

    /// <summary>
    /// 破壊されたか？
    /// </summary>
    public void IsDestroyed()
    {
        int hp = m_cannons.Sum(cannon => cannon.Core.Hp);
        if (hp == 0)
        {
            m_state = 8;
            m_move = Vector3.zero;
            GameInfo.Instance.Player.IsDamage = false;

            // Scene上のミサイルとレーザーを全て削除
            var missiles = FindObjectsOfType<Missile>();
            var lasers = FindObjectsOfType<Laser>();
            foreach (var missile in missiles)
            {
                ObjectPoolManager.Instance.Return(missile);
            }
            foreach (var laser in lasers)
            {
                ObjectPoolManager.Instance.Return(laser);
            }

            // スコア更新
            UpdateScore();
        }
    }

    /// <summary>
    /// 爆破
    /// </summary>
    private void DestroyExplosion()
    {
        m_timer++;
        if (m_timer > 20)
        {
            m_timer = 0;

            for(int i = 0; i < 3; i++)
            {
                float px = Transform.position.x + Random.Range(-100, 100);
                float py = Transform.position.y + Random.Range(-100, 100);
                ResourceGenerator.GenerateEnemyExplosion(new Vector3(px, py));
            }

            m_count++;
            if (m_count > 30)
            {
                m_state = 9;

                var sequence = DOTween.Sequence();
                var renderers = GetComponentsInChildren<Renderer>();
                foreach (var renderer in renderers)
                {
                    sequence.Join(renderer.material.DOFade(0, 1.5f));
                }

                sequence.AppendCallback(() =>
                {
                    GameInfo.Instance.Player.State = Player.StateType.Exit;
                    m_state = 10;
                    ObjectPoolManager.Instance.Return(this);
                });
            }
        }
    }
}
