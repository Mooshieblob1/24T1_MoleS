using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGridLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Destroy the GameObject when anything enters its trigger collider
        Destroy(other.gameObject);
    }
}
