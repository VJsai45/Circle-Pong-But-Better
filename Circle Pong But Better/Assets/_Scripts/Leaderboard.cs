using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{

    // Use this for initialization

    public static Leaderboard instance;
    public string[] leaderboardIds;

    public bool loggedin = false;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {


        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

        Debug.Log("eee");


        SignIn();

    }


    private void SignIn()
    {
        Social.localUser.Authenticate(sucess =>
        {

        });
    }


    public void AddScoreToLeaderBoard(int level, long score)
    {
        string leaderBoardId = leaderboardIds[level];
        Social.ReportScore(score, leaderBoardId, success =>
        {
        });
    }


    public void ShowLeaderboardUI(string leaderboardId)
    {

        Debug.Log(PlayGamesPlatform.Instance.IsAuthenticated());

        if (PlayGamesPlatform.Instance.IsAuthenticated())
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
        else
            Social.localUser.Authenticate((bool success) => {
                PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
            });
    }

    void OnMouseDown()
    {
        switch(HomeController.instance.level)
           {
            case 0:
                ShowLeaderboardUI(leaderboardIds[0]);
                break;
            case 1:
                ShowLeaderboardUI(leaderboardIds[1]);
                break;
            case 2:
                ShowLeaderboardUI(leaderboardIds[2]);
                break;
            }
            

    }


}
