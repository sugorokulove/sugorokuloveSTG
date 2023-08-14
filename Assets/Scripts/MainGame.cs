using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    [SerializeField] private int m_stock = 3;                               // 残機数指定

    private GameObject[] m_stockList = null;                                // 残機管理(GameObject)
    public int Stock => m_stockList.Where(obj => obj != null).Count();      // 残機数

    void Awake()
    {
        m_stockList = new GameObject[m_stock];
        Array.Fill(m_stockList, null);
    }

    void Start()
    {
        // 指定数残機追加
        for (int i = 0; i < m_stock; i++)
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

        if (Stock > 0)
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
        m_stockList[Stock] = GenerateStock();
    }

    /// <summary>
    /// 残機削除
    /// </summary>
    private void RemoveStock()
    {
        Destroy(m_stockList[Stock - 1]);
        m_stockList[Stock - 1] = null;
    }

    /// <summary>
    /// 自機の生成
    /// </summary>
    void GeneratePlayer()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Plane/Player");
        var player = Instantiate(prefab);
        player.GetComponent<Player>().Initialize();

        RemoveStock();
    }

    /// <summary>
    /// 残機の生成
    /// </summary>
    GameObject GenerateStock()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/UI/Stock");
        var stock = Instantiate(prefab);
        stock.transform.position = new Vector3(
            -GameInfo.Instance.ScreenBound.x + Stock * 16 + 16,
            -GameInfo.Instance.ScreenBound.y + 16);
        return stock;
    }

    /// <summary>
    /// ゲームオーバー生成
    /// </summary>
    void GenerateGameover()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/UI/Gameover");
        Instantiate(prefab);

        prefab = Resources.Load<GameObject>("Prefabs/UI/Veil");
        Instantiate(prefab);

        StartCoroutine(TransitionToTitle(3.0f));
    }

    /// <summary>
    /// シーン遷移(指定秒後)
    /// </summary>
    /// <param name="second">秒数</param>
    IEnumerator TransitionToTitle(float second)
    {
        yield return new WaitForSeconds(second);

        SceneManager.LoadScene("TitleScene");
    }
}
