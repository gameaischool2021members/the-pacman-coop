using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TileMatrix class allows you to modify Tile Matrices and load/save them to files.
/// </summary>
public class TileMatrix : UnityEngine.Object
{
    /// <summary> List of list (contains Tiles).</summary>
    protected List<List<GameObject>> _matrice;
    /// <summary> Height of the TileMatrix.</summary>
    protected int _height;
    /// <summary> Width of the TileMatrix.</summary>
    protected int _width;
    /// <summary> Position of the TileMatrix.</summary>
    protected Vector2 _position;
    /// <summary> Array of sprites that will hold all the Tiles to be loaded.</summary>
    protected Sprite[] _tilesSprites;
    /// <summary> Path to the texture containing the tiles.</summary>
    protected string _tilesPath;

    /// <summary>
    /// Reading accessor on pitch.
    /// </summary>
    /// <returns> Height of the TileMatrix. </returns>
    public int height { get { return _height; } }

    /// <summary>
    /// Read accessor on the width.
    /// </summary>
    /// <returns> Width of the TileMatrix. </returns>
    public int width { get { return _width; } }

    /// <summary>
    /// Read/write accessor on the position of the TileMatrix.
    /// </summary>
    /// <returns> position of the TileMatrix. </returns>
    public Vector2 position { get { return _position; } set { _position = value; } }

    /// <summary>
    /// Read accessor to the number of tiles available with the loaded texture.
    /// </summary>
    /// <returns> Name of available tuiles. </returns>
    public int TilesNumber { get { return _tilesSprites.Length; } }

    /// <summary>
    /// Read accessor on the path of the loaded texture containing the tiles.
    /// </summary>
    /// <returns> Path as a String. </returns>
    public string TilesPath { get { return _tilesPath; } }

    /// <summary>
    /// Checks if the TileMatrix is empty (zero height and width).
    /// </summary>
    /// <returns> True if empty false otherwise. </returns>
    public bool isEmpty() { return width == 0 && height == 0; }

