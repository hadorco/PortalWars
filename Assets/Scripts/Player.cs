using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPunObservable
{
    #region Member Variables

    // Serialized fields
    [SerializeField] float mRunSpeed = 10f;
    [SerializeField] float mJumpSpeed = 15f;
    [SerializeField] GameObject mProjectilePrefab;
    [SerializeField] GameObject mFirePoint; // The starting point from which bullets are fired
    [SerializeField] HealthBar mHealthBar;

    // Cached references
    Rigidbody2D mRigidBody;
    Animator mAnimator;
    CapsuleCollider2D mBodyCollider;
    BoxCollider2D mFeetCollider;
    PhotonView mPhotonView;
    int mForegroundLayer = 0; // A number that represents the "Foreground" layer of the scene

    // State
    int mHealth = 100;

    #endregion

    #region Unity Lifetime Functions

    void Awake()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mBodyCollider = GetComponent<CapsuleCollider2D>();
        mFeetCollider = GetComponent<BoxCollider2D>();
        mPhotonView = GetComponent<PhotonView>();
        mForegroundLayer = LayerMask.GetMask("Foreground");

        Debug.Log("Num of playerssssss: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (mPhotonView.IsMine)
        {
            mHealthBar = GameObject.Find("HealthBarLeft").GetComponent<HealthBar>();
        }
        else
        {
            mHealthBar = GameObject.Find("HealthBarRight").GetComponent<HealthBar>();
        }
    }

    private void Start()
    {
        mHealthBar.SetMaxHealth(mHealth);
    }
    void Update()
    {
        if (mPhotonView.IsMine)
        {
            Run();
            Jump();
            FlipSprite();
            Shoot();
        }
    }

    #endregion

    #region Public Functions

    // Called from the projectile that hits the player
    public void TakeDamage(int damage)
    {
        mPhotonView.RPC("RpcTakeDamage", RpcTarget.AllBuffered, damage);

        mHealthBar.SetHealth(mHealth);
    }

    #endregion

    #region Private Functions

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire"))
        {
            // Create and fire bullet
            mPhotonView.RPC("RpcFireBullet", RpcTarget.AllBuffered);

            // Start animation
            mAnimator.SetTrigger("Shooting");
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && mFeetCollider.IsTouchingLayers(mForegroundLayer))
        {
            mPhotonView.RPC("RpcJump", RpcTarget.All);
            
            //mRigidBody.AddForce(new Vector2(0, 110000 * Time.deltaTime));

            // Start animation
            mAnimator.SetBool("Jumping", true);
        }
        // We use 0.005f because when landing into corners our rigidbody's velocity still has a nonzero value, so the animation gets stuck
        else if (mFeetCollider.IsTouchingLayers(mForegroundLayer) && Mathf.Abs(mRigidBody.velocity.y) < 0.0005f)
        {
            // Stop animation
            mAnimator.SetBool("Jumping", false);
        }
    }

    private void Run()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        Vector2 newVelocity = new Vector2(horizontalAxis * mRunSpeed, mRigidBody.velocity.y);
        mRigidBody.velocity = newVelocity;
        if (horizontalAxis != 0)
        {
            // Start animation
            mAnimator.SetBool("Running", true);
        }
        else
        {
            // Stop animation
            mAnimator.SetBool("Running", false);
        }
    }

    private void FlipSprite()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        if (mRigidBody.velocity.x < 0)
        {
            currentScale.x = -1;
        }
        else if (mRigidBody.velocity.x > 0)
        {
            currentScale.x = 1;
        }

        gameObject.transform.localScale = currentScale;
    }

    #endregion

    #region RPC Functions

    [PunRPC]
    private void RpcJump()
    {
        Vector2 currentVelocity = mRigidBody.velocity;
        currentVelocity += new Vector2(0, mJumpSpeed);
        mRigidBody.velocity = currentVelocity;
    }

    [PunRPC]
    private void RpcTakeDamage(int damage)
    {
        if (mPhotonView.IsMine)
        {
            mHealth -= damage;
        }

        Debug.Log("Player: " + mPhotonView.Owner.NickName + ", health after taking damage: " + mHealth);
        if (mHealth <= 0)
        {
            mAnimator.SetTrigger("Dead");
            if (mPhotonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
                PhotonNetwork.Instantiate("Prefabs\\Robot", new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
    }

    [PunRPC]
    private void RpcFireBullet()
    {
        Projectile bullet = Instantiate(mProjectilePrefab, mFirePoint.transform.position, mFirePoint.transform.rotation).GetComponent<Projectile>();
        bullet.SetSpriteDirection(transform.localScale);
        bullet.SetVelocity(transform.right * transform.localScale.x);
    }

    #endregion

    #region Photon Interface Functions

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo messageInfo)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mHealth);
        }
        else
        {
            mHealth = (int)stream.ReceiveNext();
        }
    }

    #endregion
}