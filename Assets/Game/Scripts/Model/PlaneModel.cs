using thelab.mvc;

public class PlaneModel : Model<Game>
{
    public int Id;
    public float Health;
    public float MaxHealth;
    public float DamagePerSecond;
    public float StartSpeed;
    public bool IsFlying;
    public int PlanesDestroyed;

    public PlaneModel(int id, float dps, float speed, float maxHealth)
    {
        Id = id;
        DamagePerSecond = dps;
        StartSpeed = speed;
        MaxHealth = Health = maxHealth;
    }
}
