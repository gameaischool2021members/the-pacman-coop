using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System;
/// <summary>
///  This script allows the yellow ghost to move according to its AI.
/// </summary>
public class DeplacementphantomeR : MonoBehaviour
{
    /// <summary>
    ///  speed variable
    /// </summary>
    private float speed;
    private float speedV;
    /// <summary>
    ///  movement variable
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
    ///  the objects
    /// </summary>
    private phantomes phantomeRM;
    public Transform phantomeR;
    public GameObject pacman;
    public Rigidbody2D pacmanR;

    /// <summary>
    ///  the images
    /// </summary>
    public Sprite hunt;
    public Sprite alive;
    public Sprite dead;
    /// <summary>
    ///  back to life variables.
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
        phantomeR = GetComponent<Transform>();
        M = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().M;
        phantomeRM = new phantomes(phantomeR.position.x, phantomeR.position.y, "right");
        pacman = GameObject.Find("pacman(Clone)");
        pacmanR = pacman.GetComponent<Rigidbody2D>();
        buffer = phantomeR.position;
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

    // Update is called once per frame
    void Update()
    {

        /// <summary>
        ///  Test to see if the ghost can come back to life after being eaten (it must be in the centre)
        /// </summary>
        if (phantomeR.position.x > xInf && phantomeR.position.x < xSup && phantomeR.position.y < ySup && phantomeR.position.y > yInf && GetComponent<SpriteRenderer>().sprite == dead)
        {
            GetComponent<SpriteRenderer>().sprite = alive;
            speed = speedV;

        }
        /// <summary>
        ///  depending on the direction of the ghost see if it has reached its destination if so then it moves more
        /// </summary>
        if (inputX == 1)
        {
            if (phantomeR.position.x >= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == 1)
        {
            if (phantomeR.position.y >= buffer.y)
            {
                move = false;
            }
        }

        if (inputX == -1)
        {
            if (phantomeR.position.x <= buffer.x)
            {
                move = false;
            }
        }
        if (inputY == -1)
        {
            if (phantomeR.position.y <= buffer.y)
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
            /// it moves
            /// </summary>
            move = true;

            /// <summary>
            ///  replaces it precisely
            /// </summary>
            phantomeR.position = buffer;

            /// <summary>
            ///  search according to its state (alive, hunting, dead)
            /// </summary>
            if (GetComponent<SpriteRenderer>().sprite == hunt)
            {
                phantomeRM.findR(phantomeR, new Vector2(0, 0), M);
            }
            else
            {
                if (GetComponent<SpriteRenderer>().sprite == dead)
                {
                    speed = 8;
                    phantomeRM.findR(phantomeR, new Vector2(xSup - 0.05f, ySup - 0.05f), M);

                }
                else
                {

                    /// <summary>
                    /// the target is pacman
                    /// </summary>
                    phantomeRM.findR(phantomeR, pacmanR.position, M);

                }
            }

            /// <summary>
            ///  applies the direction
            /// </summary>

            if (phantomeRM.getDirection() == "up")
            {
                inputX = 0;
                inputY = 1;

                buffer = buffer + new Vector2(0f, 0.24f);
                //animate();

            }
            if (phantomeRM.getDirection() == "down")
            {
                inputX = 0;
                inputY = -1;
                buffer = buffer + new Vector2(0f, -0.24f);
                //animate();


            }
            if (phantomeRM.getDirection() == "right")
            {
                inputX = 1;
                inputY = 0;
                buffer = buffer + new Vector2(0.24f, 0f);
                //animate();

            }
            if (phantomeRM.getDirection() == "left")
            {
                inputX = -1;
                inputY = 0;
                buffer = buffer + new Vector2(-0.24f, 0f);
                //animate();

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
