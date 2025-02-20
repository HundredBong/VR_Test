using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIRequest : MonoBehaviour
{
    #region GET
    //private void Start()
    //{
    //    StartCoroutine(GetDataCoroutine());
    //}

    //private IEnumerator GetDataCoroutine()
    //{
    //    //테스트 API
    //    string url = "https://jsonplaceholder.typicode.com/todos/1";

    //    using (UnityWebRequest www = UnityWebRequest.Get(url))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError("데이터를 가져오는데 실패함");
    //        }
    //        else 
    //        {
    //            //응답 데이터 출력
    //            Debug.Log(www.downloadHandler.text);
    //        }  
    //    }
    //}
    #endregion
    #region POST
    private void Start()
    {
        StartCoroutine(PostDataCoroutine());
    }

    private IEnumerator PostDataCoroutine()
    {
        string url = "https://jsonplaceholder.typicode.com/posts";

        //JSON 데이터 생성
        string jsonData = "{\"title\":\"헌드레드봉\", \"body\":\"REST API TEST\", \"userId\":123}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        //POST 요청
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"데이터 전송에 실패함, {www.error}");
        }
        else
        {
            Debug.Log($"데이터 전송 성공 : {www.downloadHandler.text}");
        }
    }
    #endregion
}
