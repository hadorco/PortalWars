using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// An interface for all projectiles (bullets, ninja-stars, etc)
public interface Projectile
{
    void SetVelocity(Vector2 newVelocity);
    void SetSpriteDirection(Vector3 newScale);

    // Since this is an interface and cannot be instantiated, the function will return a Unity GameObject that can
    UnityEngine.GameObject GetObjectToInstantiate();
}
