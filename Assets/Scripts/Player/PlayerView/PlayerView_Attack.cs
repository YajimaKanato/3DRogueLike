using UnityEngine;

public partial class PlayerView
{
    [Header("攻撃の入力有効時間"), SerializeField] InputBuffer _attackBuffer;

    void Attack()
    {
        if (_attack.WasPressedThisFrame()) _attackBuffer?.Push();
    }
}
