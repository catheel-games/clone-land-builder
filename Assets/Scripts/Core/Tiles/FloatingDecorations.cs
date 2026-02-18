using System;
using Unity.Collections;
using UnityEngine;
using DG.Tweening;

public class FloatingDecorations : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private GameObject[] floatingDecorations;
    [SerializeField] private float airplaneSpeed = 2f;
    [SerializeField] private float cloudSpeed = 0.05f;
    private Vector3 cloudTarget;
    private GameObject airplane;
    private GameObject cloud;
    private bool cloudStarted;

    void Start()
    {
        SpawnAirplane();
        tileGrid.OnTilePlace += OnTilePlace;
    }

    private void SpawnAirplane()
    {
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 direction = Quaternion.Euler(0f, UnityEngine.Random.Range(-30f, 30f), 0f) * camForward;
        Vector3 spawnPosition = Camera.main.transform.position - direction * 20f;
        spawnPosition.y = 2.5f;

        airplane = Instantiate(floatingDecorations[0], spawnPosition, Quaternion.LookRotation(direction));
        airplane.GetComponentInChildren<AirplaneVisibility>().OnDisappear += OnAirplaneDisappear;
    }

    private void OnAirplaneDisappear()
    {
        GameObject old = airplane;
        airplane = null;
        Destroy(old);
        SpawnAirplane();
    }

    void Update()
    {
        if (airplane != null)
            airplane.transform.Translate(Vector3.forward * airplaneSpeed * Time.deltaTime);
    }

    private void SpawnCloud()
    {
        Vector3 tilePos = tileGrid.GetRandomTilePosition();
        Vector3 spawnPosition = new Vector3(tilePos.x, 2.5f, tilePos.z);

        float angle = UnityEngine.Random.Range(0f, 360f);
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
        float distance = cloudSpeed * 25f;
        cloudTarget = spawnPosition + direction * distance;

        cloud = Instantiate(floatingDecorations[1], spawnPosition, Quaternion.identity);
        cloud.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(cloud.transform.DOScale(1, 1f).SetEase(Ease.OutQuad));
        seq.Append(cloud.transform.DOMove(cloudTarget, 25f).SetEase(Ease.Linear));
        seq.Append(cloud.transform.DOScale(0, 1f).SetEase(Ease.InQuad));
        seq.AppendInterval(60f);
        seq.OnComplete(() => { Destroy(cloud); SpawnCloud(); });
    }

    private void OnTilePlace()
    {
        if (!cloudStarted)
        {
            cloudStarted = true;
            DOVirtual.DelayedCall(UnityEngine.Random.Range(5f, 10f), SpawnCloud);
        }
    }
}
