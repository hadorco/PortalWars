using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, Projectile
{
    Rigidbody2D mRigidBody;
    [SerializeField] float mSpeed = 20f;
    int mDamage = 30;

    const string foregroundName = "Foreground";

    // Start is called before the first frame update
    void Awake()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        mRigidBody.velocity = newVelocity * mSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player opponent = collision.GetComponent<Player>();
        if (opponent != null)
        {
            opponent.TakeDamage(mDamage);
        }
        
        // To account for the spawning of a new projectile in the destination portal, we use a small delay before destroying
        Destroy(gameObject, 0.00001f);
    }

    public GameObject GetObjectToInstantiate()
    {
        return gameObject;
    }

    public void SetSpriteDirection(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
}
