using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ProfileSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI profileNameText;
    [SerializeField] private RawImage thumbnailImage;
    [SerializeField] private Button selectButton;

    private string associatedDirectoryName;
    private Action<string> onProfileClickedCallback;

    public void SetupSlot(string directoryName, Texture2D thumbnail, Action<string> onClickCallback)
    {
        associatedDirectoryName = directoryName;
        profileNameText.text = directoryName;
        onProfileClickedCallback = onClickCallback;

        if (thumbnail != null)
        {
            thumbnailImage.texture = thumbnail;
        } else
        {
            thumbnailImage.enabled = false;
        }
        
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(HandleSelection);
    }

    private void HandleSelection()
    {
        onProfileClickedCallback?.Invoke(associatedDirectoryName);
    }
}
