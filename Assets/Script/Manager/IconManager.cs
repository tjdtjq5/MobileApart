using UnityEngine;

public class IconManager : MonoBehaviour
{
    public Icon[] iconList;

    [System.Serializable]
    public struct Icon
    {
        public string itemName;
        public string itemCode;
        public string inGameName;
        public GameObject iconObj;
    }
    

}


