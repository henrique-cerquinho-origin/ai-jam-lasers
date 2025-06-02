using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private Target[] targets;
    [SerializeField] private float nextLevelDelay = 2f;
    [SerializeField] private string nextLevelName;
    [SerializeField] private bool isFinalLevel;
    [SerializeField] private string endScreenSceneName = "EndScreen";
    
    private int completedTargets = 0;
    
    private void Start()
    {
        // If targets array is empty, try to find all targets in the scene
        if (targets == null || targets.Length == 0)
        {
            targets = FindObjectsOfType<Target>();
        }
        
        // Subscribe to each target's completion event
        foreach (Target target in targets)
        {
            target.onTargetComplete.AddListener(OnTargetComplete);
        }
    }
    
    private void OnTargetComplete()
    {
        completedTargets++;
        
        // Check if all targets are complete
        if (completedTargets >= targets.Length)
        {
            LevelComplete();
        }
    }
    
    private void LevelComplete()
    {
        Debug.Log("Level Complete!");
        
        if (isFinalLevel)
        {
            // Load the end screen scene
            SceneManager.LoadScene(endScreenSceneName);
        }
        else if (!string.IsNullOrEmpty(nextLevelName))
        {
            // Load next level after delay
            Invoke(nameof(LoadNextLevel), nextLevelDelay);
        }
    }
    
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
    
    // Optional: Method to restart the current level
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
} 