using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] SaveLoadSystem saveLoadSystem;
    [Header("Parametrs")]
    public int maxAnimalLvl = 3;
    public int goldIncome = 75;
    [SerializeField] int animalPrice = 50;
    [SerializeField] int[] targetHPs;
    int targetHP;
    [Header("Objects")]
    [SerializeField] GameObject[] lvls;
    [SerializeField] AnimalSO[] animalsSO;
    [SerializeField] AnimalPlacesScript[] animalsPlacesScript;
    [SerializeField] CinemachineVirtualCamera[] cameras;
    [SerializeField] GameObject[] animalSpawnPlaces;
    [SerializeField] LaunchScript launchScript;
    List<AnimalScript> animalScripts = new List<AnimalScript>();
    int animalScriptPointer = 0;
    private void Awake()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 60;
        instance = this;
    }
    public void WinGameUI()
    {
        saveLoadSystem.CurrentData.lvl += 1;
        saveLoadSystem.CurrentData.gold += goldIncome;
        saveLoadSystem.SaveDataIntoFile();
    }
    public void StartAnimalPlaces()
    {
        targetHP = targetHPs[saveLoadSystem.CurrentData.lvl % targetHPs.Length];
        for (int i = 0; i < GameData.gridCapacity; i ++ )
        {
            if (saveLoadSystem.CurrentData.gridElements[i])
            {
                animalsPlacesScript[i].OpenPlace();
            }
            if (saveLoadSystem.CurrentData.gridAnimals[i] == 0)
            {
                continue;
            }
            UIManager.instance.EnableHuntBtn();
            Instantiate(animalsSO[saveLoadSystem.CurrentData.gridAnimals[i] - 1].panelAnimal, animalsPlacesScript[i].animalPoint.position, Quaternion.identity, lvls[0].transform)
                .GetComponent<DragAndSnapObject>().EnableDragAndSnapObject(animalsSO[saveLoadSystem.CurrentData.gridAnimals[i] - 1], animalsPlacesScript[i]);
        }
    }
    public void BuyAnimal()
    {
        if(!CheckFreePlace())
        {
            return;
        }
        if (UIManager.instance.BuyStuff(animalPrice))
        {
            for (int i = 0; i < GameData.gridCapacity; i++)
            {
                if (saveLoadSystem.CurrentData.gridElements[i] && saveLoadSystem.CurrentData.gridAnimals[i] == 0)
                {
                    UIManager.instance.EnableHuntBtn();
                    Instantiate(animalsSO[0].panelAnimal, animalsPlacesScript[i].animalPoint.position, Quaternion.identity, lvls[0].transform)
                        .GetComponent<DragAndSnapObject>().EnableDragAndSnapObject(animalsSO[0], animalsPlacesScript[i]);
                    saveLoadSystem.CurrentData.gridAnimals[i] = 1;
                    saveLoadSystem.SaveDataIntoFile();
                    return;
                }
            }
        }
    }
    public void MergeAnimals(AnimalPlacesScript animalPlacesScript1, AnimalPlacesScript animalPlacesScript2, int lvl)
    {
        ChangeAnimalPlace(animalPlacesScript1, animalPlacesScript2, lvl + 1);
        Instantiate(animalsSO[lvl].panelAnimal, animalPlacesScript2.animalPoint.position, Quaternion.identity, lvls[0].transform)
            .GetComponent<DragAndSnapObject>().EnableDragAndSnapObject(animalsSO[lvl], animalPlacesScript2);
    }
    public void ChangeAnimalPlace(AnimalPlacesScript animalPlacesScript1, AnimalPlacesScript animalPlacesScript2, int lvl)
    {
        saveLoadSystem.CurrentData.gridAnimals[animalPlacesScript1.GetID()] = 0;
        saveLoadSystem.CurrentData.gridAnimals[animalPlacesScript2.GetID()] = lvl;
        saveLoadSystem.SaveDataIntoFile();
    }
    public void OpenPlace(AnimalPlacesScript animalPlacesScript)
    {
        saveLoadSystem.CurrentData.gridElements[animalPlacesScript.GetID()] = true;
        saveLoadSystem.SaveDataIntoFile();
    }
    public void StartHunt()
    {
        lvls[0].SetActive(false);
        lvls[1].SetActive(true);
        UIManager.instance.ToHunt();
        for (int i = 0; i < GameData.gridCapacity; i++)
        {
            if (saveLoadSystem.CurrentData.gridAnimals[i] == 0)
            {
                continue;
            }
            animalScripts.Add(Instantiate(animalsSO[saveLoadSystem.CurrentData.gridAnimals[i] - 1].gameAnimal, animalSpawnPlaces[i].transform.position, Quaternion.Euler(0,25f,0), animalSpawnPlaces[i].transform.parent)
                .GetComponent<AnimalScript>().StartAnimal(animalsSO[saveLoadSystem.CurrentData.gridAnimals[i] - 1]));
            cameras[animalScriptPointer].Follow = animalScripts[animalScriptPointer].transform;
            animalScripts[animalScriptPointer].huntEvent.AddListener(RenewAnimal);
            animalScripts[animalScriptPointer].animator.SetTrigger("Move");
            if (animalScriptPointer == 0)
            {
                launchScript.hunter = animalScripts[animalScriptPointer].gameObject;
            }
            animalScriptPointer++;
        }
        animalScriptPointer = 0;
    }
    public bool RecieveAttack(int attack)
    {
        targetHP -= attack;
        if(targetHP <= 0)
        {
            launchScript.gameObject.SetActive(false);
            UIManager.instance.WinGame();
            return true;
        }
        return false;
    }
    bool CheckFreePlace()
    {
        for (int i = 0; i < GameData.gridCapacity; i++)
        {
            if (saveLoadSystem.CurrentData.gridElements[i] && saveLoadSystem.CurrentData.gridAnimals[i] == 0)
            {
                return true;
            }
        }
        return false;
    }
    void RenewAnimal()
    {
        Debug.Log(1);
        cameras[animalScriptPointer].Priority = 9;
        animalScriptPointer++;
        if (animalScriptPointer >= animalScripts.Count)
        {
            UIManager.instance.LoseGame();
            launchScript.gameObject.SetActive(false);
            return;
        }
        launchScript.hunter = animalScripts[animalScriptPointer].gameObject;
        launchScript.RenewAnimal();
        cameras[animalScriptPointer].Priority = 11;
    }
}