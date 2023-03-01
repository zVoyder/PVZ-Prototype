using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for the bullets that damage other entities
/// </summary>
public class Bullet : Harm // Extending the class Harm
{
    public float speed = 1f;
    public float disposeTime = 5f;

    private void Start()
    {
        Damage = 1;
        Destroy(gameObject, disposeTime);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); // Destroy this gameobject on trigger enter with something else.
    }
}
