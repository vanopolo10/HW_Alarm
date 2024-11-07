using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Thief : MonoBehaviour
{
    [SerializeField] private Vector3[] _places;
    [SerializeField] private float _speed = 3;
    
    private int _placeIndex;
    private Vector3 _target;

    private void Awake()
    {
        _placeIndex = 0;
        _target = GetNextPlace();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

        if (transform.position == _target)
        {
            _target = GetNextPlace();
        }
    }
    
    private Vector3 GetNextPlace()
    {
        _placeIndex = ++_placeIndex % _places.Length;
        return  _places[_placeIndex];
    }
}
