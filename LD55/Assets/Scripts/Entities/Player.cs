using System;
using UnityEngine;

[RequireComponent(typeof(HealthContainer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public float Speed = 5f;
    public SpriteRenderer SpriteRenderer;
    
    
    private Vector2 _velocity;
    private Rigidbody2D _rigidbody;
    

    private Animator _animator;
    
    private bool _faceLeft = false;

    [SerializeField]
    private Orbs _orbsContainer;
    [SerializeField]
    private SpellResources _spellResources;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _velocity = input.normalized * Speed;

        // check if the player is facing left
        if (input.x != 0)
        {
            _faceLeft = input.x < 0;
        }

        if (_animator)
        {
            _animator.SetFloat("Speed", input.magnitude);
        }
        
        //flip the sprite if the player is facing left
        SpriteRenderer.flipX = !_faceLeft;
        
        if (Input.GetButtonDown("Fire1"))
        {
            _spellResources.CastSpell();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _spellResources.EnhanceType(SpellResources.SpellType.Utility);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _spellResources.EnhanceType(SpellResources.SpellType.Offensive);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _spellResources.EnhanceType(SpellResources.SpellType.Defensive);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //flip sprite on the x axis
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
        }
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }

    public int GetSortOrder()
    { 
        return SpriteRenderer.sortingOrder;
    }
}
