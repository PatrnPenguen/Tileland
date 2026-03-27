using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] public int health;
    [SerializeField] private bool isBig;
    private float scale;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] float deathSFXVolume;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isBig)
        {
            scale = 3f;
        }
        else
        {
            scale = 1;
        }
    }

    void Update()
    {
        rb.linearVelocity = new Vector2 (moveSpeed, rb.linearVelocity.y);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Platforms"))
        {
            moveSpeed = -moveSpeed;
            EnemyFlip();
        }
    }

    private void EnemyFlip()
    {
        transform.localScale = new Vector2(-Mathf.Sign(rb.linearVelocity.x)*scale, 
                                            rb.transform.localScale.y);
    }

    public void Hit(int damage)
    {
        health -= damage;
    }

    public void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, transform.position, deathSFXVolume);
        Destroy(gameObject);
    }
}
