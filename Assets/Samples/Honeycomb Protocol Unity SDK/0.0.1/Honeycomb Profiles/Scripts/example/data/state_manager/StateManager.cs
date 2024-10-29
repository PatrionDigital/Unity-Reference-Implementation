using UnityEngine;
using HplEdgeClient.Types;
using HplEdgeClient.Client;
using HplEdgeClient.Inputs;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }
    public AppState State { get; private set; }

    public HplClient Client { get; set; } = new("https://edge.test.honeycombprotocol.com");  // Honeynet

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ensure this object persists across scenes

            State = new AppState();
            LoadAuthToken();  // Load the auth token from persistent storage when the app starts
        }
    }

    private void LoadAuthToken()
    {
        State.AuthToken = PlayerPrefs.GetString("AuthToken", null);
    }

    public void ClearState()
    {
        ClearAuthToken();
        ClearUser();
        ClearProfile();
    }

    public void SaveAuthToken(string token)
    {
        State.AuthToken = token;
        PlayerPrefs.SetString("AuthToken", token);
        PlayerPrefs.Save();
    }

    public void ClearAuthToken()
    {
        State.AuthToken = null;
        PlayerPrefs.DeleteKey("AuthToken");
    }

    public void SaveUser(User user)
    {
        State.CurrentUser = user;
    }

    public void ClearUser()
    {
        State.CurrentUser = null;
    }

    public void SaveProfile(Profile profile)
    {
        State.CurrentProfile = profile;
    }

    public void ClearProfile()
    {
        State.CurrentProfile = null;
    }

    public void SaveCreateProfileInfo(string key, string value)
    {
        switch (key)
        {
            case "bio":
                State.CreateProfileInfo.Bio = value;
                break;
            case "name":
                State.CreateProfileInfo.Name = value;
                break;
            case "pfp":
                State.CreateProfileInfo.Pfp = value;
                break;
        }
    }

    public string GetCreateProfileValues(string key)
    {
        switch (key)
        {
            case "bio":
                return State.CreateProfileInfo.Bio;
            case "name":
                return State.CreateProfileInfo.Name;
            case "pfp":
                return State.CreateProfileInfo.Pfp;
            default:
                return "";
        }
    }

    public void ClearCreateProfileInfo()
    {
        State.CreateProfileInfo = new UserInfoInput
        {
            Bio = "",
            Name = "",
            Pfp = "",
        };
    }
}
