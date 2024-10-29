using Solana.Unity.SDK.Example;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class ProfileScreen : SimpleScreen
{
    public Button close_btn;
    public TextMeshProUGUI username_txt;
    public TextMeshProUGUI name_txt;
    public TextMeshProUGUI description_txt;
    public Image profile_img;

    private void Start()
    {
        close_btn.onClick.AddListener(() =>
        {
            manager.ShowScreen(this, "wallet_screen");
        });
    }

    IEnumerator LoadImage(string link)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            profile_img.sprite = newSprite;
        }
    }
    private void OnEnable()
    {
    }

    public override void ShowScreen(object data = null)
    {
        base.ShowScreen();
        gameObject.SetActive(true);


        username_txt.text = StateManager.Instance.State.CurrentUser.Info.Username;
        name_txt.text = StateManager.Instance.State.CurrentProfile.Info.Name;
        description_txt.text = StateManager.Instance.State.CurrentProfile.Info.Bio;
        StartCoroutine(LoadImage(StateManager.Instance.State.CurrentProfile.Info.Pfp));
    }

    public override void HideScreen()
    {
        base.HideScreen();
        gameObject.SetActive(false);
    }

    public void OnClose()
    {
        var wallet = GameObject.Find("wallet");
        wallet.SetActive(false);
    }
}