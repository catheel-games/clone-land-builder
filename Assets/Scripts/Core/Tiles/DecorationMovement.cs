using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class DecorationMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 8f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float rotationOffset = 180f;
    [SerializeField] private float wanderRadius = 3f;

    public static List<DecorationMovement> AllDecorations = new List<DecorationMovement>();
    public int GroupIndex {get; set; }
    public int PrefabIndex {get; set; }
    public Hexagons.Coords SpawnCoord {get; set; }

    public Hexagons.Type TileType { get; set; }
    public TileGrid Grid { get; set; }

    void OnEnable() => AllDecorations.Add(this);
    void OnDisable() => AllDecorations.Remove(this);

    private float height;
    private Vector3 lastPosition;

    public void Initialize(float height)
    {
        this.height = height;
        lastPosition = transform.position;
        Movement();
    }

    private void Update()
    {
        Vector3 direction = (transform.position - lastPosition).normalized;
        if (direction.magnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, rotationOffset, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        lastPosition = transform.position;
    }

    private void Movement()
    {
        Vector3 target = Vector3.zero;
        int attempts = 25;

        while (attempts > 0)
        {
            Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
            Vector3 candidate = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            if (IsValidPosition(candidate))
            {
                candidate.y = height;
                target = candidate;
                break;
            }
            attempts--;
        }

        if (target != Vector3.zero)
        {
            transform.DOMove(target, moveDuration).OnComplete(() => Movement());
        }
    }

    private bool IsValidPosition(Vector3 point)
    {
        RaycastHit hit;
        if (Physics.Raycast(point + Vector3.up * 3f, Vector3.down, out hit, 10f))
        {
            Debug.DrawLine(Vector3.zero, hit.point, Color.blue, 10000f);
            return Mathf.Abs(hit.point.y - height - 0.1f) < 0.03f;
        }
        return false;
    }
}
