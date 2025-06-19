using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    [SerializeField] Image _fillImage;
    [SerializeField] CanvasGroup _canvasGroup;
    public float fadeDelay = 1.0f;     // 피격 후 몇 초 뒤에 페이드 시작
    public float fadeDuration = 1.0f;  // 페이드 아웃 시간

    private Tween _fadeTween;
    private float _maxHp;
    private float _currentHp;

    public void Init(float maxHp)
    {
        this._maxHp = maxHp;
        _currentHp = maxHp;
        _fillImage.fillAmount = 1f;
        _canvasGroup.alpha = 0f;
    }

    public void SetHP(float hp)
    {
        _currentHp = Mathf.Clamp(hp, 0, _maxHp);
        _fillImage.fillAmount = _currentHp / _maxHp;
        ShowAndFade();
    }

    void ShowAndFade()
    {
        _fadeTween?.Kill();
        _canvasGroup.alpha = 1f;

        _fadeTween = DOVirtual.DelayedCall(fadeDelay, () => {
            _canvasGroup.DOFade(0f, fadeDuration);
        });
    }
}
