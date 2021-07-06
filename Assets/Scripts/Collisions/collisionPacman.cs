using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This script allows to manage collisions on pacman.
/// </summary>
public class collisionPacman : MonoBehaviour
{

    /// <summary>
    ///  initialization of unity objects
    /// </summary>

    public List<GameObject> fantomes = new List<GameObject>();
    /// <summary>
    ///  different sprite used
    /// </summary>
    public Sprite hunt;
    public Sprite dead;
    /// <summary>
    ///  counter to count the number of pacgums and superpacgums eaten
    /// </summary>
    public int compteur2 { get; set; }
    /// <summary>
    ///  Pacman score during a game.
    /// </summary>
    public int score { get; set; }



    void OnCollisionEnter2D(Collision2D coll)
    {
        /// <summary>
        ///  collision test with pacGums then it destroys it and increments the eaten pacGums counter + score.
        /// </summary>
        if (coll.gameObject.tag == "pacgomme")
        {
            Destroy(coll.gameObject);
            compteur2++;
            score += 10;
        }
        /// <summary>
        ///  collision test with the cherry then it destroys it and increments the counter of eaten pacgum + score.
        /// </summary>
        if (coll.gameObject.tag == "cerise")
        {
            Destroy(coll.gameObject);
            compteur2++;
            score += 150;
        }
        /// <summary>
        /// collision test with superPacGums then it destroys it and increments the eaten pacgums counter + score.
        /// and if the ghosts are not dead (images with just the eyes) then put them in hunt (blue images).
        /// and puts the pacman object in hunter state.

        /// </summary>
        if (coll.gameObject.tag == "superPacgomme")
        {
            Destroy(coll.gameObject);
            fantomes = GameObject.Find("LevelsGenerator").GetComponent<LevelsGenerator>().fantomes;
            foreach (GameObject ghost in fantomes)
            {
                if (ghost.GetComponent<SpriteRenderer>().sprite != dead)
                {
                    ghost.gameObject.GetComponent<SpriteRenderer>().sprite = hunt;
                }
            }
            score += 100;
            compteur2++;
        }

    }


}