using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Tuple;
//public class TestModeScript : MonoBehaviour
//{

//}

class Unit
{
    public string type { get; set; }
    public int hp { get; set; }
    public int dmg { get; set; }
    public int moveRange { get; set; }
    public int attackRange { get; set; }
    public string team { get; set; }
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
        hp = 0;
        dmg = 0;
        moveRange = 0;
        attackRange = 0;
        team = "None";
    }
}

class Tile
{
    public string type { get; set; }
    public Tile(string typeA)
    {
        type = typeA;
    }
}

class Board
{
    public static int width;

    public static int height;

    public static string team;

    public static Tuple<Unit, Tile>[,] board;

    public static void createBoard(int i, int j)
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
                result[a, b] = Tuple<Unit, Tile>.Create(unit, tile);
            }
        }
        width = i;
        height = j;
        board = result;
    }

    public static void printBoard()
    {
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < height; b++)
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

    public static List<Tuple<int, int>> selectableUnits()
    {
        List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < height; b++)
            {
                if (board[a, b].Item1.team == team)
                {
                    moves.Add(Tuple<int, int>.Create(a, b));
                }
            }
        }
        return moves;
    }

    public static bool selectUnit(int unitX, int unitY)
    {
        List<Tuple<int, int>> moves = selectableUnits();
        Tuple<int, int> current = Tuple<int, int>.Create(unitX, unitY);
        for (int i = 0; i < moves.Count; i++)
        {
            if (current.Item1 == moves[i].Item1 && current.Item2 == moves[i].Item2)
            {
                return true;
            }
        }
        return false;
    }

    public static void initialTurn(string turn)
    {
        team = turn;
    }

    public static void turnSwitch(string turn)
    {
        if (turn == "TeamA")
        {
            team = "TeamB";
        }
        else if (turn == "TeamB")
        {
            team = "TeamA";
        }
    }

    public static bool moveUnit(int unitX, int unitY, int moveX, int moveY)
    {
        List<Tuple<int, int>> moves = movableSpots(unitX, unitY);
        Tuple<int, int> current = Tuple<int, int>.Create(moveX, moveY);
        for (int i = 0; i < moves.Count; i++)
        {
            if (current.Item1 == moves[i].Item1 && current.Item2 == moves[i].Item2)
            {
                board[moveX, moveY].Item1.type = board[unitX, unitY].Item1.type;
                board[moveX, moveY].Item1.hp = board[unitX, unitY].Item1.hp;
                board[moveX, moveY].Item1.dmg = board[unitX, unitY].Item1.dmg;
                board[moveX, moveY].Item1.moveRange = board[unitX, unitY].Item1.moveRange;
                board[moveX, moveY].Item1.attackRange = board[unitX, unitY].Item1.attackRange;
                board[moveX, moveY].Item1.team = board[unitX, unitY].Item1.team;
                board[unitX, unitY].Item1.type = "None";
                board[unitX, unitY].Item1.hp = 0;
                board[unitX, unitY].Item1.dmg = 0;
                board[unitX, unitY].Item1.moveRange = 0;
                board[unitX, unitY].Item1.attackRange = 0;
                board[unitX, unitY].Item1.team = "None";
                return true;
            }
        }
        return false;
    }

    public static List<Tuple<int, int>> movableSpots(int unitX, int unitY)
    {
        List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < height; b++)
            {
                if (validMove(unitX, unitY, a, b))
                {
                    moves.Add(Tuple<int, int>.Create(a, b));
                }
            }
        }
        return moves;
    }

    public static bool validMove(int unitX, int unitY, int moveX, int moveY)
    {
        int x = moveX - unitX;
        int y = moveY - unitY;
        if (x < 0)
        {
            x *= -1;
        }
        if (y < 0)
        {
            y *= -1;
        }
        if (x + y <= board[unitX, unitY].Item1.moveRange && board[moveX, moveY].Item1.team == "None")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool attackUnit(int unitX, int unitY, int attackX, int attackY)
    {
        List<Tuple<int, int>> moves = attackSpots(unitX, unitY);
        Tuple<int, int> current = Tuple<int, int>.Create(attackX, attackY);
        for (int i = 0; i < moves.Count; i++)
        {
            if (current.Item1 == moves[i].Item1 && current.Item2 == moves[i].Item2)
            {
                int dmg = board[unitX, unitY].Item1.dmg;
                int hp = board[attackX, attackY].Item1.hp;
                if (dmg >= hp)
                {
                    board[attackX, attackY].Item1.type = "None";
                    board[attackX, attackY].Item1.hp = 0;
                    board[attackX, attackY].Item1.dmg = 0;
                    board[attackX, attackY].Item1.moveRange = 0;
                    board[attackX, attackY].Item1.attackRange = 0;
                    board[attackX, attackY].Item1.team = "None";
                }
                else
                {
                    board[attackX, attackY].Item1.hp = board[attackX, attackY].Item1.hp - board[unitX, unitY].Item1.dmg;
                }
                return true;
            }
        }
        return false;
    }

    public static List<Tuple<int, int>> attackSpots(int unitX, int unitY)
    {
        List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < height; b++)
            {
                if (validAttack(unitX, unitY, a, b))
                {
                    moves.Add(Tuple<int, int>.Create(a, b));
                }
            }
        }
        return moves;
    }

    public static bool validAttack(int unitX, int unitY, int attackX, int attackY)
    {
        int x = attackX - unitX;
        int y = attackY - unitY;
        if (x < 0)
        {
            x *= -1;
        }
        if (y < 0)
        {
            y *= -1;
        }
        if (x + y <= board[unitX, unitY].Item1.moveRange && board[attackX, attackY].Item1.team != "None" && board[attackX, attackY].Item1.team != team)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool winCondition()
    {
        bool teamAAlive = false;
        bool teamBAlive = false;
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < height; b++)
            {
                if (board[a, b].Item1.team == "TeamA")
                {
                    teamAAlive = true;
                }
                else if (board[a, b].Item1.team == "TeamB")
                {
                    teamBAlive = true;
                }
            }
        }
        if (teamAAlive && teamBAlive)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}