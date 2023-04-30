
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Rami;

namespace Rami.DebugHelper
{
    public class DistanceLine : BaseLine
    {
        public float distanceScaleMultiplier = 1f;
        public string distanceUnit = "m";


        // See https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
        const string numberFormat = "0.00";

        protected override void Start()
        {
            base.Start();

        }

        protected override void Update()
        {
            base.Update();

            if(usingCenterText)
            {
                centerText.text = FormattedDistanceBetweenAllVertices();
            }

            if(usingSegmentText)
            {
                UpdateTextSegmentValues();
            }

        }





        string FormattedDistanceBetweenAllVertices()
        {
            float totalDistance = 0.0f;
            for (int i = 1; i < SegmentPositions.Length; ++i)
            {
                totalDistance += Vector3.Distance(SegmentPositions[i - 1], SegmentPositions[i]);
            }

            if (looping)
            {
                totalDistance += Vector3.Distance(SegmentPositions[0], SegmentPositions[SegmentPositions.Length - 1]);
            }

            return totalDistance.ToString(distanceUnit) + distanceUnit;

        }

        void UpdateTextSegmentValues()
        {
            for (int i = 0; i < SegmentPositions.Length - 1; ++i)
            {
                segmentTexts[i].text = FormattedDistanceBetween2Vertices(i, i  + 1);
            }
             if(looping)
            {
                segmentTexts[SegmentPositions.Length - 1].text = FormattedDistanceBetween2Vertices(0, SegmentPositions.Length - 1);
            }
        }

        string FormattedDistanceBetween2Vertices(int index1, int index2)
        {
            float distance = distanceScaleMultiplier * Vector3.Distance(SegmentPositions[index1], SegmentPositions[index2]);
            return distance.ToString(distanceUnit) + distanceUnit;
        }

    }
}
