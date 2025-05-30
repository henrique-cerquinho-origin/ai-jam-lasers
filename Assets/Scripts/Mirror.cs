using UnityEngine;

public class Mirror : SelectableObject
{
    [Header("Mirror Settings")]
    [SerializeField] private bool isRotatable = true;
    
    public override void Select()
    {
        if (!isRotatable) return;
        base.Select();
    }
} 