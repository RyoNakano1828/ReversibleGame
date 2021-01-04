using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// マス目クラス
/// </summary>

public class Game_Cell : UIBehaviour
{

    // クリック可能か否か

    public bool clickable ;

    public void SetClickable(bool clickable)
    {
        this.clickable = clickable;
        if(clickable == true)
        {
            frame.color = Color.red;
            button.enabled = clickable;
        }
        else
        {
            frame.color = Color.black;
            button.enabled = clickable;
        }

    }
    public bool GetClickable()
    {
        return clickable;
    }

    // マスX座標
    public int X;
    // マスY座標  
    public int Y;

    // マスに配置されている石の色

    public void SetStoneColor(Game_Field.StoneColor color)
    {
        switch (color)
        {
            case Game_Field.StoneColor.None:
                stone.gameObject.SetActive(false);
                break;
            case Game_Field.StoneColor.Black:
                stone.gameObject.SetActive(true);
                stone.color = Color.black;
                break;
            case Game_Field.StoneColor.White:
                stone.gameObject.SetActive(true);
                stone.color = Color.white;
                break;
        }
    }

    //色の出力
    public Game_Field.StoneColor GetStoneColor()
    {
        if (stone.gameObject.activeSelf == false)
        {
            return Game_Field.StoneColor.None;
        }
        if(stone.color == Color.black)
        {
            return Game_Field.StoneColor.Black;
        }
        else
        {
            return Game_Field.StoneColor.White;
        }
    }



    [SerializeField]
    Image frame;
    [SerializeField]
    Image stone;
    Button button;
    
    

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// マスを初期化します
    /// </summary>
    
    public void Initialize(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// マスがクリックされた際の処理
    /// </summary>
    void OnClick()
    {
        Game_SceneController.Instance.OnCellClick(this);
        
    }
}
