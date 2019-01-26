using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float speed = 8f;
    public Vector3 offset = new Vector3(0f, -3.5f, -10f);

    void FixedUpdate() {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position + offset,
            Time.fixedDeltaTime * speed
            );
    }
}
