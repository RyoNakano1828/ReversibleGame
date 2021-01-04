using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ゲームシーン制御クラス
/// </summary>
public class Game_SceneController : MonoBehaviour
{
    public static Game_SceneController Instance;
   
    static Game_SceneController instance;

    [SerializeField]
    Game_Field field;
    [SerializeField]
    Game_Message message;

    int turnNumber;

    /// <summary>
    /// 今の手番プレイヤーの石の色
    /// </summary>
    
    Game_Field.StoneColor CurrentPlayerStoneColor
    {
        get { return (Game_Field.StoneColor)((turnNumber + 1) % 2 + 1); }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameStart();
    }

    /// <summary>
    /// ゲームを開始します
    /// </summary>
    public void GameStart()
    {
        turnNumber = 0;
        field.Initialize();
        IncrementTurnNumber();
    }

    /// <summary>
    /// マスがクリックされた時の処理です
    /// </summary>
    /// <param name="cell">Cell.</param>
    public void OnCellClick(Game_Cell cell)
    {
        field.Lock();
        cell.SetStoneColor(Instance.CurrentPlayerStoneColor);
        Game_SoundManager.Instance.put.Play();
        field.TurnOverStoneIfPossible(cell);
    }

    /// <summary>
    /// 石をひっくり返し終わった後の処理です
    /// </summary>
    public void OnTurnStoneFinished()
    {
        StartCoroutine(NextTurnCoroutine());
        
    }

    /// <summary>
    /// 次の手番に移るコルーチンです
    /// </summary>
    /// <returns>The turn coroutine.</returns>
    IEnumerator NextTurnCoroutine()
    {
        if(field.CountStone(Game_Field.StoneColor.None) == 0)
        {
            yield return message.Show("Game FINISHED");
            StartCoroutine(GameFinishedCoroutine());
        }
        else
        {
            IncrementTurnNumber();
            if(field.CountClickableCells() == 0)
            {
                yield return message.Show(string.Format("{0} cannot put stone. Turn Skipped", CurrentPlayerStoneColor.ToString()));
                IncrementTurnNumber();
                if(field.CountClickableCells() == 0)
                {
                    yield return message.Show(string.Format("{0} cannot put stone too. Game Finished", CurrentPlayerStoneColor.ToString()));
                    StartCoroutine(GameFinishedCoroutine());
                }
            }
        }
    }

    /// <summary>
    /// 手番番号をインクリメントし、盤面を更新します
    /// </summary>
    void IncrementTurnNumber()
    {
        turnNumber++;
        field.UpdateCellsClickable(CurrentPlayerStoneColor);
    }

    /// <summary>
    /// ゲーム終了時のコルーチンです
    /// </summary>
   
    IEnumerator GameFinishedCoroutine()
    {
        string winner = "Draw";
        int blackCount = field.CountStone(Game_Field.StoneColor.Black);
        int whiteCount = field.CountStone(Game_Field.StoneColor.White);

        if(blackCount > whiteCount)
        {
            winner = " Black Win";
        }
        else if(blackCount < whiteCount)
        {
            winner = "White Win";
        }

        yield return message.Show($"{winner} Black[{blackCount}] : White[{whiteCount}]");
    }
}
