using System;
using Unity.Collections;
using UnityEngine;
using DG.Tweening;

public class FloatingDecorations : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private GameObject[] floatingDecorations;
    [SerializeField] private int airplaneSpeed;
    [SerializeField] private float cloudSpeed = 0.5f;
    private int spawnInterval;
    private int moveDuration;
    private Vector3 airplaneTarget;
    private Vector3 cloudTarget;
    private GameObject airplane;
    private GameObject cloud;
    private bool isSpawned;

    void Start()
    {
        SpawnAirplane();
        SpawnCloud();
    }

    void Update()
    {
        MoveAirplane();
    }

    private void SpawnAirplane()
    {
        spawnInterval = UnityEngine.Random.Range(30, 60);
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        airplaneTarget = new Vector3(-UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        airplane = Instantiate(floatingDecorations[0], spawnPosition, Quaternion.identity);
    }

    private void MoveAirplane()
    {
        float step = airplaneSpeed * Time.deltaTime;
        airplane.transform.position = Vector3.MoveTowards(airplane.transform.position, airplaneTarget, step);

        if (airplane.transform.position == airplaneTarget)
        {
            airplane.SetActive(false);
            Destroy(airplane);
            SpawnAirplane();
        }
    }

    private void SpawnCloud()
    {
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        cloudTarget = new Vector3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        cloud = Instantiate(floatingDecorations[1], spawnPosition, Quaternion.identity);
        cloud.transform.localScale = Vector3.zero;

        float moveDur = Vector3.Distance(spawnPosition, cloudTarget) / cloudSpeed;

        Sequence seq = DOTween.Sequence();
        seq.Append(cloud.transform.DOScale(1, 1f).SetEase(Ease.OutQuad));
        seq.Append(cloud.transform.DOMove(cloudTarget, moveDur).SetEase(Ease.Linear));
        seq.Append(cloud.transform.DOScale(0, 1f).SetEase(Ease.InQuad));
        seq.AppendInterval(60f);
        seq.OnComplete(() => { Destroy(cloud); SpawnCloud(); });
    }
}
