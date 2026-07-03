using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerView
{
    [Header("移動関連のパラメータ")]
    [SerializeField] PlayerMoveParam _moveParam;
    [Header("回転関連のパラメータ")]
    [Header("マウスによる回転感度\n1ピクセルでどれくらい回転するか"), SerializeField] float _mouseRotation = 0.5f;
    [Header("ゲームパッドによる回転感度\n1秒間に最大どれくらい回転するか"), SerializeField] float _gamepadRotation = 180;

    /// <summary>直前のフレームで入力された方向</summary>
    Vector2 _preDir;
    bool _isRunning;

    void Move()
    {
        var dir = _move.ReadValue<Vector2>();
        // キーボード入力の場合は正規化
        if (Gamepad.current == null || Gamepad.current.rightStick.ReadValue().sqrMagnitude <= 0.01f)
            dir = dir.normalized;
        if (dir != Vector2.zero) _preDir = dir;

        // 速度取得
        var velo = _moveParam != null ? _moveParam.GetVelocity(_isRunning, dir) : Vector3.zero;
        var moveDir = transform.forward * velo.z + transform.right * velo.x;
        moveDir.y = _rb.linearVelocity.y;
        _rb.linearVelocity = moveDir;
        // 入力と移動速度が一致した時に入力保存
    }

    void Run()
    {
        _isRunning = _run.IsPressed();
    }

    void Rotation()
    {
        var look = _rotation.ReadValue<Vector2>();

        // ゲームパッドによる入力かどうかを判定
        var usingGamepad = Gamepad.current != null
            && Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.01f;

        // 入力デバイスによって回転量を変えて計算
        float yaw;
        if (usingGamepad)
        {
            yaw = look.x * _gamepadRotation * Time.deltaTime;
        }
        else
        {
            yaw = look.x * _mouseRotation;
        }

        transform.Rotate(0, yaw, 0);
    }
}
