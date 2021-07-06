using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System;
/// <summary>
/// This script allows the yellow ghost to move according to its AI.
/// </summary>
public class DeplacementphantomeJ : MonoBehaviour
{
    /// <summary>
    /// variable speed
    /// </summary>
    private float speed;
    private float speedV;
    /// <summary>
    ///  movements variables
    /// </summary>
    private int inputX;
    private int inputY;
    /// <summary>
    ///  destination variables
    /// </summary>
    private Vector2 buffer;
    /// <summary>
    ///  variables to know if the pacman is moving
    /// </summary>
    private bool move;
    /// <summary>
    ///  the images
    /// </summary>
    public Sprite hunt;
    public Sprite alive;
    public Sprite dead;
    /// <summary>
    ///  the objets
    /// </summary>
    private phantomes phantomeJM;
    public Transform phantomeJ;
    public GameObject pacman;
    public Rigidbody2D pacmanR;

    /// <summary>
    ///  ressurection variables
    /// </summary>
    private float xSup;
    private float xInf;
    private float ySup;
    private float yInf;
    /// <summary>
    ///  the tileMatrix
    /// </summary>
    private TileMatrix M;


    void Start()
    {
        /// <summary>
        ///  recovery and initialization of the different variables
        /// </summary>
        phantomeJ = GetComponent<Transform>();
        M = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().M;
        phantomeJM = new phantomes(phantomeJ.position.x, phantomeJ.position.y, "right");
        pacman = GameObject.Find("pacman(Clone)");
        pacmanR = pacman.GetComponent<Rigidbody2D>();
        buffer = phantomeJ.position;
        /// <summary>
        /// recovery of the ressurection position.
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

        /// Test to know if the ghost can become alive again after being eaten (it must be in the centre)
        /// </summary>
        if (phantomeJ.position.x > xInf && phantomeJ.position.x < xSup && phantomeJ.position.y < ySup && phantomeJ.position.y > yInf && GetComponent<SpriteRenderer>().sprite == dead)
        {
            GetComponent<SpriteRenderer>().sprite = alive;

            speed = speedV;
        }
        /// <summary>
        /// depending on the direction of the ghost see if it has reached its destination if so then it moves more
        /// </summary>
        if (inputX == 1)
        {
            if (phantomeJ.position.x >= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == 1)
        {
            if (phantomeJ.position.y >= buffer.y)
            {
                move = false;
            }
        }

        if (inputX == -1)
        {
            if (phantomeJ.position.x <= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == -1)
        {
            if (phantomeJ.position.y <= buffer.y)
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
            ///  it moves
            /// </summary>
            move = true;
            /// <summary>
            ///  replaces it precisely
            /// </summary>
            phantomeJ.position = buffer;
            /// <summary>
            /// search according to its state (alive, hunting, dead)
            /// </summary>
            if (GetComponent<SpriteRenderer>().sprite == hunt || (Math.Sqrt(((pacmanR.position.x - phantomeJ.position.x) * (pacmanR.position.x - phantomeJ.position.x)) + ((pacmanR.position.y - phantomeJ.position.y) * (pacmanR.position.y - phantomeJ.position.y)))) < 1.92f && GetComponent<SpriteRenderer>().sprite != dead)
            {
                phantomeJM.findR(phantomeJ, new Vector2(0, -M.height * 0.24f), M);
            }
            else
            {
                if (GetComponent<SpriteRenderer>().sprite == dead)
                {
                    speed = 8;
                    phantomeJM.findR(phantomeJ, new Vector2(xSup - 0.05f, ySup - 0.05f), M);

                }
                else
                {
                    /// <summary>
                    /// the target is Pacman but he runs away from him if he is less than eight squares away.
                    /// </summary>
                    phantomeJM.findR(phantomeJ, pacmanR.position, M);

                }
            }

            /// <summary>
            ///  applies the direction
            /// </summary>
            if (phantomeJM.getDirection() == "up")
            {
                inputX = 0;
                inputY = 1;

                buffer = buffer + new Vector2(0f, 0.24f);
                // animate();

            }
            if (phantomeJM.getDirection() == "down")
            {
                inputX = 0;
                inputY = -1;
                buffer = buffer + new Vector2(0f, -0.24f);
                //  animate();
            }
            if (phantomeJM.getDirection() == "right")
            {
                inputX = 1;
                inputY = 0;
                buffer = buffer + new Vector2(0.24f, 0f);
                //  animate();
            }
            if (phantomeJM.getDirection() == "left")
            {
                inputX = -1;
                inputY = 0;
                buffer = buffer + new Vector2(-0.24f, 0f);
                //  animate();
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
            if (transform.position.y < -(M.height) * 0.24f)
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
