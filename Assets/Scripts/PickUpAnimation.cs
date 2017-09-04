using UnityEngine;
using System.Collections;

public class PickUpAnimation : MonoBehaviour
{
    public float rotateSpeed = 15f;
    public Vector3 rotationAxis = new Vector3(0f, 0f, 0f);
    public float moveSpeed = 1f;
    public float units = 2f;
    
    Vector3 direction;
  
    float max;
    float min;

    void Start()
    {
        min = transform.position.y;
        max = transform.position.y + units;
        direction = Vector3.up;
    }

    void Update()
    {
        
        // Rotate object
        transform.Rotate(rotationAxis * rotateSpeed * Time.deltaTime, Space.World);

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        if (transform.position.y >= max)
            direction = Vector3.down;

        if (transform.position.y <= min)
            direction = Vector3.up;

    }
}
