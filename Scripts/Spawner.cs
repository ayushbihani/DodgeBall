using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Spawner : MonoBehaviour
{
    public GameObject blockPrefab;
    private Vector2 screenSize;
    float spawnFrameTime = 40f;
    float spawnTime = 0f;
    float Angle = 12f;
    float cameraSize;
    bool stop;

    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        cameraSize = Camera.main.orthographicSize;
        screenSize = new Vector2(Camera.main.aspect*cameraSize, cameraSize);
        InvokeRepeating("InstantiateMarginBlocks",1.0f, 2.0f);
    }


    public void DestroyAllSpawnedBlocks()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("ball"))
        {
            Destroy(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop)
        {
            if(Time.time > spawnTime){
            spawnTime = Time.time + Time.deltaTime *spawnFrameTime;   
            InstantiateNewBlock();
            spawnFrameTime -= 0.5f;
            if(spawnFrameTime < 20)
                spawnFrameTime = 20f;
        }
        }
    }

    public void InstantiateNewBlock()
    {
        float size = Random.Range(1,3);
        float spawnAngle = Random.Range(-Angle, Angle);
        Vector2 spawnPosition = new Vector2(Random.Range(-screenSize.x*0.5f , screenSize.x*0.5f),screenSize.y + size);
        GameObject fallingBlock = (GameObject)Instantiate(blockPrefab, spawnPosition, Quaternion.Euler(Vector3.forward * spawnAngle));
        fallingBlock.transform.localScale = Vector3.one * size;
    }

    public void InstantiateMarginBlocks()
    {
        float size = Random.Range(1,3);
        float spawnAngle = Random.Range(-Angle, Angle);
        Vector2 spawnPosition = new Vector2(-screenSize.x ,screenSize.y + size);
        Vector2 spawnPosition_ = new Vector2(screenSize.x ,screenSize.y + size);
        GameObject fallingBlock = (GameObject)Instantiate(blockPrefab, spawnPosition, Quaternion.Euler(Vector3.forward));
        GameObject fallingBlock_ = (GameObject)Instantiate(blockPrefab, spawnPosition_, Quaternion.Euler(Vector3.forward));
        fallingBlock.transform.localScale = Vector3.one * size;
        fallingBlock_.transform.localScale = Vector3.one * size;
    }

    public void Stop()
    {
        stop = true;
    }

    public void Reset()
    {
        stop = false;
        spawnFrameTime = 50f;
    }
}
