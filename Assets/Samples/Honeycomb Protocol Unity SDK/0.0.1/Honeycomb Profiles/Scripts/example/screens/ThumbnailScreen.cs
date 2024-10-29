using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Solana.Unity.SDK.Example
{
    public class ThumbnailScreen : SimpleScreen
    {
        [SerializeField]
        private Transform thumbnailContainer;

        [SerializeField]
        private GameObject thumbnailPrefab;

        [SerializeField]
        private ScrollRect scrollRect;


        public SimpleScreenManager parentManager;

        private CancellationTokenSource _stopTask;
        private List<string> _thumbnails = new() {
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/1.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/2.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/3.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/4.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/5.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/6.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/7.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/8.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/9.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/10.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/11.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/12.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/13.png",
            "https://edge.test.honeycombprotocol.com/cdn/common_thumbnails/14.png",
        };

        public void Start()
        {
        }

        private async void OnEnable()
        {
            await LoadThumbnails();
        }

        private async UniTask LoadThumbnails()
        {
            // Clear existing thumbnails if any
            foreach (Transform child in thumbnailContainer)
            {
                Destroy(child.gameObject);
            }

            // Lock the scroll position at the top and disable user interaction
            scrollRect.verticalNormalizedPosition = 1f;
            scrollRect.enabled = false; // Disables scrolling interaction

            // Step 1: Create all the thumbnail instances first without loading images
            List<(Image imageComponent, string thumbnailUrl)> imageLoadTasks = new List<(Image, string)>();

            foreach (var thumbnailUrl in _thumbnails)
            {
                var thumbnailInstance = Instantiate(thumbnailPrefab, thumbnailContainer);

                // Get the Image component, assign a placeholder sprite if necessary
                var imageComponent = thumbnailInstance.GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    // Optional: You can assign a placeholder sprite here
                    // imageComponent.sprite = placeholderSprite;

                    // Store the image component and URL for later asynchronous loading
                    imageLoadTasks.Add((imageComponent, thumbnailUrl));
                }

                // Set up the button click listener
                var button = thumbnailInstance.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnThumbnailClick(thumbnailUrl));
                }
            }

            // Step 2: Load images asynchronously after all thumbnails have been instantiated
            foreach (var (imageComponent, thumbnailUrl) in imageLoadTasks)
            {
                if (imageComponent != null)
                {
                    // Load the sprite asynchronously and set it to the image component
                    imageComponent.sprite = await LoadSpriteAsync(thumbnailUrl);
                }
            }

            // Once loading is complete, re-enable scrolling and keep the scroll at the top
            scrollRect.verticalNormalizedPosition = 1f;
            scrollRect.enabled = true; // Re-enable scrolling interaction
        }

        private void OnThumbnailClick(string thumbnailUrl)
        {
            StateManager.Instance.SaveCreateProfileInfo("pfp", thumbnailUrl);
            manager.ShowScreen(this, "create_profile_screen");
        }

        private async UniTask<Sprite> LoadSpriteAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await UniTask.Yield();
                }

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                    return null;
                }
                else
                {
                    Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }

        public override void ShowScreen(object data = null)
        {
            base.ShowScreen();
            gameObject.SetActive(true);
        }

        public override void HideScreen()
        {
            base.HideScreen();
            gameObject.SetActive(false);
        }

        public void OnClose()
        {
            manager.ShowScreen(this, "create_profile_screen");
        }

        private void OnDestroy()
        {
            if (_stopTask is null) return;
            _stopTask.Cancel();
        }
    }
}
