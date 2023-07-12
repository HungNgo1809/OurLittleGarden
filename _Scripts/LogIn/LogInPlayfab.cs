using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using PlayFab.DataModels;
using PlayFab.ProfilesModels;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LogInPlayfab : MonoBehaviour
{
    private string userEmail;
    private string userPassword;
    private string username;

    public GameObject loginPanel;
    public GameObject registPanel;

    public InputField userEmailPanel;
    public InputField passPanel;

    public DataManager dataManager;
    public LargeMapData largeMapData;

    public GameObject RegistSuccessPanel;
    public void Start()
    {
        //largeMapData.SaveVersionData("version","0.0.0");
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "1B883"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        //PlayerPrefs.DeleteAll();
        //var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            userEmailPanel.text = userEmail = PlayerPrefs.GetString("EMAIL");
            passPanel.text = userPassword = PlayerPrefs.GetString("PASSWORD");

            //var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            //PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");

        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        dataManager.userId = result.PlayFabId;
        dataManager.userEmail = userEmail;
        dataManager.userPassword = userPassword;
        GetDisplayName(result.PlayFabId);

        dataManager.LoadVersionData("version");

        dataManager.LoadFriendList();
        //dataManager.isNewbie = false;

        GetAccountInfo();
        //loginPanel.SetActive(false);
        //Load data
        //StartCoroutine(LoadDataAndChangeScene(result));
        //SceneManager.LoadScene(1);
        LoadDataAndChangeScene_(result);
    }

    private void GetAccountInfo()
    {
        // Lấy thông tin tài khoản của người chơi
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        // Lấy PlayFab username từ thông tin tài khoản
        dataManager.userName = result.AccountInfo.Username;
    }

    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError("Failed to get account info: " + error.ErrorMessage);
    }

    IEnumerator LoadDataAndChangeScene(LoginResult result)
    {
        dataManager.LoadDataPlayfab(result.PlayFabId);

        yield return new WaitUntil(() => dataManager.isLoadedData);
        Debug.Log("heresdaasd");
        LoadingScreen.Instance.StartLoadingScreenFirst("Main" , dataManager.isLoadedData);
        //SceneManager.LoadScene(1);
    }   
    // t them dieu kien de no load luon xong bh isLoadData xong thi no moi xong r de nhu o tren thi no phai doi isLoadedData = true no moi bat dau load screen
    public void LoadDataAndChangeScene_(LoginResult result)
    {
        dataManager.LoadDataPlayfab(result.PlayFabId);

        //SceneManager.LoadScene("Main");
        //LoadingScreen.Instance.StartLoadingScreenFirst("Main", dataManager.isLoadedData);
        LoadingScreen.Instance.FlexLoadingScreen("Main", dataManager.isLoadedData);
        Debug.Log(LoadingScreen.Instance.isReleaseLoading);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        SetDisplayName(username);
        RegistSuccessPanel.SetActive(true);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log(error);
    }

    public void OnClickRegist()
    {
        Debug.Log("regist");
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = RemoveSpaces(username) };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }    

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void GetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }

    public void GetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }

    public void GetUsername(string usernameIn)
    {
        username = usernameIn;
    }

    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void OnClickRegistButton()
    {
        loginPanel.SetActive(false);
        registPanel.SetActive(true);
    }
    public void SetDisplayName(string username)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSetDisplayNameSuccess, OnSetDisplayNameFailure);
    }

    private void OnSetDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Display name updated successfully!");
    }

    private void OnSetDisplayNameFailure(PlayFabError error)
    {
        Debug.LogError("Failed to update display name: " + error.ErrorMessage);
    }
    public void GetDisplayName(string id)
    {
        // Gọi hàm lấy thông tin người chơi
        GetPlayerProfileRequest request = new GetPlayerProfileRequest();
        request.PlayFabId = id;

        PlayFabClientAPI.GetPlayerProfile(request, OnGetPlayerProfileSuccess, OnGetPlayerProfileFailure);
    }

    // Xử lý khi lấy thông tin người chơi thành công
    private void OnGetPlayerProfileSuccess(GetPlayerProfileResult result)
    {
        dataManager.displayName = result.PlayerProfile.DisplayName;
    }

    // Xử lý khi lấy thông tin người chơi thất bại
    private void OnGetPlayerProfileFailure(PlayFabError error)
    {
        Debug.LogError("GetPlayerProfile failed: " + error.ErrorMessage);
    }
    public string RemoveSpaces(string input)
    {
        return input.Replace(" ", string.Empty);
    }
}