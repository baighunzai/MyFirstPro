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
        public void Dispose() { inUse = false; }
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
    PoolObject[] poolObjects;
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
        for(int i = 0; i<poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one;
        }
        configure();
    }
    void Update()
    {
        if (game.GameOver) return;

        Shift();
        spwanTimer += Time.deltaTime;
        if (spwanTimer > spwanRate)
        {
            Spawn();
            spwanTimer = 0;
        }
    }
    void configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[PoolSize];
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            go.SetActive(true);
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one;
            poolObjects[i] = new PoolObject(t);
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
        Transform t = GetPoolObject();
        if (t == null) return;
        Vector3 pos = Vector3.zero;
        pos.x = immediateSpawanPos.x;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        Spawn();
    }
    void Shift()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.position += -Vector3.right * shiftspeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }

    }
    void CheckDisposeObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x < -defaultSpawnPos.x)
            poolObject.Dispose();
        poolObject.transform.position = Vector3.one;

    }
    Transform GetPoolObject()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
