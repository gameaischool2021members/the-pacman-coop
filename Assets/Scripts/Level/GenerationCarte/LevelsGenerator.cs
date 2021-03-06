using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  Ce script génère le niveaux en fonction d'un fichier texte.
/// </summary>
public class LevelsGenerator : MonoBehaviour {
	/// <summary>
	///  chemin pour trouver le fichier texte qui contient la carte.
	/// </summary>
	private string chemin;
	/// <summary>
	///  la matrice d'image.
	/// </summary>
    public TileMatrix M;
	/// <summary>
	///  le compteur de pacgomme et superpacgomme
	/// </summary>
    public int compteur = 0;
	/// <summary>
	///  les différent objets.
	/// </summary>
    public phantomes phantomeRM;
	public phantomes phantomePM;
    public phantomes phantomeJM;
	public phantomes phantomeBM;
	/// <summary>
	///  liste des fantomes
	/// </summary>
	public List<GameObject> fantomes = new List<GameObject>();
	/// <summary>
	///  variable de caméra
	/// </summary>
    protected float CameraInitSize;
    protected Vector3 CameraInitPos;
	/// <summary>
	///  les différent objet unity.
	/// </summary>
    public GameObject pacman1;
    public GameObject phantomeR;
    public GameObject phantomeP;
    public GameObject phantomeJ;
    public GameObject phantomeB;
    public GameObject pacgomme;
	public GameObject superPacgomme;
	public GameObject cerise;


		/// <summary>
	///  les différentes liste d'objet unity.
	/// </summary>
	public List<GameObject> liPacgomme = new List<GameObject>();
	public List<GameObject> liSpPacgomme = new List<GameObject>();
	public List<GameObject> liMurTemp = new List<GameObject>();

    public int num = 0;
    private string[] tabLien;

    // Use this for initialization
    void Start()
    {
		/// <summary>
		///  initialise la caméra.
		/// </summary>
        
 
        CameraInitSize = Camera.main.orthographicSize;
        CameraInitPos = Camera.main.transform.position;
        num = PlayerPrefs.GetInt("num");

        if (PlayerPrefs.GetString("lien") != ""){


			chemin = PlayerPrefs.GetString ("lien");
            tabLien = chemin.Split('/');
            if (num >= tabLien.Length -1)
            {
                SceneManager.LoadScene("Victoire");
            }
            else
            {
                chemin = "Maps/"+tabLien[num];
            }
        }
        else{
			chemin = "Maps/default.map";
		}


        /// <summary>
        ///  créer une tilematrix
        /// </summary>
        M = new TileMatrix(transform.position, "Sprites/Default/tiles");
		/// <summary>
		///  charge la carte
		/// </summary>
        M.load(chemin);
		/// <summary>
		///  centre la caméra
		/// </summary>
        CenterCamera();
		/// <summary>
		///  creer les objets afin de lancer le jeux
		/// </summary>
        playLvl();
    }

    public TileMatrix tileMatrix
    {
        get
        {
            return M;
        }
    }

    public void MakeLvlFitOnScreen()
    {
        float ScreenSize = Screen.height;
        float HauteurNiveau = M.hauteur;

        if(HauteurNiveau >= 42)
        {
            while (HauteurNiveau >= 42)
            {
                HauteurNiveau -= 8.4f;
                Camera.main.orthographicSize++;
            }
            Camera.main.orthographicSize++;
        }
        else
        {
            while (HauteurNiveau < 42)
            {
                HauteurNiveau += 8.4f;
                Camera.main.orthographicSize--;
            }
            Camera.main.orthographicSize++;
        }
    }

    public void CenterCamera()
    {
        float LargeurNiveau = ((M.largeur - 1) * 0.24f);
        float HauteurNiveau = ((M.hauteur - 1) * 0.24f);

        if (CameraInitPos != Vector3.zero)
        {
            Camera.main.transform.position = new Vector3(CameraInitPos.x + (LargeurNiveau / 2), CameraInitPos.y - (HauteurNiveau / 2), CameraInitPos.z);
            MakeLvlFitOnScreen();
        }
        else
        {
            Debug.Log("Camera non initialisé");
        }
        if (M.hauteur == 0 && M.largeur == 0)
        {
            Debug.Log("TileMatrix non initialisé");
        }
    }
		
