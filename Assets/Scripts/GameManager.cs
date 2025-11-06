using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputWidth;
    [SerializeField] private TMP_InputField inputHeight;
    [SerializeField] private TMP_InputField inputNBombs;
    [SerializeField] private Button btnBot;
    [SerializeField] private Button btnPlay;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelError;

    public int width, height, nBombs;

    public bool endgame = false;
    public bool bot = false;


    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        inputWidth.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputHeight.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputNBombs.contentType = TMP_InputField.ContentType.IntegerNumber;
        btnBot.onClick.AddListener(TaskOnClickBot);
        btnPlay.onClick.AddListener(TaskOnClick);
        panelMenu.SetActive(true);
        panelGameOver.SetActive(false);
        panelWin.SetActive(false);
        panelError.SetActive(false);
    }

    private int Validate()
    {
        int errorCode = 0;

        if (width <= 1) errorCode += 4;
        if (height <= 1) errorCode += 2;
        if (!(nBombs >= 0 && nBombs < width*height)) errorCode += 1;

        return errorCode;
    }

    void TaskOnClick()
    {
        string widthText = inputWidth.text;
        string heightText = inputHeight.text;
        string nBombsText = inputNBombs.text;

        bool widthOk = int.TryParse(widthText, out width);
        bool heightOk = int.TryParse(heightText, out height);
        bool nBombsOk = int.TryParse(nBombsText, out nBombs);

        if (widthOk && heightOk && nBombsOk)
        {
            StartGame(width, height, nBombs);
        }
        else
        {
            Debug.LogError("Uno o ambos campos no son números válidos.");
        }
    }

    void TaskOnClickBot()
    {
        bot = true;
        TaskOnClick();
    }

    private void StartGame(int width, int height, int nBombs)
    {
        Generator.gen.SetWidth(width);
        Generator.gen.SetHeight(height);
        Generator.gen.SetBombsNumber(nBombs);
        if(Validate() == 0)
        {
            Generator.gen.Generate(bot);
            panelMenu.SetActive(false);
        }
        else
        {
            panelError.SetActive(true);
            TextMeshProUGUI txtError = panelError.GetComponentInChildren<TextMeshProUGUI>();
            switch (Validate())
            {
                case 1: txtError.text = "- Número de bombas no permitido."; break;
                case 2: txtError.text = "- El alto debe ser mayor a 1."; break;
                case 3: txtError.text = "- El alto debe ser mayor a 1.\n- Número de bombas no permitido."; break;
                case 4: txtError.text = "- El ancho debe ser mayor a 1."; break;
                case 5: txtError.text = "- El ancho debe ser mayor a 1.\n- Número de bombas no permitido."; break;
                case 6: txtError.text = "- El ancho debe ser mayor a 1.\n- El alto debe ser mayor a 1."; break;
                case 7: txtError.text = "- El ancho debe ser mayor a 1.\n- El alto debe ser mayor a 1.\n- Número de bombas no permitido."; break;
            }
        }
    }
    
    public void LoseGame()
    {
        panelGameOver.SetActive(true);
        endgame = true;
    }
    public void WinGame()
    {
        panelWin.SetActive(true);
        endgame = true;
    }
}
