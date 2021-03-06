using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    ///  La class TileMatrix permet de modifier des Matrices de Tuiles ainsi que de les charger/sauvegarder dans des fichiers.
    /// </summary>
    public class TileMatrix : UnityEngine.Object
    {
        /// <summary> Liste de liste (contient les Tuiles).</summary>
        protected List<List<GameObject>> _matrice;
        /// <summary> Hauteur de la TileMatrix.</summary>
        protected int _hauteur;
        /// <summary> Largeur de la TileMatrix.</summary>
        protected int _largeur;
        /// <summary> Position de la TileMatrix.</summary>
        protected Vector2 _position;
        /// <summary> Tableau de sprites qui va contenir toutes les Tuiles à charger.</summary>
        protected Sprite[] _tilesSprites;
        /// <summary> Chemin de la texture contenant les tuiles.</summary>
        protected string _tilesPath;

        /// <summary>
        /// Accesseur en lecture sur la hauteur.
        /// </summary>
        /// <returns> Hauteur de la TileMatrix. </returns>
        public int hauteur { get { return _hauteur; } }

        /// <summary>
        /// Accesseur en lecture sur la largeur.
        /// </summary>
        /// <returns> Largeur de la TileMatrix. </returns>
        public int largeur { get { return _largeur; } }

        /// <summary>
        /// Accesseur en lecture/ecriture sur la position de la TileMatrix.
        /// </summary>
        /// <returns> position de la TileMatrix. </returns>
        public Vector2 position { get { return _position; } set { _position = value; } }

        /// <summary>
        /// Accesseur en lecture sur le nombre de Tuiles disponible avec la texture chargée.
        /// </summary>
        /// <returns> Nombre de tuiles disponibles. </returns>
        public int TilesNumber { get { return _tilesSprites.Length; } }

        /// <summary>
        /// Accesseur en lecture sur le chemin de la texture chargée contenant les tuiles.
        /// </summary>
        /// <returns> Chemin en chaine de caractères. </returns>
        public string TilesPath { get { return _tilesPath; } }

        /// <summary>
        /// Verifie si la TileMatrix est vide (hauteur et largeur nulle).
        /// </summary>
        /// <returns> Vrai si vide faux sinon. </returns>
        public bool isEmpty() { return largeur == 0 && hauteur == 0; }

        /// <summary>
        /// Charge un sprite à partir d'un entier envoie un message d'erreur si le l'entier est trop bas où trop grand
        /// </summary>
        /// <param name="spriteNumber"> Un entier correspondant à un sprite </param>
        /// <returns> Un Sprite correspondant à un entier. </returns>
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
        /// Modifie une Tuile de la TileMatrix avec un entier qui code une Tuile.
        /// </summary>
        /// <param name="i"> Ligne de la TileMatrix </param>
        /// <param name="j"> Colonne de la TileMatrix </param>
        /// <param name="TileCode"> Entier correspondant à une Tuile </param>
        public void updateTileAt(int i, int j, int TileCode)
        {
            this[i, j].GetComponent<SpriteRenderer>().sprite = loadSprite(TileCode);
            this[i, j].GetComponent<Tile>().code = TileCode;
        }

        /// <summary>
        /// Convertie une TileMatrix en une Matrice d'entier.
        /// </summary>
        /// <param name="TM"> TileMatrix à convertir </param>
        /// <returns> Matrice d'entier correspondant à une TileMatrix. </returns>
        static Matrice TileMatrixToMatrice(TileMatrix TM)
        {
            Matrice M = new Matrice(TM.hauteur, TM.largeur);

            for (int i = 0; i < TM.hauteur; i++)
            {
                for (int j = 0; j < TM.largeur; j++)
                {
                    M[i, j] = TM.getTileCodeAt(i, j);
                }
            }
            return M;
        }

        /// <summary>
        /// Creer une Tuile à partir d'un numéro de ligne, de colonne et d'un numero corespondant à un Sprite à charger dans cette Tuile.
        /// </summary>
        /// <param name="i"> Ligne de la TileMatrix </param>
        /// <param name="j"> Colonne de la TileMatrix </param>
        /// <param name="numSprite"> Entier correspondant à un Sprite à charger </param>
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
        /// Constructeur par default.
        /// </summary>
        /// <param name="position"> Position où sera afficher la TileMatrix </param>
        /// <param name="tilesPath"> Chemin de la texture contenant les Tuiles </param>
        public TileMatrix(Vector2 position, string tilesPath)
        {
            _matrice = null;
            _hauteur = 0;
            _largeur = 0;
            _position = position;
            _tilesPath = tilesPath;
            _tilesSprites = Resources.LoadAll<Sprite>(tilesPath);
        }

        /// <summary>
        /// Constructeur avec plus de paramètres.
        /// </summary>
        /// <param name="hauteur"> Nombres de lignes de la TileMatrix </param>
        /// <param name="largeur"> Nombres de collonnes de la TileMatrix </param>
        /// <param name="position"> Position où sera affichée la TileMatrix </param>
        /// <param name="tilesPath"> Chemin de la texture contenant les Tuiles </param>
        /// <param name="tileNumber"> Entier correspondant à une Tuile avec laquel sera initialisée la TileMatrix </param>
        public TileMatrix(int hauteur, int largeur, Vector2 position, string tilesPath, int tileNumber)
        {
            _tilesSprites = Resources.LoadAll<Sprite>(tilesPath);
            _matrice = new List<List<GameObject>>();
            _hauteur = hauteur;
            _largeur = largeur;
            _position = position;
            _tilesPath = tilesPath;

        for (int i = 0; i < hauteur; i++)
            {
                _matrice.Add(new List<GameObject>());

                for (int j = 0; j < largeur; j++)
                {
                    _matrice[i].Add(CreateTile(i, j, tileNumber));
                }
            }
        }

        /// <summary>
        /// Constructeur par copy.
        /// </summary>
        /// <param name="M"> La TileMatrix que l'on veut copier </param>
        public TileMatrix(TileMatrix M)
        {
            copy(M);
        }

        /// <summary>
        /// Copie toute les valeur et le format d'une Matrice M.
        /// </summary>
        /// <param name="M"> La TileMatrix que l'on veut copier </param>
        public void copy(TileMatrix M)
        {
            _matrice = new List<List<GameObject>>();
            _hauteur = M.hauteur;
            _largeur = M.largeur;
            _tilesPath = M.TilesPath;
            _tilesSprites = Resources.LoadAll<Sprite>(_tilesPath);

            for (int i = 0; i < _hauteur; i++)
            {
                _matrice.Add(new List<GameObject>());

                for (int j = 0; j < _largeur; j++)
                {
                    _matrice[i].Add(CreateTile(i, j, M.getTileCodeAt(i, j)));
                }
            }
        }

        /// <summary>
        /// Max
        /// </summary>
        /// <returns> Le Max </returns>
        public double max()
        {
            return Math.Round(Math.Sqrt((this.largeur * this.largeur) + (this.hauteur * this.hauteur))) + 2;
        }

        /// <summary>
        /// Indexeur de la TileMatrix permet un acces au données en lecture et en ecriture.
        /// </summary>
        /// <param name="i"> Ligne N°i </param>
        /// <param name="j"> Colonne N°j </param>
        /// <returns> La Tuile que la matrice contiens à la ligne i et la colonne j </returns>
        public GameObject this[int i, int j]
        {
            get
            {
                if (i < 0 || i >= _hauteur || j < 0 || j >= _largeur)
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
                if (i < 0 || i >= _hauteur || j < 0 || j >= _largeur)
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
        /// Accesseur en lecture sur L'entier qui code la Tuile à la ligne i et la colonne j de la TileMatrix.
        /// </summary>
        /// <param name="i"> Ligne N°i </param>
        /// <param name="j"> Colonne N°j </param>
        /// <returns> Le code de la Tuile à la ligne i et la colonne j de la TileMatrix </returns>
        public int getTileCodeAt(int i, int j)
        {
            if (i < 0 || i >= _hauteur || j < 0 || j >= _largeur)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return this[i, j].GetComponent<Tile>().code;
            }
        }

        /// <summary>
        /// Modifie le code de la Tuile à la ligne i et la colonne j de la TileMatrix.
        /// </summary>
        /// <param name="i"> Ligne N°i </param>
        /// <param name="j"> Colonne N°j </param>
        /// <param name="val"> Correspond à la val à changer</param>
        public void setTileCodeAt(int i, int j, int val)
        {
            if (i < 0 || i >= _hauteur || j < 0 || j >= _largeur)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                this[i, j].GetComponent<Tile>().code = val;
            }
        }

        /// <summary>
        /// Deplace une ligne de la TileMatrix à l'affichage.
        /// </summary>
        /// <param name="i"> Ligne à deplacer </param>
        /// <param name="stepX"> Pas avec lequel la ligne sera deplacé sur les X</param>
        /// <param name="stepY"> Pas avec lequel la ligne sera deplacé sur les Y </param>
        public void moveLineAt(int i, int stepX, int stepY)
        {
            for (int j = 0; j < _largeur; j++)
            {
                this[i, j].GetComponent<Tile>().move(stepX, stepY);
            }
        }

        /// <summary>
        /// Deplace une colonne de la TileMatrix à l'affichage.
        /// </summary>
        /// <param name="j"> Colonne à deplacer </param>
        /// <param name="stepX"> Pas avec lequel la colonne sera deplacé sur les X</param>
        /// <param name="stepY"> Pas avec lequel la colonne sera deplacé sur les Y </param>
        public void moveColomnAt(int j, int stepX, int stepY)
        {
            for (int i = 0; i < _hauteur; i++)
            {
                this[i, j].GetComponent<Tile>().move(stepX, stepY);
            }
        }

        /// <summary>
        /// Deplace certaine lignes de la TileMatrix à l'affichage.
        /// </summary>
        /// <param name="i1"> Ligne à partir de laquel les lignes seront deplacées </param>
        /// <param name="i2"> Ligne à partir de laquel les lignes ne seront plus deplacées </param>
        /// <param name="stepX"> Pas avec lequel la ligne sera deplacé sur les X</param>
        /// <param name="stepY"> Pas avec lequel la ligne sera deplacé sur les Y </param>
        public void moveLinesBetween(int i1, int i2, int stepX, int stepY)
        {
            for (int i = i1; i < i2; i++)
            {
                moveLineAt(i, stepX, stepY);
            }
        }

        /// <summary>
        /// Deplace certaine colonnes de la TileMatrix à l'affichage.
        /// </summary>
        /// <param name="j1"> Colonne à partir de laquel les colonnes seront deplacées </param>
        /// <param name="j2"> Colonne à partir de laquel les colonnes ne seront plus deplacées </param>
        /// <param name="stepX"> Pas avec lequel la colonne sera deplacé sur les X</param>
        /// <param name="stepY"> Pas avec lequel la colonne sera deplacé sur les Y </param>
        public void moveColumnsBetween(int j1, int j2, int stepX, int stepY)
        {
            for (int j = j1; j < j2; j++)
            {
                moveColomnAt(j, stepX, stepY);
            }
        }

        /// <summary>
        /// Ajoute une ligne à la fin de la TileMatrix.
        /// </summary>
        /// <param name="tileNumber"> Code de la Tuile avec laquel sera initialiser la ligne </param>
        public void AddLine(int tileNumber)
        {
            _matrice.Add(new List<GameObject>());

            for (int j = 0; j < _largeur; j++)
            {
                _matrice[_hauteur].Add(CreateTile(_hauteur, j, tileNumber));
            }
            _hauteur++;
        }

        /// <summary>
        /// Ajoute une colonne à la fin de la TileMatrix.
        /// </summary>
        /// <param name="tileNumber"> Code de la Tuile avec laquel sera initialiser la colonne </param>
        public void AddColumn(int tileNumber)
        {
            for (int i = 0; i < _hauteur; i++)
            {
                _matrice[i].Add(CreateTile(i, _largeur, tileNumber));
            }
            _largeur++;
        }

        /// <summary>
        /// Insert une ligne dans la TileMatrix.
        /// </summary>
        /// <param name="i"> Numero de la ligne après laquel doit être ajoutée ajouter une nouvelle ligne </param>
        /// <param name="tileNumber"> Code de la Tuile avec laquel sera initialiser la ligne </param>
        public void insertLigneAt(int i, int tileNumber)
        {
            moveLinesBetween(i, _hauteur - 1, 0, -1);

            _matrice.Insert(i, new List<GameObject>());

            for (int j = 0; j < _largeur; j++)
            {
                _matrice[i].Add(CreateTile(i, j, tileNumber));
            }

            _hauteur++;
        }

        /// <summary>
        /// Insert une colonne dans la Matrice.
        /// </summary>
        /// <param name="j"> Numero de la colonne après laquel doit être ajoutée une nouvelle colonne </param>
        /// <param name="tileNumber"> Code de la Tuile avec laquel sera initialiser colonne </param>
        public void insertColonneAt(int j, int tileNumber)
        {
            moveColumnsBetween(j, _largeur - 1, 1, 0);

            for (int i = 0; i < _hauteur; i++)
            {
                _matrice[i].Insert(j, CreateTile(i, j, tileNumber));
            }

            _largeur++;
        }

        /// <summary>
        /// Detruit une ligne de la TileMatrix (affichage).
        /// </summary>
        /// <param name="i"> Numero de la ligne à detruire </param>
        private void destroyLine(int i)
        {
            for (int j = 0; j < _largeur; j++)
            {
                Destroy(this[i, j]);
            }
        }

        /// <summary>
        /// Detruit une colonne de la TileMatrix (affichage).
        /// </summary>
        /// <param name="j"> Numero de la colonne à detruire </param>
        private void destroyColonne(int j)
        {
            for (int i = 0; i < _hauteur; i++)
            {
                Destroy(this[i, j]);
            }
        }

        /// <summary>
        /// Detruit entierement la TileMatrix.
        /// </summary>
        public void destroy()
        {
            while (_hauteur > 0)
            {
                destroyLine(_hauteur - 1);
                _matrice.RemoveAt(_hauteur - 1);
                _hauteur--;
            }
            _largeur = 0;
        }

        /// <summary>
        /// Supprime une ligne de la tileMatrix.
        /// </summary>
        /// <param name="i"> Ligne à supprimer </param>
        public void removeLigneAt(int i)
        {
            destroyLine(i);

            _matrice.RemoveAt(i);
            _hauteur--;
            moveLinesBetween(i, _hauteur - 1, 0, -1);
        }

        /// <summary>
        /// Supprime une colonne de la tileMatrix.
        /// </summary>
        /// <param name="j"> Colonne à supprimer </param>
        public void removeColonneAt(int j)
        {
            destroyColonne(j);

            for (int i = 0; i < _hauteur; i++)
            {
                _matrice[i].RemoveAt(j);
            }
            _largeur--;

            moveColumnsBetween(j, _largeur - 1, -1, 0);
            //moveAllColonneAt(j, -1);
        }




        /// <summary>
        /// Accesseur en ecriture, change toute les Tuiles d'une ligne.
        /// </summary>
        /// <param name="i"> Ligne N°i </param>
        /// <param name="tileNumber"> Code des nouvelle Tuiles sur toute la ligne i </param>
        public void setLigne(int i, int tileNumber)
        {
            int j = 0;
            while (j < largeur)
            {
                updateTileAt(i, j, tileNumber);
                j++;
            }
        }

        /// <summary>
        /// Accesseur en ecriture, change toute les Tuiles d'une colonne.
        /// </summary>
        /// <param name="j"> Colonne N°j </param>
        /// <param name="tileNumber"> Code des nouvelles Tuiles sur toute la colonne j </param>
        public void setColonne(int j, int tileNumber)
        {
            int i = 0;
            while (i < hauteur)
            {
                updateTileAt(i, j, tileNumber);
                i++;
            }
        }

        /// <summary>
        /// Dessine un rectangle composé d'une certaine Tuile.
        /// </summary>
        /// <param name="iMin"> Ligne où le rectangle commencera à se dessiner </param>
        /// <param name="iMax"> Ligne où le rectangle finira de se dessiner </param>
        /// <param name="jMin"> Colonne où le rectangle commencera à se dessiner </param>
        /// <param name="jMax"> Colonne où le rectangle finira de se dessiner </param>
        /// <param name="tileCode"> Code de la Tuile qui composera le rectangle </param>
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
        /// Fais tourner une Tuile à chaques appels.
        /// </summary>
        /// <param name="tileCode"> Code de la Tuile </param>
        /// <param name="nbRotationMax"> Nombre de Rotation possible maximal de cette Tuile </param>
        /// <param name="nbRotation"> Nombre de rotation à executé </param>
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
        /// Dessine un cadre composé d'une certaine Tuile dans la TileMatrix.
        /// </summary>
        /// <param name="iMin"> Ligne où le cadre commencera à se dessiner </param>
        /// <param name="iMax"> Ligne où le cadre finira de se dessiner </param>
        /// <param name="jMin"> Colonne où le cadre commencera à se dessiner </param>
        /// <param name="jMax"> Colonne où le cadre finira de se dessiner </param>
        /// <param name="tileCode"> Code de la Tuile qui composera le cadre </param>
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
                            int nbMaxR = 0, mur = tileCode;

                            //if (tileCode == 1 ) { mur = 5; nbMaxR = 1;}
                            if (tileCode == 11) { mur = 5; nbMaxR = 1; }
                            if (tileCode == 21) { mur = 25; nbMaxR = 3; }

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
                                updateTileAt(i, j, rotateTile(mur, nbMaxR, 0));
                            }
                            else if (j == jMin)
                            {
                                updateTileAt(i, j, rotateTile(mur, nbMaxR, 3));
                            }
                            else if (j == jMax)
                            {
                                updateTileAt(i, j, rotateTile(mur, nbMaxR, 1));
                            }
                            else if (i == iMax)
                            {
                                updateTileAt(i, j, rotateTile(mur, nbMaxR, 2));
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
        /// Creer un buffer sous forme de list d'entier avec une partie rectangulaire de la TileMatrix.
        /// </summary>
        /// <param name="iMin"> Ligne où la TileMatrix commencera à creer un buffer correspondant </param>
        /// <param name="iMax"> Ligne où la TileMatrix finira la copie du buffer correspondant </param>
        /// <param name="jMin"> Colonne où la TileMatrix commencera à creer un buffer correspondant </param>
        /// <param name="jMax"> Colonne où la TileMatrix finira la copie du buffer correspondant </param>
        /// <returns> Buffer contenant toute les valeurs du rectangle selectionner sous forme de List d'entier </returns>
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
                Debug.Log("Erreur out of bound/rect sort de la matrice");
            }
            return copyBuffer;
        }

        /// <summary>
        /// Copy une rectangle de la TileMatrix à une autre position de la TileMatrix.
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
                Debug.Log("Erreur out of bound/rect sort de la matrice");
            }
            return copyBuffer;
        }

        /// <summary>
        /// Dessine un buffer sous forme de list d'entier dans la TileMatrix
        /// </summary>
        /// <param name="iMin"> Dimension du buffer iMin </param>
        /// <param name="iMax"> Dimension du buffer iMax </param>
        /// <param name="jMin"> Dimension du buffer jMin </param>
        /// <param name="jMax"> Dimension du buffer jMax </param>
        /// <param name="indI"> Ligne de la TileMatrix où le buffer doit être dessiné </param>
        /// <param name="indJ"> Colonne de la TileMatrix où le buffer doit être dessiné </param>
        /// <param name="copyBuffer"> Buffer à dessiner dans la TileMatrix </param>
        /// <returns> Buffer contenant toute les valeurs du rectangle selectionner sous forme de List d'entier </returns>
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
                Debug.Log("Erreur out of bound/rect sort de la matrice");
            }
        }
        /*
        public bool[,] TileMatrixToPathFindableMatrix()
        {
            bool[,] walkable_map = new bool[largeur, hauteur];

            for (int x = 0; x < largeur; x++)
            {
                for (int y = 0; y < hauteur; y++)
                {
                    walkable_map[x, y] = (getTileCodeAt(y, x) == 0 || getTileCodeAt(y, x) == 10 || getTileCodeAt(y, x) == 36);
                }
            }
            return walkable_map;
        }
        */

        /// <summary>
        /// Verifie si le couple i, j est bien indexé dans la matrice et pas en dehors.
        /// </summary>
        /// <param name="i"> Ligne de la TileMatrix </param>
        /// <param name="j"> Colonne de la TileMatrix </param>
        /// <returns> Vrai si le couple i, j est bien indexé, faut sinon </returns>
        public bool checkBounds(int i, int j)
        {
            return !(i < 0 || i >= _hauteur || j < 0 || j >= _largeur);
        }


        /// <summary>
        /// Sauvegarde toute la TileMatrix dans un fichier.
        /// </summary>
        /// <param name="chemin"> Chemin où l'on veut sauvegarder la TileMatrix </param>
        /// <param name="append"> Booléen determinant si les donnés doivent êtres ajouter (vrai) où ecraser (false) </param>
        public void save(string chemin, bool append = true)
        {
            if (append)
            {
                StreamWriter fichier = File.AppendText(chemin);

                string ligne = "";

                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
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

                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        ligne += this[i, j].GetComponent<Tile>().code + " ";
                    }
                    text += ligne + Environment.NewLine;
                    ligne = "";
                }
                text += "-" + Environment.NewLine;
                File.WriteAllText(chemin, text);
            }
        }

        /// <summary>
        /// Charge une TileMatrix contenu dans un fichier.
        /// </summary>
        /// <param name="chemin"> Chemin du fichier que l'on veut charger </param>
        /// <param name="num"> Numero de la matrice que l'on veut charger si le fichier contient plus d'une TileMatrix (0 est la valeur de base) </param>                
        public void load(string chemin, int num = 0)
        {
            List<int> list = new List<int>();
            string ligne = "";
            string[] words;
            int hauteur = 0;
            int largeur = 0;
            int k = 0;

            StreamReader fichier = new StreamReader(chemin);

            for (int i = 0; i < num; i++)
            {
                while (fichier.ReadLine() != "-") { }
            }

            while ((ligne = fichier.ReadLine()) != null && ligne != "-")
            {
                largeur = 0;
                words = ligne.Split();

                for (int i = 0; i < words.Length - 1; i++)
                {
                    int val = 0;
                    int.TryParse(words[i], out val);
                    list.Add(val);
                    largeur++;
                }
                hauteur++;
            }

            destroy();

            _matrice = new List<List<GameObject>>();
            _hauteur = hauteur;
            _largeur = largeur;

            for (int i = 0; i < hauteur; i++)
            {
                _matrice.Add(new List<GameObject>());
                for (int j = 0; j < largeur; j++)
                {
                    _matrice[i].Add(CreateTile(i, j, list[k]));
					_matrice [i] [j].tag = "mur";
                    k++;
                }
            }
            fichier.Close();
        }
    }