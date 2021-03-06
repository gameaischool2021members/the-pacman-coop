using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Ce script permet de gérer les collisions sur pacman.
/// </summary>
public class collisionPacman : MonoBehaviour {


	/// <summary>
	///  initialisation des objets unity
	/// </summary>

	public List<GameObject> fantomes =new List<GameObject>();
	/// <summary>
	///  différent sprite utilisé
	/// </summary>
	public Sprite chasser;
	public Sprite mort;
	/// <summary>
	///  compteur afin de compter le nombres de pacgommes et superpacgomme mangé
	/// </summary>
	public int compteur2 { get; set; }
    /// <summary>
    ///  Score du Pacman durant une partie.
    /// </summary>
    public int score { get; set; }



    void OnCollisionEnter2D(Collision2D coll) {
		/// <summary>
		///  test de collision avec les pacGommes alors ca la détruit et ca incrémente le compteur de pacgomme mangé + score.
		/// </summary>
		if (coll.gameObject.tag == "pacgomme") {
			Destroy (coll.gameObject);
			compteur2++;
			score += 10;
		}		
		/// <summary>
		///  test de collision avec les cerise alors ca la détruit et ca incrémente le compteur de pacgomme mangé + score.
		/// </summary>
		if (coll.gameObject.tag == "cerise") {
			Destroy (coll.gameObject);
			compteur2++;
			score += 150;
		}
		/// <summary>
		///  test de collision avec les superPacGommes alors ca la détruit et ca incrémente le compteur de pacgomme mangé + score.
		/// et si les fantomes ne sont pas mort(images avec juste les yeux) alors les mets en chasser (images bleu).
		/// et met l'objet pacman en etat chasseur.

		/// </summary>
		if (coll.gameObject.tag == "superPacgomme") {
			Destroy (coll.gameObject);
			fantomes = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().fantomes;
			foreach (GameObject fantome in fantomes) {
				if (fantome.GetComponent<SpriteRenderer> ().sprite != mort) {
					fantome.gameObject.GetComponent<SpriteRenderer> ().sprite = chasser;
				}
			}
            score += 100;
            compteur2++;
		}

	}


}