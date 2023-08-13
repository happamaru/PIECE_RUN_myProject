using UnityEngine;

public class PuzzleTable : MonoBehaviour
{
    [SerializeField, Tooltip("HP�����o�[")]
    private PuzzleGauge _hpGauge;
    [SerializeField, Tooltip("�W�����v�����o�[")]
    private PuzzleGauge _jumpGauge;
    [SerializeField, Tooltip("�X�s�[�h�����o�[")]
    private PuzzleGauge _speedGauge;

    private enum TableSize
    {
        Three,
        five
    }
    [SerializeField, Tooltip("�p�Y���e�[�u���̑傫��")]
    private TableSize _tableSize = default;

    [Tooltip("�p�Y���̈�ӂ̑傫��")]
    private int _puzzleSize = default;
    [Tooltip("_tableArray�̍���Z���̃|�C���^")]
    private (int x, int y) _startCellPointer = default;

    [Tooltip("�p�Y���e�[�u�����Ǘ�����z��")]
    private int[,] _tableArray = default;

    [Tooltip("�s�[�X���Z�b�g�\")]
    private bool _canSet = false;

    private int _hp = default;
    private int _jump = default;
    private int _speed = default;

    /// <summary>
    /// �s�[�X���Z�b�g�\
    /// </summary>
    public bool CanSet
    {
        get
        {
            return _canSet;
        }
    }


    private void Start()
    {
        //CheckToSetable(new PieceInfo(new int[3, 3] { { 1, 0, 0 }, { 1, 0, 0 }, { 0, 0, 0 } }, PieceStatusType.Hp, 1, PieceType.IPiece), (0, 0));

        // �v���n�u�̎�ނɂ���āA�p�Y���̑傫����������
        switch (_tableSize)
        {
            case TableSize.Three:
                _puzzleSize = 3;
                _tableArray = new int[7, 7]
                {
                    {1, 1, 1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1, 1, 1},
                    {1, 1, 0, 0, 0, 1, 1},
                    {1, 1, 0, 0, 0, 1, 1},
                    {1, 1, 0, 0, 0, 1, 1},
                    {1, 1, 1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1, 1, 1}
                };

                _startCellPointer = (2, 2);
                break;

            case TableSize.five:
                _puzzleSize = 5;
                _tableArray = new int[13, 13]
                {
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                };

                _startCellPointer = (4, 4);
                break;
        }

        // �Q�[�W�̏�����
        _hpGauge.SetValue(0);
        _jumpGauge.SetValue(0);
        _speedGauge.SetValue(0);
    }


    /// <summary>
    /// �s�[�X���Z�b�g�ł��邩�`�F�b�N����
    /// </summary>
    public void CheckToSetable(PieceInfo pieceInfo, (int x, int y) cellPosition)
    {
        (int x, int y) tableArrayPointer = (_startCellPointer.x + cellPosition.x, _startCellPointer.y + cellPosition.y);

        // �n���ꂽ�s�[�X��3*3�̔z����A���ォ�珇�ԂɃ`�F�b�N����
        for (int yy = 0; yy < 3; yy++)
        {
            for (int xx = 0; xx < 3; xx++)
            {
                // �󔒂̃}�X�i�`���Ȃ��ӏ��j�Ȃ�X�L�b�v���Ď��̔�r��
                if (pieceInfo.PieceArray[pieceInfo.RotateNum, yy, xx] == 0) continue;

                // ���łɑ��̃s�[�X���͂܂��Ă��邩�A�g�O��������
                if (_tableArray[tableArrayPointer.y + yy, tableArrayPointer.x + xx] == 1)
                {
                    _canSet = false;
                    return;
                }
            }
        }

        _canSet = true;
    }

    /// <summary>
    /// �s�[�X���͂߂�
    /// </summary>
    /// <param name="pieceInfo"></param>
    /// <param name="pointer"></param>
    public void SetPiece(PieceInfo pieceInfo, (int x, int y) cellPosition)
    {
        (int x, int y) tableArrayPointer = (_startCellPointer.x + cellPosition.x, _startCellPointer.y + cellPosition.y);

        for (int yy = 0; yy < 3; yy++)
        {
            for (int xx = 0; xx < 3; xx++)
            {
                if (pieceInfo.PieceArray[pieceInfo.RotateNum, yy, xx] == 0) continue;

                _tableArray[tableArrayPointer.y + yy, tableArrayPointer.x + xx] = 1;
            }
        }

        _canSet = false;

        switch (pieceInfo.PieceStatus)
        {
            case PieceStatusType.Hp:
                _hp += pieceInfo.InscreasedNum;
                _hpGauge.AddValue(pieceInfo.InscreasedNum);
                break;

            case PieceStatusType.Jump:
                _jump += pieceInfo.InscreasedNum;
                _jumpGauge.AddValue(pieceInfo.InscreasedNum);
                break;

            case PieceStatusType.Speed:
                _speed += pieceInfo.InscreasedNum;
                _speedGauge.AddValue(pieceInfo.InscreasedNum);
                break;
        }

#if UNITY_EDITOR
        PuzzleDebug();
#endif
    }

    /// <summary>
    /// �s�[�X���O��
    /// </summary>
    /// <param name="pieceInfo"></param>
    /// <param name="cellPosition"></param>
    public void RemovePiece(PieceInfo pieceInfo, (int x, int y) cellPosition)
    {
        (int x, int y) tableArrayPointer = (_startCellPointer.x + cellPosition.x, _startCellPointer.y + cellPosition.y);

        for (int yy = 0; yy < 3; yy++)
        {
            for (int xx = 0; xx < 3; xx++)
            {
                if (pieceInfo.PieceArray[pieceInfo.RotateNum, yy, xx] == 0) continue;

                _tableArray[tableArrayPointer.y + yy, tableArrayPointer.x + xx] = 0;
            }
        }

        switch (pieceInfo.PieceStatus)
        {
            case PieceStatusType.Hp:
                _hpGauge.AddValue(-pieceInfo.InscreasedNum);
                break;

            case PieceStatusType.Jump:
                _jumpGauge.AddValue(-pieceInfo.InscreasedNum);
                break;

            case PieceStatusType.Speed:
                _speedGauge.AddValue(-pieceInfo.InscreasedNum);
                break;
        }

#if UNITY_EDITOR
        PuzzleDebug();
#endif
    }

    /// <summary>
    /// ���ׂẴs�[�X�����Z�b�g���鏈��
    /// </summary>
    public void AllRemovePiece()
    {
        for (int yy = 0; yy < _puzzleSize; yy++)
        {
            for (int xx = 0; xx < _puzzleSize; xx++)
            {
                _tableArray[yy + _startCellPointer.y, xx + _startCellPointer.x] = 0;
            }
        }

        _hpGauge.SetValue(0);
        _jumpGauge.SetValue(0);
        _speedGauge.SetValue(0);

#if UNITY_EDITOR
        PuzzleDebug();
#endif
    }

    /// <summary>
    /// �p�Y���t�F�[�Y���I�����ăX�e�[�^�X�𑗐M
    /// </summary>
    public void FinishPuzzlePhase()
    {
        Debug.Log("�X�e�[�^�X���M�I" + "\n" +
            "HP�F" + _hp + "\n" +
            "Jump�F" + _jump + "\n" +
            "Speed�F" + _speed);

        NewPlayerMove move = GameObject.FindWithTag("Player").GetComponent<NewPlayerMove>();
        PlayerHP hp = GameObject.FindWithTag("Player").GetComponent<PlayerHP>();
        move.JumpValue = _jump;
        move.SpeedValue = _speed;
        hp.HpValue = _hp;
    }

#if UNITY_EDITOR
    /// <summary>
    /// ���݂̃p�Y���̏����R���\�[���ɕ\������
    /// </summary>
    private void PuzzleDebug()
    {
        string comma = ", ";
        int x = _startCellPointer.x;
        int y = _startCellPointer.y;

        var log = "\n";
        for (int yy = 0; yy < _puzzleSize; yy++)
        {
            for (int xx = 0; xx < _puzzleSize; xx++)
            {
                log += _tableArray[y + yy, x + xx] + comma;
            }

            log += "\n";
        }

        Debug.Log(log);
    }
#endif
}
