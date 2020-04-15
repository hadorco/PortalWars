using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Serialized fields
    [SerializeField] float mRunSpeed = 10f;
    [SerializeField] float mJumpSpeed = 15f;
    [SerializeField] GameObject mProjectilePrefab;
    [SerializeField] GameObject mFirePoint; // The starting point from which bullets are fired

    // Cached references
    Rigidbody2D mRigidBody;
    Animator mAnimator;
    CapsuleCollider2D mBodyCollider;
    BoxCollider2D mFeetCollider;
    private Dictionary<string, string> mInputDictionary;

    // State
    int mHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mBodyCollider = GetComponent<CapsuleCollider2D>();
        mFeetCollider = GetComponent<BoxCollider2D>();
        mInputDictionary = InputHelper.CalibrateInput(gameObject.tag);
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipSprite();
        Shoot();
    }

    public void TakeDamage(int damage)
    {
        mHealth -= damage;

        Debug.Log("Player health after taking damage: " + mHealth);
        if (mHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FireBullet()
    {
        Projectile bullet = Instantiate(mProjectilePrefab, mFirePoint.transform.position, mFirePoint.transform.rotation).GetComponent<Projectile>();
        bullet.SetSpriteDirection(transform.localScale);
        bullet.SetVelocity(transform.right * transform.localScale.x);
    }

    private void Shoot()
    {
        if (Input.GetButtonDown(mInputDictionary["Fire"]))
        {
            //Create bullet
            FireBullet();

            //Set animation
            mAnimator.SetTrigger("Shooting");
        }
    }

    private void Jump()
    {
        int foregroundLayer = LayerMask.GetMask("Foreground");

        if (Input.GetButtonDown(mInputDictionary["Jump"]) && mFeetCollider.IsTouchingLayers(foregroundLayer))
        {
            Vector2 currentVelocity = mRigidBody.velocity;
            currentVelocity += new Vector2(0, mJumpSpeed);
            mRigidBody.velocity = currentVelocity;

            mAnimator.SetBool("Jumping", true);
        }
        // We use 0.005f because when landing into corners our rigidbody's velocity still has a nonzero value, so the animation gets stuck
        else if (mFeetCollider.IsTouchingLayers(foregroundLayer) && Mathf.Abs(mRigidBody.velocity.y) < 0.0005f)
        {
            mAnimator.SetBool("Jumping", false);
        }
    }

    private void Run()
    {
        float horizontalAxis = Input.GetAxis(mInputDictionary["Horizontal"]);
        Vector2 newVelocity = new Vector2(horizontalAxis * mRunSpeed, mRigidBody.velocity.y);
        mRigidBody.velocity = newVelocity;
        if (horizontalAxis != 0)
        {
            //Change animation
            mAnimator.SetBool("Running", true);
        }
        else
        {
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

    private void CalibrateInput()
    {

    }
}
