using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System;
/// <summary>
///  Ce script permet de mettre le fantome jaune en mouvement en fonction de son IA.
/// </summary>
public class DeplacementphantomeR : MonoBehaviour {
	/// <summary>
	///  variable vitesse
	/// </summary>
	private float speed ;
	private float speedV;
	/// <summary>
	///  variables de déplacement
	/// </summary>
	private int inputX;
	private int inputY;
	/// <summary>
	///  variables de destination
	/// </summary>
	private Vector2 tampon;
	/// <summary>
	///  variables de si le fantome est en mouvement
	/// </summary>
	private bool move;

	/// <summary>
	///  les objets
	/// </summary>
	private phantomes phantomeRM;
	public Transform phantomeR ;
	public GameObject pacman;
	public Rigidbody2D pacmanR;

	/// <summary>
	///  les images
	/// </summary>
	public Sprite chasser;
	public Sprite vivant;
	public Sprite mort;
	/// <summary>
	///  variables de retour a la vie.
	/// </summary>
	private float xSup;
	private float xInf;
	private float ySup;
	private float yInf;

	/// <summary>
	///  la tileMatrix
	/// </summary>
	private TileMatrix M;

	void Start () {
		/// <summary>
		///  récupératioin et initialisation des différentes variables
		/// </summary>
		phantomeR = GetComponent<Transform> ();
		M = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().M;
		phantomeRM = new phantomes(phantomeR.position.x, phantomeR.position.y, "droite");
		pacman=GameObject.Find ("pacman(Clone)");
		pacmanR = pacman.GetComponent<Rigidbody2D> ();
		tampon = phantomeR.position;
		/// <summary>
		///  récupératioin de la position de résurection.
		/// </summary>
		xSup = GetComponent<Transform> ().position.x + 0.05f;
		xInf = GetComponent<Transform> ().position.x - 0.05f;
		ySup = GetComponent<Transform> ().position.y + 0.05f;
		yInf = GetComponent<Transform> ().position.y - 0.05f;
		speedV = PlayerPrefs.GetInt ("speedF");
		if (speedV == 0) {
			speedV = 3;
		}
		speed = speedV;

	}

	// Update is called once per frame
	void Update (){

		/// <summary>
		///  Test pour savoir si le fantome peut redevenir vivant apres avoir été manger (il faut qu'il soit au centre)
		/// </summary>
		if (phantomeR.position.x > xInf && phantomeR.position.x < xSup && phantomeR.position.y < ySup && phantomeR.position.y > yInf && GetComponent<SpriteRenderer> ().sprite == mort ) 
		{
			GetComponent<SpriteRenderer> ().sprite = vivant;
			speed = speedV;

		}
		/// <summary>
		///  celon la direction du fantome regarde si il est bien arrivé a destination si oui alors il bouge plus
		/// </summary>
		if (inputX == 1 ){
			if (phantomeR.position.x >= tampon.x)  {
				move = false;
			}
		}
		if (inputY == 1) {
			if (phantomeR.position.y >= tampon.y) {
				move = false;
			}
		}

		if (inputX == -1 ){
			if (phantomeR.position.x <= tampon.x) {
				move = false;
			}
		}
		if (inputY == -1) {
			if (phantomeR.position.y <= tampon.y) {
				move = false;
			}
		}
		/// <summary>
		///  si le fantome bouge plus alors recherche la ou il dois aller
		/// </summary>
		if (move == false) {

			/// <summary>
			///  il bouge
			/// </summary>
			move = true;

			/// <summary>
			///  le replace précisément
			/// </summary>
			phantomeR.position = tampon;

			/// <summary>
			///  fais la recherche en fonction de son etat (vivant, chasser, mort)
			/// </summary>
			if (GetComponent<SpriteRenderer> ().sprite == chasser) {
				phantomeRM.rechercheR (phantomeR, new Vector2 (0, 0), M);
			} else {
				if (GetComponent<SpriteRenderer> ().sprite == mort) {
					speed = 8;
					phantomeRM.rechercheR (phantomeR, new Vector2 (xSup - 0.05f, ySup - 0.05f), M);

				} else {

					/// <summary>
					///  la cible c'est pacman.
					/// </summary>
					phantomeRM.rechercheR (phantomeR, pacmanR.position, M);

				}
			}

			/// <summary>
			///  applique la direction.
			/// </summary>

			if (phantomeRM.getDirection () == "haut") {
				inputX = 0;
				inputY = 1;

				tampon = tampon + new Vector2 (0f, 0.24f);
                //animate();

            }
			if (phantomeRM.getDirection () == "bas") {
				inputX = 0;
				inputY = -1;
				tampon = tampon + new Vector2 (0f, -0.24f);
                //animate();


            }
			if (phantomeRM.getDirection () == "droite") {
				inputX = 1;
				inputY = 0;
				tampon = tampon + new Vector2 (0.24f, 0f);
                //animate();

            }
			if (phantomeRM.getDirection () == "gauche") {
				inputX = -1;
				inputY = 0;	
				tampon = tampon + new Vector2 (-0.24f, 0f);
                //animate();

            }


			/// <summary>
			///  permet la téléportation sur l'axe des x
			/// </summary>
			if (transform.position.x > (M.largeur-1) * 0.24f) {
				transform.position = new Vector2 (0, transform.position.y);
				tampon = transform.position;
			}
			if (transform.position.x < 0) {
				transform.position = new Vector2 ((M.largeur-1) * 0.24f, transform.position.y);
				tampon = transform.position;

			}
			/// <summary>
			///  permet la téléportation sur l'axe des y
			/// </summary>
			if (transform.position.y < -(M.hauteur-1) * 0.24f) {
				transform.position = new Vector2 (transform.position.x, 0);
				tampon = transform.position;

			}
			if (transform.position.y > 0) {
				transform.position = new Vector2 (transform.position.x, -(M.hauteur-1) * 0.24f);
				tampon = transform.position;

			}
		
		}	



	}

	void FixedUpdate(){
		/// <summary>
		///  déplacement.
		/// </summary>


		transform.position = Vector3.MoveTowards(transform.position, tampon, Time.deltaTime * speed *0.24f);
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
