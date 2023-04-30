// Just an internal tool I use for testing out some stuff
// base class for floating lines

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Rami;

namespace Rami.DebugHelper
{
    public class BaseLine : UdonSharpBehaviour
    {
        [field: SerializeField]
        public bool usingCenterText
        {
            set 
            {
                if (value)
                {
                    centerText = GameObject.Instantiate(textPrefab);

                    _usingCenterText = true;
                }
                else
                {
                    Destroy(centerText);
                    centerText = null;

                    _usingCenterText = false;
                }
            }
            get { return _usingCenterText; }
        }

        [field: SerializeField]
        public bool usingSegmentText
        {
            set
            {
                if (value)
                {
                    GenerateSegmentGOArray();

                    _usingSegmentText = true;
                }
                else
                {
                    DeleteSegmentGOArray();

                    _usingSegmentText = false;
                }
            }
            get { return _usingSegmentText; }
        }

        [field: SerializeField]
        public bool looping
        {
            set { lineRenderer.loop = value; }
            get { return lineRenderer.loop; }
        }

        [field: SerializeField]
        public Color colorStart
        {
            set { lineRenderer.startColor = value; }
            get { return lineRenderer.startColor; }
        }

        [field: SerializeField]
        public Color colorEnd
        {
            set { lineRenderer.endColor = value; }
            get { return lineRenderer.endColor; }
        }

        [field: SerializeField]
        public float lineWidth
        {
            set { lineRenderer.startWidth = value; lineRenderer.endWidth = value; }
            get { return lineRenderer.startWidth; }
        }



        [SerializeField] GameObject textPrefab;


        public Vector3[] SegmentPositions = {Vector3.zero, Vector3.zero };

        LineRenderer lineRenderer;
        GameObject centerText = null;
        GameObject[] segmentTexts = null;
        bool _usingCenterText = false;
        bool _usingSegmentText = false;
        int _previousVertexCount;


        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            _previousVertexCount = SegmentPositions.Length;
        }

        private void Update()
        {

            /// update text positions if exist

            lineRenderer.SetPositions(SegmentPositions);

            // If number of segments changed as per number of Segment Positions, we must adjust number of Texts
            if(SegmentPositions.Length != _previousVertexCount)
            {
                _previousVertexCount = SegmentPositions.Length;

                if(usingSegmentText)
                {
                    usingSegmentText = false; usingSegmentText = true; // Property abuse lol
                }

                if(usingCenterText)
                {
                    usingCenterText = false; usingCenterText = true;
                }
            }

            if(usingCenterText) { UpdateCenterTextPos(); }
            if(usingSegmentText) { UpdateSegmentTextPos(); }


        }



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
            int offset = looping ? 0 : 1;
            segmentTexts = new GameObject[_previousVertexCount - offset];
            for (int i = 0; i < segmentTexts.Length; ++i)
            {
                GameObject go = Instantiate(textPrefab);
                segmentTexts[i] = go;
            }
        }


        void UpdateCenterTextPos()
        {
            centerText.transform.position = Rami.Rami_Utils.CenterOfVector3s(SegmentPositions);
        }

        void UpdateSegmentTextPos()
        {
            for(int i = 0; i < _previousVertexCount - 1; ++i)
            {
                segmentTexts[i].transform.position = GetCenterOf1Segment(i);
            }
            if(looping)
            {
                segmentTexts[_previousVertexCount- 1].transform.position = (SegmentPositions[0] + SegmentPositions[_previousVertexCount - 1]) / 2.0f;
            }

        }

        Vector3 GetCenterOf1Segment(int index)
        {
            if (index - 1 > _previousVertexCount | index < 0)
            {
                Debug.LogError("DEBUG_HELPER - Can't get segment of invalid index");
            }

            return Rami_Utils.CenterOfVector3s(Rami_Utils.GetSubArrayFromArray_StartSize(SegmentPositions, index, 2));
        }


    }
}