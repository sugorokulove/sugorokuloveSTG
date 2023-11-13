using UnityEngine;

public class Stock : MonoBehaviour, IPoolable
{
    public ObjectType BaseObjectType { get; set; } = ObjectType.Stock;
}
