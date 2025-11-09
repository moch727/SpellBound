using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using System;

public class SpellManager : MonoBehaviour
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


    private int[,] pattern = new int[3,3];
    public int[,] getPatternValues()
    {
        return pattern;
    }

    public SpellScript generateSpell(String spellName)
    {
        SpellScript spell = null;

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

            spell = new SpellScript();
            spell.Instantiate(data[i], 0, 0, pattern); //initialize variables
        }

        return spell;

    }

    private void readCSVFile()
    {
        data = dataFile.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        int dataSize = ((data.Length / numOfColumns) - 2) / 4; //determines amount of rows to go through, 3 is number of data type columns, 
                                                               //2 is the initial row with descriptions, multiply 4 by number of rows
    }
}
