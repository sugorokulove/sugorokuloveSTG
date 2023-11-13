using UnityEngine;
using UniRx.Toolkit;

/// <summary>
/// オブジェクトプールの素（扱う型はMonoBehaviour）
/// </summary>
public class ShootingObjectPool : ObjectPool<MonoBehaviour>
{
    public Object UnityObject { get; private set; }

    public ShootingObjectPool(Object obj)
    {
        UnityObject = obj;
    }

    /// <summary>
    /// オブジェクトを借り受ける
    /// </summary>
    /// <returns></returns>
    public new MonoBehaviour Rent()
    {
        var rent = base.Rent();

        Debug.Assert(rent.gameObject, "don't rent : " + UnityObject.name);

        return rent;
    }

    /// <summary>
    /// オブジェクトを返却
    /// </summary>
    /// <param name="_object"></param>
    public new void Return(MonoBehaviour _object)
    {
        if (!_object)
        {
            return;
        }
        base.Return(_object);
    }

    /// <summary>
    /// オブジェクト作成
    /// </summary>
    /// <returns></returns>
    protected override MonoBehaviour CreateInstance()
    {
        var go = Object.Instantiate(UnityObject) as GameObject;
        Debug.Assert(go, $"{UnityObject.name} instantiate failure.");
        var mb = go.GetComponent<MonoBehaviour>();
        Debug.Assert(mb, $"{UnityObject.name} has no MonoBehaviour.");
        return mb;
    }
}