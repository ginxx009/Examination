using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class testResults
{
    public int totalScore = 0;


    [Header("REF IU")]
    public UILabel textTime;
    public UILabel textTotalScore;


    [Header("REF RESULT SCREEN")]
    public GameObject resultCanvas;
    //public Image[] stars;
    //public Text textResultScore;
    //public Text textInfo;
    public UISprite[] stars;
    public UILabel textResultScore;
    public UILabel textInfo;

    [Space(10)]
    public Color starOn;
    public Color starOff;

    public void ShowResult()
    {
        textResultScore.text = totalScore.ToString();
        textInfo.text = "You finished " + testScript.main.words.Length + " questions.";

        int allTimeLimit = testScript.main.GetAllTimeLimit();

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = totalScore >= allTimeLimit / (3 - i) ? starOn : starOff;
        }

        resultCanvas.SetActive(true);
    }
}
[System.Serializable]
public class testWords
{
    public string word;
    [Header("Leave empty if you want randomized")]
    public string desiredRandom;

    [Space(10)]
    public float timeLimit;

    public string GetString()
    {
        if (!string.IsNullOrEmpty(desiredRandom))
        {
            return desiredRandom;
        }
        
        string result = word;

        while (result == word)
        {
            result = "";
            List<char> characters = new List<char>(word.ToCharArray());
            while (characters.Count > 0)
            {
                int indexChar = Random.Range(0, characters.Count - 1);
                result += characters[indexChar];

                characters.RemoveAt(indexChar);
            }
        }
        return result;
    }
}


public class testScript : MonoBehaviour
{
    public static List<string> textArray;
    public int[] rowsToReadFrom = new int[1] { 0 };
    public string FileName;
    public UILabel textComp;

    private TextAsset myTextAsset;
    private int indexx = 0;
    


    public testWords[] words;

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

    private float totalScore;


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
    }

    public void readTextFile()
    {
        //textComp.text = "";
        //rowsToReadFrom[0] = index;
        //textArray = myTextAsset.text.Split('\n').ToList();
        //textComp.text += textArray[rowsToReadFrom[0]] + "\n";

        //index += 1;
    }


    void Update()
    {
        RepositionObject();

        totalScore = Mathf.Lerp(totalScore, result.totalScore, Time.deltaTime * 5);
        result.textTotalScore.text = Mathf.RoundToInt(totalScore).ToString();
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
        foreach (testWords w in words)
        {
            result += w.timeLimit / 2;
        }

        return Mathf.RoundToInt(result);
    }

    /// <summary>
    /// Show a random word to the screen
    /// </summary>
    public void ShowScramble()
    {
        ShowScramble(Random.Range(0, words.Length - 1));
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
        //WORDS FINISHED
        //SHOW RESULT SCREEN
        if (index > words.Length - 1)
        {
            result.ShowResult();
            wordCanvas.SetActive(false);
            //Debug.LogError("Index out of range. Please enter range between 0 to " + (words.Length - 1).ToString());
            return;
        }

        char[] chars = words[index].GetString().ToCharArray();
        foreach (char c in chars)
        {
            testObject clone = Instantiate(prefab.gameObject).GetComponent<testObject>();
            clone.transform.SetParent(container);

            charObjects.Add(clone.Init(c));
        }

        textComp.text = "";

        rowsToReadFrom[0] = indexx;
        textArray = myTextAsset.text.Split('\n').ToList();
        textComp.text += textArray[rowsToReadFrom[0]];

        string temp = textArray[rowsToReadFrom[0]];
        temp = System.Text.RegularExpressions.Regex.Replace(temp, @"\s", "");
        char[] chArr = temp.ToCharArray();
        foreach (char c in chArr)
        {
            testObject clone = Instantiate(prefab.gameObject).GetComponent<testObject>();
            clone.transform.SetParent(container);

            charObjects.Add(clone.Init(c));

            Debug.Log(c);
        }

        

        indexx += 1;


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

        if (timeLimit <= 0)
        {
            currentWord++;
            ShowScramble(currentWord);
            yield break;
        }

        if (word == words[currentWord].word)
        {
            currentWord++;
            result.totalScore += Mathf.RoundToInt(timeLimit);

            //StopCoroutine(TimeLimit());

            ShowScramble(currentWord);
        }
    }
    float timeLimit;
    IEnumerator TimeLimit()
    {
        timeLimit = words[currentWord].timeLimit;
        result.textTime.text = Mathf.RoundToInt(timeLimit).ToString();

        int myWord = currentWord;

        yield return new WaitForSeconds(1f);

        while (timeLimit > 0)
        {
            if (myWord != currentWord) { yield break; }

            timeLimit -= Time.deltaTime;
            result.textTime.text = Mathf.RoundToInt(timeLimit).ToString();
            yield return null;
        }
        CheckWord();
    }
}
