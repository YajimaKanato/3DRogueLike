using UnityEngine.InputSystem;

/// <summary>InputActionのキャッシュをするPlayerViewの分割クラス</summary>
public partial class PlayerView
{
    InputAction _move;
    InputAction _run;
    InputAction _rotation;
    InputAction _jump;
    InputAction _attack;
    InputAction _sheathe;

    /// <summary>
    /// アクションをキャッシュするメソッド
    /// </summary>
    /// <param name="actions">InputActionのクラス</param>
    void CacheAction(Actions actions)
    {
        _actions = actions;
        _move = _actions.Ingame.Move;
        _run = _actions.Ingame.Run;
        _rotation = _actions.Ingame.Rotation;
        _jump = _actions.Ingame.Jump;
        _attack = _actions.Ingame.Attack;
        _sheathe = _actions.Ingame.Sheathe;
    }
}
