using UnityEngine;

public class Cannon : ObjectBase
{
    public enum StateType
    {
        None = 0,       // 無効
        Stop,           // 停止
        SelectAction,   // 動作選択
        Snipe,          // 狙撃弾
        Direction,      // 方向弾
        Laser           // レーザー
    }

    public static Vector3[] CannonPosition = new Vector3[3];

    [SerializeField] private int m_cannonIndex;
    [SerializeField] private Transform m_shotPoint;
    [SerializeField] private Core m_core;

    private StateType m_state = StateType.None;
    private int m_timer = 0;
    private int m_count = 0;

    public StateType State { get => m_state; set => m_state = value; }
    public Core Core => m_core;

    void Start()
    {
        Initialize();

        m_state = StateType.Stop;
        m_timer = 0;
        m_count = 0;

        Transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }

    void Update()
    {
        switch (m_state)
        {
            // 行動選択
            case StateType.SelectAction:
                NextAction();
                break;
            // 狙撃弾
            case StateType.Snipe:
                ActionSnipeMissile();
                break;
            // 方向弾
            case StateType.Direction:
                ActionDirectionMissile();
                break;
            // レーザー
            case StateType.Laser:
                ActionLaser();
                break;
        }
    }

    /// <summary>
    /// 行動選択
    /// </summary>
    private void NextAction()
    {
        m_timer--;
        if (m_timer <= 0)
        {
            m_state = (StateType)Random.Range((int)StateType.Snipe, (int)StateType.Laser + 1);
            m_count = 0;
            m_timer = 0;
        }
    }

    /// <summary>
    /// 狙撃弾作成
    /// </summary>
    private void ActionSnipeMissile()
    {
        m_timer--;
        if (m_timer <= 0)
        {
            m_timer = 40;

            var move = Vector3.down;
            if (GameInfo.Instance.Player != null)
            {
                move = (GameInfo.Instance.Player.Transform.position - Transform.position).normalized;
            }

            Transform.rotation = Quaternion.FromToRotation(Vector3.up, move);
            
            ResourceGenerator.GenerateMissile(m_shotPoint.position, move);

            m_count++;
            if (m_count >= 5)
            {
                ResetState(200, 400);
                Transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
            }
        }
    }

    /// <summary>
    /// 方向弾作成
    /// </summary>
    private void ActionDirectionMissile()
    {
        int angle = 190;
        for (int i = 0; i < 9; i++)
        {
            var radian = angle * Mathf.Deg2Rad;
            var move = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0.0f).normalized;

            ResourceGenerator.GenerateMissile(m_shotPoint.position, move);

            angle += 20;
        }
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);

        ResetState(200, 400);
    }

    /// <summary>
    /// レーザー作成
    /// </summary>
    private void ActionLaser()
    {
        m_timer--;
        if (m_timer <= 0)
        {
            m_timer = 6;

            ResourceGenerator.GenerateLaser(m_shotPoint.position, m_cannonIndex);

            m_count++;
            if (m_count >= 30)
            {
                ResetState(200, 400);
            }
        }

        CannonPosition[m_cannonIndex] = m_shotPoint.position;
    }

    /// <summary>
    /// 共通のリセット
    /// </summary>
    private void ResetState(int min, int max)
    {
        m_state = StateType.SelectAction;
        m_timer = Random.Range(min, max);
    }
}
