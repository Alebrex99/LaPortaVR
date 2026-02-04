using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMove : MonoBehaviour
{
    public float MovementSpeed = 0.1f;

    public float Limit = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * MovementSpeed * Time.deltaTime);
        // if (transform.position.y > Limit)
        // {
        //     transform.Translate(Vector3.down * Limit);
        // }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
