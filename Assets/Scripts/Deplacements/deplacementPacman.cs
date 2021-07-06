using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
// using UnityEngine.PlaymodeTestsRunner;

/// <summary>
///  This script sets the pacman in motion according to the keys.
/// </summary>
public class deplacementPacman : MonoBehaviour {
    /// <summary>
    ///  variable speed
    /// </summary>
    private int speed ;
    /// <summary>
    ///  movement variables
    /// </summary>
    private int inputX;
	private int inputY;


    /// <summary>
    ///  variables of whether the pacman is in motion
    /// </summary>
    private bool move;
	private string memory;
    /// <summary>
    ///  destination variables
    /// </summary>
    private Vector2 buffer;
	/// <summary>
	///  la tilematrix
	/// </summary>
	private TileMatrix M;

	private RaycastHit2D rayhit;

	public Rigidbody2D pacman;
	// Use this for initialization
	void Start () {
		pacman =GetComponent<Rigidbody2D> ();
		buffer = pacman.position;
		move = false;
		speed = PlayerPrefs.GetInt ("speedP");
		if (speed == 0) {
			speed = 4;
		}
	}

    /// <summary>
    /// at each frame
    /// </summary>
    void Update (){

		M = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().M;


        /// <summary>
        /// if up key then pacman goes up
        /// </summary>
        if (Input.GetKey ("up")) {

				memory = "up";
		}


        /// <summary>
        ///  if down key then pacman goes down
        /// </summary>
        if (Input.GetKey ("down")) {
				memory = "down";
			}


        /// <summary>
        ///  if right key then pacman goes right
        /// </summary>
        if (Input.GetKey ("right")) {

				memory = "right";
			}

        /// <summary>
        ///  if left key then pacman goes left
        /// </summary>
        if (Input.GetKey ("left")) {


				memory = "left";
			}

		
		if (inputX == 1 ){


			if (transform.position.x >= buffer.x)  {

				move = false;
			}
		}
		if (inputY == 1) {
			if (transform.position.y >= buffer.y) {
				move = false;
			}
		}

		if (inputX == -1 ){
			if (transform.position.x <= buffer.x) {
				move = false;
			}
		}
		if (inputY == -1) {
			if (transform.position.y <= buffer.y) {
				move = false;
			}
		}




		if (move == false) {
			pacman.position = buffer;
			if (memory == "up") {
				rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x, pacman.position.y + 0.24f), new Vector2 (pacman.position.x, pacman.position.y + 0.24f), 0.1f);
				if (rayhit && rayhit.collider.tag != "wall" || !rayhit) {
					PlayerPrefs.SetString ("dirP", "up");


				}
			}			
			if (memory == "down") {
				rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x, pacman.position.y - 0.24f), new Vector2 (pacman.position.x, pacman.position.y - 0.24f), 0.1f);
				if (rayhit && rayhit.collider.tag != "wall" || !rayhit) {
					PlayerPrefs.SetString ("dirP", "down");


				}
			}
			if (memory == "right") {
				rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x + 0.24f, pacman.position.y), new Vector2 (pacman.position.x + 0.24f, pacman.position.y), 0.1f);
				if (rayhit && rayhit.collider.tag != "wall" || !rayhit) {
					PlayerPrefs.SetString ("dirP", "right");


				}
			}
			if (memory == "left") {
				rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x - 0.24f, pacman.position.y), new Vector2 (pacman.position.x - 0.24f, pacman.position.y ), 0.1f);
				if (rayhit && rayhit.collider.tag != "wall" || !rayhit) {
					PlayerPrefs.SetString ("dirP", "left");


				}
			}
			if (PlayerPrefs.GetString ("dirP") == "up") {

					rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x, pacman.position.y + 0.24f), new Vector2 (pacman.position.x, pacman.position.y + 0.24f), 0.1f);
					inputY = 1;
					inputX = 0;		
					animate ();

					if (!rayhit || rayhit.collider.tag != "wall") {
						buffer = new Vector2 (buffer.x, buffer.y + 0.24f);
						move = true;
				}
			}

			if (PlayerPrefs.GetString ("dirP") == "down") {
				rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x, pacman.position.y - 0.24f), new Vector2 (pacman.position.x, pacman.position.y - 0.24f), 0.1f);
				inputY = -1;
				inputX = 0;
				animate ();

				if (!rayhit || rayhit.collider.tag != "wall") {
					buffer = new Vector2 (buffer.x, buffer.y - 0.24f);
					move = true;
				}
			}

			if (PlayerPrefs.GetString ("dirP") == "right") {
				rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x + 0.24f, pacman.position.y), new Vector2 (pacman.position.x + 0.24f, pacman.position.y), 0.1f);
				inputY = 0;
				inputX = 1;
				animate ();

				if (!rayhit || rayhit.collider.tag != "wall") {
					buffer = new Vector2 (buffer.x + 0.24f, buffer.y);
					move = true;
				}
			}

			if (PlayerPrefs.GetString ("dirP") == "left") {
				rayhit = Physics2D.Raycast (new Vector2 (pacman.position.x - 0.24f, pacman.position.y), new Vector2 (pacman.position.x - 0.24f, pacman.position.y ), 0.1f);
				inputY = 0;
				inputX = -1;
				animate ();

				if (!rayhit ||rayhit.collider.tag != "wall") {
					buffer = new Vector2 (buffer.x - 0.24f, buffer.y);
					move = true;
				}
			}
		}
        /// <summary>
        ///  allows teleportation on the x-axis
        /// </summary>
        if (transform.position.x > (M.width-1) * 0.24f) {
			transform.position = new Vector2 (0, transform.position.y);
			buffer = transform.position;
		}
		if (transform.position.x < 0) {
			transform.position = new Vector2 ((M.width-1) * 0.24f, transform.position.y);
			buffer = transform.position;

		}
        /// <summary>
        ///  allows teleportation on the y-axis
        /// </summary>
        if (transform.position.y < -(M.height-1) * 0.24f) {
			transform.position = new Vector2 (transform.position.x, 0);
			buffer = transform.position;

		}
		if (transform.position.y > 0) {
			transform.position = new Vector2 (transform.position.x, -(M.height-1) * 0.24f);
			buffer = transform.position;

		}

	}


	void FixedUpdate(){
        /// <summary>
        ///  movement
        /// </summary>
        pacman.velocity = new Vector2 ((speed * inputX)*0.24f, (speed * inputY)*0.24f);

	}
	    void animate()
    {
        GetComponent<Animator>().SetFloat("DirX", inputX);
        GetComponent<Animator>().SetFloat("DirY", inputY);
    }

}
