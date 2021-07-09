using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
///  test les collisions des fantomes.
/// </summary>
public class collisionPhantome : MonoBehaviour {

	/// <summary>
	///  image nécéssaire
	/// </summary>
	public Sprite vivant;
	public Sprite chasser;
	public Sprite mort;
	/// <summary>
	///  Si un objet entre dans la zone de collicsion
	/// </summary>
	void OnCollisionEnter2D(Collision2D coll) {

		/// <summary>
		///  test de collision avec le pacman ou si le fantome est vivant alors le pacman est mangé et la partie est perdu.
		/// </summary>
		if (coll.gameObject.tag == "pacman" && GetComponent<SpriteRenderer> ().sprite == vivant) {
			//Destroy (coll.gameObject);
			//SceneManager.LoadScene ("Defaite");

			//agent collision update
			PacmanAgent agent = coll.gameObject.GetComponent<PacmanAgent>();
			if (agent != null)
            {
				agent.CollidedActionAgent = "ghostAlive";
            }
		}
		/// <summary>
		///  test de collision avec pacman si le fantome est chasser alors il meurt.
		/// </summary>
		/// 

		if (coll.gameObject.tag == "pacgomme" || coll.gameObject.tag == "superPacgomme")
		{
			Physics2D.IgnoreCollision(coll.gameObject.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
		}




		if (coll.gameObject.tag == "pacman" && GetComponent<SpriteRenderer> ().sprite == chasser) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = mort;
            coll.gameObject.GetComponent<collisionPacman>().score += 200;

			//agent collision update
			PacmanAgent agent = coll.gameObject.GetComponent<PacmanAgent>();
			if (agent != null)
			{
				agent.CollidedActionAgent = "ghostDead";
			}
		}
	}
}
