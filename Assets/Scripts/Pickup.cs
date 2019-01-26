using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float rotationSpeed = 10;

    private void Update() {
        transform.Rotate(-Vector3.forward * Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter2D() {
        Destroy(gameObject);

    }
}
