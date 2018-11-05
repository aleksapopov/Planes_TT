using UnityEngine;
using UnityEngine.UI;
using thelab.mvc;

public class UIView : View<Game> {

    public GameObject StartGO;

    private Button _btnStart; 

	// Use this for initialization
	void Start () {
        _btnStart = StartGO.GetComponent<Button>();
        AddListeners();
	}
	
	private void AddListeners()
    {
        _btnStart.onClick.AddListener(OnStartPressed);
    }

    private void RemoveListeners()
    {
        _btnStart.onClick.RemoveListener(OnStartPressed);
    }

    private void OnStartPressed() 
    {
        RemoveListeners();
        StartGO.SetActive(false);
        Messenger.Broadcast(GameEvents.START_GAME);
    }
}
