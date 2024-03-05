using GameManagerSpace;
using UnityEngine;

public class RequestManager : MonoBehaviour
{


    [System.Serializable]
    public struct requestObj {
        public bool isPrimary;
        public Item itemRequested;
    }
    public struct requestType
    {
        public bool isPrimary;
        public GMStatic.tagType typeRequested;
        public int nbRequested;
    }
    public struct requestColor
    {
        public bool isPrimary;
        public Color colorRequested;
        public int nbRequested;
    }
    public struct requestMaterial
    {
        public bool isPrimary;
        public GMStatic.tagMaterial materialRequested;
        public int nbRequested;
    }
    public struct requestProximity
    {
        public bool isPrimary;
        public GMStatic.tagType closeFromRequested;
        public GMStatic.tagType closeToRequested;
    }

    public void SetRequestedConstraint()
    {

    }
}
