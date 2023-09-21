using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speed = 1f; // speed of the platform
    private Vector2 startPos; // starting position of the platform
    public float dist = 2f;
    public bool isHorizontal;
    public bool isPlatform;

    void Start()
    {
        speed = 10f;
        startPos = transform.position; // store the starting position of the platform
        speed = speed/dist;
    }

    void FixedUpdate()
    {
        if (isHorizontal == true)
        {
            // Move the platform horizontally
            Vector2 targetPos = startPos + new Vector2(Mathf.Sin(Time.time * speed)*dist, 0f);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime);
        }
        else
        {
            Vector2 targetPos = startPos + new Vector2(0f, Mathf.Sin(Time.time * speed) * dist);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime);
        }

         

        // Check if any objects are on the platform
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, transform.rotation.eulerAngles.z);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && isPlatform == true)
            {
                collider.transform.SetParent(transform);
            }
        }
    }

    // Unparent any objects that are no longer on the platform
    private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Player"))
    {
        collision.collider.transform.SetParent(null);
    }
}
}