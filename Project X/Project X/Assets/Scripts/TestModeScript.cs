using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class TestModeScript : MonoBehaviour
//{

//}

using System.IO;
using System;

class Unit
{
    public string type;
    public int hp;
    public int dmg;
    public int moveRange;
    public int attackRange;
    public string team;
    public Unit(string typeA, int hpA, int dmgA, int moveRangeA, int attackRangeA, string teamA)
    {
        type = typeA;
        hp = hpA;
        dmg = dmgA;
        moveRange = moveRangeA;
        attackRange = attackRangeA;
        team = teamA;
    }
    public Unit()
    {
        type = "None";
    }
}

class Tile
{
    public string type;
    public Tile(string typeA)
    {
        type = typeA;
    }
}

class Board
{
    public static Tuple<Unit, Tile>[,] createBoard(int i, int j)
    {
        Tuple<Unit, Tile>[,] result = new Tuple<Unit, Tile>[i, j];
        for (int a = 0; a < i; a++)
        {
            for (int b = 0; b < j; b++)
            {
                Unit unit;
                if (a == 0)
                {
                    unit = new Unit("Infantry", 10, 5, 5, 5, "TeamA");
                }
                else if (a == (i - 1))
                {
                    unit = new Unit("Infantry", 10, 5, 5, 5, "TeamB");
                }
                else
                {
                    unit = new Unit();
                }
                Tile tile = new Tile("Normal");
                result[a, b] = Tuple.Create(unit, tile);
            }
        }
        return result;
    }

    public static void printBoard(Tuple<Unit, Tile>[,] board, int i, int j)
    {
        for (int a = 0; a < i; a++)
        {
            for (int b = 0; b < j; b++)
            {
                if (board[a, b].Item1.type == "None")
                {
                    System.Console.Write("- ");
                }
                else if (board[a, b].Item1.team == "TeamA")
                {
                    System.Console.Write("A ");
                }
                else if (board[a, b].Item1.team == "TeamB")
                {
                    System.Console.Write("B ");
                }
            }
            System.Console.WriteLine();
        }
    }

    static void Main()
    {
        Tuple<Unit, Tile>[,] board = createBoard(10, 10);
        printBoard(board, 10, 10);
        System.Console.WriteLine("Make a move for " + "Team A");
        string x = System.Console.ReadLine();
        while (x != "a")
        {
            System.Console.Write("hi");
            x = System.Console.ReadLine();
        }
    }
}