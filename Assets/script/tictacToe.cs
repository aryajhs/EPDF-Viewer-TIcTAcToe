using UnityEngine;
using UnityEngine.UI;

public class TicTacToeGame : MonoBehaviour
{
    public Button[] gridButtons;
    public Text statusText;
    public Button restartButton;
    public Sprite xSprite;
    public Sprite oSprite;

    public AudioClip clickSound;
    public AudioClip winSound;
    public AudioClip drawSound;
    private AudioSource audioSource;

    private bool isPlayerTurn = true;
    private string[] board = new string[9];

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < gridButtons.Length; i++)
        {
            int index = i;
            gridButtons[i].onClick.AddListener(() => OnGridButtonClick(index));
        }
        restartButton.onClick.AddListener(RestartGame);
        RestartGame();
    }

     void OnGridButtonClick(int index)
    {
        if (string.IsNullOrEmpty(board[index]))
        {
            board[index] = isPlayerTurn ? "X" : "O";
            
            Image buttonImage = gridButtons[index].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = isPlayerTurn ? xSprite : oSprite;
                buttonImage.color = Color.white;
            }

            audioSource.PlayOneShot(clickSound);
            gridButtons[index].interactable = false;

            if (CheckWin(board[index]))
            {
                EndGame(isPlayerTurn ? "Player Wins!" : "AI Wins!", winSound);
            }
            else if (IsBoardFull())
            {
                EndGame("It's a Draw!", drawSound);
            }
            else
            {
                isPlayerTurn = !isPlayerTurn;
                if (!isPlayerTurn)
                {
                    AIMove();
                }
            }
        }
    }
    void AIMove()
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 9; i++)
        {
            if (string.IsNullOrEmpty(board[i]))
            {
                board[i] = "O";
                int score = Minimax(board, 0, false);
                board[i] = "";

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        if (bestMove != -1)
        {
            OnGridButtonClick(bestMove);
        }
    }

    int Minimax(string[] board, int depth, bool isMaximizing)
    {
        string result = CheckWinnerForAI();
        if (result != null)
        {
            return result == "O" ? 10 - depth : depth - 10;
        }

        if (IsBoardFull())
        {
            return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (string.IsNullOrEmpty(board[i]))
                {
                    board[i] = "O";
                    int score = Minimax(board, depth + 1, false);
                    board[i] = "";
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (string.IsNullOrEmpty(board[i]))
                {
                    board[i] = "X";
                    int score = Minimax(board, depth + 1, true);
                    board[i] = "";
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    string CheckWinnerForAI()
    {
        // Check rows, columns, and diagonals
        for (int i = 0; i < 3; i++)
        {
            if (board[i * 3] == board[i * 3 + 1] && board[i * 3 + 1] == board[i * 3 + 2] && !string.IsNullOrEmpty(board[i * 3]))
                return board[i * 3];
            if (board[i] == board[i + 3] && board[i + 3] == board[i + 6] && !string.IsNullOrEmpty(board[i]))
                return board[i];
        }
        if (board[0] == board[4] && board[4] == board[8] && !string.IsNullOrEmpty(board[0])) return board[0];
        if (board[2] == board[4] && board[4] == board[6] && !string.IsNullOrEmpty(board[2])) return board[2];

        return null;
    }

    bool CheckWin(string player)
    {
        // Check rows, columns, and diagonals
        for (int i = 0; i < 3; i++)
        {
            if (board[i * 3] == player && board[i * 3 + 1] == player && board[i * 3 + 2] == player) return true; // Row
            if (board[i] == player && board[i + 3] == player && board[i + 6] == player) return true; // Column
        }
        if (board[0] == player && board[4] == player && board[8] == player) return true; // Diagonal
        if (board[2] == player && board[4] == player && board[6] == player) return true; // Diagonal

        return false;
    }

    bool IsBoardFull()
    {
        return !System.Array.Exists(board, cell => string.IsNullOrEmpty(cell));
    }

     void EndGame(string message, AudioClip endSound)
    {
        statusText.text = message;
        audioSource.PlayOneShot(endSound);
        foreach (var button in gridButtons)
        {
            button.interactable = false;
        }
    }

    void RestartGame()
    {
        isPlayerTurn = true;
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = "";
            Image buttonImage = gridButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = null;
                buttonImage.color = Color.white;
            }
            gridButtons[i].interactable = true;
        }
        statusText.text = "Player's Turn";
    }
}