using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;

public class TestGetPNG : MonoBehaviour
{
    void Start()
    {
        // TLS1.2 通信にする。
        HTTPManager.UseAlternateSSLDefaultValue = true;
    }

    // [SerializeField] string url;
    public void OnTap()
    {
        StartCoroutine(HttpCoutine());
    }


    [SerializeField] Image setImage;
    [SerializeField] string getUri;
    IEnumerator HttpCoutine()
    {
        System.Uri uri = new System.Uri(getUri);

        // Postメソッドだよ
        HTTPRequest bestHttpRequest = new HTTPRequest(uri, HTTPMethods.Get);

        // ヘッダが必要な場合のみ付ける。
        var isNeed = true;
        if (isNeed)
        {
            bestHttpRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        }

        // // body のデータ！
        // bestHttpRequest.AddField("uid", "903a5c5f29f7645683b3f16b848fab0d6481ad95");
        // bestHttpRequest.AddField("sno", "2");
        // var DataSent = new LitJson.JsonData();
        // DataSent[""] = ""; // この行も必要
        // // DataSent["partyTypeCode"] = PartyTypeCode;
        // // DataSent["partyNo"] = PartyNo;
        // // DataSent["partyName"] = PartyName;        
        // bestHttpRequest.AddField("params", DataSent.ToJson());

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

                    // 画像の場合どうするのか不明
                    setImage.sprite = BytesToSprite(bestHttpRequest.Response.Data);
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

    // すごい汎用的なメソッド。
    private Sprite BytesToSprite(byte[] data)
    {
        // byte[] を Texture2D に変換。
        var texture2D = new Texture2D(0, 0); // 引数の0, 0はなんでもいいかも。
        texture2D.LoadImage(data);

        // Texture2D を Sprite に変換。
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
    }

}
