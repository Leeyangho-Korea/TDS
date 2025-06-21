using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private float _currentSpeed;
    [SerializeField] Renderer[] _bgs;

    bool _isZombieCommand = false; // 좀비 명령 여부

    public static Background Instance;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        _currentSpeed = scrollSpeed;
    }

    void Update()
    {
        for(int i = 0; i < _bgs.Length; i++)
        {
            Vector2 offset = _bgs[i].material.mainTextureOffset;
            offset.x += _currentSpeed * Time.deltaTime;
            _bgs[i].material.mainTextureOffset = offset;
        }
    }

    public void StopScroll(bool isZombieCommand = false)
    {
        if (isZombieCommand) _isZombieCommand = true;

        if (_currentSpeed == 0) return;

        _currentSpeed = 0f;
    }

    public void ResumeScroll()
    {
        if(_currentSpeed == scrollSpeed || _isZombieCommand) return;

        _currentSpeed = scrollSpeed;
    }
}
