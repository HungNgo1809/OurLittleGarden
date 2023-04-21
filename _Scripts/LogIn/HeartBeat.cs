using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HeartBeat : MonoBehaviour
{
    public DataManager userData;
    public GameObject DisconnectPanel;

    public HostUrl url;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(DisconnectPanel);

        // Check if there is another instance of this object in the scene
        HeartBeat[] objects = FindObjectsOfType<HeartBeat>();

        if (objects.Length > 1)
        {
            // Destroy all but the newest instance of the object
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != this.GetComponent<HeartBeat>())
                {
                    Destroy(objects[i].gameObject);
                }
            }
        }

        //StartCoroutine(CheckConnection(userData.userEmail));
    }
    /*
    IEnumerator Start()
    {
        yield return StartCoroutine(SendHeartbeat());
    }*/
    IEnumerator Start()
    {
        yield return StartCoroutine(SendHeartbeat(userData.userEmail));
        //yield return StartCoroutine(CheckConnection(userData.userEmail));
    }

    IEnumerator SendHeartbeat(string email)
    {
        while (true)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);

            // Send heartbeat request to server
            UnityWebRequest www = UnityWebRequest.Post(url.hostUrl + "heartbeat.php", form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending heartbeat request: " + www.error);
                DisconnectPanel.SetActive(true);
                StartCoroutine(TimeOverDestroy());
            }
            else
            {
                //Debug.Log("thich thich");

            }

            yield return new WaitForSeconds(5); // Send heartbeat every 10 seconds
            //StartCoroutine(SendHeartbeat());
        }
      
    }

    /*
    IEnumerator CheckConnection(string email)
    {

        WWWForm form = new WWWForm();
        form.AddField("email", email);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/SceneBuilderTool/CheckConnect.php", form);
        yield return www.SendWebRequest();

        //Debug.Log("ddcm");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Lost connection");
            //Hien thi panel mat ket noi, sau 5s tat app
        }
        else
        {

        }
    }*/
    
    IEnumerator TimeOverDestroy()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
        DisconnectPanel.SetActive(false); // quit roi con false duoc khong nhe :v
    }
}