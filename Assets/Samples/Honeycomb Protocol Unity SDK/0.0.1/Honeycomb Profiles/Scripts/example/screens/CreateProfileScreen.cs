using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HplEdgeClient.Inputs;
using HplEdgeClient.Params;
using UnityEngine.Networking;
using System.Collections;
using HplEdgeClient.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Solana.Unity.Wallet.Utilities;

namespace Solana.Unity.SDK.Example
{
    public class CreateProfileScreen : SimpleScreen
    {
        [SerializeField]
        public Button close_btn;
        [SerializeField]
        public Button select_profile_btn;

        [SerializeField]
        private Image pfp;
        [SerializeField]
        private Image plusIcon;

        [SerializeField]
        private TMP_InputField usernameInputField;
        [SerializeField]
        private TMP_InputField nameInputField;
        [SerializeField]
        private TMP_InputField bioInputField;

        [SerializeField]
        private TextMeshProUGUI messageTxt;

        [SerializeField]
        private Button submitBtn;
        [SerializeField]
        private TMP_Dropdown dropdownRpcCluster;

        private void OnEnable()
        {
            dropdownRpcCluster.interactable = false;
            usernameInputField.text = StateManager.Instance.GetCreateProfileValues("username");
            nameInputField.text = StateManager.Instance.GetCreateProfileValues("name");
            bioInputField.text = StateManager.Instance.GetCreateProfileValues("bio");

            if (Web3.Wallet != null)
            {
                dropdownRpcCluster.interactable = false;
            }

            UpdateProfileImage();
        }

        private void Start()
        {
            select_profile_btn.onClick.AddListener(() =>
            {
                StateManager.Instance.SaveCreateProfileInfo("username", usernameInputField.text);
                StateManager.Instance.SaveCreateProfileInfo("name", nameInputField.text);
                StateManager.Instance.SaveCreateProfileInfo("bio", bioInputField.text);
                manager.ShowScreen(this, "thumbnail_screen");
            });

            // submitBtn.onClick.AddListener(handleUpdateProfile);
            submitBtn.onClick.AddListener(handleCreateProfile);
            close_btn.onClick.AddListener(() =>
            {
                StateManager.Instance.SaveCreateProfileInfo("username", usernameInputField.text);
                StateManager.Instance.SaveCreateProfileInfo("name", nameInputField.text);
                StateManager.Instance.SaveCreateProfileInfo("bio", bioInputField.text);
                manager.ShowScreen(this, "wallet_screen");
            });

            usernameInputField.onEndEdit.AddListener((string value) =>
            {
                StateManager.Instance.SaveCreateProfileInfo("username", value);
            });

            nameInputField.onEndEdit.AddListener((string value) =>
            {
                StateManager.Instance.SaveCreateProfileInfo("name", value);
            });

            bioInputField.onEndEdit.AddListener((string value) =>
            {
                StateManager.Instance.SaveCreateProfileInfo("bio", value);
            });
        }

        private async void handleCreateProfile()
        {
            var name = StateManager.Instance.State.CreateProfileInfo.Name;
            var bio = StateManager.Instance.State.CreateProfileInfo.Bio;

            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(bio) || string.IsNullOrEmpty(StateManager.Instance.State.CreateProfileInfo.Pfp))
                {
                    Debug.LogWarning("Please fill all fields");
                    messageTxt.text = "Please fill all fields";
                    messageTxt.gameObject.SetActive(true);
                    return;
                }

                Debug.Log("Creating new user");

                // ProfileInfoInput profileInfo = new()
                // {
                //     Bio = StateManager.Instance.State.CreateProfileInfo.Bio,
                //     Name = StateManager.Instance.State.CreateProfileInfo.Name,
                //     Pfp = StateManager.Instance.State.CreateProfileInfo.Pfp,
                // };

                var tx = await StateManager.Instance.Client.CreateNewUserWithProfileTransaction(new CreateNewUserWithProfileTransactionParams
                {
                    Project = StateManager.Instance.State.Project,
                    Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                    Payer = Web3.Wallet.Account.PublicKey.ToString(),
                    UserInfo = StateManager.Instance.State.CreateProfileInfo,
                });

                Debug.Log(tx.CreateNewUserWithProfileTransaction.Transaction_);
                var txHelper = new TransactionHelper(tx.CreateNewUserWithProfileTransaction);
                txHelper = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(txHelper.ToSendParams());

                if (res.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("User created successfully");
                    StateManager.Instance.ClearCreateProfileInfo();
                    manager.ShowScreen(this, "wallet_screen");
                }
                else
                {
                    Debug.LogError("Failed to create user");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                // Try parsing the exception message as JSON
                try
                {
                    // The exception message might be in JSON format
                    var json = JToken.Parse(e.Message);

                    // Access the error message in the array
                    var errorMessage = json[0]?["message"]?.ToString() ?? "Unknown error occurred";

                    // Display the error message in your UI text component
                    messageTxt.text = errorMessage;
                }
                catch (JsonReaderException)
                {
                    // If it's not a valid JSON format, just show the raw exception message
                    messageTxt.text = e.Message;
                }
                catch (System.Exception jsonParsingEx)
                {
                    // Handle any unexpected errors during parsing
                    Debug.LogError($"Error while parsing exception message: {jsonParsingEx.Message}");
                    messageTxt.text = "An unexpected error occurred.";
                }

                messageTxt.gameObject.SetActive(true);
            }
        }


