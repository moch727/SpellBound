using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.InputSystem;
using NUnit.Framework;
using UnityEngine;



public class NoteManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    [SerializeField] GameObject note;
    [SerializeField] GameObject bar;

    private int[,] plane = new int[3,3]; //plane to use for the notes
    private List<NoteScript> noteList = new List<NoteScript>(3); //List of scripts for every note
    private int currentNote = 0;

    private bool playing = false;

void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createImages(int[,] notePattern)
    {
        bar = Instantiate(bar, transform);
        bar.SetActive(true);

        //for(int i = 0; i < plane.GetLength(0); i++) //Randomization of notes
        //{
        //    int rowToAdd = RandomNumberGenerator.GetInt32(plane.GetLength(1));

        //    GameObject newNote = Instantiate(note, transform);
        //    newNote.SetActive(true);
        //    newNote.transform.Translate(new Vector2(i * 50, -50 * rowToAdd)); //(0,0) is top left
        //    newNote.GetComponent<NoteScript>().setColor(rowToAdd);

        //    noteList.Add(newNote.GetComponent<NoteScript>());
        //}
        plane = notePattern;

        for(int i = 0; i < plane.GetLength(1); i++)
        {
            for(int j = 0; j < plane.GetLength(0); j++)
            {
                if (plane[j, i] == 1)
                {
                    GameObject newNote = Instantiate(note, transform);
                    newNote.SetActive(true);
                    newNote.transform.Translate(new Vector2(i * 50, -50 * j)); //(0,0) is top left
                    newNote.GetComponent<NoteScript>().setColor(j);


                    noteList.Add(newNote.GetComponent<NoteScript>());
                }
            }
        }
    }

    public KeyCode getKey()
    {
        return noteList[currentNote].getKey();
    }
    public bool noteKeyPressed()
    {
        Debug.Log(currentNote);

        if (!playing)
        {
            bar.GetComponent<BarScript>().setDetection(true);

            noteList[currentNote].gameObject.SetActive(false);

            currentNote++;
            playing = true;
            return false;
        }
        else
        {
            if (bar.GetComponent<BarScript>().checkInputTiming(noteList[currentNote].gameObject.transform.position))
            {
                noteList[currentNote].gameObject.SetActive(false);
                currentNote++;
                if (currentNote >= noteList.Count) //all notes have been pressed, return true to initiate attack
                {
                    bar.SetActive(false);
                    return true;
                }
                return false;
            }
            else
            {
                bar.GetComponent<BarScript>().setDetection(false); //reset bar
                bar.transform.position = this.transform.position;
                bar.transform.Translate(new Vector3(-50, 0, 0));


                foreach (var note in noteList)
                {
                    note.gameObject.SetActive(true);
                }

                currentNote = 0;
                playing = false;

                return false;
            }
        }
    }
}
