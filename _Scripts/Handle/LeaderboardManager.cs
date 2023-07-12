using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public Transform coinView;
    public Transform reputationView;

    public GameObject rankingUiPrefab;

    public Transform curParrent;
    public void GetLeaderboard(int maxResults, string statisticName)
    {
        if(statisticName == "coins")
        {
            curParrent = coinView;
        }else if(statisticName == "reputation")
        {
            curParrent = reputationView;
        }
        
        var request = new GetLeaderboardRequest
        {
            StatisticName = statisticName,
            MaxResultsCount = maxResults
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        foreach (var playerStat in result.Leaderboard)
        {
            Debug.Log("Player: " + playerStat.DisplayName + " - Rank: " + playerStat.Position + " - Score: " + playerStat.StatValue);
            GameObject ui = Instantiate(rankingUiPrefab);

            ui.GetComponent<rankingComponent>().rank.text = (playerStat.Position + 1).ToString();
            ui.GetComponent<rankingComponent>().username.text = playerStat.DisplayName;
            ui.GetComponent<rankingComponent>().score.text = playerStat.StatValue.ToString();

            if (curParrent != null)
            {
                ui.transform.SetParent(curParrent);
            }    

            ui.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError("Failed to get leaderboard: " + error.ErrorMessage);
    }
}
