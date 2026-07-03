using UnityEngine;

public class InputActionManager : MonoBehaviour
{
    [SerializeField] bool _selfInitialize;
    [SerializeField] PlayerView _player;
    Actions _actions;

    static InputActionManager _instance;
    public static InputActionManager Instance => _instance;

    void Init()
    {
        if (_instance == null)
        {
            // DDOL設定
            _instance = this;
            DontDestroyOnLoad(gameObject);

            _actions = new Actions();
            _actions.Enable();
            _player?.Init(_actions);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        if (_selfInitialize) Init();
    }
}
