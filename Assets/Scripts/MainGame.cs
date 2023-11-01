using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    private GameObject[] m_stockList = null;                                // 残機管理(GameObject)
    public int GetStock => m_stockList.Where(obj => obj != null).Count();   // 残機数

    void Awake()
    {
        m_stockList = new GameObject[GameInfo.Instance.Stock];
        Array.Fill(m_stockList, null);
    }

    void Start()
    {
        // 指定数残機追加
        for (int i = 0; i < GameInfo.Instance.Stock; i++)
        {
            AddStock();
        }

        // ゲーム開始時の自機の生成
        ReStart();
    }

    /// <summary>
    /// 再スタート
    /// </summary>
    public void ReStart()
    {
        GameInfo.Instance.PowerUpCount = 0;

        if (GetStock > 0)
        {
            Invoke(nameof(GeneratePlayer), 1.0f);
        }
        else
        {
            Invoke(nameof(GenerateGameover), 1.0f);
        }
    }

    /// <summary>
    /// 残機追加
    /// </summary>
    private void AddStock()
    {
        var position = new Vector3(
            -GameInfo.Instance.ScreenBound.x + GetStock * 16 + 16,
            -GameInfo.Instance.ScreenBound.y + 16);

        var stock = ResourceGenerator.GenerateStock();
        UIManager.Instance.AddStock(stock);
        m_stockList[GetStock] = stock;
    }

    /// <summary>
    /// 残機削除
    /// </summary>
    private void RemoveStock()
    {
        Destroy(m_stockList[GetStock - 1]);
        m_stockList[GetStock - 1] = null;
    }

    /// <summary>
    /// 自機の生成
    /// </summary>
    void GeneratePlayer()
    {
        var player = ResourceGenerator.GeneratePlayer();

        GameInfo.Instance.Player = player;

        RemoveStock();
    }

    /// <summary>
    /// ゲームオーバー生成
    /// </summary>
    void GenerateGameover()
    {
        UIManager.Instance.GameOver();

        StartCoroutine(TransitionToTitle(3.0f));
    }

    /// <summary>
    /// シーン遷移(指定秒後)
    /// </summary>
    /// <param name="second">秒数</param>
    public IEnumerator TransitionToTitle(float second)
    {
        yield return new WaitForSeconds(second);

        SceneManager.LoadScene("TitleScene");
    }
}
