using thelab.mvc;

public class GameController : Controller<Game> {

	// Use this for initialization
	void Start () {
        //Add all other controllers to the scene
        this.gameObject.AddComponent<PlanesController>();
        InitGame();
        AddListeners();
    }

    private void AddListeners () {
        Messenger.AddListener(GameEvents.START_GAME, OnStart);
    }

    /// <summary>
    /// Init the game. Create data model based on Config.cs values and create all related game objects.
    /// </summary>
    private void InitGame()
    {
        app.model.NumPlanes = Config.NumPlanes;
        app.model.PlaneStartDelay = Config.PlaneSatrtDelay;
        for (int i = 0; i < app.model.NumPlanes; i++)
        {
            app.model.Planes.Add(new PlaneModel(i, Config.PlaneDPS, Config.StartPlaneSpeed, Config.PlaneMaxHealth));
        }
        app.view.CreatePlanes();
    }

    /// <summary>
    /// Ste the game into the active mode. Start launching planes.
    /// </summary>
    private void OnStart() 
    {
        app.model.IsGameStarted = true;
    }
}
