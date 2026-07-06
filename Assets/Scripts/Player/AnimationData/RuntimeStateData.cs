using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class RuntimeStateData : IState
{
    AnimationPlayer _animator;
    AnimationClipPlayable _animationClipPlayable;
    StateName _stateName;
    bool _hasExitTime;
    float _exitTime;
    float _transitionDuration;
    bool _isLoop;

    public StateName StateName => _stateName;

    public RuntimeStateData(AnimationPlayer animator, StateData stateData)
    {
        _animator = animator;
        _stateName = stateData.State;
        _hasExitTime = stateData.HasExitTime;
        _exitTime = stateData.ExitTime;
        _transitionDuration = stateData.TransitionDuration;
        _isLoop = stateData.IsLoop;
    }

    public void Entry()
    {
        _animationClipPlayable = _animator.Play(_stateName);
    }

    public async UniTask Exit(CancellationToken token)
    {
        // HasExitTimeを使用しないまたは使用する場合は時間になったら次へ
        await UniTask.WaitUntil(
            () => !_hasExitTime
            || _animationClipPlayable.GetTime() >= _exitTime
            , cancellationToken: token);
    }

    public StateName Update()
    {
        // フェード（ブレンド）
        _animator.Fade(_transitionDuration);

        var nextState = _stateName;
        // ループか一回限りで時間まで流したら条件判定
        if ((!_isLoop && _animationClipPlayable.GetTime() >= _animationClipPlayable.GetDuration())
            || _isLoop)
        {
            nextState = EvaluateConditions();
        }

        return nextState;
    }

    StateName EvaluateConditions()
    {
        return StateName.Idle;
    }
}
