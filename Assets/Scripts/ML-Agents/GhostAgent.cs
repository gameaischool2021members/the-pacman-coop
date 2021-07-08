using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;


public class GhostAgent : Agent
{
    private Pacman API = null;
    private TileMatrix M = null;

    public string CollidedActionAgent { private get; set; } = "";

    private List<Vector2> initialPositionGhosts = null;

    private Vector2 initialPositionPacman;

    private float rewardWinning = 500.0f;
    private float penaltyGhost = -200.0f;
    private float penaltyDefault = -10.0f;

    [SerializeField]
    private Rigidbody2D ghostBody = null;

    void Awake()
    {

        if (API == null)
        {
            API = GameObject.Find("ML-Agents").GetComponent<Pacman>();
        }

        initialPositionGhosts = new List<Vector2>();
        List<GameObject> ghosts = API.GetGhosts();
        for (int i = 0; i < ghosts.Count; i++)
        {
            initialPositionGhosts.Add(ghosts[i].transform.localPosition);
        }

    }

    public override void OnEpisodeBegin()
    {
        if (API == null)
        {
            API = GameObject.Find("ML-Agents").GetComponent<Pacman>();
        }

        //update pacman pos
        this.transform.localPosition = initialPositionPacman;
        //update ghosts pos
        List<GameObject> ghosts = API.GetGhosts();

        //for (int i = 0; i < ghosts.Count; i++)
        //{
        //    ghosts[i].transform.localPosition = initialPositionGhosts[i];
        //}

        //restore pellets
        List<GameObject> pellets = API.GetPellets();

        for (int i = 0; i < pellets.Count; i++)
        {
            pellets[i].SetActive(true);
        }

        //restore large pellets
        List<GameObject> largePellets = API.GetLargePellets();

        for (int i = 0; i < largePellets.Count; i++)
        {
            largePellets[i].SetActive(true);
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float speed = 4.0f;

        int directionX = 0;
        int directionY = 0;

        //Get the action index for movement
        int movement = actions.DiscreteActions[0];
        // Look up the index in the movement action list:
        if (movement == 1) { directionY = 1; }
        else if (movement == 2) { directionY = -1; }
        else if (movement == 3) { directionX = -1; }
        else if (movement == 4) { directionX = 1; }

 
        // Apply the action results to move the Agent
        ghostBody.velocity = new Vector2((speed * directionX) * 0.24f, (speed * directionY) * 0.24f);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        if (API == null)
        {
            API = GameObject.Find("ML-Agents").GetComponent<Pacman>();
        }

        AddObservations(sensor, API.GetPacmen());
        AddObservations(sensor, API.GetGhosts());

        sensor.AddObservation(this.transform.localPosition);

    }

    void AddObservations(VectorSensor sensor, List<GameObject> lst)
    {
        if (lst != null)
        {
            foreach (GameObject p in lst)
                sensor.AddObservation(p.transform.localPosition);
        }
  

        if (CollidedActionAgent == "ghostDead")
        {
            SetReward(penaltyGhost);
            CollidedActionAgent = "";
        }
        else if (CollidedActionAgent == "ghostAlive")
        {
            SetReward(rewardWinning);
            CollidedActionAgent = "";
            EndEpisode();
        }
        else
        {
            SetReward(penaltyDefault); //state of game is changing
        }

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        

    }
}