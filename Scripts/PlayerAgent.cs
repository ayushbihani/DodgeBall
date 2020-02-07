using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;
using Barracuda;

public class PlayerAgent : Agent
{
    public float speed = 6f;
    Renderer renderer;
    public float maxTime = 100f;
    private float targetTime;
    public Text text;
    public float score = 0f;
    public GameObject spawn;
    private Spawner script;
    private float screenWidth, screenHeight;
    private float[] angles;
    float height;
    public float angle = 12f;
    private RaycastHit hit;
    private float rayDistance;
    Ray upRay;

    private void Start()
    {
        targetTime = Time.time + maxTime; 
        script = (Spawner) spawn.GetComponent<Spawner>();
        renderer = gameObject.GetComponent<Renderer>();
        screenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        height = renderer.bounds.size.y;
        upRay = new Ray(transform.position  , Vector3.up);
        InvokeRepeating("CalculateScore",0.0f, 1.0f);
    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
    }

    public override void CollectObservations()
    {
        AddVectorObs(rayCast(upRay, hit));
    }

    public override void AgentAction(float[] action) 
    {
        MoveAgent(action);
        //Give reward for every timeframe survived
        if(Time.time > targetTime)
        {
            AddReward(1f);
            Done();
        }
    }

    public void CalculateScore()
    {
        score += 2;
        text.text = "Score: " + score;
    }

    public void MoveAgent(float[] action)
    {
        AddReward(0.001f);
        //Scaling up the score for UI. Its not the same as rewards. 
        //I am going to give 2 points for every second survived. 
       int act = Mathf.FloorToInt(action[0]);
       switch (act)
       {
            case 0:
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(-Vector2.right * speed * Time.deltaTime);
                break;
       }
    }

    public override void AgentReset()
    {
        speed = 6f;
        angle = 15f;
        score = 0f;
        text.text = "Score: ";
        targetTime = maxTime + Time.time;
        transform.position = new Vector3(0, -Camera.main.orthographicSize/2, 0);
        script.Reset();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "ball")
        {
            script.Stop();
            AddReward(-1f);
            script.DestroyAllSpawnedBlocks();
            Done();
        }
    }

    private void FixedUpdate() {

    float width = renderer.bounds.size.x; 
        if(transform.position.x < -screenWidth){
           transform.position = new Vector2(-screenWidth + width, transform.position.y);
       }

       if(transform.position.x > screenWidth){
           transform.position = new Vector2(screenWidth-width, transform.position.y);
       }
    }

    private float rayCast(Ray ray, RaycastHit hit)
    {
        float distance = 0f;
        if(Physics.Raycast(ray, out hit, Camera.main.orthographicSize)){
            if(hit.collider.tag == "block")
            {
                distance = Vector3.Distance(hit.point, transform.position);
                //Debug.Log("Hit distance = " + Vector3.Distance(hit.point, transform.position));
            }
        }
        return distance;
    }

    public override float[] Heuristic()
    {
         if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("D");
            return new float[] {0};
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] {1};
        }
        return new float[] {-1};
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time); // Wait for time
        Done();
    }

}
