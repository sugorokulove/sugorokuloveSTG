using UnityEngine;

public class MainGame : MonoBehaviour
{
    void Start()
    {
        // ゲーム開始時の自機の生成
        GeneratePlayer();
    }

    /// <summary>
    /// 再スタート
    /// </summary>
    public void ReStart()
    {
        Invoke(nameof(GeneratePlayer), 1.0f);
    }

    /// <summary>
    /// 自機の生成
    /// </summary>
    void GeneratePlayer()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Plane/Player");
        var player = Instantiate(prefab);
        player.GetComponent<Player>().Initialize();
    }
}
