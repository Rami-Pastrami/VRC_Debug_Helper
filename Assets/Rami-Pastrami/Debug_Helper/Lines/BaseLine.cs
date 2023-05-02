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

        public bool usingCenterText
        {
            set 
            {
                if (value)
                {
                    GameObject go = GameObject.Instantiate(textPrefab);
                    centerText = go.GetComponent<FloatingText>();

                    _usingCenterText = true;
                }
                else
                {
                    Destroy(centerText.gameObject);
                    centerText = null;

                    _usingCenterText = false;
                }
            }
            get { return _usingCenterText; }
        }

        public bool usingSegmentText
        {
            set
            {
                if (value)
                {
                    GenerateSegmentCompArray();

                    _usingSegmentText = true;
                }
                else
                {
                    DeleteSegmentCompArray();

                    _usingSegmentText = false;
                }
            }
            get { return _usingSegmentText; }
        }

        public bool looping
        {
            set { lineRenderer.loop = value; }
            get { return lineRenderer.loop; }
        }

        public Color colorStart
        {
            set { lineRenderer.startColor = value; }
            get { return lineRenderer.startColor; }
        }

        public Color colorEnd
        {
            set { lineRenderer.endColor = value; }
            get { return lineRenderer.endColor; }
        }

        public float lineWidth
        {
            set { lineRenderer.startWidth = value; lineRenderer.endWidth = value; }
            get { return lineRenderer.startWidth; }
        }



        [SerializeField] GameObject textPrefab;


        public Vector3[] SegmentPositions = {Vector3.zero, Vector3.zero };

        LineRenderer lineRenderer;
        [SerializeField] protected FloatingText centerText = null;
        [SerializeField] protected FloatingText[] segmentTexts = null;
        protected RectTransform[] segmentTextsTrans = null;
        [SerializeField] bool _usingCenterText = false;
        [SerializeField] bool _usingSegmentText = false;
        int _previousVertexCount;


        protected virtual void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null )
            {
                // Why doesn't U# allow adding components?
                Debug.LogError("LineRenderer component not found!");
            }
            _previousVertexCount = SegmentPositions.Length;

        }

        protected virtual void Update()
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
                    if (usingSegmentText) { UpdateSegmentTextPos(); }
                }

                if(usingCenterText)
                {
                    //usingCenterText = false; usingCenterText = true;
                    //if(usingCenterText) { UpdateCenterTextPos(); }
                }



            }

            
            

        }



        void DeleteSegmentCompArray()
        {
            if (segmentTexts == null) { return; }
            for (int i = 0; i < segmentTexts.Length; i++)
            {
                Destroy(segmentTexts[i].gameObject);
            }
        }

        void GenerateSegmentCompArray()
        {
            int offset = looping ? 0 : 1;
            segmentTexts = new FloatingText[_previousVertexCount - offset];
            for (int i = 0; i < segmentTexts.Length; ++i)
            {
                GameObject go = Instantiate(textPrefab);
                segmentTexts[i] = go.GetComponent<FloatingText>();
                segmentTextsTrans[i] = go.GetComponent<RectTransform>();
            }
        }


        void UpdateCenterTextPos()
        {
            centerText.followingPositions = SegmentPositions;
        }

        void UpdateSegmentTextPos()
        {
            for(int i = 0; i < _previousVertexCount - 1; ++i)
            {
                segmentTexts[i].followingPositions = Rami_Utils.GetSubArrayFromArray_StartSize(SegmentPositions,i,2);
            }
            if(looping)
            {
                segmentTexts[_previousVertexCount - 1].followingPositions = new Vector3[] { SegmentPositions[0], SegmentPositions[_previousVertexCount - 1] };
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


        private void OnDestroy()
        {
            DeleteSegmentCompArray();
        }

    }
}