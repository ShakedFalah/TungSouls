using UnityEngine;

[CreateAssetMenu(fileName = "ProfileSO", menuName = "Scriptable Objects/ProfileSO")]
public class ProfileSO : ScriptableObject
{
    public string profileName = "";
    public bool iSProfileLoaded = false;
}
