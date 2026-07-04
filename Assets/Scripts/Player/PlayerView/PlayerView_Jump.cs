using UnityEngine;

public partial class PlayerView
{
    [Header("ジャンプ関連のパラメータ")]
    [Header("ジャンプの力"), SerializeField] float _jumpPower = 10;
    [Header("ジャンプの入力有効時間"), SerializeField] InputBuffer _jumpBuffer;
    [Header("SphereCastの半径"), SerializeField] float _sphereCastRadius = 0.1f;
    [Header("SphereCastを飛ばす距離"), SerializeField] float _rayDistance;
    [Header("当たり判定を有効にする対象"), SerializeField] LayerMask _groundLayer;
    bool _isGround;

    void Jump()
    {
        _isGround = Physics.SphereCast(transform.position, _sphereCastRadius, Vector3.down, out var hit, _rayDistance, _groundLayer);

        if (_jump.WasPressedThisFrame())
        {
            _jumpBuffer?.Push();
            if (_isGround) ExecuteJump();   // 別口で呼び出すようにする
        }
    }

    void ExecuteJump()
    {
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
    }

    void DrawSphereCast()
    {
        // Gizmoの色設定
        Gizmos.color = Color.green;

        // SphereCastを飛ばした軌跡の情報
        var origin = transform.position;
        var end = origin + Vector3.down * _rayDistance;

        // 開始位置の球、終了位置の球の描画
        Gizmos.DrawWireSphere(origin, _sphereCastRadius);
        Gizmos.DrawWireSphere(end, _sphereCastRadius);

        // 球と球を結ぶ線を描画
        Gizmos.DrawLine(origin + transform.right * _sphereCastRadius, end + transform.right * _sphereCastRadius);
        Gizmos.DrawLine(origin - transform.right * _sphereCastRadius, end - transform.right * _sphereCastRadius);
        Gizmos.DrawLine(origin + transform.forward * _sphereCastRadius, end + transform.forward * _sphereCastRadius);
        Gizmos.DrawLine(origin - transform.forward * _sphereCastRadius, end - transform.forward * _sphereCastRadius);
    }
}
