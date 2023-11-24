using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField]private SpriteRenderer sr;
    
    [Header("Events")]
    public static Action<Fruit,Fruit> OnCollisionWithFruit;

    [Header("Data")]
    [SerializeField] private FruitType type;
    private bool _hasCollided;
    private bool _canBeMerged;

    [Header("Effects")]
    [SerializeField] private ParticleSystem mergeParticles;

    private void Start()
    {
        Invoke("AllowMerge",0.25f);
    }

    private void AllowMerge()
    {
        _canBeMerged = true;
    }

    public void EnablePhysics()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
       ManageCollision(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        ManageCollision(col);
    }

    private void ManageCollision(Collision2D col)
    {
        _hasCollided = true;

        if (!_canBeMerged)
        {
            return;
        }
        
        if (col.collider.TryGetComponent(out Fruit otherFruit))
        {
            if (otherFruit.GetFruitType()!=type)
            {
                return;
            }

            if (!otherFruit.CanBeMerged())
            {
                return;
            }
            
            OnCollisionWithFruit?.Invoke(this,otherFruit);
        }
    }

    public FruitType GetFruitType()
    {
        return type;
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }

    public bool HasCollided()
    {
        return _hasCollided;
    }

    public bool CanBeMerged()
    {
        return _canBeMerged;
    }

    public void Merge()
    {
        if (mergeParticles!=null)
        {
            mergeParticles.transform.SetParent(null);
            mergeParticles.Play();
        }
        
        Destroy(gameObject);
    }
}
