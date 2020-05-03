using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public bool canMove = false;
    public int grandma = 0;
    public int school = 0;
    public int obstacle = 0;
    public int moves = 0;
    public Movement() { }
    public Movement(Movement mov)
    {
        this.canMove = mov.canMove;
        this.obstacle += mov.obstacle;
        this.school += mov.school;
        this.grandma += mov.grandma;
        this.moves += mov.moves;
    }
}
