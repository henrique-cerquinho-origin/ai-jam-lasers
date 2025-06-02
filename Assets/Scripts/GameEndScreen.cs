using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button restartButton;
    [SerializeField] private string firstLevelName = "Level1";
    
    private void Start()
    {
        // Make sure time is running normally
        Time.timeScale = 1f;
        
        // Add listener to restart button
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }
    
    private void RestartGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }
} 