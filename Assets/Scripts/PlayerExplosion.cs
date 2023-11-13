public class PlayerExplosion : Explosion, IPoolable
{
    public ObjectType BaseObjectType { get; set; } = ObjectType.PlayerExplosion;
}
