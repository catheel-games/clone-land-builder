using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class DecorationMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 8f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float rotationOffset = 180f;
    [SerializeField] private float wanderRadius = 0.8f;
    [SerializeField] private float centerRadius = 0.35f;

    public static List<DecorationMovement> AllDecorations = new List<DecorationMovement>();
    public int GroupIndex { get; set; }
    public int PrefabIndex { get; set; }
    public Hexagons.Coords SpawnCoord { get; set; }

    public Hexagons.Type TileType { get; set; }
    public TileGrid Grid { get; set; }

    void OnEnable() => AllDecorations.Add(this);
    void OnDisable() => AllDecorations.Remove(this);

    private float height;
    private Vector3 moveDirection;
    private Vector3 spawnPosition;

    public void Initialize(float height)
    {
        this.height = height;
        spawnPosition = transform.position;
        Movement();
    }

    private void Update()
    {
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection) * Quaternion.Euler(0, rotationOffset, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Movement()
    {
        Vector3 target = Vector3.zero;
        int attempts = 50;

        while (attempts > 0)
        {
            Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
            Vector3 candidate = spawnPosition + new Vector3(randomOffset.x, 0, randomOffset.y);

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
            Vector3 dir = transform.position - target;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
                moveDirection = dir.normalized;

            float distance = Vector3.Distance(transform.position, target);
            float duration = Mathf.Max(2f, moveDuration * (distance / wanderRadius));
            float pause = Random.Range(0.5f, 2f);

            transform.DOMove(target, duration)
                .SetDelay(pause)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => Movement());
        }
        else
        {
            Invoke(nameof(Movement), 2f);
        }
    }

    private bool IsValidPosition(Vector3 point)
    {
        Hexagons.Coords hex = Hexagons.WorldToHex(point);
        Tile tile = Grid.GetTile(hex);
        if (tile == null) return false;

        Vector3 hexCenter = Hexagons.HexToWorld(hex);
        Vector3 offset = point - hexCenter;
        offset.y = 0;

        if (offset.magnitude < centerRadius)
            return tile.CenterType == TileType;

        float angle = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        int side = Mathf.FloorToInt(angle / 60f) % 6;

        return tile.GetSide(side) == TileType;
    }
}
