using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class testResults
{
    public int totalScore = 0;
    public int highscore;

    [Header("REF IU")]
    public UILabel textTime;
    public UILabel textTotalScore;


    [Header("REF RESULT SCREEN")]
    public GameObject resultCanvas;
    public UISprite[] stars;
    public UILabel textResultScore;
    public UILabel textInfo;

    [Header("HIGHSCORE")]
    public UILabel texthighscore;

    [Space(10)]
    public Color starOn;
    public Color starOff;

    public void ShowResult()
    {
        textResultScore.text = totalScore.ToString();
        textInfo.text = "GAME OVER";

        highscore = PlayerPrefs.GetInt("hscore",0);

        if (totalScore > highscore)
        {
            PlayerPrefs.SetInt("hscore", totalScore);
            PlayerPrefs.Save();

            highscore = totalScore;
            texthighscore.text = highscore.ToString();
        }

        int allTimeLimit = testScript.main.GetAllTimeLimit();

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = totalScore >= allTimeLimit / (3 - i) ? starOn : starOff;
        }

        

        resultCanvas.SetActive(true);
    }
}
//[System.Serializable]
//public class testWords
//{
//    public string word;
//    [Header("Leave empty if you want randomized")]
//    public string desiredRandom;

//    [Space(10)]
//    public float timeLimit;

//    public string GetString()
//    {
//        if (!string.IsNullOrEmpty(desiredRandom))
//        {
//            return desiredRandom;
//        }
        
//        string result = word;

//        while (result == word)
//        {
//            result = "";
//            List<char> characters = new List<char>(word.ToCharArray());
//            while (characters.Count > 0)
//            {
//                int indexChar = Random.Range(0, characters.Count - 1);
//                result += characters[indexChar];

//                characters.RemoveAt(indexChar);
//            }
//        }
//        return result;
//    }
//}


public class testScript : MonoBehaviour
{
    public static List<string> textArray;
    public int[] rowsToReadFrom = new int[1] { 0 };
    public string FileName;

    public UILabel hint;
    public UILabel lbl_hint;
    private bool usedHint;
    private float timerHint = 60;
    public GameObject hintObj;

    [Space(10)]
    public testResults result;

    [Header("UI REFERENCE")]
    public GameObject wordCanvas;
    public testObject prefab;
    public Transform container;
    public float space;
    public float lerpSpeed = 5f;

    List<testObject> charObjects = new List<testObject>();
    testObject firstSelected;

    public int currentWord;

    public static testScript main;

    public float timeLimit;

    public UILabel lbl_info;
    private int lblInfoCounter;

    private bool gamepaused = false;
    private int pauseCounter = 0; 

    private float totalScore;

    public string temp;
    private string charResult;
    private char[] chArr;
    private char[] randomCharArray;

    private TextAsset myTextAsset;
    private int indexx = 0;

    private int hscore = 0;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        myTextAsset = Resources.Load("words") as TextAsset;
        string myString = myTextAsset.text;

        ShowScramble(currentWord);
        result.textTotalScore.text = result.totalScore.ToString();

