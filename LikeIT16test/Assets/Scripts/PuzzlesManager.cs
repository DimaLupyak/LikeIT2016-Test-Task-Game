using System.Collections.Generic;
using UnityEngine;

class PuzzlesManager
{
    public string[,] Table { get; set; }

    private int houseCount;
    private string[] colours;
    private string[] pets = { "cat", "dog", "zebra", "pikachu", "snake", "pig", "elephant"};
    private string[] drinks = { "water", "beer", "milk", "juice", "coffee", "vodka" };
    private string[] fatherNames = { "Dima", "Slavik", "Max", "Petro", "Ivan", "Panas" };
    private string[] momNames = { "Marina", "Olya", "Natasha", "Sveta", "Anna", "Nastya" };
    public string Target { get; set; }
    public string TargetForGui { get; set; }
    public List<string> GeneratedHints { get; set; }


    public PuzzlesManager(int houseCount) 
    {
        this.houseCount = houseCount;
        GeneratedHints = new List<string>();
		colours = new string[MainController.Instance.houses.Length];
		for (int i = 0; i < MainController.Instance.houses.Length; i++)
			colours[i] = MainController.Instance.houses[i].color;
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
        int row = Random.Range(1, 3);
        int column = Random.Range(0, houseCount);
        Target = Table[row, column];
        switch (row)
        {
            case 1:
                TargetForGui = "Deliver the baby \n in house with " + Table[row, column] + ".";
                    break;
            case 2:
                TargetForGui = "Deliver the baby \n to family, that drink \n " + Table[row, column] + ".";
                break;
            case 3:
                TargetForGui = Table[row, column] + " \n are the parents of baby.";
                break;
        }

		PopUpManager.Instance.SetTargetText(TargetForGui);
		PopUpManager.Instance.OpenPage(PageType.Target);
		if (!PlayerPrefs.HasKey("FIRST"))
		{
			PopUpManager.Instance.OpenPage(PageType.HowToPlay);
			PlayerPrefs.SetInt("FIRST", 0);
		}
    }

    public string GetHint()
    {
        if (GeneratedHints.Count == 6 * houseCount)
        {
            return ("You already have \n the all hints!");
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
                return Table[3, houseNumber] + " drink \n" + Table[2, houseNumber]+".";
            case 1:
                return Table[3, houseNumber] + " live \n in the " + Table[0, houseNumber] + " house.";
            case 2:
                return Table[3, houseNumber] + " own \n the " + Table[1, houseNumber] + ".";
            case 3:
                return "The " + Table[2, houseNumber] + "-drinkers \n own the " + Table[1, houseNumber] + ".";
            case 4:
                return "The " + Table[2, houseNumber] + "-drinkers \n live in the " + Table[0, houseNumber] + " house.";
            case 5:
                return "The couple, \n that own the " + Table[1, houseNumber] + " \n live in the " + Table[0, houseNumber] + " house.";
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
