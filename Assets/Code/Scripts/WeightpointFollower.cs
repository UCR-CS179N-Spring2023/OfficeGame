using UnityEngine;
public class WeightpointFollower : MonoBehaviour
{

    [SerializeField] private GameObject[] _waypoints;
    private int _currentWayPointIndex = 0;

    [SerializeField] private float _speed = 2f;

    private Vector3 _delta_pos = Vector2.zero;
    private Rigidbody2D _rider;
    private bool _triggered;

    private void Awake() {
        _rider =  GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(Vector2.Distance(_waypoints[_currentWayPointIndex].transform.position, transform.position) < .1f) 
            _currentWayPointIndex = (_currentWayPointIndex+1)% _waypoints.Length;
        
        Vector3 new_pos = Vector2.MoveTowards(transform.position, _waypoints[_currentWayPointIndex].transform.position, Time.deltaTime * _speed);
        _delta_pos = new_pos - transform.position;

        if (_triggered) {
            Vector2 delta = _delta_pos;
            _rider.position +=delta;
        }
        
        transform.position += _delta_pos; 
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        _triggered = true;
    }

        private void OnTriggerExit2D(Collider2D collision) {
        _triggered = false;
    }
}
