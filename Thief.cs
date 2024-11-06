using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Thief : MonoBehaviour
{
    [SerializeField] private Vector3[] _places;
    [SerializeField] private float _speed = 3;
    
    private int _placeIndex;
    private Vector3 _positionToGo;

    private void Awake()
    {
        _placeIndex = 0;
        _positionToGo = GetNextPlace();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _positionToGo, _speed * Time.deltaTime);

        if (transform.position == _positionToGo)
        {
            _positionToGo = GetNextPlace();
            
        }
    }
    
    private Vector3 GetNextPlace()
    {
        _placeIndex = ++_placeIndex % _places.Length;
        Vector3 thisPointVector = _places[_placeIndex];
        
        return thisPointVector;
    }
}
