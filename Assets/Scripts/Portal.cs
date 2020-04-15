using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 mSpawnPosition;
    public Vector2 mSpawnVelocity;

    [SerializeField] Portal mDestinationPortal = null;
    
    // Called by the Manager class on startup
    public void AssignDestinationPortal(Portal destinationPortal)
    {
        mDestinationPortal = destinationPortal;
    }

    public void CalculateSpawnInfo()
    {
        Vector3 positionOffset = new Vector3(0, 0, 0);
        Vector2 newVelocity = new Vector2(0, 0);
        switch (gameObject.tag)
        {
            case "PortalTop":
                positionOffset.y = -1;
                newVelocity.y = -1;
                break;
            case "PortalBottom":
                positionOffset.y = 1;
                newVelocity.y = 1;
                break;
            case "PortalLeft":
                positionOffset.x = 1f;
                newVelocity.x = 1;
                break;
            case "PortalRight":
                positionOffset.x = -1;
                newVelocity.x = -1;
                break;
            default:
                break;
        }

        mSpawnPosition = transform.position + positionOffset;
        mSpawnVelocity = newVelocity;
    }

    void SpawnClone(GameObject original)
    {
        GameObject clone = Instantiate(original, mSpawnPosition, Quaternion.identity);

        Projectile projectile = clone.GetComponent<Projectile>();
        projectile.SetSpriteDirection(new Vector3(1,1,1) * mSpawnVelocity.x);
        projectile.SetVelocity(mSpawnVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            mDestinationPortal.SpawnClone(projectile.GetObjectToInstantiate());
        }
    }
}
