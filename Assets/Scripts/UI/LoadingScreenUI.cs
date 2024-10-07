using GameEvents;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LoadingScreenUI : MonoBehaviour
{
    [Header("Broadcasting on channels")]
    public BoolEvent _loadingScreenToggled;

    [Header("Private Dependencies")]
    private Animator _animator;

    private int _showHash;
    private int _hideHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _showHash = Animator.StringToHash("Show");
        _hideHash = Animator.StringToHash("Hide");
    }

    // Function that will be called from SceneLoaderManager
    public void ToggleScreen(bool enable)
    {
        if (enable)
        {
            _animator.SetTrigger(_showHash);
        }
        else
        {
            _animator.SetTrigger(_hideHash);
        }
    }

    // Function that will be called from Animator at the end of the clip
    public void SendLoadingScreenShownEvent()
    {
        _loadingScreenToggled.Raise(true);
    }

    // Function that will be called from Animator at the end of the clip
    public void SendLoadingScreenHiddenEvent()
    {
        _loadingScreenToggled.Raise(false);
    }
}