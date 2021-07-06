using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System;
/// <summary>
/// This script allows the pink ghost to move according to its AI.
/// </summary>
public class DeplacementphantomeP : MonoBehaviour
{
    /// <summary>
    ///  variable speed
    /// </summary>
    private float speed;
    private float speedV;
    /// <summary>
    /// movement variables
    /// </summary>
    private int inputX;
    private int inputY;
    /// <summary>
    /// destination variables
    /// </summary>
    private Vector2 buffer;
    /// <summary>
    /// variables of whether the ghost is moving
    /// </summary>
    private bool move;
    /// <summary>
    /// the images
    /// </summary>
    public Sprite hunt;
    public Sprite alive;
    public Sprite dead;
    /// <summary>
    /// the objects
    /// </summary>
    private phantomes phantomePM;
    public Transform phantomeP;
    public GameObject pacman;
    public Rigidbody2D pacmanR;
    /// <summary>
    ///  the tileMatrix
    /// </summary>
    private TileMatrix M;
    /// <summary>
    ///  variables for ressurection.
    /// </summary>
    private float xSup;
    private float xInf;
    private float ySup;
    private float yInf;


    void Start()
    {
        /// <summary>
        /// recovery and initialization of the different variables
        /// </summary>
        M = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().M;
        phantomeP = GetComponent<Transform>();
        pacman = GameObject.Find("pacman(Clone)");
        pacmanR = pacman.GetComponent<Rigidbody2D>();
        phantomePM = new phantomes(phantomeP.position.x, phantomeP.position.y, "right");
        buffer = phantomeP.position;
        /// <summary>
        /// recovery of the resurrection position.
        /// </summary>
        xSup = GetComponent<Transform>().position.x + 0.05f;
        xInf = GetComponent<Transform>().position.x - 0.05f;
        ySup = GetComponent<Transform>().position.y + 0.05f;
        yInf = GetComponent<Transform>().position.y - 0.05f;
        speedV = PlayerPrefs.GetInt("speedF");
        if (speedV == 0)
        {
            speedV = 3;
        }
        speed = speedV;

    }



    // Update is called once per frame
    void Update()
    {

        /// <summary>
        /// Test to know if the ghost can become alive again after being eaten (it must be in the centre)
        /// </summary>
        if (phantomeP.position.x > xInf && phantomeP.position.x < xSup && phantomeP.position.y < ySup && phantomeP.position.y > yInf && GetComponent<SpriteRenderer>().sprite == dead)
        {
            GetComponent<SpriteRenderer>().sprite = alive;
            speed = speedV;
        }
        /// <summary>
        /// depending on the direction of the ghost see if it has reached its destination if so then it moves more
        /// </summary>
        if (inputX == 1)
        {
            if (phantomeP.position.x >= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == 1)
        {
            if (phantomeP.position.y >= buffer.y)
            {
                move = false;
            }
        }

        if (inputX == -1)
        {
            if (phantomeP.position.x <= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == -1)
        {
            if (phantomeP.position.y <= buffer.y)
            {
                move = false;
            }
        }
        /// <summary>
        /// if the ghost doesn't move anymore then look for where it has to go
        /// </summary>
        if (move == false)
        {
            /// <summary>
            /// it moves
            /// </summary>
            move = true;
            /// <summary>
            /// places it precisely
            /// </summary>
            phantomeP.position = buffer;
            /// <summary>
            /// search according to its state (alive, hunting, dead)
            /// </summary>
            if (GetComponent<SpriteRenderer>().sprite == hunt)
            {
                phantomePM.findR(phantomeP, new Vector2(M.width * 0.24f, 0), M);
            }
            else
            {
                if (phantomeP.GetComponent<SpriteRenderer>().sprite == dead)
                {
                    phantomePM.findR(phantomeP, new Vector2(xSup - 0.05f, ySup - 0.05f), M);
                    speed = 8;
                }
                else
                {
                    /// <summary>
                    ///  la cible ce situe deux case devant pacman.
                    /// </summary>
                    if (PlayerPrefs.GetString("dirP") == "right")
                    {
                        phantomePM.findR(phantomeP, new Vector2(pacmanR.position.x + 0.48f, pacmanR.position.y), M);
                    }
                    if (PlayerPrefs.GetString("dirP") == "up")
                    {
                        phantomePM.findR(phantomeP, new Vector2(pacmanR.position.x, pacmanR.position.y + 0.48f), M);
                    }
                    if (PlayerPrefs.GetString("dirP") == "down")
                    {
                        phantomePM.findR(phantomeP, new Vector2(pacmanR.position.x, pacmanR.position.y - 0.48f), M);
                    }
                    if (PlayerPrefs.GetString("dirP") == "left")
                    {
                        phantomePM.findR(phantomeP, new Vector2(pacmanR.position.x - 0.48f, pacmanR.position.y), M);
                    }

                }
            }
            /// <summary>
            ///  applies the direction.
            /// </summary>
            if (phantomePM.getDirection() == "up")
            {
                inputX = 0;
                inputY = 1;
                buffer = buffer + new Vector2(0f, 0.24f);
                // animate();


            }
            if (phantomePM.getDirection() == "down")
            {
                inputX = 0;
                inputY = -1;
                buffer = buffer + new Vector2(0f, -0.24f);
                // animate();
            }
            if (phantomePM.getDirection() == "right")
            {
                inputX = 1;
                inputY = 0;
                buffer = buffer + new Vector2(0.24f, 0f);
                // animate();
            }
            if (phantomePM.getDirection() == "left")
            {
                inputX = -1;
                inputY = 0;
                buffer = buffer + new Vector2(-0.24f, 0f);
                // animate();
            }
            /// <summary>
            /// allows teleportation on the x-axis
            /// </summary>
            if (transform.position.x > (M.width - 1) * 0.24f)
            {
                transform.position = new Vector2(0, transform.position.y);
                buffer = transform.position;
            }
            if (transform.position.x < 0)
            {
                transform.position = new Vector2((M.width - 1) * 0.24f, transform.position.y);
                buffer = transform.position;

            }
            /// <summary>
            /// allows teleportation on the y-axis
            /// </summary>
            if (transform.position.y < -(M.height - 1) * 0.24f)
            {
                transform.position = new Vector2(transform.position.x, 0);
                buffer = transform.position;

            }
            if (transform.position.y > 0)
            {
                transform.position = new Vector2(transform.position.x, -(M.height - 1) * 0.24f);
                buffer = transform.position;

            }

        }


    }

    void FixedUpdate()
    {
        /// <summary>
        ///  movement.
        /// </summary>

        transform.position = Vector3.MoveTowards(transform.position, buffer, Time.deltaTime * speed * 0.24f);
    }
    /// <summary>
    ///  Animation
    /// </summary>
    void animate()
    {
        GetComponent<Animator>().SetFloat("DirX", inputX);
        GetComponent<Animator>().SetFloat("DirY", inputY);
    }

}
