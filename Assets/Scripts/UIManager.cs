using TMPro;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private TextMeshProUGUI m_uiScore;         // スコア表示
    [SerializeField] private GameObject m_gameClear;            // ゲームクリア

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
}
