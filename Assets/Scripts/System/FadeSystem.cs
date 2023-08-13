using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSystem : MonoBehaviour
{
    [SerializeField, Tooltip("Fade�̃p�l��")]
    private Image _image;

    [SceneName]
    [SerializeField] private string _nextScene;
    [SerializeField] private bool _isStart = false;

    void Start()
    {
        if (_isStart)
        {
            FadeIn();
        }
    }

    public void GoToTitleScene() 
    {
        _nextScene = "TitleScene";
        FadeOut();
    }

    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    public void FadeOut()
    {
        Debug.Log("�Ă΂ꂽ");
        _image.enabled = true;
        _image.DOFade(1, 0.8f)
            .SetDelay(0.5f)
            //fadeout���I�������Ă΂��
            .OnComplete(() => SceneManager.LoadScene(_nextScene));
    }

    /// <summary>
    /// �t�F�[�h�C��
    /// </summary>
    public void FadeIn()
    {
        _image.enabled = true;
        _image.DOFade(0, 1.5f)
        .SetDelay(0.5f)
        .OnComplete(() => _image.enabled = false);
    }
}
