using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
///  Ce script sert a gérer les objets de parties.
/// </summary>
public class gestionParti : MonoBehaviour {
	/// <summary>
	///  nombre total de pacgomme.
	/// </summary>
	public int totalpacg;
	/// <summary>
	///  nombre de pacgomme en jeux.
	/// </summary>
	public int nbrpacg;
	/// <summary>
	///  le temps passé
	/// </summary>
	private float time=0;
	/// <summary>
	///  nombre de carte joué.
	/// </summary>
	private int num;

	private int i =0;
	private int j =0;
	/// <summary>
	///  liste des murs qui vont ce détruire.
	/// </summary>
	private List<GameObject> liMurTemp =new List<GameObject>();
	/// <summary>
	///  Le pacman unity.
	/// </summary>
	private GameObject pacman;

	public List<GameObject> fantomes =new List<GameObject>();


	public Sprite vivantR;
	public Sprite vivantP;
	public Sprite vivantJ;
	public Sprite vivantB;
	public Sprite chasser;
	public Sprite mort;

	private bool enChasse;

	private int tempChasse;

	void Start () {
		/// <summary>
		///  recherche des différentes valeurs.
		/// </summary>
		enChasse=false;
		totalpacg = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().compteur;
		num = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().num;
		liMurTemp = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().liMurTemp;
		pacman=GameObject.Find ("pacman(Clone)");

		fantomes = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().fantomes;
	}

	void Update () {
		nbrpacg = totalpacg - (pacman.GetComponent<collisionPacman> ().compteur2);
		if (nbrpacg == 0) {
			Destroy(GameObject.Find ("Main Camera").GetComponent<FlareLayer>());
			// Destroy(GameObject.Find ("Main Camera").GetComponent<GUILayer>());
			Destroy(GameObject.Find ("Main Camera").GetComponent<Animator>());
			Destroy(GameObject.Find ("Main Camera").GetComponent<Camera>());
			GameObject.Find ("Main Camera").name = "Son";
			DontDestroyOnLoad(GameObject.Find ("Son"));
            PlayerPrefs.SetInt("num", num + 1);
            SceneManager.LoadScene ("Jouer");
		}
		/// <summary>
		///  gestion du temps
		/// </summary>
		i++;
		if (i == 60) {
			time++;
			i = 0;
		}
		if (enChasse == false) {
			foreach (GameObject fantome in fantomes) {
				if (fantome.GetComponent<SpriteRenderer> ().sprite == chasser) {
					enChasse = true;
				}
			}
		}
		if (enChasse == true) {
			j++;
			if (j == 60) {
				tempChasse++;
				j = 0;
			}
		}
		if (tempChasse == 8) {
			tempChasse = 0;
			enChasse = false;

			foreach (GameObject fantome in fantomes) {
				if (fantome.GetComponent<SpriteRenderer> ().sprite != mort) {
					if (fantome.tag == "phantomeR") {
						fantome.GetComponent<SpriteRenderer> ().sprite = vivantR;
					}
					if (fantome.tag == "phantomeP") {
						fantome.GetComponent<SpriteRenderer> ().sprite = vivantP;
					}
					if (fantome.tag == "phantomeB") {
						fantome.GetComponent<SpriteRenderer> ().sprite = vivantB;
					}
					if (fantome.tag == "phantomeJ") {
						fantome.GetComponent<SpriteRenderer> ().sprite = vivantJ;
					}
				}
			}
		}
		/// <summary>
		///  Si 5 seconde ce sont écoulé détruit les murs temporaires
		/// </summary>
		if (time == 3) {
			foreach (GameObject murTemp in liMurTemp) {
				Destroy (murTemp);
			}
		}
	}
}
