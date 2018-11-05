using System.Collections;
using UnityEngine;
using thelab.mvc;

public class PlaneView : View<Game> {

    public int Id;

    private PolygonCollider2D _collider;
    private LineRenderer _lineRenderer;
    private ParticleSystem _partSystem;
    private Transform _trBody;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private Coroutine _movingCoroutine;

    // Use this for initialization
    void Start () {
        _collider = this.GetComponent<PolygonCollider2D>();
        _collider.enabled = false;
        _lineRenderer = this.GetComponentInChildren<LineRenderer>();
        _lineRenderer.enabled = false;
        _partSystem = this.GetComponentInChildren<ParticleSystem>();
        _trBody = this.transform.Find("Body");
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D _hit = Physics2D.Linecast((Vector2)this.transform.position, (Vector2)_endPos);
        if (_hit) {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, _lineRenderer.transform.position);
            _lineRenderer.SetPosition(1, _hit.point);
            Messenger.Broadcast<int, int>(PlaneEvents.GET_DEMAGE, Id, int.Parse(_hit.transform.name));
        } else {
            _lineRenderer.enabled = false;
        }
	}

    /// <summary>
    /// Detect the collisions with other planes.
    /// </summary>
    /// <param name="col">Col.</param>
    void OnCollisionEnter2D(Collision2D col)
    {
        if(app.model.IsGameStarted)
            Messenger.Broadcast<int>(PlaneEvents.COLLISION_DETECTED, Id);
    }

    /// <summary>
    /// Set the plane to the random position outside of the screen rectangle.
    /// Launch plane in forward the random point on the apposite side if the screen.
    /// </summary>
    public void StartMoving () {
        float _maxY = Camera.current.orthographicSize;
        float _maxX = _maxY * Camera.current.aspect;
        switch ((int)Mathf.Floor(Random.value * 4)) {
            case 0:
                _startPos = new Vector3(-_maxX, Random.Range(-_maxY, _maxY));
                _endPos = new Vector3(_maxX, Random.Range(-_maxY, _maxY));
                break;

            case 1:
                _startPos = new Vector3(Random.Range(-_maxX, _maxX), _maxY);
                _endPos = new Vector3(Random.Range(-_maxX, _maxX), -_maxY);
                break;

            case 2:
                _startPos = new Vector3(_maxX, Random.Range(-_maxY, _maxY));
                _endPos = new Vector3(-_maxX, Random.Range(-_maxY, _maxY));
                break;

            case 3:
                _startPos = new Vector3(Random.Range(-_maxX, _maxX), -_maxY);
                _endPos = new Vector3(Random.Range(-_maxX, _maxX), _maxY);
                break;

            default:
                _startPos = new Vector3(-_maxX, Random.Range(-_maxY, _maxY));
                _endPos = new Vector3(_maxX, Random.Range(-_maxY, _maxY));
                break;
        }

        this.transform.position = _startPos;
        Vector3 vectorToTarget = _startPos - _endPos;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = q;
        this.gameObject.SetActive(true);
        _collider.enabled = true;
        _movingCoroutine = StartCoroutine(MoveRoutime ());
    }

    /// <summary>
    /// Start moving corutine. Use StopCoroutine fot "pause game" feature if needed.
    /// </summary>
    /// <returns>The routime.</returns>
    private IEnumerator MoveRoutime () {
        float _remainingDistance = (this.transform.position - _endPos).magnitude;
        float _movingTime = _remainingDistance / app.model.Planes[Id].StartSpeed;
        while (_remainingDistance > float.Epsilon) {
            Vector3 newPostion = Vector3.MoveTowards(this.transform.position, _endPos, Time.deltaTime / _movingTime);
            this.transform.position = newPostion;
            _remainingDistance = (this.transform.position - _endPos).magnitude;
            yield return null;
        }

        Messenger.Broadcast<int, bool>(PlaneEvents.FLIGHT_FINISHED, Id, true);
    }

    /// <summary>
    /// Hide the plane after it was destroyed, or the plane had reached the end point.
    /// </summary>
    public void Hide () {
        _collider.enabled = false;
        this.gameObject.SetActive(false);
        _trBody.gameObject.SetActive(true);
    }

    /// <summary>
    /// Start plane explode animation coroutine.
    /// </summary>
    public void Explode () {
        _collider.enabled = false;
        StartCoroutine(ExplodeCoroutine());
    }

    /// <summary>
    /// Start explode animation if plane was destroyed. Broadcast the event when the animation is over with a delay.
    /// </summary>
    private IEnumerator ExplodeCoroutine() {
        _trBody.gameObject.SetActive(false);
        _partSystem.Play();
        yield return new WaitForSeconds(.3f);
        Messenger.Broadcast<int, bool>(PlaneEvents.FLIGHT_FINISHED, Id, false);
    }
}
