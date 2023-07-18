using UnityEngine;

public class MainGame : MonoBehaviour
{
    void Start()
    {
        // 自機の作成
        var prefab = Resources.Load<GameObject>("Prefabs/Plane/Player");
        var player = Instantiate(prefab);
        player.GetComponent<Player>().Initialize();
    }

    void Update()
    {
        
    }
}
