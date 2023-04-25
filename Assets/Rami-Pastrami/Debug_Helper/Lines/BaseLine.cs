// Just an internal tool I use for testing out some stuff
// base class for floating lines

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Rami;
using Cysharp.Threading.Tasks.Triggers;

namespace Rami.DebugHelper
{
    public class BaseLine : UdonSharpBehaviour
    {
        public bool usingCenterText
        {
            set { }
        }
        public bool usingSegmentText
        {
            set { }
        }

        public bool usingDistanceCenterText = true;
        public bool usingDistanceSegmentText = true;
        public bool followingRootTransformInstead = false;

        public Vector3 globalOffset = Vector3.zero;
        public Vector3[] SegmentOffsets;

        GameObject centerText = null;
        GameObject[] segmentTexts = null;

        public GameObject REF_DebugText;













        void DeleteSegmentGOArray()
        {
            if (segmentTexts == null) { return; }
            for (int i = 0; i < segmentTexts.Length; i++)
            {
                Destroy(segmentTexts[i]);
            }
            segmentTexts = null;
        }

        void GenerateSegmentGOArray()
        {
            segmentTexts = new GameObject[SegmentOffsets.Length - 1];
            for (int i = 0; i < segmentTexts.Length; ++i)
            {
                GameObject go = Instantiate(REF_DebugText);
                segmentTexts[i] = go;
            }
        }

        void UpdateCenterTextPos()
        {
            centerText.transform.position = Rami.Rami_Utils.CenterOfVector3s(SegmentOffsets) + globalOffset;
        }

        Vector3 GetCenterOf1Segment(int index)
        {
            if (index - 1 > SegmentOffsets.Length | index < 0)
            {
                Debug.LogError("DEBUG_HELPER - Can't get segment of invalid index");
            }

            return Rami.Rami_Utils.CenterOfVector3s(Rami.Rami_Utils.GetSubArrayFromArray_StartSize(SegmentOffsets, index, 2));
        }


    }
}