using UnityEngine;

public partial class PlayerView
{
    [Header("移動関連のパラメータ")]
    [Header("毎フレームかける力"), SerializeField] float _framePower = 0.5f;
    [Header("歩きの最大速度"), SerializeField] float _walkMaxSpeed = 10;
    [Header("走りの最大速度"), SerializeField] float _runMaxSpeed = 20;

    /// <summary>直前のフレームで入力された方向</summary>
    Vector2 _preDir;
    bool _isRunning;

    void Move()
    {
        _preDir = _move.ReadValue<Vector2>().normalized;
        var currentVelo = _rb.linearVelocity;
        currentVelo.y = 0;

        if ((_isRunning && currentVelo.sqrMagnitude < (_runMaxSpeed * _runMaxSpeed))
            || (!_isRunning && currentVelo.sqrMagnitude < (_walkMaxSpeed * _walkMaxSpeed)))
        {
            // 最大速度でなかったらオブジェクトの正面に対して加速
            Vector3 moveDir = transform.forward * _preDir.y + transform.right * _preDir.x;
            _rb.AddForce(moveDir * _framePower);
        }
    }

    void Run()
    {
        _isRunning = _run.IsPressed();
    }
}
