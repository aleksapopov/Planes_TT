public class Config  {
    /// <summary>
    /// The number of planes in a pool.
    /// </summary>
    public static readonly int NumPlanes = 15;

    /// <summary>
    /// The plane max health.
    /// </summary>
    public static readonly float PlaneMaxHealth = 1f;

    /// <summary>
    /// The plane's laser damage per second.
    /// </summary>
    public static readonly float PlaneDPS = .35f;

    /// <summary>
    /// The start plane speed. Spped is constant during the flight in current version.
    /// </summary>
    public static readonly float StartPlaneSpeed = 30f;

    /// <summary>
    /// Time interval between planes launch.
    /// </summary>
    public static readonly float PlaneSatrtDelay = .3f;
}
