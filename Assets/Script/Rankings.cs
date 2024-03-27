using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Rankings : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;

    private List<RankingEntry> rankingEntries = new List<RankingEntry>();

    private void Awake()
    {
        LoadDataFromJson();
        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 0f;

        for (int i = 0; i < Mathf.Min(rankingEntries.Count, 10); i++)
        {
            templateHeight += 0.8f;
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight);
            entryRectTransform.gameObject.SetActive(true);

            entryTransform.Find("PosText").GetComponent<Text>().text = (i + 1) + GetRankSuffix(i + 1);
            entryTransform.Find("NameText").GetComponent<Text>().text = rankingEntries[i].name;
            entryTransform.Find("LevelText").GetComponent<Text>().text = rankingEntries[i].level.ToString();
            entryTransform.Find("ScoreText").GetComponent<Text>().text = rankingEntries[i].score.ToString();
        }
    }

    private void LoadDataFromJson()
    {
        string filePath = Application.persistentDataPath + "/gameData.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            rankingEntries = JsonUtility.FromJson<RankingData>(jsonData).entries;

            // Sắp xếp danh sách theo điểm số giảm dần
            rankingEntries.Sort((x, y) => y.score.CompareTo(x.score));
        }
    }

    private string GetRankSuffix(int rank)
    {
        if (rank % 10 == 1 && rank != 11)
            return "ST";
        else if (rank % 10 == 2 && rank != 12)
            return "ND";
        else if (rank % 10 == 3 && rank != 13)
            return "RD";
        else
            return "TH";
    }
}

[System.Serializable]
public class RankingData
{
    public List<RankingEntry> entries = new List<RankingEntry>();
}

[System.Serializable]
public class RankingEntry
{
    public string name;
    public int level;
    public int score;
}
