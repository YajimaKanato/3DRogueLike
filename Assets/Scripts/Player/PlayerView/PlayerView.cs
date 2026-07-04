using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Animator))]
public partial class PlayerView : MonoBehaviour
{
    Actions _actions;
    Rigidbody _rb;
    CapsuleCollider _cc;
    Animator _animator;

    public void Init(Actions actions)
    {
        CacheAction(actions);
        _rb = GetComponent<Rigidbody>();
        _cc = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Rotation();
        Attack();
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        var currentVelo = _rb.linearVelocity;
        currentVelo.y = 0;
        _animator?.SetFloat("Velocity", currentVelo.sqrMagnitude);
        // Rigidbody.linearVelocityとtransformの右や前との内積を取ると
        // 内積を取った方向にかかる速度の大きさがわかる
        _animator?.SetFloat("MoveX", _preDir.x);
        _animator?.SetFloat("MoveZ", _preDir.y);
        _animator?.SetFloat("VelocityVertical", _rb.linearVelocity.y);
        _animator?.SetBool("IsGround", _isGround);
        _animator?.SetBool("Attack", _attackBuffer.HasInput);
    }

    private void OnDrawGizmosSelected()
    {
        DrawSphereCast();
    }
}
