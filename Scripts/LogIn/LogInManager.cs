using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LogInManager : MonoBehaviour
{
    //public LoadingPanel loadingPanel; // Ngan them vao

    public InputField emailInput;
    public InputField passInput;

    public InputField registEmailInput;
    public InputField registPassInput;
    public InputField registUsernameInput;

    public GameObject Error_gameobject;
    public Text Error_text;

    public GameObject ErrorRegist_gameobject;
    public Text ErrorRegist_text;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public Toggle isSaveInfo;

    public DataManager userData;
    public HostUrl hostUrl;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            emailInput.text = PlayerPrefs.GetString("EMAIL");
            passInput.text = PlayerPrefs.GetString("PASSWORD");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickLogIn()
    {
        StartCoroutine(Login(emailInput.text, passInput.text));
    }

    IEnumerator Login(string email, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", pass);

        //Debug.Log(hostUrl.hostUrl + "Login.php");
        using (UnityWebRequest www = UnityWebRequest.Post(hostUrl.hostUrl + "Login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                //Debug.Log(www.error);
                //Hiển thị Panel lỗi kết nối
                if (Error_gameobject.activeSelf == false)
                {
                    Error_gameobject.SetActive(true);
                }
                Error_text.text = "Lỗi kết nối, xin vui lòng kiểm tra lại mạng";

            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                //Chuyển vào scene
                switch (www.downloadHandler.text)
                {
                    case "0":
                        if(isSaveInfo.isOn)
                        {
                            //Lưu cho lần sau
                            PlayerPrefs.SetString("EMAIL", email);
                            PlayerPrefs.SetString("PASSWORD", pass);

                            //lưu vào dataManager các thông tin cơ bản
                            userData.userEmail = email;
                            userData.userPassword = pass;

                            //Load dữ liệu vào dataManager (có tính vào loading scene)
                        }
                        if (Error_gameobject.activeSelf == true)
                        {
                            Error_gameobject.SetActive(false);
                        }
                        //loadingPanel.LoadingScene(); // Ngan them vao 
                        SceneManager.LoadScene(1); // Loading panel co load roi thi bo cai nay di nhi?
                        break;
                    case "1":
                        Debug.Log("Invalid username or password");
                        if (Error_gameobject.activeSelf == false)
                        {
                            Error_gameobject.SetActive(true);
                        }
                        Error_text.text = "Lỗi: Thông tin đăng nhập không hợp lệ";

                        break;
                    case "2":
                        Debug.Log("Duplicate Login");
                        if (Error_gameobject.activeSelf == false)
                        {
                            Error_gameobject.SetActive(true);
                        }
                        Error_text.text = "Lỗi: Tài khoản đã có người đăng nhập";
                        break;
                }
            }
        }
    }

    public void OnClickRegist()
    {
        StartCoroutine(Regist(registEmailInput.text, registPassInput.text, registUsernameInput.text));
    }

    IEnumerator Regist(string email, string pass, string username)
    {
        if (!validateEmail(email))
        {
            //Hiển thị panel email ko hợp lệ
            Debug.Log("Email không hợp lệ");
            if (ErrorRegist_gameobject.activeSelf == false)
            {
                ErrorRegist_gameobject.SetActive(true);
            }
            ErrorRegist_text.text = "Lỗi: Email không hợp lệ";

        }else if(registPassInput.text.Length < 6){
            // Hiển thị thông báo cần mk > 6 ký tự
            Debug.Log("Mật khẩu quá ngắn");
            if (ErrorRegist_gameobject.activeSelf == false)
            {
                ErrorRegist_gameobject.SetActive(true);
            }
            ErrorRegist_text.text = "Mật khẩu phải gồm tối thiểu 6 ký tự";
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", pass);
            form.AddField("username", username);


            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/SceneBuilderTool/Regist.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    //Hiển thị Panel lỗi
                    if (ErrorRegist_gameobject.activeSelf == false)
                    {
                        ErrorRegist_gameobject.SetActive(true);
                    }
                    ErrorRegist_text.text = "Lỗi kết nối, xin vui lòng kiểm tra lại mạng";
                }
                else
                {
                    //Debug.Log(www.downloadHandler.text);
                    //Chuyển vào scene
                    switch (www.downloadHandler.text)
                    {
                        case "0":
                            //Hiển thị panel thông báo thành công
                            Debug.Log("success");

                            if (ErrorRegist_gameobject.activeSelf == true)
                            {
                                ErrorRegist_gameobject.SetActive(false);
                            }

                            break;
                        case "1":
                            //Hiển thị panel thông báo thất bại
                            Debug.Log("false");

                            if (ErrorRegist_gameobject.activeSelf == false)
                            {
                                ErrorRegist_gameobject.SetActive(true);
                            }
                            ErrorRegist_text.text = "Lỗi: Thông tin đăng ký không hợp lệ";

                            break;
                        case "2":
                            //Hiển thị panel thông báo thất bại
                            Debug.Log("false");

                            if (ErrorRegist_gameobject.activeSelf == false)
                            {
                                ErrorRegist_gameobject.SetActive(true);
                            }
                            ErrorRegist_text.text = "Lỗi: Email đã được đăng ký từ trước";

                            break;
                    }
                }
            }
        }
    }

    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
}