        hscore = PlayerPrefs.GetInt("hscore", hscore);
        result.texthighscore.text = hscore.ToString();
    }

    void Update()
    {
        RepositionObject();

        totalScore = Mathf.Lerp(totalScore, result.totalScore, Time.deltaTime * 5);
        result.textTotalScore.text = Mathf.RoundToInt(totalScore).ToString();

        result.texthighscore.text = PlayerPrefs.GetInt("hscore",0).ToString();

        if (usedHint)
        {
            timerHint -= Time.deltaTime;

            string seconds = (timerHint % 60).ToString("00");

            lbl_hint.text = seconds;

            if (timerHint <= 0)
            {
                usedHint = false;
                hintObj.GetComponent<UISprite>().enabled = true;
                timerHint = 60;
                lbl_hint.text = "HINT";
            }
        }
    }

    /// <summary>
    /// Reposition the words and its spaces 
    /// </summary>
    private void RepositionObject()
    {
        if (charObjects.Count == 0)
        {
            return;
        }

        float center = (charObjects.Count - 1) / 2;
        for (int i = 0; i < charObjects.Count; i++)
        {
            charObjects[i].rectTransform.anchoredPosition
                = Vector2.Lerp(charObjects[i].rectTransform.anchoredPosition,
                new Vector2((i - center) * space, 0), lerpSpeed * Time.deltaTime);
            charObjects[i].index = i;
        }
    }

    /// <summary>
    /// Get the whole time limit of every word
    /// </summary>
    /// <returns></returns>
    public int GetAllTimeLimit()
    {
        float result = 0;
        result += timeLimit / 2;
        
        return Mathf.RoundToInt(result);
    }

    /// <summary>
    /// Show a random word to the screen
    /// </summary>
    public void ShowScramble()
    {
        ShowScramble(Random.Range(0, temp.Length - 1));
    }

    /// <summary>
    ///  Show word from collection with desired index
    /// </summary>
    /// <param name="index"> index of the element</param>
    public void ShowScramble(int index)
    {
        
        charObjects.Clear();
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        //textComp.text = "";
        rowsToReadFrom[0] = indexx;
        textArray = myTextAsset.text.Split('\n').ToList();
        //textComp.text += textArray[rowsToReadFrom[0]];

        temp = textArray[rowsToReadFrom[0]];
        temp = System.Text.RegularExpressions.Regex.Replace(temp, @"\s", "");
        chArr = temp.ToCharArray();

        //Randomize
        System.Random rnd = new System.Random();
        randomCharArray = chArr.OrderBy(x => rnd.Next()).ToArray();
        charResult = string.Join("", randomCharArray);


        foreach (char c in charResult)
        {

            testObject clone = Instantiate(prefab.gameObject).GetComponent<testObject>();
            clone.transform.SetParent(container);

            charObjects.Add(clone.Init(c));
        }

        indexx += 1;

        
        if (indexx == 5)
        {
            if (PlayerPrefs.HasKey("Achievement2Endless"))
            {
                Debug.Log("Already has keys");
            }
            else
            {
                Achievement.instance.lbl.text = "5 Correct Words!";
                Achievement.instance.achievement.GetComponent<Animator>().Play("", 0, 0f);
            }

            PlayerPrefs.SetString("Achievement2Endless", "two");
            PlayerPrefs.Save();
        }

        currentWord = index;
        StartCoroutine(TimeLimit());

    }

    /// <summary>
    /// Swapping 1st element to 2nd element
    /// </summary>
    /// <param name="indexA"> index A</param>
    /// <param name="indexB"> index B</param>
    public void Swap(int indexA, int indexB)
    {
        testObject tmpA = charObjects[indexA];

        charObjects[indexA] = charObjects[indexB];
        charObjects[indexB] = tmpA;

        charObjects[indexA].transform.SetAsLastSibling();
        charObjects[indexB].transform.SetAsLastSibling();
        CheckWord();
    }

    /// <summary>
    /// Selection of words
    /// </summary>
    /// <param name="charObject"></param>
    public void Select(testObject charObject)
    {
        if (firstSelected)
        {
            Swap(firstSelected.index, charObject.index);

            firstSelected.Select();
            charObject.Select();
        }
        else
        {
            firstSelected = charObject;
        }
    }

    /// <summary>
    /// Unselect selected words
    /// </summary>
    public void UnSelect()
    {
        firstSelected = null;
    }

    public void CheckWord()
    {
        StartCoroutine(CoCheckWord());
    }

    /// <summary>
    /// Check every words
    /// </summary>
    /// <returns></returns>
    IEnumerator CoCheckWord()
    {
        yield return new WaitForSeconds(0.5f);

        string word = "";
        foreach (testObject charObject in charObjects)
        {
            word += charObject.character;
        }

        if (timelimit <= 0)
        {
            //WORDS FINISHED
            //SHOW RESULT SCREEN
            result.ShowResult();
            wordCanvas.SetActive(false);
            yield break;
        }

        if(word == temp)
        {
            currentWord++;
            result.totalScore += Mathf.RoundToInt(timelimit);
            hint.text = "";
            ShowScramble(currentWord);
        }
    }
    float timelimit;
    IEnumerator TimeLimit()
    {
        timelimit = timeLimit;
        result.textTime.text = Mathf.RoundToInt(timelimit).ToString();

        int myWord = currentWord;

        yield return new WaitForSeconds(1f);

        while (timelimit > 0)
        {
            if (myWord != currentWord) { yield break; }

            if (!gamepaused)
            {
                timelimit -= Time.deltaTime;
            
                result.textTime.text = Mathf.RoundToInt(timelimit).ToString();

            }
            yield return null;
        }
        CheckWord();
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void Pause()
    {
        pauseCounter += 1;
        gamepaused = true;
        container.gameObject.SetActive(false);
        if (pauseCounter > 1)
        {
            gamepaused = false;
            pauseCounter = 0;
            container.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Go home
    /// </summary>
    public void Home()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    /// <summary>
    /// Reload Scene
    /// </summary>
    public void Retry()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// Information About the Mode
    /// </summary>
    public void InfoMode()
    {
        lblInfoCounter += 1;
        lbl_info.text = "This mode is an endless word puzzle";
        if (lblInfoCounter == 2)
        {
            lblInfoCounter = 0;
            lbl_info.text = "";
        }
    }

    /// <summary>
    /// Hint of the whole word!
    /// </summary>
    public void Hint()
    {
        hint.text = "";

        hint.text = textArray[rowsToReadFrom[0]];
        usedHint = true;
        if (usedHint)
        {
            hintObj.GetComponent<UISprite>().enabled = false; 
        }
        else
        {
            hintObj.GetComponent<UISprite>().enabled = true;
        }
        
    }
}