    /// <summary>
    /// Loads a sprite from an integer sends an error message if the integer is too low or too high
    /// </summary>
    /// <param name="spriteNumber">An integer corresponding to a sprite </param>
    /// <returns> A Sprite corresponding to an integer. </returns>
    public Sprite loadSprite(int spriteNumber)
    {
        if (spriteNumber >= 0 && spriteNumber < TilesNumber)
        {
            return _tilesSprites[spriteNumber];
        }
        else
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Modifies a Tile in the TileMatrix with an integer that codes a Tile.
    /// </summary>
    /// <param name="i"> TileMatrix line </param>
    /// <param name="j"> TileMatrix column </param>
    /// <param name="TileCode"> Integer corresponding to a Tile </param>
    public void updateTileAt(int i, int j, int TileCode)
    {
        this[i, j].GetComponent<SpriteRenderer>().sprite = loadSprite(TileCode);
        this[i, j].GetComponent<Tile>().code = TileCode;
    }

    /// <summary>
    /// Convert a TileMatrix into an Integer Matrix.
    /// </summary>
    /// <param name="TM"> TileMatrix to convert </param>
    /// <returns> Integer matrix corresponding to a TileMatrix. </returns>
    static Matrix TileMatrixToMatrice(TileMatrix TM)
    {
        Matrix M = new Matrix(TM.height, TM.width);

        for (int i = 0; i < TM.height; i++)
        {
            for (int j = 0; j < TM.width; j++)
            {
                M[i, j] = TM.getTileCodeAt(i, j);
            }
        }
        return M;
    }

    /// <summary>
    /// Create a Tile from a row and column number and a number corresponding to a Sprite to be loaded in this Tile.
    /// </summary>
    /// <param name="i"> TileMatrix line </param>
    /// <param name="j"> TileMatrix column </param>
    /// <param name="numSprite"> Integer corresponding to a Sprite to be loaded </param>
    public GameObject CreateTile(int i, int j, int numSprite)
    {
        GameObject GO = new GameObject();

        GO.AddComponent<SpriteRenderer>();
        GO.GetComponent<SpriteRenderer>().sprite = loadSprite(numSprite);
        GO.AddComponent<BoxCollider2D>();

        float tileHeight = GO.GetComponent<SpriteRenderer>().sprite.textureRect.height / 100;
        float tileWidth = GO.GetComponent<SpriteRenderer>().sprite.textureRect.width / 100;

        GO.AddComponent<Tile>();
        GO.GetComponent<Tile>().code = numSprite;
        GO.GetComponent<Tile>().i = i;
        GO.GetComponent<Tile>().j = j;

        GameObject newGO = Instantiate(GO, new Vector2(position.x + tileWidth * j, position.y - tileHeight * i), Quaternion.identity) as GameObject;
        Destroy(GO);
        return newGO;
    }

    /// <summary>
    ///  Default Constructor.
    /// </summary>
    /// <param name="position"> Position where the TileMatrix will be displayed </param>
    /// <param name="tilesPath"> Path of the texture containing the tiles </param>
    public TileMatrix(Vector2 position, string tilesPath)
    {
        _matrice = null;
        _height = 0;
        _width = 0;
        _position = position;
        _tilesPath = tilesPath;
        _tilesSprites = Resources.LoadAll<Sprite>(tilesPath);
    }

    /// <summary>
    /// Constructeur avec plus de paramètres.
    /// </summary>
    /// <param name="height"> Number of lines in the TileMatrix </param>
    /// <param name="width"> Number of columns in the TileMatrix </param>
    /// <param name="position"> Position where the TileMatrix will be displayed </param>
    /// <param name="tilesPath"> Path of the texture containing the tiles </param>
    /// <param name="tileNumber"> Integer corresponding to a Tile with which the TileMatrix will be initialized </param>
    public TileMatrix(int height, int width, Vector2 position, string tilesPath, int tileNumber)
    {
        _tilesSprites = Resources.LoadAll<Sprite>(tilesPath);
        _matrice = new List<List<GameObject>>();
        _height = height;
        _width = width;
        _position = position;
        _tilesPath = tilesPath;

        for (int i = 0; i < height; i++)
        {
            _matrice.Add(new List<GameObject>());

            for (int j = 0; j < width; j++)
            {
                _matrice[i].Add(CreateTile(i, j, tileNumber));
            }
        }
    }

    /// <summary>
    /// Constructor by copy.
    /// </summary>
    /// <param name="M">The TileMatrix we want to copy</param>
    public TileMatrix(TileMatrix M)
    {
        copy(M);
    }

    /// <summary>
    /// Copies all the values and format of an M-matrix.
    /// </summary>
    /// <param name="M"> The TileMatrix we want to copy </param>
    public void copy(TileMatrix M)
    {
        _matrice = new List<List<GameObject>>();
        _height = M.height;
        _width = M.width;
        _tilesPath = M.TilesPath;
        _tilesSprites = Resources.LoadAll<Sprite>(_tilesPath);

        for (int i = 0; i < _height; i++)
        {
            _matrice.Add(new List<GameObject>());

            for (int j = 0; j < _width; j++)
            {
                _matrice[i].Add(CreateTile(i, j, M.getTileCodeAt(i, j)));
            }
        }
    }

    /// <summary>
    /// Max
    /// </summary>
    /// <returns> The Max </returns>
    public double max()
    {
        return Math.Round(Math.Sqrt((this.width * this.width) + (this.height * this.height))) + 2;
    }

    /// <summary>
    /// TileMatrix indexer allows read and write access to data.
    /// </summary>
    /// <param name="i"> Line N°i </param>
    /// <param name="j"> Column N°j </param>
    /// <returns> The Tile that the matrix contains in row i and column j </returns>
    public GameObject this[int i, int j]
    {
        get
        {
            if (i < 0 || i >= _height || j < 0 || j >= _width)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return _matrice[i][j];
            }
        }
        set
        {
            if (i < 0 || i >= _height || j < 0 || j >= _width)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                _matrice[i][j] = value;
            }
        }
    }

    /// <summary>
    /// Read accessor on the integer that encodes the Tile at row i and column j of the TileMatrix.
    /// </summary>
    /// <param name="i"> Line N°i </param>
    /// <param name="j"> Column N°j </param>
    /// <returns> The code of the Tile at row i and column j of the TileMatrix </returns>
    public int getTileCodeAt(int i, int j)
    {
        if (i < 0 || i >= _height || j < 0 || j >= _width)
        {
            throw new IndexOutOfRangeException();
        }
        else
        {
            return this[i, j].GetComponent<Tile>().code;
        }
    }

    /// <summary>
    /// Modifies the code of the Tile in row i and column j of the TileMatrix.
    /// </summary>
    /// <param name="i"> Line N°i </param>
    /// <param name="j"> Column N°j </param>
    /// <param name="val"> Corresponds to the value to be changed</param>
    public void setTileCodeAt(int i, int j, int val)
    {
        if (i < 0 || i >= _height || j < 0 || j >= _width)
        {
            throw new IndexOutOfRangeException();
        }
        else
        {
            this[i, j].GetComponent<Tile>().code = val;
        }
    }

    /// <summary>
    /// Moves a line from the TileMatrix to the display.
    /// </summary>
    /// <param name="i"> Line to be moved </param>
    /// <param name="stepX"> Step with which the line will be moved on the X</param>
    /// <param name="stepY"> Step with which the line will be moved on the Y </param>
    public void moveLineAt(int i, int stepX, int stepY)
    {
        for (int j = 0; j < _width; j++)
        {
            this[i, j].GetComponent<Tile>().move(stepX, stepY);
        }
    }

    /// <summary>
    /// Move a column from the TileMatrix to the display.
    /// </summary>
    /// <param name="j"> Column to be moved </param>
    /// <param name="stepX"> Step with which the line will be moved on the X</param>
    /// <param name="stepY"> Step with which the line will be moved on the Y </param>
    public void moveColomnAt(int j, int stepX, int stepY)
    {
        for (int i = 0; i < _height; i++)
        {
            this[i, j].GetComponent<Tile>().move(stepX, stepY);
        }
    }

    /// <summary>
    /// Moves certain lines of the TileMatrix to the display.
    /// </summary>
    /// <param name="i1"> Line from which the lines will be moved </param>
    /// <param name="i2"> Line from which the lines will no longer be moved </param>
    /// <param name="stepX"> Step with which the line will be moved on the X</param>
    /// <param name="stepY"> Step with which the line will be moved on the Y </param>
    public void moveLinesBetween(int i1, int i2, int stepX, int stepY)
    {
        for (int i = i1; i < i2; i++)
        {
            moveLineAt(i, stepX, stepY);
        }
    }

    /// <summary>
    /// Move certain columns of the TileMatrix to the display.
    /// </summary>
    /// <param name="j1"> Column from which the columns will be moved </param>
    /// <param name="j2"> Column from which the columns will no longer be moved </param>
    /// <param name="stepX"> Step with which the column will be moved on the X </param>
    /// <param name="stepY"> Step with which the line will be moved on the Y </param>
    public void moveColumnsBetween(int j1, int j2, int stepX, int stepY)
    {
        for (int j = j1; j < j2; j++)
        {
            moveColomnAt(j, stepX, stepY);
        }
    }

    /// <summary>
    /// Adds a line to the end of the TileMatrix.
    /// </summary>
    /// <param name="tileNumber"> Code of the Tile with which to initialize the line </param>
    public void AddLine(int tileNumber)
    {
        _matrice.Add(new List<GameObject>());

        for (int j = 0; j < _width; j++)
        {
            _matrice[_height].Add(CreateTile(_height, j, tileNumber));
        }
        _height++;
    }

    /// <summary>
    /// Adds a column to the end of the TileMatrix.
    /// </summary>
    /// <param name="tileNumber"> Code of the Tile with which to initialize the column </param>
    public void AddColumn(int tileNumber)
    {
        for (int i = 0; i < _height; i++)
        {
            _matrice[i].Add(CreateTile(i, _width, tileNumber));
        }
        _width++;
    }

    /// <summary>
    /// Insert a line in the TileMatrix.
    /// </summary>
    /// <param name="i"> Number of the line after which a new line should be added </param>
    /// <param name="tileNumber"> Code of the Tile with which to initialize the line </param>
    public void insertLigneAt(int i, int tileNumber)
    {
        moveLinesBetween(i, _height - 1, 0, -1);

        _matrice.Insert(i, new List<GameObject>());

        for (int j = 0; j < _width; j++)
        {
            _matrice[i].Add(CreateTile(i, j, tileNumber));
        }

        _height++;
    }

    /// <summary>
    // Insert a column in the Matrix.
    /// </summary>
    /// <param name="j"> Number of the column after which a new column should be added </param>
    /// <param name="tileNumber"> Code of the Tile with which to initialize the column </param>
    public void insertColonneAt(int j, int tileNumber)
    {
        moveColumnsBetween(j, _width - 1, 1, 0);

        for (int i = 0; i < _height; i++)
        {
            _matrice[i].Insert(j, CreateTile(i, j, tileNumber));
        }

        _width++;
    }

    /// <summary>
    /// Destroys a line in the TileMatrix (display).
    /// </summary>
    /// <param name="i"> Number of the line to be destroyed </param>
    private void destroyLine(int i)
    {
        for (int j = 0; j < _width; j++)
        {
            Destroy(this[i, j]);
        }
    }

    /// <summary>
    /// Destroys a column of the TileMatrix (display).
    /// </summary>
    /// <param name="j"> Number of columns to be destroyed </param>
    private void destroyColumn(int j)
    {
        for (int i = 0; i < _height; i++)
        {
            Destroy(this[i, j]);
        }
    }

    /// <summary>
    /// Destroys the entire TileMatrix.
    /// </summary>
    public void destroy()
    {
        while (_height > 0)
        {
            destroyLine(_height - 1);
            _matrice.RemoveAt(_height - 1);
            _height--;
        }
        _width = 0;
    }

    /// <summary>
    /// Deletes a row from the tileMatrix.
    /// </summary>
    /// <param name="i"> Line to be deleted </param>
    public void removeLineAt(int i)
    {
        destroyLine(i);

        _matrice.RemoveAt(i);
        _height--;
        moveLinesBetween(i, _height - 1, 0, -1);
    }

    /// <summary>
    /// Deletes a column from the tileMatrix.
    /// </summary>
    /// <param name="j"> Column to be deleted </param>
    public void removeColumnAt(int j)
    {
        destroyColumn(j);

        for (int i = 0; i < _height; i++)
        {
            _matrice[i].RemoveAt(j);
        }
        _width--;

        moveColumnsBetween(j, _width - 1, -1, 0);
        //moveAllColonneAt(j, -1);
    }




    /// <summary>
    /// Write accessor, changes all tiles in a row.
    /// </summary>
    /// <param name="i"> Line N°i </param>
    /// <param name="tileNumber"> New Tile Code on all line i </param>
    public void setLine(int i, int tileNumber)
    {
        int j = 0;
        while (j < width)
        {
            updateTileAt(i, j, tileNumber);
            j++;
        }
    }

    /// <summary>
    /// Write accessor, changes all tiles in a column.
    /// </summary>
    /// <param name="j"> Column N°j </param>
    /// <param name="tileNumber"> Code new Tiles on the whole column j </param>
    public void setColumn(int j, int tileNumber)
    {
        int i = 0;
        while (i < height)
        {
            updateTileAt(i, j, tileNumber);
            i++;
        }
    }

    /// <summary>
    /// Draw a rectangle consisting of a certain tile.
    /// </summary>
    /// <param name="iMin"> Line where the rectangle will start to be drawn </param>
    /// <param name="iMax"> Line where the rectangle will end up </param>
    /// <param name="jMin"> Column where the rectangle will start to be drawn </param>
    /// <param name="jMax"> Column where the rectangle will end up </param>
    /// <param name="tileCode"> Code of the Tile that will compose the rectangle </param>
    public void drawRectFull(int iMin, int iMax, int jMin, int jMax, int tileCode)
    {
        for (int i = iMin; i < iMax + 1; i++)
        {
            for (int j = jMin; j < jMax + 1; j++)
            {
                updateTileAt(i, j, tileCode);
            }
        }
    }

    /// <summary>
    /// Spin a tile every time you call.
    /// </summary>
    /// <param name="tileCode"> Tile Code </param>
    /// <param name="nbRotationMax"> Maximum possible number of rotations of this tile </param>
    /// <param name="nbRotation"> Number of rotations to be performed </param>
    static int rotateTile(int tileCode, int nbRotationMax, int nbRotation)
    {
        int newTileCode = tileCode;
        for (int i = 0; i < nbRotation; i++)
        {
            newTileCode++;
            if (newTileCode > tileCode + nbRotationMax)
            {
                newTileCode = tileCode;
            }
        }
        return newTileCode;
    }

    /// <summary>
    /// Draws a frame consisting of a certain Tile in the TileMatrix.
    /// </summary>
    /// <param name="iMin"> Line where the frame will start to take shape </param>
    /// <param name="iMax"> Line where the frame will end up </param>
    /// <param name="jMin"> Column where the frame will start to take shape </param>
    /// <param name="jMax"> Column where the frame will end up </param>
    /// <param name="tileCode"> Code of the Tile that will compose the frame </param>
    public void drawRectCorner(int iMin, int iMax, int jMin, int jMax, int tileCode)
    {
        for (int i = iMin; i < iMax + 1; i++)
        {
            for (int j = jMin; j < jMax + 1; j++)
            {
                if (i == iMin || j == jMin || i == iMax || j == jMax)
                {
                    if (/*tileCode == 1 ||*/ tileCode == 11 || tileCode == 21)
                    {
                        int nbMaxR = 0, wall = tileCode;

                        //if (tileCode == 1 ) { wall = 5; nbMaxR = 1;}
                        if (tileCode == 11) { wall = 5; nbMaxR = 1; }
                        if (tileCode == 21) { wall = 25; nbMaxR = 3; }

                        if (i == iMin && j == jMin)
                        {
                            updateTileAt(i, j, tileCode);
                        }
                        else if (i == iMax && j == jMin)
                        {
                            updateTileAt(i, j, tileCode + 3);
                        }
                        else if (i == iMin && j == jMax)
                        {
                            updateTileAt(i, j, tileCode + 1);
                        }
                        else if (i == iMax && j == jMax)
                        {
                            updateTileAt(i, j, tileCode + 2);
                        }
                        else if (i == iMin)
                        {
                            updateTileAt(i, j, rotateTile(wall, nbMaxR, 0));
                        }
                        else if (j == jMin)
                        {
                            updateTileAt(i, j, rotateTile(wall, nbMaxR, 3));
                        }
                        else if (j == jMax)
                        {
                            updateTileAt(i, j, rotateTile(wall, nbMaxR, 1));
                        }
                        else if (i == iMax)
                        {
                            updateTileAt(i, j, rotateTile(wall, nbMaxR, 2));
                        }
                    }
                    else
                    {
                        updateTileAt(i, j, tileCode);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Create an integer list buffer with a rectangular part of the TileMatrix.
    /// </summary>
    /// <param name="iMin"> Line where the TileMatrix will start creating a corresponding buffer </param>
    /// <param name="iMax"> Line where the TileMatrix will finish copying the corresponding buffer </param>
    /// <param name="jMin"> Column where the TileMatrix will start creating a corresponding buffer </param>
    /// <param name="jMax"> Column where the TileMatrix will finish copying the corresponding buffer </param>
    /// <returns> Buffer containing all the values of the selected rectangle as an integer list </returns>
    public List<int> drawRectToBuffer(int iMin, int iMax, int jMin, int jMax)
    {
        List<int> copyBuffer = new List<int>();
        if (checkBounds(iMin, jMin) && checkBounds(iMax, jMax))
        {
            for (int i = iMin; i < iMax + 1; i++)
            {
                for (int j = jMin; j < jMax + 1; j++)
                {
                    copyBuffer.Add(getTileCodeAt(i, j));
                }
            }
        }
        else
        {
            Debug.Log("Error out of bound/rect exit from the matrix");
        }
        return copyBuffer;
    }

    /// <summary>
    /// Copy a rectangle from the TileMatrix to another position in the TileMatrix.
    /// </summary>
    public List<int> copyRectToPos(int iMin, int iMax, int jMin, int jMax, int indI, int indJ)
    {
        int newIMax = indI + iMax - iMin + 1;
        int newJMax = indJ + jMax - jMin + 1;
        List<int> copyBuffer = new List<int>();
        if (checkBounds(indI, indJ) && checkBounds(newIMax - 1, newJMax - 1))
        {
            int index = 0;
            for (int i = iMin; i < iMax + 1; i++)
            {
                for (int j = jMin; j < jMax + 1; j++)
                {
                    copyBuffer.Add(getTileCodeAt(i, j));
                }
            }

            for (int i = indI; i < newIMax; i++)
            {
                for (int j = indJ; j < newJMax; j++)
                {
                    updateTileAt(i, j, copyBuffer[index]);
                    index++;
                }
            }
        }
        else
        {
            Debug.Log("Error out of bound/rect exit from the matrix");
        }
        return copyBuffer;
    }

    /// <summary>
    /// Draw a buffer as an integer list in the TileMatrix
    /// </summary>
    /// <param name="iMin"> Buffer size iMin </param>
    /// <param name="iMax"> Buffer size iMax </param>
    /// <param name="jMin"> Buffer size jMin </param>
    /// <param name="jMax"> Buffer size jMax </param>
    /// <param name="indI"> Line of the TileMatrix where the buffer should be drawn </param>
    /// <param name="indJ"> Column of the TileMatrix where the buffer should be drawn </param>
    /// <param name="copyBuffer"> Buffer to be drawn in the TileMatrix </param>
    /// <returns> Buffer containing all the values of the selected rectangle as an integer list </returns>
    public void drawBufferRectToPos(int iMin, int iMax, int jMin, int jMax, int indI, int indJ, List<int> copyBuffer)
    {
        int newIMax = indI + iMax - iMin + 1;
        int newJMax = indJ + jMax - jMin + 1;
        if (checkBounds(indI, indJ) && checkBounds(newIMax - 1, newJMax - 1))
        {
            int index = 0;

            for (int i = indI; i < newIMax; i++)
            {
                for (int j = indJ; j < newJMax; j++)
                {
                    updateTileAt(i, j, copyBuffer[index]);
                    index++;
                }
            }
        }
        else
        {
            Debug.Log("Error out of bound/rect exit from the matrix");
        }
    }
    /*
    public bool[,] TileMatrixToPathFindableMatrix()
    {
        bool[,] walkable_map = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                walkable_map[x, y] = (getTileCodeAt(y, x) == 0 || getTileCodeAt(y, x) == 10 || getTileCodeAt(y, x) == 36);
            }
        }
        return walkable_map;
    }
    */

    /// <summary>
    /// Checks if the pair i, j is well indexed in the matrix and not outside.
    /// </summary>
    /// <param name="i"> Line of the TileMatrix </param>
    /// <param name="j"> Column of the TileMatrix </param>
    /// <returns> True if the pair i, j is well indexed, otherwise </returns>
    public bool checkBounds(int i, int j)
    {
        return !(i < 0 || i >= _height || j < 0 || j >= _width);
    }


    /// <summary>
    /// Save the entire TileMatrix to a file.
    /// </summary>
    /// <param name="path"> Path where you want to save the TileMatrix </param>
    /// <param name="append"> Boolean determining whether data should be added (true) or overwritten (false) </param>
    public void save(string path, bool append = true)
    {
        if (append)
        {
            StreamWriter fichier = File.AppendText(path);

            string ligne = "";

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ligne += this[i, j].GetComponent<Tile>().code + " ";
                }
                fichier.WriteLine(ligne);
                ligne = "";
            }
            fichier.WriteLine("-");
            fichier.Close();
        }
        else
        {
            string text = "";
            string ligne = "";

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ligne += this[i, j].GetComponent<Tile>().code + " ";
                }
                text += ligne + Environment.NewLine;
                ligne = "";
            }
            text += "-" + Environment.NewLine;
            File.WriteAllText(path, text);
        }
    }

    /// <summary>
    /// Loads a TileMatrix contained in a file.
    /// </summary>
    /// <param name="path"> Path of the file to be loaded </param>
    /// <param name="num"> Number of the matrix to be loaded if the file contains more than one TileMatrix (0 is the base value) </param>                
    public void load(string path, int num = 0)
    {
        List<int> list = new List<int>();
        string ligne = "";
        string[] words;
        int height = 0;
        int width = 0;
        int k = 0;

        StreamReader file = new StreamReader(path);

        for (int i = 0; i < num; i++)
        {
            while (file.ReadLine() != "-") { }
        }

        while ((ligne = file.ReadLine()) != null && ligne != "-")
        {
            width = 0;
            words = ligne.Split();

            for (int i = 0; i < words.Length - 1; i++)
            {
                int val = 0;
                int.TryParse(words[i], out val);
                list.Add(val);
                width++;
            }
            height++;
        }

        destroy();

        _matrice = new List<List<GameObject>>();
        _height = height;
        _width = width;

        for (int i = 0; i < height; i++)
        {
            _matrice.Add(new List<GameObject>());
            for (int j = 0; j < width; j++)
            {
                _matrice[i].Add(CreateTile(i, j, list[k]));
                _matrice[i][j].tag = "wall";
                k++;
            }
        }
        file.Close();
    }
}