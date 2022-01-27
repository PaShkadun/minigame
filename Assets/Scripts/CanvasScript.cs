using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private int levelPassed = 0;
    [SerializeField] private List<int> levelsStars;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject[] emptyStars;
    [SerializeField] private GameObject[] fullStars;
    [SerializeField] private GameObject[] levels;
    public GameObject previousPage;
    public GameObject levelPopup;
    private int page = 1;
    private bool isPressed = false;
    private bool backToLevels = false;
    private bool goToLevel = false;

    private Transform[] shootingStars;
    [SerializeField] private Transform[] points;
    private float timer;
    private bool isLevelStarted = false;
    private int currentLevel = 0;
    private float scalling = 1f;

    public void ChooseLevel(GameObject button)
    {
        var choosenLevel = int.Parse(button.name);

        if (choosenLevel > levelPassed + 1)
        {
            return;
        }
        
        goToLevel = true;
        currentLevel = int.Parse(button.name);

        if (currentLevel - 1 >= levelsStars.Count)
        {
            levelsStars.Add(0);
        }
    }
    
    private void ChangeLevels()
    {
        for (var i = 0; i < levels.Length; i++)
        {
            var levelNumber = (page - 1) * 3 + i + 1;
            
            if (levelNumber >= levelPassed + 1)
            {
                levels[i].transform.GetChild(2).gameObject.SetActive(false);
                levels[i].transform.GetChild(3).gameObject.SetActive(false);
                levels[i].transform.GetChild(4).gameObject.SetActive(false);
                levels[i].transform.GetChild(5).gameObject.SetActive(true);
                levels[i].transform.GetChild(6).gameObject.SetActive(true);
                levels[i].transform.GetChild(7).gameObject.SetActive(true);
                levels[i].transform.GetChild(8).gameObject.SetActive(false);
                levels[i].transform.GetChild(9).gameObject.SetActive(false);
                levels[i].transform.GetChild(10).gameObject.SetActive(false);

                if (levelNumber == levelPassed + 1)
                {
                    levels[i].transform.GetChild(11).gameObject.SetActive(false);
                }
                else
                {
                    levels[i].transform.GetChild(11).gameObject.SetActive(true);
                }
            }
            else
            {
                var stars = levelsStars[levelNumber - 1];
                
                levels[i].transform.GetChild(2).gameObject.SetActive(true);
                levels[i].transform.GetChild(5).gameObject.SetActive(false);
                levels[i].transform.GetChild(11).gameObject.SetActive(false);

                if (stars == 3)
                {
                    levels[i].transform.GetChild(3).gameObject.SetActive(true);
                    levels[i].transform.GetChild(4).gameObject.SetActive(true);
                    levels[i].transform.GetChild(6).gameObject.SetActive(false);
                    levels[i].transform.GetChild(7).gameObject.SetActive(false);
                    levels[i].transform.GetChild(8).gameObject.SetActive(true);
                    levels[i].transform.GetChild(9).gameObject.SetActive(false);
                    levels[i].transform.GetChild(10).gameObject.SetActive(false);
                }
                else if (stars == 2)
                {
                    levels[i].transform.GetChild(3).gameObject.SetActive(true);
                    levels[i].transform.GetChild(4).gameObject.SetActive(false);
                    levels[i].transform.GetChild(6).gameObject.SetActive(false);
                    levels[i].transform.GetChild(7).gameObject.SetActive(true);
                    levels[i].transform.GetChild(8).gameObject.SetActive(false);
                    levels[i].transform.GetChild(9).gameObject.SetActive(true);
                    levels[i].transform.GetChild(10).gameObject.SetActive(false);
                }
                else
                {
                    levels[i].transform.GetChild(6).gameObject.SetActive(true);
                    levels[i].transform.GetChild(7).gameObject.SetActive(true);
                    levels[i].transform.GetChild(8).gameObject.SetActive(false);
                    levels[i].transform.GetChild(9).gameObject.SetActive(false);
                    levels[i].transform.GetChild(10).gameObject.SetActive(true);
                }
            }

            levels[i].transform.GetChild(12).gameObject.name = levelNumber.ToString();
            levels[i].transform.GetChild(1).GetComponent<Text>().text = levelNumber + " level";
        }
    }

    private void PopupStars()
    {
        var stars = levelsStars[currentLevel - 1];

        levelPopup.transform.GetChild(1).gameObject.SetActive(false);
        levelPopup.transform.GetChild(4).gameObject.SetActive(true);
        
        if (stars == 3)
        {
            levelPopup.transform.GetChild(2).gameObject.SetActive(false);
            levelPopup.transform.GetChild(3).gameObject.SetActive(false);
            levelPopup.transform.GetChild(5).gameObject.SetActive(true);
            levelPopup.transform.GetChild(6).gameObject.SetActive(true);
        }
        else if (stars == 2)
        {
            levelPopup.transform.GetChild(2).gameObject.SetActive(false);
            levelPopup.transform.GetChild(3).gameObject.SetActive(true);
            levelPopup.transform.GetChild(5).gameObject.SetActive(true);
            levelPopup.transform.GetChild(6).gameObject.SetActive(false);
        }
        else
        {
            levelPopup.transform.GetChild(2).gameObject.SetActive(true);
            levelPopup.transform.GetChild(3).gameObject.SetActive(true);
            levelPopup.transform.GetChild(5).gameObject.SetActive(false);
            levelPopup.transform.GetChild(6).gameObject.SetActive(false);
        }
    }
    
    public void PreviousPage()
    {
        if (page == 2)
        {
            previousPage.SetActive(false);
        }

        page--;

        ChangeLevels();
    }

    public void NextPage()
    {
        if (page == 1)
        {
            previousPage.SetActive(true);
        }

        page++;
        
        ChangeLevels();
    }
    
    public void ReturnToLevels()
    {
        backToLevels = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelsStars = new List<int>();
        GameObject.FindWithTag("Prev").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (backToLevels)
        {
            level.transform.localScale = new Vector3(level.transform.localScale.x - scalling * Time.deltaTime,
                level.transform.localScale.y - scalling * Time.deltaTime,
                level.transform.localScale.z - scalling * Time.deltaTime);

            if (level.transform.localScale.x <= 0)
            {
                level.SetActive(false);
                level.transform.localScale = new Vector3(1, 1, 1);
                panel.SetActive(true);
                backToLevels = false;
                ChangeLevels();
            }
        }

        if (goToLevel)
        {
            panel.transform.localScale = new Vector3(panel.transform.localScale.x - scalling * Time.deltaTime,
                panel.transform.localScale.y - scalling * Time.deltaTime,
                panel.transform.localScale.z - scalling * Time.deltaTime);

            if (panel.transform.localScale.x <= 0)
            {
                panel.SetActive(false);
                panel.transform.localScale = new Vector3(1, 1, 1);
                level.SetActive(true);
                levelPopup.SetActive(false);
                isLevelStarted = true;
                goToLevel = false;
            }
        }
        
        if (isLevelStarted)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                isPressed = true;
                levelsStars[currentLevel - 1] = 1;
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                isPressed = true;
                levelsStars[currentLevel - 1] = 2;
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                isPressed = true;
                levelsStars[currentLevel - 1] = 3;
            }

            if (isPressed)
            {
                isLevelStarted = false;
                isPressed = false;
                levelPopup.SetActive(true);

                PopupStars();

                if (currentLevel > levelPassed)
                {
                    levelPassed = currentLevel;
                }
            }
        }
    }

    private void SetActiveStars(int amount)
    {
        for (var i = 0; i < emptyStars.Length; i++)
        {
            emptyStars[i].SetActive(i >= amount);
            fullStars[i].SetActive(i < amount);
        }

        if (amount == 0)
        {
            return;
        }

        shootingStars = new Transform[amount];

        for (var i = 0; i < amount; i++)
        {
            shootingStars[i] = fullStars[i].transform;
        }

        timer = 0f;
    }
}
