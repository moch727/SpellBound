using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using System;

public class Spell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    [SerializeField] private TextAsset dataFile;
    [SerializeField] private static string[] data;
    private const int numOfColumns = 3 + 0; //Number of rows ***Due to pattern, it starts at 3,
    private int patternStartIndex;
    private bool spellFound = false;

    private string spellName;

    private float effectPercent;
    private float damage;

    private int[,] pattern = new int[3,3];
    public int[,] getPatternValues()
    {
        return pattern;
    }

    public void generateSpell(String spellName)
    {
        spellFound = false;
        if (data == null)
        {
            readCSVFile();
        }

        int i = 2 * numOfColumns;
        while (i < data.Length)
        {
            if (data[i].Equals(spellName))
            {
                spellName = data[i];
                patternStartIndex = i + numOfColumns;
                spellFound = true;
                break;
            }
            i += numOfColumns * 4;
        }

        if (spellFound)
        {
            for (int j = patternStartIndex; j < patternStartIndex + 9; j++) //Adds linearly the nums from left to right, top to bottom
            {
                int x = (j - patternStartIndex) % numOfColumns; //Fix size here if adding more data types
                int y = (j - patternStartIndex) / numOfColumns;

                this.pattern[y, x] = int.Parse(data[j]);
            }
        }
    }

    private void readCSVFile()
    {
        data = dataFile.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        int dataSize = ((data.Length / numOfColumns) - 2) / 4; //determines amount of rows to go through, 3 is number of data type columns, 
                                                               //2 is the initial row with descriptions, multiply 4 by number of rows
    }
}
