using UnityEngine;
using System.Collections.Generic;

public class LaserController : SelectableObject
{
    [Header("Laser Settings")]
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask reflectiveLayers;
    [SerializeField] private int maxReflections = 10;
    [SerializeField] private float minPointDistance = 0.01f;
    
    private LayerMask raycastMask;
    
    [Header("References")]
    [SerializeField] private LineRenderer laserLine;
    
    private readonly List<Vector2> cleanedPoints = new List<Vector2>();
    
    protected override void Start()
    {
        base.Start();
        
        if (laserLine == null)
        {
            laserLine = gameObject.GetComponent<LineRenderer>();
        }
        
        laserLine.startWidth = 0.1f;
        laserLine.endWidth = 0.1f;
        laserLine.useWorldSpace = true;
        
        // Combine the reflective layers with the Wall layer
        raycastMask = reflectiveLayers | (1 << LayerMask.NameToLayer("Wall"));
        
        // Auto-select the laser at start
        Select();
    }
    
    protected override void Update()
    {
        base.Update();
        UpdateLaserBeam();
    }
    
    private bool IsPointTooClose(Vector2 newPoint)
    {
        if (cleanedPoints.Count == 0) return false;
        
        Vector2 lastPoint = cleanedPoints[cleanedPoints.Count - 1];
        return Vector2.Distance(newPoint, lastPoint) < minPointDistance;
    }
    
    private void AddPointToLaser(Vector2 point)
    {
        if (!IsPointTooClose(point))
        {
            cleanedPoints.Add(point);
        }
    }
    
    private void UpdateLaserBeam()
    {
        cleanedPoints.Clear();
        Vector2 position = transform.position;
        Vector2 direction = transform.right;
        
        AddPointToLaser(position);
        
        int reflectionCount = 0;
        bool hitTarget = false;
        
        while (reflectionCount < maxReflections && !hitTarget)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, direction, maxDistance, raycastMask);
            
            if (hit.collider != null)
            {
                AddPointToLaser(hit.point);
                
                // Check if we hit a wall
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    break; // Stop the laser at the wall
                }
                
                Mirror mirror = hit.collider.GetComponent<Mirror>();
                if (mirror != null)
                {
                    direction = Vector2.Reflect(direction, hit.normal).normalized;
                    position = hit.point + (direction * minPointDistance);
                    reflectionCount++;
                }
                else if (hit.collider.CompareTag("Target"))
                {
                    Target target = hit.collider.GetComponent<Target>();
                    if (target != null)
                    {
                        target.OnLaserHit();
                        hitTarget = true;
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                AddPointToLaser(position + (direction * maxDistance));
                break;
            }
        }
        
        laserLine.positionCount = cleanedPoints.Count;
        for (int i = 0; i < cleanedPoints.Count; i++)
        {
            laserLine.SetPosition(i, cleanedPoints[i]);
        }
    }
} 