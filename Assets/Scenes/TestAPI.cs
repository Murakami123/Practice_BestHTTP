using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;

public class TestAPI : MonoBehaviour
{
    void Start()
    {
        // TLS1.2 通信にする。
        HTTPManager.UseAlternateSSLDefaultValue = true;
    }

    public void OnTap()
    {
        StartCoroutine(HttpCoutine());
    }

    IEnumerator HttpCoutine()
    {
        System.Uri uri = new System.Uri("http://52.68.173.184/index.php/game/nowTimeAPI");

        // Getメソッドだよ
        HTTPRequest bestHttpRequest = new HTTPRequest(uri, HTTPMethods.Get);

        // ヘッダが必要な場合のみ付ける。
        var isNeed = true;
        if (isNeed)
        {
            bestHttpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        }

        // Body が必要な場合のみ付ける。
        // (Body完全になしでよければ AddField 不要）

        // 送る＆結果
        yield return bestHttpRequest.Send();
        switch (bestHttpRequest.State)
        {
            case HTTPRequestStates.Finished:
                // サーバーからレスポンスが返ってきたらHTTPRequestStates.Finishedになります。   
                if (bestHttpRequest.Response.StatusCode == 200)
                {
                    // 成功時の処理  
                    Debug.Log("通信成功！！！！！" + bestHttpRequest.Response.DataAsText);
                }
                else
                {
                    // 失敗時の処理  
                    Debug.LogError("通信失敗！URLがおかしいかも。Post とか Get が間違ってるのかも。");
                    Debug.Log("bestHttpRequest.Response.StatusCode:" + bestHttpRequest.Response.StatusCode);
                    Debug.Log("bestHttpRequest.Response.Message:" + bestHttpRequest.Response.Message);
                    Debug.Log("bestHttpRequest.Response.IsStreamed:" + bestHttpRequest.Response.IsStreamed);
                    Debug.Log("bestHttpRequest.Response.IsStreamingFinished:" + bestHttpRequest.Response.IsStreamingFinished);
                    Debug.Log("bestHttpRequest.Response.Data:" + bestHttpRequest.Response.Data);
                    Debug.Log("bestHttpRequest.Response.DataAsText:" + bestHttpRequest.Response.DataAsText);
                    Debug.Log("bestHttpRequest.Response.DataAsTexture2D:" + bestHttpRequest.Response.DataAsTexture2D);
                }
                break;
            case HTTPRequestStates.Error:
                // 予期しないエラー  
                Debug.Log("6");
                break;
            case HTTPRequestStates.Aborted:
                // リクエストをHTTPRequest.Abort()でAbortさせた場合  
                Debug.Log("7");
                break;
            case HTTPRequestStates.ConnectionTimedOut:
                // サーバーとのコネクションのタイムアウト  
                Debug.Log("8");
                break;
            case HTTPRequestStates.TimedOut:
                // リクエストのタイムアウト  
                Debug.Log("9");
                break;
            default:
                Debug.Log("10");
                break;
        }
    }

}
