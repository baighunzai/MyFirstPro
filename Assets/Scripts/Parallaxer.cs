using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class Parallaxer : MonoBehaviour
{
    class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t) { transform = t; }
        public void Use() { inUse = false; }
    }
    [System.Serializable]
    public struct YSpawnRange
    {
        public float min;
        public float max;
    }
    public GameObject Prefab;
    public int PoolSize;
    public float shiftspeed;
    public float spwanRate;
    public YSpawnRange ySpawnRange;

    public Vector3 defaultSpawnPos;
    public bool spawnImmedite;
    public Vector3 immediateSpawanPos;
    public Vector2 targetAspectRatio;
    float spwanTimer;
    float targetAspect;
    PoolObject[] PoolObjects;
    GameManager game;
    void Awake()
    {
        configure();
    }
    void Start()
    {
        game = GameManager.Instance;
    }
    void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameOverConfirmed()
    {
        configure();
    }
    void Update()
    {

    }
    void configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        PoolObjects = new PoolObject[PoolSize];
        for (int i = 0; i < PoolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            PoolObjects[i] = new PoolObject(t);
        }
        if (spawnImmedite)
        {
            SpawnImmediate();
        }
    }
    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) return;
        Vector3 pos = Vector3.zero;
        pos.x = defaultSpawnPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
    }
    void SpawnImmediate()
    {

    }
    void Shift()
    {
        for (int i = 0; i < PoolObject.Length; i++)
        {
            PoolObjects[i].transform.position += -Vector3.right * shiftspeed * Time.deltaTime;
            CheckDisposeObject(PoolObjects[i]);
        }

    }
    void CheckDisposeObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x < -defaultSpawnPos.x)
            poolObject.Dispose();
        poolObject.transform.position = Vector3.one * 1000;

    }
    Transform GetPoolObject()
    {
        for (int i = 0; i < PoolObjects.Length; i++)
        {
            if (!PoolObjects[i].inUse)
            {
                PoolObjects[i].Use();
                {
                    return PoolObjects[i];
                }
            }
        }
    }
}
