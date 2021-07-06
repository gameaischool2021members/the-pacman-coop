using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  This script generates the level based on a text file.
/// </summary>
public class LevelsGenerator : MonoBehaviour
{
    /// <summary>
    ///  path to find the text file that contains the map.
    /// </summary>
    private string path;
    /// <summary>
    ///  the image matrix.
    /// </summary>
    public TileMatrix M;
    /// <summary>
    ///  the pacgum and superpacgum counter
    /// </summary>
    public int counter = 0;
    /// <summary>
    ///  the different objects.
    /// </summary>
    public phantomes phantomeRM;
    public phantomes phantomePM;
    public phantomes phantomeJM;
    public phantomes phantomeBM;
    /// <summary>
    ///  list of ghosts
    /// </summary>
    public List<GameObject> ghosts = new List<GameObject>();
    /// <summary>
    ///  camera variable
    /// </summary>
    protected float CameraInitSize;
    protected Vector3 CameraInitPos;
    /// <summary>
    ///  the different unity objects.
    /// </summary>
    public GameObject pacman1;
    public GameObject phantomeR;
    public GameObject phantomeP;
    public GameObject phantomeJ;
    public GameObject phantomeB;
    public GameObject pacgum;
    public GameObject superPacgum;
    public GameObject cerise;


    /// <summary>
	///  the different unity object lists.
	/// </summary>
	public List<GameObject> liPacgum = new List<GameObject>();
    public List<GameObject> liSpPacgum = new List<GameObject>();
    public List<GameObject> liMurTemp = new List<GameObject>();

    public int num = 0;
    private string[] tabLien;

    // Use this for initialization
    void Start()
    {
        /// <summary>
        ///  initializes the camera.
        /// </summary>


        CameraInitSize = Camera.main.orthographicSize;
        CameraInitPos = Camera.main.transform.position;
        num = PlayerPrefs.GetInt("num");

        if (PlayerPrefs.GetString("lien") != "")
        {


            path = PlayerPrefs.GetString("lien");
            tabLien = path.Split('/');
            if (num >= tabLien.Length - 1)
            {
                SceneManager.LoadScene("Victoire");
            }
            else
            {
                path = "Maps/" + tabLien[num];
            }
        }
        else
        {
            path = "Maps/default.map";
        }


        /// <summary>
        ///  create a tilematrix
        /// </summary>
        M = new TileMatrix(transform.position, "Sprites/Default/tiles");
        /// <summary>
        ///  load the map
        /// </summary>
        M.load(path);
        /// <summary>
        ///  centre the camera
        /// </summary>
        CenterCamera();
        /// <summary>
        ///  create the objects to launch the game
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
        float HeightLevel = M.height;

        if (HeightLevel >= 42)
        {
            while (HeightLevel >= 42)
            {
                HeightLevel -= 8.4f;
                Camera.main.orthographicSize++;
            }
            Camera.main.orthographicSize++;
        }
        else
        {
            while (HeightLevel < 42)
            {
                HeightLevel += 8.4f;
                Camera.main.orthographicSize--;
            }
            Camera.main.orthographicSize++;
        }
    }

    public void CenterCamera()
    {
        float WidthLevel = ((M.width - 1) * 0.24f);
        float HeightLevel = ((M.height - 1) * 0.24f);

        if (CameraInitPos != Vector3.zero)
        {
            Camera.main.transform.position = new Vector3(CameraInitPos.x + (WidthLevel / 2), CameraInitPos.y - (HeightLevel / 2), CameraInitPos.z);
            MakeLvlFitOnScreen();
        }
        else
        {
            Debug.Log("Camera not initialized");
        }
        if (M.height == 0 && M.width == 0)
        {
            Debug.Log("TileMatrix not initialized.");
        }
    }

    public void playLvl()
    {

        /// <summary>
        ///  runs the matrix along the y-axis.
        /// </summary>
        for (int i = 0; i < M.height; i++)
        {
            /// <summary>
            ///  runs the matrix along the x-axis.
            /// </summary>
            for (int j = 0; j < M.width; j++)
            {
                /// <summary>
                ///  If it's image 7 or 8 then it's a temporary wall which I add to the corresponding list.
                /// </summary>
                if (M.getTileCodeAt(i, j) == 7 || M.getTileCodeAt(i, j) == 8)
                {
                    liMurTemp.Add(M[i, j]);
                }
                /// <summary>
                ///  If it's image 0 or 10 then it's a blank or I put an object unity pacgums and I add it in the corresponding list.
                /// </summary>
                if (M.getTileCodeAt(i, j) == 0 || M.getTileCodeAt(i, j) == 10)
                {

                    Destroy(M[i, j]);
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    M[i, j] = Instantiate(pacgum, pos, Quaternion.identity) as GameObject;
                    liPacgum.Add(M[i, j]);
                    counter++;
                }
                /// <summary>
                ///  If it's image 35 then it's a superpacgum or I put a unity superpacgums object and add it to the corresponding list.
                /// </summary>
                else if (M.getTileCodeAt(i, j) == 35)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(superPacgum, pos, Quaternion.identity) as GameObject;
                    liSpPacgum.Add(M[i, j]);
                    counter++;

                }
                /// <summary>
                ///  If it's picture 41 then it's a cherry or I put a unity object superpacgums and I add it to the corresponding list.
                /// </summary>
                else if (M.getTileCodeAt(i, j) == 41)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(cerise, pos, Quaternion.identity) as GameObject;
                    liSpPacgum.Add(M[i, j]);
                    counter++;

                }
                /// <summary>
                ///  If it is image 36 then it is pacman and I instantiate the unity object pacman1.
                /// </summary>
                else if (M.getTileCodeAt(i, j) == 36)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(pacman1, pos, Quaternion.identity) as GameObject;
                    PlayerPrefs.SetString("dirP", "right");
                }
                /// <summary>
                ///  If it's image 37 then it's the red phantom and I instantiate the unity object phantomR.
                /// </summary>
                else if (M.getTileCodeAt(i, j) == 37)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(phantomeR, pos, Quaternion.identity) as GameObject;
                    ghosts.Add(M[i, j]);
                }
                /// <summary>
                ///  If it's image 38 then it's the blue phantom and I instantiate the unity object phantomB.
                /// </summary>
                else if (M.getTileCodeAt(i, j) == 38)
                {

                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(phantomeB, pos, Quaternion.identity) as GameObject;
                    ghosts.Add(M[i, j]);

                }
                /// <summary>
                ///  If it is image 39 then it is the pink phantom and I instantiate the unity object phantomP.
                /// </summary>
                else if (M.getTileCodeAt(i, j) == 39)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(phantomeP, pos, Quaternion.identity) as GameObject;
                    ghosts.Add(M[i, j]);

                }
                /// <summary>
                ///  If it is image 40 then it is the yellow phantom and I instantiate the unity object phantomJ.
                /// </summary>
                else if (M.getTileCodeAt(i, j) == 40)
                {
                    Vector3 pos = M[i, j].GetComponent<Transform>().position;
                    Destroy(M[i, j]);
                    M[i, j] = Instantiate(phantomeJ, pos, Quaternion.identity) as GameObject;
                    ghosts.Add(M[i, j]);


                }
            }
        }
    }
}