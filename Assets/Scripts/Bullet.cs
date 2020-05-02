using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun, Projectile
{
    Rigidbody2D mRigidBody;
    PhotonView mPhotonView;
    [SerializeField] float mSpeed = 20f;
    int mDamage = 30;

    const string foregroundName = "Foreground";

    // Start is called before the first frame update
    void Awake()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mPhotonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        PhotonNetwork.AllocateViewID(mPhotonView);
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        mRigidBody.velocity = newVelocity * mSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player opponent = collision.GetComponent<Player>();
        if (opponent != null && PhotonNetwork.IsMasterClient)
        {
            opponent.TakeDamage(mDamage);
        }

        if (mPhotonView.IsMine)
        {
            //mPhotonView.RPC("DestroyBullet", RpcTarget.All);
            //gameObject.SetActive(false);
            Destroy(gameObject, 0.0001f);
        }
    }

    [PunRPC]
    private void DestroyBullet(PhotonMessageInfo info)
    {
        // To account for the spawning of a new projectile in the destination portal, we use a small delay before destroying
        gameObject.SetActive(false);
        Destroy(gameObject, 0.1f);
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
