using System;
using Unity.Collections;
using UnityEngine;

public class FloatingDecorations : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private GameObject[] floatingDecorations;
    [SerializeField] private int speed;
    private int spawnInterval;
    private Vector3 targetPosition;
    private GameObject airplane;

    void Start()
    {
        SpawnAirplane();
    }

    void Update()
    {
        MoveAirplane();
    }

    private void SpawnAirplane()
    {
        
        spawnInterval = UnityEngine.Random.Range(30, 60);
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        targetPosition = new Vector3(-UnityEngine.Random.Range(-10, 10), 1.5f, UnityEngine.Random.Range(-10, 10));
        airplane = Instantiate(floatingDecorations[0], spawnPosition, Quaternion.identity);
        
    }

    private void MoveAirplane()
    {
        float step = speed * Time.deltaTime;
        airplane.transform.position = Vector3.MoveTowards(airplane.transform.position, targetPosition, step);
    }
}
