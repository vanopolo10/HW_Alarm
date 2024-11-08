using System;
using UnityEngine;

public class House : MonoBehaviour
{
    public event Action ThiefEntered; 
    public event Action ThiefLeft; 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Thief>(out _))
            ThiefEntered?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Thief>(out _))
            ThiefLeft?.Invoke();
    }
}
