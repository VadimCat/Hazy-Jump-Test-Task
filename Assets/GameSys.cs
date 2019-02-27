using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSys : MonoBehaviour
{
    [SerializeField] GameObject UIHolder;
    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject MenuUI;
    [SerializeField] Text GameScore;
    [SerializeField] Text GameOverScoreText;
    [SerializeField] Text HighScoreMenu;
    [SerializeField] Text HighScore;
    [SerializeField] GameObject CharPref;
    [SerializeField] GameObject PlatformPref;

    GameObject currentChar;
    Rigidbody2D currentCharRigid;
    public List<GameObject> platformList = new List<GameObject>();
    bool gameStarted = false;
    bool gameOver = true;

    public int platformlCount = 0;
    public int pattertnCounter = 0;

    public enum PlatformPattern
    {
        easy,
        medium,
        hard,
        insane
    }

    public PlatformPattern platformPattern;

    int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            GameOverScoreText.text = GameScore.text = value.ToString();
        }
    }

    public static GameSys Instance;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Record"))
            PlayerPrefs.SetInt("Record", 0);
        HighScore.text = HighScoreMenu.text = PlayerPrefs.GetInt("Record").ToString();

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.Space) & (gameStarted & !gameOver)))
        {
            currentCharRigid.bodyType = RigidbodyType2D.Static;
            currentCharRigid.bodyType = RigidbodyType2D.Dynamic;
            currentCharRigid.AddForce(new Vector2(0, -15), ForceMode2D.Impulse);
        }
        if ((Input.GetKey(KeyCode.Space)) & !gameStarted & gameOver)
        {
            GameStart();
        }
        if ((Input.GetKey(KeyCode.LeftControl)) & !gameStarted & gameOver)
        {
            currentCharRigid.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        }

    }

    public void PlatformSet()
    {
        Vector3 randomVector = new Vector3();
        Vector3 randomQuaternion = new Vector3();
        int randomScale = 0;
        platformPattern = (PlatformPattern)(pattertnCounter / 3);
        switch (platformPattern)
        {
            case PlatformPattern.easy:
                {

                    if (pattertnCounter % 2 == 0)
                    {
                        randomQuaternion = new Vector3(0, 0, Random.Range(8, 13));
                        randomVector = new Vector3(Random.Range(0, 150), Random.Range(-350, -250), platformlCount);
                    }
                    else
                    {
                        randomQuaternion = new Vector3(0, 0, Random.Range(-13, -8));
                        randomVector = new Vector3(Random.Range(-150, 0), Random.Range(-350, -250), platformlCount);
                    }
                    randomScale = Random.Range(6, 7);
                    break;
                }
            case PlatformPattern.medium:
                {
                    if (pattertnCounter % 2 == 0)
                    {
                        randomQuaternion = new Vector3(0, 0, Random.Range(8, 18));
                        randomVector = new Vector3(Random.Range(0, 250), Random.Range(-350, -250), platformlCount);
                    }
                    else
                    {
                        randomQuaternion = new Vector3(0, 0, Random.Range(-18, -8));
                        randomVector = new Vector3(Random.Range(-250, 0), Random.Range(-350, -250), platformlCount);
                    }
                    randomScale = Random.Range(4, 5);
                    break;
                }
            case PlatformPattern.hard:
                {
                    if (pattertnCounter % 2 == 0)
                    {
                        randomVector = new Vector3(Random.Range(0, 300), Random.Range(-350, -250));
                        randomQuaternion = new Vector3(0, 0, Random.Range(8, 23));
                    }
                    else
                    {
                        randomQuaternion = new Vector3(0, 0, Random.Range(-23, -8));
                        randomVector = new Vector3(Random.Range(-300, 0), Random.Range(-350, -250));

                    }
                    randomScale = Random.Range(2, 4);
                    break;
                }
            case PlatformPattern.insane:
                {
                    randomVector = new Vector3(Random.Range(-350, 350), Random.Range(-350, -300));
                    if (pattertnCounter % 2 == 0)
                    {
                        randomQuaternion = new Vector3(0, 0, Random.Range(8, 28));
                    }
                    else
                    {
                        randomQuaternion = new Vector3(0, 0, Random.Range(-28, -8));

                    }
                    randomScale = Random.Range(1, 2);
                    break;
                }

        }
        platformList.Add(Instantiate(PlatformPref, randomVector,
        Quaternion.Euler(randomQuaternion)));
        platformList[platformList.Count - 1].transform.localScale = new Vector3(randomScale, randomScale);
        platformList[platformList.Count - 1].transform.SetParent(GameUI.transform, false);
        platformList[platformList.Count - 1].transform.SetAsFirstSibling();
        platformlCount++;
        pattertnCounter++;
        if (pattertnCounter == 11)
        {
            pattertnCounter = 1;
        }
    }

    private void Start()
    {
        ScreenStart();
    }

    public void ScreenStart()
    {

        gameOver = true;
        Physics2D.gravity = new Vector2(0, 0);
        gameStarted = false;
        GameUI.SetActive(false);
        MenuUI.SetActive(true);
        GameOverUI.SetActive(false);
        platformList.Add(Instantiate(PlatformPref, GameUI.transform));
        platformList[0].transform   .localScale = new Vector3(8, 8, 1);
        platformList[platformList.Count - 1].transform.SetAsFirstSibling();
        for (int i = 0; i < 3; i++)
        {
            PlatformSet();
        }
    }

    public void GameStart()
    {
        Score = 0;
        gameOver = false;
        gameStarted = true;
        GameUI.SetActive(true);
        MenuUI.SetActive(false);
        Physics2D.gravity = new Vector2(0, -10f);
        currentChar = Instantiate(CharPref, GameUI.transform);
        currentCharRigid = currentChar.GetComponent<Rigidbody2D>();

        currentCharRigid.AddForce(new Vector2(0, -5), ForceMode2D.Impulse);
        platformList[0].GetComponent<Platorm>().Activate();
    }

    public void GameOver()
    {
        int temp = platformList.Count;
        for (int i = 0; i <  temp; i++)
        {
            Debug.Log(i);
            Destroy(platformList[0]);
            platformList.RemoveAt(0);
        }
        if (PlayerPrefs.GetInt("Record") < Score)
        {
            PlayerPrefs.SetInt("Record", Score);
            HighScore.text = HighScoreMenu.text = PlayerPrefs.GetInt("Record").ToString();
        }
        gameOver = false;
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
        gameStarted = false;
        

        platformlCount = 0;
        pattertnCounter = 0;
        platformPattern = 0;
    }
}