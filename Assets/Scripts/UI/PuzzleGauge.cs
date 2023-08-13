using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGauge : MonoBehaviour
{
    [Header("Time��RectTransform")]
    [SerializeField] private RectTransform _rectCurrent;
    [Tooltip("�o�[�̍ő�l")]
    [SerializeField] private float _maxNum;
    [Tooltip("�o�[�Œ��̒���")]
    private float _maxWidth;
    [Tooltip("���݂̒l")]
    private float _currentNum;

    private void Awake()
    {
        _maxWidth = _rectCurrent.sizeDelta.x;
        SetValue(0);
    }

    /// <summary>
    /// �l�ɂ���ăo�[�̒�����ς���
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        _currentNum = value;    
        //�o�[�̒������X�V
        _rectCurrent.SetWidth(GetWidth(value));
    }

    /// <summary>
    /// ���݂̒l�Ɉ����ŗ^����ꂽ�l�𑫂��Z����
    /// </summary>
    /// <param name="value"></param>
    public void AddValue(float value) 
    {
        _currentNum += value;
        _rectCurrent.SetWidth(GetWidth(_currentNum));
    }

    private float GetWidth(float value)
    {
        float width = Mathf.InverseLerp(0, _maxNum, value);
        return Mathf.Lerp(0, _maxWidth, width);
    }
}

public static class UIExtensions
{
    /// <summary>
    /// ���݂̒l��Rect�ɃZ�b�g����
    /// </summary>
    /// <param name="width"></param>
    public static void SetWidth(this RectTransform rect, float width)
    {
        Vector2 s = rect.sizeDelta;
        s.x = width;
        rect.sizeDelta = s;
    }
}