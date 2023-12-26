

public class PlayerCharacter : Entity
{
    protected override void Awake()
    {
        base.Awake();
        this.PlayerCreated();
    }
}
