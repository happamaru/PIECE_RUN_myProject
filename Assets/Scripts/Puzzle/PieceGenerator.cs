using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
/// <summary>
/// �s�[�X�𐶐����邽�߂̃N���X
/// </summary>
public class PieceGenerator : MonoBehaviour
{
    [Tooltip("��������ꏊ�Ɛ�")]
    [SerializeField] private Transform[] _generationPos;
    [SerializeField] private GameObject[] _piecePrefab;
    [Tooltip("��������ۂ̐e")]
    [SerializeField] private Transform _parent;

    void Start()
    {
        PieceGeneration();
    }

    /// <summary>
    /// �s�[�X�̐���
    /// </summary>
    private void PieceGeneration()
    {
        for (int i = 0; i < _generationPos.Length; i++)
        {
            var num = Random.Range(0, _piecePrefab.Length);
            var obj = Instantiate(_piecePrefab[num], _generationPos[i].position, _generationPos[i].rotation);
            //�e�̐ݒ�
            obj.transform.parent = _parent;

            //Status��Type�������_���ɐݒ�
            if (obj.TryGetComponent(out PuzzlePieces pieces))
            {
                var value = Random.Range(0, Enum.GetValues(typeof(PieceStatusType)).Length);
                pieces.SetPieceStatusType((PieceStatusType)value);
            }
        }
    }

    public void Generate() 
    {
        for (int i = 0; i < _parent.childCount; i++) 
        {
           Destroy(_parent.GetChild(i).gameObject);
        }

        for (int i = 0; i < _generationPos.Length; i++)
        {
            var num = Random.Range(0, _piecePrefab.Length);
            var obj = Instantiate(_piecePrefab[num], _generationPos[i].position, _generationPos[i].rotation);
            //�e�̐ݒ�
            obj.transform.parent = _parent;

            //Status��Type�������_���ɐݒ�
            if (obj.TryGetComponent(out PuzzlePieces pieces))
            {
                var value = Random.Range(0, Enum.GetValues(typeof(PieceStatusType)).Length);
                pieces.SetPieceStatusType((PieceStatusType)value);
            }
        }
    }
}
