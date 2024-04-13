using UnityEngine;

[RequireComponent(typeof(HealthContainer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public float Speed = 5f;

    private Vector2 _velocity;
    private Rigidbody2D _rigidbody;

    private Animator _animator;

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

        if (_animator)
        {
            _animator.SetFloat("Speed", input.magnitude);
            _animator.SetFloat("MovementX", input.x);
            _animator.SetFloat("MovementY", input.y);
        }
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }
}