    public void playLvl()
    {

		/// <summary>
		///  parcours la matrice sur l'axe des y.
		/// </summary>
        for (int i = 0; i < M.hauteur; i++)
        {
			/// <summary>
			///  parcours la matrice sur l'axe des X.
			/// </summary>
            for (int j = 0; j < M.largeur; j++)
            {
				/// <summary>
				///  Si c'est l'image 7 ou 8 alors c'est un mur temporaire que j'ajoute a la liste correspondante.
				/// </summary>
                if (M.getTileCodeAt(i, j) == 7 || M.getTileCodeAt(i, j) == 8)
                {
                    liMurTemp.Add(M[i, j]);
                }
				/// <summary>
				///  Si c'est l'image 0 ou 10 alors c'est un vide ou je met un objet unity pacgommes et que je rajoute dans la listes correspondante .
				/// </summary>
                if (M.getTileCodeAt(i, j) == 0 || M.getTileCodeAt(i, j) == 10)
                {

                    Destroy(M[i, j]);
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    M[i, j] = Instantiate(pacgomme, pos, Quaternion.identity) as GameObject;
                    liPacgomme.Add(M[i, j]);
                    compteur++;
				}
				/// <summary>
				///  Si c'est l'image 35 alors c'est une superpacgomme ou je met un objet unity superpacgommes et que je rajoute dans la listes correspondante .
				/// </summary>
                else if (M.getTileCodeAt(i, j) == 35)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(superPacgomme, pos, Quaternion.identity) as GameObject;
                    liSpPacgomme.Add(M[i, j]);
                    compteur++;

                }
				/// <summary>
				///  Si c'est l'image 41 alors c'est une cerise ou je met un objet unity superpacgommes et que je rajoute dans la listes correspondante .
				/// </summary>
				else if (M.getTileCodeAt(i, j) == 41)
				{
					Vector3 pos = M[i, j].GetComponent<Transform>().position;
					Destroy(M[i, j]);
					M[i, j] = Instantiate(cerise, pos, Quaternion.identity) as GameObject;
					liSpPacgomme.Add(M[i, j]);
					compteur++;

				}
				/// <summary>
				///  Si c'est l'image 36 alors c'est pacman et j'instantie l'objet unity pacman1.
				/// </summary>
                else if (M.getTileCodeAt(i, j) == 36)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(pacman1, pos, Quaternion.identity) as GameObject;
					PlayerPrefs.SetString ("dirP", "droite");
                }
				/// <summary>
				///  Si c'est l'image 37 alors c'est le fantome rouge et j'instantie l'objet unity phantomeR.
				/// </summary>
                else if (M.getTileCodeAt(i, j) == 37)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(phantomeR, pos, Quaternion.identity) as GameObject;
					fantomes.Add(M [i, j]);
                }
				/// <summary>
				///  Si c'est l'image 38 alors c'est le fantome bleu et j'instantie l'objet unity phantomeB.
				/// </summary>
                else if (M.getTileCodeAt(i, j) == 38)
                {
                    
					Vector3 pos = M[i, j].GetComponent<Transform>().position;
					Destroy(M[i, j]);
					M[i, j] = Instantiate(phantomeB, pos, Quaternion.identity) as GameObject;
					fantomes.Add (M [i, j]);

                }
				/// <summary>
				///  Si c'est l'image 39 alors c'est le fantome rose et j'instantie l'objet unity phantomeP.
				/// </summary>
                else if (M.getTileCodeAt(i, j) == 39)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(phantomeP, pos, Quaternion.identity) as GameObject;
					fantomes.Add (M [i, j]);

                }
				/// <summary>
				///  Si c'est l'image 40 alors c'est le fantome jaune et j'instantie l'objet unity phantomeJ.
				/// </summary>
                else if (M.getTileCodeAt(i, j) == 40)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(phantomeJ, pos, Quaternion.identity) as GameObject;
					fantomes.Add (M [i, j]);


                }
            }
        }
    }
}