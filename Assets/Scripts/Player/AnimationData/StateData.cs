using System;
using UnityEngine;

[Serializable]
public class StateData
{
    [SerializeField] StateName _state;
    [SerializeField] AnimationClip _clip;
    [SerializeField] bool _hasExitTime;
    [SerializeField] float _exitTime;
    [SerializeField] float _transitionDuration;
    [SerializeField] bool _isLoop;

    public StateName State => _state;
    public AnimationClip Clip => _clip;
    public bool HasExitTime => _hasExitTime;
    public float ExitTime => _exitTime;
    public float TransitionDuration => _transitionDuration;
    public bool IsLoop => _isLoop;
}