        private async void handleUpdateProfile()
        {
            var name = StateManager.Instance.State.CreateProfileInfo.Name;
            var bio = StateManager.Instance.State.CreateProfileInfo.Bio;

            try
            {
                // if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(bio) || string.IsNullOrEmpty(StateManager.Instance.State.CreateProfileInfo.Pfp))
                // {
                //     Debug.LogWarning("Please fill all fields");
                //     messageTxt.text = "Please fill all fields";
                //     messageTxt.gameObject.SetActive(true);
                //     return;
                // }

                Debug.Log("Creating new user");

                // var tx = await StateManager.Instance.Client.CreateNewUserWithProfileTransaction(new CreateNewUserWithProfileTransactionParams
                // {
                //     Project = StateManager.Instance.State.Project,
                //     Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                //     Payer = Web3.Wallet.Account.PublicKey.ToString(),
                //     ProfileInfo = profileInfo,
                //     UserInfo = StateManager.Instance.State.CreateProfileInfo,
                // });

                var authmsg = await StateManager.Instance.Client.AuthRequest(new AuthRequestParams
                {
                    Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                });

                Debug.Log(authmsg.AuthRequest.Message);
                var msgToSign = authmsg.AuthRequest.Message.Trim().Replace("\u200B", "");
                var messageBytes = Encoding.UTF8.GetBytes(msgToSign);
                Debug.Log(msgToSign);
                var sign = await Web3.Wallet.SignMessage(messageBytes);
                // Debug.Log("Signature Valid: " + Web3.Wallet.Account.PublicKey.Verify(messageBytes, sign));

                var authToken = await StateManager.Instance.Client.AuthConfirm(new AuthConfirmParams
                {
                    Wallet = Web3.Wallet.Account.PublicKey.ToString(),
                    Signature = Encoders.Base58.EncodeData(sign)
                });
                var token = authToken.AuthConfirm.AccessToken;
                Debug.Log(token);
                // var txHelper = new TransactionHelper(tx.CreateNewUserWithProfileTransaction);
                Debug.Log("Updating user");
                var userParams = new CreateUpdateUserTransactionParams
                {
                    Info = new()
                    {
                        Bio = StateManager.Instance.State.CreateProfileInfo.Bio,
                        Name = StateManager.Instance.State.CreateProfileInfo.Name,
                        Pfp = StateManager.Instance.State.CreateProfileInfo.Pfp,
                    },
                    Payer = Web3.Wallet.Account.PublicKey.ToString(),

                };
                var tx = await StateManager.Instance.Client.CreateUpdateUserTransaction(userParams, token);
                Debug.Log(tx.CreateUpdateUserTransaction.Transaction_);

                var profileParams = new CreateUpdateProfileTransactionParams
                {
                    Info = new()
                    {
                        Bio = StateManager.Instance.State.CreateProfileInfo.Bio,
                        Name = StateManager.Instance.State.CreateProfileInfo.Name,
                        Pfp = StateManager.Instance.State.CreateProfileInfo.Pfp,
                    },
                    Payer = Web3.Wallet.Account.PublicKey.ToString(),
                    Profile = StateManager.Instance.State.CurrentProfile.Address,

                };
                // var profileTx = await StateManager.Instance.Client.CreateUpdateProfileTransaction(profileParams, token);

                // Debug.Log(profileTx.CreateUpdateProfileTransaction.Transaction_);



                var txHelper = new TransactionHelper(tx.CreateUpdateUserTransaction);
                var profileTxHelper = new TransactionHelper(tx.CreateUpdateUserTransaction);
                txHelper = await txHelper.Sign();
                var res = await StateManager.Instance.Client.SendBulkTransactions(txHelper.ToSendParams());

                if (res.SendBulkTransactions[0].Error == null)
                {
                    Debug.Log("User created successfully");
                    StateManager.Instance.ClearCreateProfileInfo();
                    manager.ShowScreen(this, "wallet_screen");
                }
                else
                {
                    Debug.LogError("Failed to create user");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                // Try parsing the exception message as JSON
                try
                {
                    // The exception message might be in JSON format
                    var json = JToken.Parse(e.Message);

                    // Access the error message in the array
                    var errorMessage = json[0]?["message"]?.ToString() ?? "Unknown error occurred";

                    // Display the error message in your UI text component
                    messageTxt.text = errorMessage;
                }
                catch (JsonReaderException)
                {
                    // If it's not a valid JSON format, just show the raw exception message
                    messageTxt.text = e.Message;
                }
                catch (System.Exception jsonParsingEx)
                {
                    // Handle any unexpected errors during parsing
                    Debug.LogError($"Error while parsing exception message: {jsonParsingEx.Message}");
                    messageTxt.text = "An unexpected error occurred.";
                }

                messageTxt.gameObject.SetActive(true);
            }
        }
        private void UpdateProfileImage()
        {
            string selectedImageUrl = StateManager.Instance.State.CreateProfileInfo.Pfp;
            if (!string.IsNullOrEmpty(selectedImageUrl))
            {
                StartCoroutine(LoadProfileImage(selectedImageUrl));

                // Hide the plus icon
                plusIcon.gameObject.SetActive(false);
            }
        }

        private IEnumerator LoadProfileImage(string imageUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                pfp.sprite = newSprite;
            }
        }


        public override void HideScreen()
        {
            base.HideScreen();
            messageTxt.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OnClose()
        {
            var wallet = GameObject.Find("wallet");
            wallet.SetActive(false);
        }
    }
}
