using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Plane/Player");
        Instantiate(prefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
