using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIController : MonoBehaviour
{
    public float turnTime = 2f;

    public int width, height;

    public GameObject[][] map;

    public static AIController bot;
    int ite = 1;



    public void Awake()
    {
        bot = this;
    }

    public void StartBot()
    {
        StartCoroutine(Play());
    }


    System.Collections.IEnumerator Play()
    {
        yield return new WaitForSeconds(1f);

        while (!GameManager.instance.endgame)
        {
            bool actionDone = LogicPlay();
            if (!actionDone)
            {
                RandomPlay();
            }

            ite++;
            yield return new WaitForSeconds(turnTime);
        }
    }


    // Lógica general del bot

    bool LogicPlay()
    {
        bool action = false;

        for(int i = 0; i < GetCheckedPieces().Count; i++)
        {
            Piece piece = GetCheckedPieces()[i];
            action = Rule1(piece) || Rule2(piece) || action;
        }

        return action;
    }

    private bool Rule1(Piece piece)
    {
        List<Piece> uncheckedAndFlaggedPieces = GetUncheckedAndFlaggedPiecesAround(piece.x, piece.y);
        List<Piece> uncheckedPieces = GetUncheckedPiecesAround(piece.x, piece.y);

        if (uncheckedAndFlaggedPieces.Count == 0 || uncheckedPieces.Count == 0) return false;

        if (Generator.gen.GetBombsAround(piece.x, piece.y) == uncheckedAndFlaggedPieces.Count)
        {
            for (int i = 0; i < uncheckedPieces.Count; i++)
            {
                uncheckedPieces[i].FlagPiece();
            }

            return true;
        }
        else
        {
            return false;
        }
    }
    
    private bool Rule2(Piece piece)
    {
        List<Piece> flaggedPieces = GetFlaggedPiecesAround(piece.x, piece.y);
        List<Piece> uncheckedPieces = GetUncheckedPiecesAround(piece.x, piece.y);

        if (flaggedPieces.Count == 0 || uncheckedPieces.Count == 0) return false;

        if (Generator.gen.GetBombsAround(piece.x, piece.y) == flaggedPieces.Count)
        {
            for (int i = 0; i < uncheckedPieces.Count; i++)
            {
                uncheckedPieces[i].DrawBombs();
            }

            return true;
        }
        else
        {
            return false;
        }
        
    }

    void RandomPlay()
    {
        bool played = false;
        while (!played)
        {
            int rWidth = Random.Range(0, width);
            int rHeight = Random.Range(0, height);
            Piece piece = map[rWidth][rHeight].GetComponent<Piece>();

            if (!piece.IsChecked() && !piece.IsFlagged())
            {
                piece.DrawBombs();
                played = true;
            }
        }
    }

    List<Piece> GetCheckedPieces()
    {
        List<Piece> result = new List<Piece>();

        for (int j = 0; j < height; j++)
            for (int i = 0; i < width; i++)
                if (map[i][j].GetComponent<Piece>().IsChecked())
                    result.Add(map[i][j].GetComponent<Piece>());

        return result;
    }

    public List<Piece> GetUncheckedAndFlaggedPiecesAround(int x, int y)
    {
        List<Piece> result = new List<Piece>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    Piece p = map[nx][ny].GetComponent<Piece>();
                    if (!p.IsChecked() || p.IsFlagged())
                        result.Add(p);
                }
            }
        }

        return result;
    }

    public List<Piece> GetFlaggedPiecesAround(int x, int y)
    {
        List<Piece> result = new List<Piece>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    Piece p = map[nx][ny].GetComponent<Piece>();
                    if (p.IsFlagged())
                        result.Add(p);
                }
            }
        }

        return result;
    }
    
    public List<Piece> GetUncheckedPiecesAround(int x, int y)
    {
        List<Piece> result = new List<Piece>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    Piece p = map[nx][ny].GetComponent<Piece>();
                    if (!p.IsChecked() && !p.IsFlagged())
                        result.Add(p);
                }
            }
        }

        return result;
    }
}

