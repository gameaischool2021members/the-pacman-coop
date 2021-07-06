using System;
using UnityEngine;
/// <summary>
///  class for manipulating the IA of ghosts.
/// </summary>
public class phantomes : objetMobile
{
    /// <summary>
    ///  distance variables for the three possible positions of the ghost.
    /// </summary>
    protected double _distance1;
    protected double _distance2;
    protected double _distance3;
    protected int _orientation;

    /// <summary>
    ///  Constructors.
    /// </summary>
    public phantomes() : base()
    {
    }

    public phantomes(float x, float y, string direction) : base(x, y, direction)
    {
    }

    public void setDistance1(double d)
    {
        _distance1 = d;
    }
    public void setDistance2(double d)
    {
        _distance2 = d;
    }
    public void setDistance3(double d)
    {
        _distance3 = d;
    }
    public void setOrientation(int d)
    {
        _orientation = d;
    }

    public double getDistance1()
    {
        return _distance1;
    }
    public double getDistance2()
    {
        return _distance2;
    }
    public double getDistance3()
    {
        return _distance3;
    }
    public double getOrientation()
    {
        return _orientation;
    }


    /// <summary>
    /// returns a value corresponding to either the distance between pacman and the position given to the function or the largest possible value if the given position is a wall.
    /// </summary>
    public double huntP(double xp, double yp, float xf, float yf, TileMatrix M)
    {
        RaycastHit2D rayhit;
        rayhit = Physics2D.Raycast(new Vector2(xf, yf), new Vector2(xf, yf), 0.1f);
        if (rayhit && rayhit.collider.tag == "mur")
        {

            return M.max();

        }
        else
        {

            return Math.Sqrt(((xp - xf) * (xp - xf)) + ((yp - yf) * (yp - yf)));
        }
    }
    /// <summary>
    ///  return the largest of three values (1,2,3) or 4 if they are all equal.
    /// </summary>
    public int Test(double distance1, double distance2, double distance3, TileMatrix M)
    {
        if (distance1 <= distance2 && distance1 <= distance3 && distance1 != M.max())
        {
            return 1;
        }
        else
        {
            if (distance2 <= distance1 && distance2 <= distance3 && distance2 != M.max())
            {
                return 2;
            }
            else
            {
                if (distance3 <= distance1 && distance3 <= distance2 && distance3 != M.max())
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }
        }
    }
    /// <summary>
    ///  this function makes the link between all the above functions and allows to find the shortest path between the ghost (variable ghost) and the desired position (variable pacman which is a position).
    /// </summary>
    public void findR(Transform ghost, Vector2 pacman, TileMatrix M)
    {

        /// <summary>
        ///  if there is no direction to the ghost (it is immobile) then calculate the distance between its next possible position and pacman
        /// </summary>
        if (this.getDirection() == "")
        {
            /// <summary>
            /// up
            /// </summary>
            this.setDistance1(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) + 0.24f, M));
            /// <summary>
            ///  right
            /// </summary>
            this.setDistance2(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x) + 0.24f, (ghost.transform.position.y), M));
            /// <summary>
            ///  down
            /// </summary>
            this.setDistance3(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) - 0.24f, M));

            /// <summary>
            ///  determines the shortest distance
            /// </summary>
            this.setOrientation(this.Test(this.getDistance1(), this.getDistance2(), this.getDistance3(), M));
            /// <summary>
            /// Give the direction found to the ghost.
            /// </summary>
            if (this.getOrientation() == 1)
            {
                this.setDirection("up");
            }
            if (this.getOrientation() == 2)
            {
                this.setDirection("right");
            }
            if (this.getOrientation() == 3)
            {
                this.setDirection("down");
            }
            if (this.getOrientation() == 4)
            {
                this.setDirection("left");
            }
        }
        else
        {
            /// <summary>
            ///  if the direction is equal to right of the ghost (that it is immobile) then calculate the distance between its next possible position and pacman
            /// </summary>
            if (this.getDirection() == "right")
            {
                /// <summary>
                ///  up
                /// </summary>
                this.setDistance1(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) + 0.24f, M));
                /// <summary>
                ///  right
                /// </summary>
                this.setDistance2(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x) + 0.24f, (ghost.transform.position.y), M));
                /// <summary>
                ///  down
                /// </summary>
                this.setDistance3(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) - 0.24f, M));

                /// <summary>
                ///  determines the shortest distance
                /// </summary>
                this.setOrientation(this.Test(this.getDistance1(), this.getDistance2(), this.getDistance3(), M));

                /// <summary>
                ///  Give the direction found to the ghost.
                /// </summary>
                if (this.getOrientation() == 1)
                {
                    this.setDirection("up");
                }
                if (this.getOrientation() == 2)
                {
                    this.setDirection("right");
                }
                if (this.getOrientation() == 3)
                {
                    this.setDirection("down");
                }
                if (this.getOrientation() == 4)
                {
                    this.setDirection("left");
                }
            }
            else
            {

                /// <summary>
                /// if the direction is equal to down of the ghost (that it is immobile) then calculate the distance between its next possible position and pacman
                /// </summary>
                if (this.getDirection() == "down")
                {
                    /// <summary>
                    ///  right
                    /// </summary>
                    this.setDistance1(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x) + 0.24f, (ghost.transform.position.y), M));
                    /// <summary>
                    ///  down
                    /// </summary>
                    this.setDistance2(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) - 0.24f, M));
                    /// <summary>
                    ///  left
                    /// </summary>
                    this.setDistance3(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x) - 0.24f, (ghost.transform.position.y), M));

                    /// <summary>
                    ///  determines the shortest distance
                    /// </summary>
                    this.setOrientation(this.Test(this.getDistance1(), this.getDistance2(), this.getDistance3(), M));

                    /// <summary>
                    ///  Give the direction found to the ghost.
                    /// </summary>
                    if (this.getOrientation() == 1)
                    {
                        this.setDirection("right");
                    }
                    if (this.getOrientation() == 2)
                    {
                        this.setDirection("down");
                    }
                    if (this.getOrientation() == 3)
                    {
                        this.setDirection("left");
                    }
                    if (this.getOrientation() == 4)
                    {
                        this.setDirection("up");
                    }
                }
                else
                {

                    /// <summary>
                    /// if the direction is equal to left of the ghost (that it is immobile) then calculate the distance between its next possible position and pacman
                    /// </summary>
                    if (this.getDirection() == "left")
                    {

                        /// <summary>
                        ///  down
                        /// </summary>
                        this.setDistance1(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) - 0.24f, M));

                        /// <summary>
                        ///  left
                        /// </summary>
                        this.setDistance2(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x) - 0.24f, (ghost.transform.position.y), M));

                        /// <summary>
                        ///  up
                        /// </summary>
                        this.setDistance3(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) + 0.24f, M));

                        /// <summary>
                        ///  determines the shortest distance
                        /// </summary>
                        this.setOrientation(this.Test(this.getDistance1(), this.getDistance2(), this.getDistance3(), M));

                        /// <summary>
                        ///  Give the direction found to the ghost.
                        /// </summary>
                        if (this.getOrientation() == 1)
                        {
                            this.setDirection("down");
                        }
                        if (this.getOrientation() == 2)
                        {
                            this.setDirection("left");
                        }
                        if (this.getOrientation() == 3)
                        {
                            this.setDirection("up");
                        }
                        if (this.getOrientation() == 4)
                        {
                            this.setDirection("right");
                        }
                    }
                    else
                    {

                        /// <summary>
                        /// if the direction is equal to up of the ghost (that it is immobile) then calculate the distance between its next possible position and pacman
                        /// </summary>
                        if (this.getDirection() == "up")
                        {

                            /// <summary>
                            ///  left
                            /// </summary>
                            this.setDistance1(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x) - 0.24f, (ghost.transform.position.y), M));

                            /// <summary>
                            ///  up
                            /// </summary>
                            this.setDistance2(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x), (ghost.transform.position.y) + 0.24f, M));

                            /// <summary>
                            ///  right
                            /// </summary>
                            this.setDistance3(this.huntP((pacman.x), (pacman.y), (ghost.transform.position.x) + 0.24f, (ghost.transform.position.y), M));

                            /// <summary>
                            ///  determines the shortest distance
                            /// </summary>
                            this.setOrientation(this.Test(this.getDistance1(), this.getDistance2(), this.getDistance3(), M));

                            /// <summary>
                            ///  Give the direction found to the ghost.
                            /// </summary>
                            if (this.getOrientation() == 1)
                            {
                                this.setDirection("left");
                            }
                            if (this.getOrientation() == 2)
                            {
                                this.setDirection("up");
                            }
                            if (this.getOrientation() == 3)
                            {
                                this.setDirection("right");
                            }
                            if (this.getOrientation() == 4)
                            {
                                this.setDirection("down");
                            }
                        }
                    }
                }
            }
        }
    }
}



