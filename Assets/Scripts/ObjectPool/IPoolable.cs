/// <summary>
/// プールして使用するものに取り付ける
/// </summary>
public interface IPoolable
{
    ObjectType BaseObjectType { get; set; }
}
