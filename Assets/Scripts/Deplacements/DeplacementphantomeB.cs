using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System;
/// <summary>
///  This script allows the blue ghost to move according to its AI.
/// </summary>
public class DeplacementphantomeB : MonoBehaviour
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
    ///  destination variables
    /// </summary>
    private Vector2 buffer;
    /// <summary>
    ///  variables of whether the ghost is moving
    /// </summary>
    private bool move;
    /// <summary>
    ///  variables for the return to life
    /// </summary>
    private float xSup;
    private float xInf;
    private float ySup;
    private float yInf;

    /// <summary>
    ///  the objects
    /// </summary>
    public Transform phantomeR;
    private phantomes phantomeBM;
    public Transform phantomeB;
    public GameObject pacman;
    public Rigidbody2D pacmanR;
    /// <summary>
    ///  the images
    /// </summary>
    public Sprite hunting;
    public Sprite alive;
    public Sprite dead;
    /// <summary>
    ///  the  tileMatrix
    /// </summary>
    private TileMatrix M;


    void Start()
    {
        /// <summary>
        ///  recovery and initialization of the different variables
        /// </summary>
        phantomeR = GameObject.Find("phantomeR(Clone)").transform;
        phantomeB = GetComponent<Transform>();
        M = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().M;
        phantomeBM = new phantomes(phantomeB.position.x, phantomeB.position.y, "right");
        pacman = GameObject.Find("pacman(Clone)");
        pacmanR = pacman.GetComponent<Rigidbody2D>();
        buffer = phantomeB.position;
        /// <summary>
        ///  recovery of the resurection position.
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

    void Update()
    {

        /// <summary>
        ///  Test to see if the ghost can come back to life after being eaten (it must be in the centre)
        /// </summary>
        if (phantomeB.position.x > xInf && phantomeB.position.x < xSup && phantomeB.position.y < ySup && phantomeB.position.y > yInf && GetComponent<SpriteRenderer>().sprite == dead)
        {
            GetComponent<SpriteRenderer>().sprite = alive;
            speed = speedV;
        }
        /// <summary>
        /// depending on the direction of the ghost, see if it has arrived at its destination, if so, it will not move anymore
        /// </summary>
        if (inputX == 1)
        {
            if (phantomeB.position.x >= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == 1)
        {
            if (phantomeB.position.y >= buffer.y)
            {
                move = false;
            }
        }

        if (inputX == -1)
        {
            if (phantomeB.position.x <= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == -1)
        {
            if (phantomeB.position.y <= buffer.y)
            {
                move = false;
            }
        }
        /// <summary>
        ///  if the ghost doesn't move anymore then look for where it has to go
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
            phantomeB.position = buffer;
            /// <summary>
            ///  search according to its state (alive, hunting, dead)
            /// </summary>
            if (GetComponent<SpriteRenderer>().sprite == hunting)
            {
                phantomeBM.findR(phantomeB, new Vector2(M.width * 0.24f, -M.height * 0.24f), M);
            }
            else
            {

                if (GetComponent<SpriteRenderer>().sprite == dead)
                {
                    phantomeBM.findR(phantomeB, new Vector2(xSup - 0.05f, ySup - 0.05f), M);
                    speed = 8;
                    /// <summary>
                    ///  the target is located at the distance between the RED ghost and the pacman *2.
                    /// </summary>
                }
                else
                {
                    phantomeBM.findR(phantomeB, new Vector2((pacmanR.position.x - phantomeR.position.x) + pacmanR.position.x, (pacmanR.position.y - phantomeR.position.y) + pacmanR.position.y), M);

                }
            }

            /// <summary>
            ///  applies to direction
            /// </summary>

            if (phantomeBM.getDirection() == "up")
            {
                inputX = 0;
                inputY = 1;
                buffer = buffer + new Vector2(0f, 0.24f);
                animate();

            }
            if (phantomeBM.getDirection() == "down")
            {
                inputX = 0;
                inputY = -1;
                buffer = buffer + new Vector2(0f, -0.24f);
                animate();

            }
            if (phantomeBM.getDirection() == "right")
            {
                inputX = 1;
                inputY = 0;
                buffer = buffer + new Vector2(0.24f, 0f);
                animate();


            }
            if (phantomeBM.getDirection() == "left")
            {
                inputX = -1;
                inputY = 0;
                buffer = buffer + new Vector2(-0.24f, 0f);
                animate();

            }
            /// <summary>
            ///  allows teleportation on the x-axis
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
            ///  allows teleportation on the y-axis
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
