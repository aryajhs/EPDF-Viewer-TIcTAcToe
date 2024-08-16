using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button ticTacToeButton;
    public Button pdfReaderButton;

    void Start()
    {
        ticTacToeButton.onClick.AddListener(() => LoadScene("TicTacToeScene"));
        pdfReaderButton.onClick.AddListener(() => LoadScene("PDFReaderScene"));
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}