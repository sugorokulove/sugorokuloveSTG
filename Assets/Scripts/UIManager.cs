using TMPro;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private TextMeshProUGUI m_uiScore;         // スコア表示
    [SerializeField] private GameObject m_gameClear;            // ゲームクリア
    [SerializeField] private GameObject m_gameOver;             // ゲームオーバー
    [SerializeField] private GameObject m_stockLane;            // 残機配置場所

    protected override void Initialize()
    {
        UpdateScore(0);
    }

    /// <summary>
    /// スコア更新
    /// </summary>
    /// <param name="score">加算するスコア</param>
    public void UpdateScore(int score)
    {
        GameInfo.Instance.TotalScore += score;
        m_uiScore.text = $"{GameInfo.Instance.TotalScore}";
    }

    /// <summary>
    /// ゲームクリア表示
    /// </summary>
    public void GameClear()
    {
        m_gameClear.SetActive(true);
    }

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    public void GameOver()
    {
        m_gameOver.SetActive(true);
    }

    /// <summary>
    /// 残機配置
    /// </summary>
    /// <param name="stock"></param>
    public void AddStock(GameObject stock)
    {
        stock.transform.SetParent(m_stockLane.transform, false);
    }
}
