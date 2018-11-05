using System.Collections.Generic;
using thelab.mvc;

public class GameModel : Model<Game>{

    /// <summary>
    /// The is game started.
    /// </summary>
    public bool IsGameStarted;

    /// <summary>
    /// List of PlaneModel for each plane in the pool.
    /// </summary>
    public List<PlaneModel> Planes;

    /// <summary>
    /// Total planes in the poool.
    /// </summary>
    public int NumPlanes;

    /// <summary>
    /// Time interval between planes launch.
    /// </summary>
    public float PlaneStartDelay;
}
