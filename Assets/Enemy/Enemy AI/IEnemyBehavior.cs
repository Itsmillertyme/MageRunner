public interface IEnemyBehaviour : IBehave {
    void Tick(EnemyContext context);
}

public interface IEnemyMovementBehaviour : IEnemyBehaviour { }

public interface IEnemyCombatBehaviour : IEnemyBehaviour { }
