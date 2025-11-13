using UnityEngine;

public class Generator : MonoBehaviour
{
    //Declaración de variables:
    [SerializeField] private GameObject piece;
    [SerializeField] public int width, height, bombsNumber;
    [SerializeField] public GameObject[][] map;

    public bool dead = false;


    public static Generator gen;

    public void SetWidth(int width)
    {
        this.width = width;
    }
    public void SetHeight(int height)
    {
        this.height = height;
    }
    public void SetBombsNumber(int bombsNumber)
    {
        this.bombsNumber = bombsNumber;
    }

    private void Awake()
    {
        gen = this;
    }

    public void Generate(bool bot, bool playing)
    {
        map = new GameObject[width][];
        for (int i = 0; i < width; i++)
        {
            map[i] = new GameObject[height];
        }

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                map[i][j] = Instantiate(piece, new Vector3(i, j, 0), Quaternion.identity);
                map[i][j].GetComponent<Piece>().setX(i);
                map[i][j].GetComponent<Piece>().setY(j);
            }
        }

        Camera.main.transform.position = new Vector3((float)width / 2f - .5f, (float)height / 2f - .5f, -10);

        for (int i = 0; i < bombsNumber; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (!map[x][y].GetComponent<Piece>().IsBomb())
                map[x][y].GetComponent<Piece>().setBomb(true);
            else
                i--;
        }
        for (int j = 0; j < height; j++)
            for (int i = 0; i < width; i++)
                GetBombsAround(i, j);

        if (bot)
        {
            AIController.bot.width = width;
            AIController.bot.height = height;
            AIController.bot.map = map;
            AIController.bot.StartBot(playing);
        }

    }

    public int GetBombsAround(int x, int y)
    {
        int cont = 0;

        // Arriba-izquierda
        if (x > 0 && y > 0 && map[x - 1][y - 1].GetComponent<Piece>().IsBomb())
            cont++;

        // Arriba
        if (y > 0 && map[x][y - 1].GetComponent<Piece>().IsBomb())
            cont++;

        // Arriba-derecha
        if (x < width - 1 && y > 0 && map[x + 1][y - 1].GetComponent<Piece>().IsBomb())
            cont++;

        // Izquierda
        if (x > 0 && map[x - 1][y].GetComponent<Piece>().IsBomb())
            cont++;

        // Derecha
        if (x < width - 1 && map[x + 1][y].GetComponent<Piece>().IsBomb())
            cont++;

        // Abajo-izquierda
        if (x > 0 && y < height - 1 && map[x - 1][y + 1].GetComponent<Piece>().IsBomb())
            cont++;

        // Abajo
        if (y < height - 1 && map[x][y + 1].GetComponent<Piece>().IsBomb())
            cont++;

        // Abajo-derecha
        if (x < width - 1 && y < height - 1 && map[x + 1][y + 1].GetComponent<Piece>().IsBomb())
            cont++;


        return cont;
    }

    public void CheckPieceAround(int x, int y)
    {
        // Arriba-izquierda
        if (x > 0 && y > 0)
        {
            map[x - 1][y - 1].GetComponent<Piece>().DrawBombs(true);
        }

        // Arriba
        if (y > 0)
        {
            map[x][y - 1].GetComponent<Piece>().DrawBombs(true);
        }

        // Arriba-derecha
        if (x < width - 1 && y > 0)
        {
            map[x + 1][y - 1].GetComponent<Piece>().DrawBombs(true);
        }

        // Izquierda
        if (x > 0)
        {
            map[x - 1][y].GetComponent<Piece>().DrawBombs(true);
        }

        // Derecha
        if (x < width - 1)
        {
            map[x + 1][y].GetComponent<Piece>().DrawBombs(true);
        }

        // Abajo-izquierda
        if (x > 0 && y < height - 1)
        {
            map[x - 1][y + 1].GetComponent<Piece>().DrawBombs(true);
        }

        // Abajo
        if (y < height - 1)
        {
            map[x][y + 1].GetComponent<Piece>().DrawBombs(true);
        }

        // Abajo-derecha
        if (x < width - 1 && y < height - 1)
        {
            map[x + 1][y + 1].GetComponent<Piece>().DrawBombs(true);
        }
    }

    public void Die()
    {
        dead = true;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Piece piece = map[i][j].GetComponent<Piece>();
                if (piece.IsBomb()) map[i][j].GetComponent<Piece>().DrawBombs(true);
            }
        }
    }
    
    public void CheckGame()
    {
        if (dead) return;

        bool winning = true;

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Piece piece = map[i][j].GetComponent<Piece>();

                if (piece.IsBomb() && !piece.IsFlagged())
                {
                    winning = false;
                }
                
                if (!piece.IsBomb() && piece.IsFlagged())
                {
                    winning = false;
                }

            }
        }

        if (winning) GameManager.instance.WinGame();
    }

}
