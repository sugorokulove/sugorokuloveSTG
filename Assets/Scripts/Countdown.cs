using System.Collections;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private float m_min;             // 乱数：最小値
    [SerializeField] private float m_max;             // 乱数：最大値

    private float m_count = 0;                        // カウント
    private System.Action m_callback = null;        // コールバック

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="min">乱数：最小値</param>
    /// <param name="max">乱数：最大値</param>
    /// <param name="action">カウントダウン終了時に実行する処理</param>
    public void Initialize(System.Action action)
    {
        m_count = Random.Range(m_min, m_max);
        m_callback = action;
        StartCoroutine(CountDownCoroutine());
    }

    /// <summary>
    /// 非同期なカウントダウン
    /// </summary>
    IEnumerator CountDownCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(m_count);

            m_callback();

            m_count = Random.Range(m_min, m_max);
        }
    }
}
