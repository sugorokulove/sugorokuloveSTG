public class EnemyExplosion : Explosion, IPoolable
{
    public ObjectType BaseObjectType { get; set; } = ObjectType.EnemyExplosion;
}
