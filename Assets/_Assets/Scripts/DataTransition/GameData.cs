// Data for game variables
[System.Serializable]
public class GameData
{
    public int lvl;
    public int gold;
    public static int startGold = 100;
    public static int gridCapacity = 10;
    public bool[] gridElements = new bool[gridCapacity];
    public int[] gridAnimals = new int[gridCapacity];
    public GameData()// saving starting variables for new game
    {
        lvl = 0;
        gold = startGold;
        for (int i = 0; i < gridCapacity; i++)
        {
            gridElements[i] = false;
            if(i < 5)
            {
                gridElements[i] = true;
            }
            gridAnimals[i] = 0;
        }
    }
    public GameData(GameData hs)// saving variables for loading level
    {
        lvl = hs.lvl;
        gold = hs.gold;
        for (int i = 0; i < gridCapacity; i++)
        {
            gridElements[i] = hs.gridElements[i];
            gridAnimals[i] = hs.gridAnimals[i];
        }
    }
}