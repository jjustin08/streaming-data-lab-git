
/*
This RPG data streaming assignment was created by Fernando Restituto with 
pixel RPG characters created by Sean Browning.
*/

using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1


static public class dataSignifiers
{
    public const int CharacterStats = 0;
    public const int Equipment = 1;
}


static public class AssignmentPart1
{
    const char sepchar = ',';

    static public void SavePartyButtonPressed(string fileName)
    {
        
        LinkedList<string> seralizedData = SeralizePartyData();

        #region saving to file

        StreamWriter writer = new StreamWriter(Application.dataPath + fileName + ".txt");

        foreach (string line in seralizedData)
        { 
            writer.WriteLine(line);
        }

        writer.Close();

        #endregion

    }


    static public void LoadPartyButtonPressed(string fileName)
    {
        GameContent.partyCharacters.Clear();

        LinkedList<string> seralizedData = new LinkedList<string>();


        #region load data from file

        StreamReader reader = new StreamReader(Application.dataPath + fileName + ".txt");
        while(!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            seralizedData.AddLast(line);

        }

        reader.Close();

        #endregion


        DeseralizeData(seralizedData);
        

        GameContent.RefreshUI();
    }
    static public LinkedList<string> SeralizePartyData()
    {

        LinkedList<string> data = new LinkedList<string>();

        foreach (PartyCharacter character in GameContent.partyCharacters)
        {
            string tempString = "";

            tempString = Concatenate(
                dataSignifiers.CharacterStats.ToString()
                , character.classID.ToString()
                , character.health.ToString()
                , character.mana.ToString()
                , character.strength.ToString()
                , character.agility.ToString()
                , character.wisdom.ToString());

            data.AddLast(tempString);

            
            foreach (int e in character.equipment)
            {

                tempString = Concatenate(dataSignifiers.Equipment.ToString(), e.ToString());
                
                data.AddLast(tempString);
            }
        }


        return data;
    }


    static public void DeseralizeData(LinkedList<string> sereralizedData)
    {
        PartyCharacter pc = new PartyCharacter();

        foreach (string s in sereralizedData)
        {
            string[] splitLine = s.Split(sepchar);
            
            int signifyer = int.Parse(splitLine[0]);
            
            switch (signifyer) 
            {
                case dataSignifiers.CharacterStats:
                    pc = new PartyCharacter();
                    pc.classID = int.Parse(splitLine[1]);
                    pc.health = int.Parse(splitLine[2]);
                    pc.mana = int.Parse(splitLine[3]);
                    pc.strength = int.Parse(splitLine[4]);
                    pc.agility = int.Parse(splitLine[5]);
                    pc.wisdom = int.Parse(splitLine[6]);
                    GameContent.partyCharacters.AddLast(pc);
                    break;

                case dataSignifiers.Equipment:
                    pc.equipment.AddLast(int.Parse(splitLine[1]));
                    break;
            
            
            }
        
        }

    }

    static public string Concatenate(params string[] stringsToJoin)
    {
        string joinedString = "";

        if (joinedString != "")
            joinedString += sepchar;

        foreach (string s in stringsToJoin)
        {
            joinedString += s;
            joinedString += sepchar;
        }

        return joinedString;
    }
  
    

}


#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 1;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{

    static List<string> listOfPartyNames;

    static public void GameStart()
    {
        listOfPartyNames = new List<string>();

        GameContent.RefreshUI();
    }

    static public List<string> GetListOfPartyNames()
    {
        return listOfPartyNames;
    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        AssignmentPart1.LoadPartyButtonPressed(selectedName);
        GameContent.RefreshUI();
    }

    static public void SavePartyButtonPressed()
    {
        if(!GetListOfPartyNames().Contains(GameContent.GetPartyNameFromInput()))
        {
            GetListOfPartyNames().Add(GameContent.GetPartyNameFromInput());
        }
       AssignmentPart1.SavePartyButtonPressed(GameContent.GetPartyNameFromInput());
       
        GameContent.RefreshUI();
    }

    static public void DeletePartyButtonPressed(string selectedName)
    {
        GameContent.partyCharacters.Clear();
        AssignmentPart1.SavePartyButtonPressed(selectedName);
        GetListOfPartyNames().Remove(selectedName);
        
        GameContent.RefreshUI();
    }

}

#endregion


