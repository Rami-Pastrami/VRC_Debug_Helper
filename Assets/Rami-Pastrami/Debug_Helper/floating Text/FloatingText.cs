// Just an internal tool I use for testing out some stuff
// floating text use to denote variables


using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;




namespace Rami.DebugHelper
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class FloatingText : UdonSharpBehaviour
    {
        public Vector3[] followingPositions;
        public Vector3 posOffset = Vector3.zero;
        public string prefix = "";

        TextMeshProUGUI textbox;

        public string text
        {
            set { textbox.text = prefix + value; }
        }

        public Color textColor
        {
            set { textbox.color = value; }
        }


        private void Start()
        {
            textbox = GetComponent<TextMeshProUGUI>();
        }


        private void Update()
        {
            if (followingPositions != null)
            {
                transform.position = Rami.Rami_Utils.CenterOfVector3s(followingPositions) + posOffset;
            }
            else
            {
                transform.position = posOffset;
            }

        }



    }
}
