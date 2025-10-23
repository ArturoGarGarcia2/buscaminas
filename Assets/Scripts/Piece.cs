using UnityEngine;
using TMPro;

public class Piece : MonoBehaviour
{
    [SerializeField] private int x, y;
    [SerializeField] private bool bomb, check, flagged;
    private Color[] colors = {
        new Color(0f, 0f, 1f),
        new Color(0f, 0.5f, 0f),
        new Color(1f, 0f, 0f),
        new Color(0f, 0f, 0.5f),
        new Color(0.5f, 0f, 0f),
        new Color(0f, 0.5f, 0.5f),
        new Color(0f, 0f, 0f),
        new Color(0.5f, 0.5f, 0.5f)
    };

    [SerializeField] private GameObject bombRender, flag;

    public void Start()
    {
        bombRender.SetActive(false);
        flag.SetActive(false);
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public void setY(int y){
        this.y = y;
    }

    public void setBomb(bool bomb){
        this.bomb = bomb;
    }


    public int getX(){
        return x;
    }

    public int getY(){
        return y;
    }

    public bool IsBomb(){
        return bomb;
    }

    public void SetCheck(bool ch)
    {
        check = ch;
    }

    public bool IsChecked()
    {
        return check;
    }

    public bool IsFlagged()
    {
        return flagged;
    }

    public void DrawBombs()
    {
        if (!IsChecked() && !flagged)
        {
            SetCheck(true);
            if (IsBomb())
            {
                GetComponent<SpriteRenderer>().material.color = Color.red;
                bombRender.SetActive(true);
                Generator.gen.Die();
                GameManager.instance.LoseGame();
            }
            else
            {
                int bombsNumber = Generator.gen.GetBombsAround(x, y);

                GetComponent<SpriteRenderer>().material.color = Color.gray;
                if (bombsNumber != 0)
                {
                    transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = bombsNumber.ToString();
                    transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = colors[bombsNumber - 1];
                }
                else
                {
                    Generator.gen.CheckPieceAround(x, y);
                }
            }
        }

    }

    public void FlagPiece()
    {
        // if (IsChecked()) return;
        flagged = !flagged;
        flag.SetActive(flagged);
    }

    private void OnMouseOver()
    {
        if (Generator.gen.dead) return;

        if (Input.GetMouseButtonDown(0))
        {
            DrawBombs();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            FlagPiece();
            Generator.gen.CheckGame();
        }
    }
}
