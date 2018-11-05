using thelab.mvc;
using UnityEngine;

public class PlanesController : Controller<Game> {

    private float _currentStartPlaneTime = 0f;

	// Use this for initialization
	void Start () {
        AddListeners();
	}
	
	// Update is called once per frame
	void Update () {
        if (app.model.IsGameStarted) 
            UpdateStartPlaneTimer();
    }
    
    private void AddListeners () {
        Messenger.AddListener<int, bool>(PlaneEvents.FLIGHT_FINISHED, OnFlightFinished);
        Messenger.AddListener<int>(PlaneEvents.COLLISION_DETECTED, OnCollisionDetected);
        Messenger.AddListener<int, int>(PlaneEvents.GET_DEMAGE, OnGetDemage);
    }

    /// <summary>
    /// Updates timer for launch plane delay.
    /// TODO make this part as a coroutine if "pause game" functionality is neede. 
    /// </summary>
    private void UpdateStartPlaneTimer()
    {
        _currentStartPlaneTime += Time.deltaTime;
        if (_currentStartPlaneTime >= app.model.PlaneStartDelay) {
            if (LaunchNewPlane()) {
                _currentStartPlaneTime = 0f;
            } 
        }
    }

    /// <summary>
    /// Launchs the new plane if there is an unused plane object in the pool.
    /// </summary>
    /// <returns><c>true</c>, if new plane was launched, <c>false</c> otherwise.</returns>
    private bool LaunchNewPlane () {
        foreach (PlaneModel _plane in app.model.Planes) {
            if (!_plane.IsFlying) {
                _plane.IsFlying = true;
                app.view.Planes[_plane.Id].StartMoving();
                return true;
            }
        }
        return false;
    }

    //Events listeners

    /// <summary>
    /// Plane had reached the endpoint.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="success">If set to <c>true</c> success.</param>
    private void OnFlightFinished(int id, bool success) {
        if (success)
            Debug.Log("Plane " + id + " is out with " + app.model.Planes[id].PlanesDestroyed + " planes destroyed.");

        app.model.Planes[id].Health = app.model.Planes[id].MaxHealth;
        app.model.Planes[id].PlanesDestroyed = 0;
        app.model.Planes[id].IsFlying = false;
        app.view.Planes[id].Hide();
    }

    /// <summary>
    /// Collided with other plane.
    /// </summary>
    /// <param name="id">Identifier.</param>
    private void OnCollisionDetected (int id) {
        app.view.Planes[id].Explode();
    }

    /// <summary>
    /// Other plane is on the fire line and gets damage.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="targetId">Target identifier.</param>
    private void OnGetDemage (int id, int targetId) {
        app.model.Planes[targetId].Health -= app.model.Planes[targetId].DamagePerSecond * Time.deltaTime;
        if (app.model.Planes[targetId].Health <= 0f) {
            app.view.Planes[targetId].Explode();
            app.model.Planes[id].PlanesDestroyed++;
        }
    }
}
