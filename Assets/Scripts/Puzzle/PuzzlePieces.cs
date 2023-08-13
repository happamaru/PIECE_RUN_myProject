using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


/// <summary>
/// �s�[�X�̏����܂Ƃ߂ĊǗ�����N���X
/// </summary>
public class PuzzlePieces : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private PieceType _myPieceType;
    [Tooltip("Piece���\������Image��List")]
    [SerializeField] private List<Image> _imageList = new List<Image>();
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject[] _rotateObj;

    [Tooltip("�s�[�X�̏��")]
    private int[][,,] _pieceArray = new int[4][,,]
    {
        //I�~�m
        new int[,,]
        {
          {
            {1,0,0},
            {1,0,0},
            {0,0,0},
          },
            
          {
            {1,1,0},
            {0,0,0},
            {0,0,0},
          }
        },

        //L�~�m
        new int[,,]
        {
          {
            {1,0,0},
            {1,1,0},
            {0,0,0},
          },

          {
            {0,1,0},
            {1,1,0},
            {0,0,0},
          },

          {
            {1,1,0},
            {0,1,0},
            {0,0,0},
          },

          {
            {1,1,0},
            {1,0,0},
            {0,0,0},
          },

        },

        //�ʃ~�m
        new int[,,]
        {
          {
            {0,1,0},
            {1,1,1},
            {0,0,0},
          },  
          
          {
            {1,0,0},
            {1,1,0},
            {1,0,0},
          },        
          
          {
            {1,1,1},
            {0,1,0},
            {0,0,0},
          },      
          
          {
            {0,1,0},
            {1,1,0},
            {0,1,0},
          },
        },

        //T�~�m
        new int[,,]
        {
          {
            {1,1,1},
            {0,1,0},
            {0,1,0},
          },

          {
            {1,0,0},
            {1,1,1},
            {1,0,0},
          },

          {
            {0,1,0},
            {0,1,0},
            {1,1,1},
          },

          {
            {0,0,1},
            {1,1,1},
            {0,0,1},
          },
        }
    };

    [Tooltip("Status�㏸�l")]
    private int _increasedNum = 0;
    [Tooltip("�ŏ���Position")]
    private Vector3 _initialPosition;

    private PieceStatusType _pieceStatusType;
    private PieceInfo _pieceInfo;
    private bool _isMoveFenish = false;
    private bool _isSet = false;
    private bool _isSeize = false;
    private int _rotateCount = 0;
    private PuzzleCell _savePuzzleCell;

    private void Start()
    {
        DetermineAscendingValue();
        _pieceInfo = new PieceInfo(_pieceArray[(int)_myPieceType], _pieceStatusType, _increasedNum, _myPieceType);
        _initialPosition = transform.position;
    }

    private void Update()
    {
        //����ł���Ƃ��ɉE�N���b�N����������
        if (_isSeize && Input.GetMouseButtonDown(1))
        {
            RotateObj();
            _pieceInfo.SetRotateNum(_rotateCount);
            if(_savePuzzleCell) _savePuzzleCell.OnSelected(_pieceInfo);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        transform.position = eventData.position;
     
        List<RaycastResult> rayResult = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, rayResult);

        //Ray��hit�����I�u�W�F�N�g�ɖړI�̃I�u�W�F�N�g�����邩�`�F�b�N
        foreach (RaycastResult hit in rayResult)
        {
            if (hit.gameObject.TryGetComponent(out PuzzleCell puzzleCell)) 
            {
                _pieceInfo.SetRotateNum(_rotateCount);
                _savePuzzleCell = puzzleCell;
                _savePuzzleCell.OnSelected(_pieceInfo);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        _isSeize = true;
        if (_isSet)
        {
            _savePuzzleCell.OnRemovePiece(_pieceInfo);
            _pieceInfo.SetRotateNum(_rotateCount);
            _savePuzzleCell.OnSelected(_pieceInfo);
            _isSet = false;
        }
        else
        {
            _initialPosition = transform.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        _isSeize = false;
        //����ł�����̂𗣂����Ƃ�
        List<RaycastResult> rayResult = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, rayResult);

        //Ray��hit�����I�u�W�F�N�g�ɖړI�̃I�u�W�F�N�g�����邩�`�F�b�N
        foreach (RaycastResult hit in rayResult)
        {
            _pieceInfo.SetRotateNum(_rotateCount);
            if (hit.gameObject.TryGetComponent(out PuzzleCell puzzleCell))
            {
                if (puzzleCell.OnSelectExit(_pieceInfo)) 
                {
                    _savePuzzleCell = puzzleCell;
                    _isSet = true;
                    transform.position = hit.gameObject.transform.position;
                    _audioSource.Play();
                    return;
                }
            }
        }

        ReturnInitialPosition();
    }

    /// <summary>
    /// �s�[�X�̃X�e�[�^�X�^�C�v��ύX���郁�\�b�h
    /// </summary>
    /// <param name="pieceStatusType"></param>
    public void SetPieceStatusType(PieceStatusType pieceStatusType)
    {
        _pieceStatusType = pieceStatusType;

        if (pieceStatusType == PieceStatusType.Speed)
        {
            ChangeColor(Color.blue);
        }
        else if (pieceStatusType == PieceStatusType.Jump)
        {
            ChangeColor(Color.green);
        }
        else if (pieceStatusType == PieceStatusType.Hp)
        {
            ChangeColor(Color.red);
        }
    }

    /// <summary>
    /// ���̏ꏊ�ɖ߂鏈��
    /// </summary>
    public void ReturnInitialPosition()
    {
        DOTween.To(() => transform.position,
        x => transform.position = x,
        _initialPosition, 0.5f)
        .OnStart(() => _isMoveFenish = true)
        .OnComplete(() =>
        {
            transform.position = _initialPosition;
            _isMoveFenish = false;
        });
    }

    /// <summary>
    /// �s�[�X�̐��ŃX�e�[�^�X�̏㏸�l�����肷��
    /// </summary>
    public void DetermineAscendingValue()
    {
        var pieces = _pieceArray[(int)_myPieceType];
        var pieceCount = 0;

        for (int x = 0; x < pieces.GetLength(2); x++)
        {
            for (int y = 0; y < pieces.GetLength(1); y++)
            {
                if (pieces[_rotateCount, y, x] == 1) pieceCount++;
            }
        }

        _increasedNum = pieceCount;
    }

    /// <summary>
    /// Image��Color�����ׂĕς���
    /// </summary>
    /// <param name="color"></param>
    private void ChangeColor(Color color)
    {
        for (int i = 0; i < _imageList.Count; i++)
        {
            _imageList[i].color = color;
        }
    }

    private void RotateObj()
    {
        _rotateObj[_rotateCount].SetActive(false);

        if (_pieceArray[(int)_myPieceType].GetLength(0) - 2 < _rotateCount)
        {
            _rotateCount = 0;
        }
        else 
        {
            _rotateCount++;
        }
        Debug.Log(_rotateCount);

        _rotateObj[_rotateCount].SetActive(true);
    }
}

public enum PieceType
{
    IPiece,
    LPiece,
    ConvexPiece,
    TPiece
}
