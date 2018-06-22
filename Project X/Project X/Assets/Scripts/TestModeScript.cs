using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

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
                result[a, b] = Tuple.Create(unit, tile);
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
                    moves.Add(Tuple.Create(a, b));
                }
            }
        }
        return moves;
    }

    public static bool selectUnit(int unitX, int unitY)
    {
        List<Tuple<int, int>> moves = selectableUnits();
        Tuple<int, int> current = Tuple.Create(unitX, unitY);
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
        Tuple<int, int> current = Tuple.Create(moveX, moveY);
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
                    moves.Add(Tuple.Create(a, b));
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
        Tuple<int, int> current = Tuple.Create(attackX, attackY);
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
                    moves.Add(Tuple.Create(a, b));
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

    static void Main()
    {
        System.Console.WriteLine("This game is setup with a board of 10 units on two sides, TeamA (top- represented by A) and TeamB (bottom- represented by B). The units are all the same for now but can be different later on.");
        System.Console.WriteLine("Each unit can be selected, moved (and attack after), attack stationary, or end the turn immediately (END). Game ends when all units on one side die.");
        createBoard(10, 10);
        initialTurn("TeamA");
        string x;
        while (!winCondition())
        {
            printBoard();
            System.Console.WriteLine("Select a unit for " + team);
            x = System.Console.ReadLine();
            if (x == "END")
            {
                System.Console.WriteLine("Turn Ended for " + team);
                turnSwitch(team);
                continue;
            }
            string[] spliters = new string[] { " " };
            string[] read = x.Split(spliters, StringSplitOptions.RemoveEmptyEntries);
            if (read.Length < 2)
            {
                break;
            }
            int i = Int32.Parse(read[0]);
            int j = Int32.Parse(read[1]);
            bool selected = selectUnit(i, j);

            if (!selected)
            {
                System.Console.WriteLine("Unit does not exist there/is on the wrong side!");
            }

            else if (selected)
            {
                System.Console.WriteLine("For the valid unit, do you want to move (M) (and choose to attack after) or attack stationary (A)?");
                x = System.Console.ReadLine();

                if (x == "M")
                {
                    bool moved = false;
                    while (!moved)
                    {
                        System.Console.WriteLine("Select a move for " + team);
                        x = System.Console.ReadLine();
                        string[] read1 = x.Split(spliters, StringSplitOptions.RemoveEmptyEntries);
                        if (x == "END")
                        {
                            System.Console.WriteLine("Turn Ended for " + team);
                            turnSwitch(team);
                            break;
                        }
                        if (read1.Length < 2)
                        {
                            break;
                        }
                        int a = Int32.Parse(read1[0]);
                        int b = Int32.Parse(read1[1]);
                        moved = moveUnit(i, j, a, b);
                    }

                    bool attacked = false;
                    while (!attacked && moved)
                    {
                        System.Console.WriteLine("Select a unit to attack for " + team);
                        x = System.Console.ReadLine();
                        string[] read2 = x.Split(spliters, StringSplitOptions.RemoveEmptyEntries);
                        if (x == "END")
                        {
                            System.Console.WriteLine("Turn Ended for " + team);
                            turnSwitch(team);
                            break;
                        }
                        if (read2.Length < 2)
                        {
                            break;
                        }
                        int c = Int32.Parse(read2[0]);
                        int d = Int32.Parse(read2[1]);
                        attacked = attackUnit(i, j, c, d);
                        if (attacked)
                        {
                            turnSwitch(team);
                        }
                    }
                }

                else if (x == "A")
                {
                    bool attacked = false;
                    while (!attacked)
                    {
                        System.Console.WriteLine("Select a unit to attack for " + team);
                        x = System.Console.ReadLine();
                        string[] read2 = x.Split(spliters, StringSplitOptions.RemoveEmptyEntries);
                        if (x == "END")
                        {
                            System.Console.WriteLine("Turn Ended for " + team);
                            turnSwitch(team);
                            break;
                        }
                        if (read2.Length < 2)
                        {
                            break;
                        }
                        int c = Int32.Parse(read2[0]);
                        int d = Int32.Parse(read2[1]);
                        attacked = attackUnit(i, j, c, d);
                        if (attacked)
                        {
                            turnSwitch(team);
                        }
                    }
                }
            }
        }
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
        if (!teamAAlive && teamBAlive)
        {
            System.Console.WriteLine("TeamB Won!");
        }
        else if (teamAAlive && !teamBAlive)
        {
            System.Console.WriteLine("TeamA Won!");
        }
    }
}