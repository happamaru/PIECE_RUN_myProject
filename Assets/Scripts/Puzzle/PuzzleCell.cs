using UnityEngine;

public class PuzzleCell : MonoBehaviour
{
    public int _cellPositionX;
    public int _cellPositionY;
    private (int, int) _cellPosition = default;

    private PuzzleTable _puzzleTable = default;

    // Start is called before the first frame update
    void Start()
    {
        _cellPosition = (_cellPositionX, _cellPositionY);

        _puzzleTable = GetComponentInParent<PuzzleTable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �s�[�X�����g�ɃJ�[�\�������킹�Ă�����Ă΂��
    /// </summary>
    /// <param name="pieceInfo"></param>
    public void OnSelected(PieceInfo pieceInfo)
    {
        // ���������点�鏈��

        _puzzleTable.CheckToSetable(pieceInfo, _cellPosition);
    }

    /// <summary>
    /// �s�[�X���Z����ŗ����ꂽ�Ƃ��ɌĂ΂��
    /// </summary>
    public bool OnSelectExit(PieceInfo pieceInfo)
    {
        if (_puzzleTable.CanSet)
        {
            _puzzleTable.SetPiece(pieceInfo, _cellPosition);
            return true;
        }

        return false;
    }

    /// <summary>
    /// �͂߂Ă���s�[�X���O���ꂽ�Ƃ�
    /// </summary>
    public void OnRemovePiece(PieceInfo pieceInfo)
    {
        _puzzleTable.RemovePiece(pieceInfo, _cellPosition);
    }
}