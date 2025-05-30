using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] protected float rotationSpeed = 90f;
    [SerializeField] private float rotationAcceleration = 4f;
    [SerializeField] private float rotationDeceleration = 8f;
    [SerializeField] private Color selectedColor = new Color(1f, 1f, 0f, 0.5f);
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 1f);
    
    protected static SelectableObject currentlySelected;
    protected SpriteRenderer spriteRenderer;
    private float currentRotationVelocity = 0f;
    
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisuals();
    }
    
    protected virtual void Update()
    {
        if (IsSelected())
        {
            // Get input direction (-1 for left/A, 1 for right/D)
            float inputDirection = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                inputDirection = 1f;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                inputDirection = -1f;
            }
            
            // Apply acceleration or deceleration
            if (inputDirection != 0)
            {
                // Accelerate towards target velocity
                float targetVelocity = inputDirection * rotationSpeed;
                currentRotationVelocity = Mathf.MoveTowards(
                    currentRotationVelocity,
                    targetVelocity,
                    rotationAcceleration * rotationSpeed * Time.deltaTime
                );
            }
            else
            {
                // Decelerate towards zero
                currentRotationVelocity = Mathf.MoveTowards(
                    currentRotationVelocity,
                    0f,
                    rotationDeceleration * rotationSpeed * Time.deltaTime
                );
            }
            
            // Apply rotation
            transform.Rotate(Vector3.forward * (currentRotationVelocity * Time.deltaTime));
        }
        else
        {
            // Reset velocity when not selected
            currentRotationVelocity = 0f;
        }
    }
    
    private void OnMouseDown()
    {
        Select();
    }
    
    public virtual void Select()
    {
        if (currentlySelected != null)
        {
            currentlySelected.Deselect();
        }
        currentlySelected = this;
        UpdateVisuals();
    }
    
    public virtual void Deselect()
    {
        if (IsSelected())
        {
            currentlySelected = null;
        }
        UpdateVisuals();
    }
    
    protected bool IsSelected()
    {
        return currentlySelected == this;
    }
    
    protected virtual void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = IsSelected() ? selectedColor : normalColor;
        }
    }
} 