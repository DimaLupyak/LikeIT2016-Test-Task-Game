using System.Collections.Generic;
using UnityEngine;

class PuzzlesManager
{
    public string[,] Table { get; set; }

    private int houseCount;
    private string[] colours = { "white", "blue", "red", "green", "yellow", "pink", "grey", "black" };
    private string[] pets = { "cat", "dog", "zebra", "lion", "snake" };
    private string[] drinks = { "water", "beer", "milk", "juice", "coffee" };
    private string[] fatherNames = { "Dima", "Slavik", "Max", "Bob", "Leo" };
    private string[] momNames = { "Marina", "Olya", "Natasha", "Sveta", "Marry" };
    public string Target { get; set; }
    public List<string> GeneratedHints { get; set; }

    public PuzzlesManager(int houseCount)
    {

        this.houseCount = houseCount;
        GeneratedHints = new List<string>();
        ShuffleArray(pets);
        ShuffleArray(drinks);
        ShuffleArray(fatherNames);
        ShuffleArray(momNames);
        Table = new string[4, houseCount];
        for (int i = 0; i < houseCount; i++)
        {
            Table[0, i] = colours[i];
            Table[1, i] = pets[i];
            Table[2, i] = drinks[i];
            Table[3, i] = fatherNames[i] + " and " + momNames[i];
        }
        LogTable();
        Target = Table[Random.Range(1, 3), Random.Range(0, houseCount)];
    }

    public string GetHint()
    {
        if (GeneratedHints.Count == 6 * houseCount)
        {
            return ("You already have the all hints");
        }
        string newHint;

        newGeneration:
        newHint = GenerateHint();
        foreach (string hint in GeneratedHints)
        {
            if (hint == newHint) goto newGeneration;
        }
        GeneratedHints.Add(newHint);
        return newHint;
    }


    private string GenerateHint()
    {
        int hintType = Random.Range(0, 6);
        int houseNumber = Random.Range(0, houseCount);
        switch (hintType)
        {
            case 0:
                return Table[3, houseNumber] + " drink " + Table[2, houseNumber];
            case 1:
                return Table[3, houseNumber] + " live in the " + Table[0, houseNumber] + " house";
            case 2:
                return Table[3, houseNumber] + " own the " + Table[1, houseNumber];
            case 3:
                return "The " + Table[2, houseNumber] + "-drinkers own the " + Table[1, houseNumber];
            case 4:
                return "The " + Table[2, houseNumber] + "-drinkers live in the " + Table[0, houseNumber] + " house";
            case 5:
                return "The couple, that own the " + Table[1, houseNumber] + " live in the " + Table[0, houseNumber] + " house";
        }
        return "";
    }

    public int GetIndex(string str)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < houseCount; j++)
            {
                if (Table[i, j] == str) return j;
            }
        }
        return -1;
    }

    private void LogTable()
    {
        for (int i = 0; i < houseCount; i++)
        {
            Debug.Log(Table[0, i] + " " + Table[1, i] + " " + Table[2, i] + " " + Table[3, i]);
        }
    }

    private void ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            T temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    
}
