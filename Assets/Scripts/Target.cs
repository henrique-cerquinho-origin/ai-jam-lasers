using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private float requiredHitDuration = 1f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activatedColor = Color.green;
    
    [Header("Events")]
    public UnityEvent onTargetComplete;
    
    private SpriteRenderer spriteRenderer;
    private float currentHitTime;
    private bool isComplete;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
        
        currentHitTime = 0f;
        isComplete = false;
    }
    
    private void Update()
    {
        // Reset color if not being hit
        if (currentHitTime > 0)
        {
            currentHitTime -= Time.deltaTime;
            if (currentHitTime <= 0 && !isComplete)
            {
                spriteRenderer.color = normalColor;
            }
        }
    }
    
    public void OnLaserHit()
    {
        if (isComplete) return;
        
        currentHitTime = requiredHitDuration;
        spriteRenderer.color = activatedColor;
        
        // Check if target has been hit long enough
        if (currentHitTime >= requiredHitDuration)
        {
            CompleteTarget();
        }
    }
    
    private void CompleteTarget()
    {
        if (isComplete) return;
        
        isComplete = true;
        spriteRenderer.color = activatedColor;
        onTargetComplete?.Invoke();
    }
} 