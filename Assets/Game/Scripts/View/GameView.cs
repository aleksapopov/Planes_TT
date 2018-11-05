using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class 
GameView : View<Game> {

    public List<PlaneView> Planes;

    private readonly string PLANE_REFAB_PATH = "Prefabs/Plane";

    /// <summary>
    /// Create all plane game objects in the pool by the number set in Config.cs. 
    /// </summary>
    public void CreatePlanes () {
        GameObject _plane;
        PlaneView _planeView;
        for (int i = 0; i < app.model.NumPlanes; i++) {
            _plane = Instantiate(Resources.Load(PLANE_REFAB_PATH)) as GameObject;
            _plane.name = i.ToString();
            _plane.transform.SetParent(this.transform, false);
            _planeView = _plane.AddComponent<PlaneView>();
            _planeView.Id = i;
            Planes.Add(_planeView);
        }
    }
}
