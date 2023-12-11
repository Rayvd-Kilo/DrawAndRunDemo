using System;
using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;

using Dreamteck.Splines;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawAndRun.DrawingModule.Views
{
    public class CurveDrawerView : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
    {
        public event Action<Vector3[]> PointerReleased = delegate {  };
        
        [SerializeField] private SplineComputer _splineComputer;
        [SerializeField] private Camera _canvasCamera;

        private int _indexPoint;

        private RectTransform _rectTransform;

        private CancellationTokenSource _cts = new();
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            StopPointerTrack();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            CalculateSplineAsync().Forget();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            StopPointerTrack();
        }
        
        private async UniTaskVoid CalculateSplineAsync()
        {
            Vector2 pointPos = Vector2.zero;
            while (Application.isPlaying)
            {
                Vector2 currentPosition =
                    Input.touchCount < 1 ? Input.mousePosition : Input.GetTouch(0).position;
                if (currentPosition != pointPos)
                {
                    pointPos = currentPosition;
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, pointPos, _canvasCamera,
                        out Vector3 worldPoint);
                    _splineComputer.SetPoint(_indexPoint++, new SplinePoint(worldPoint + Vector3.back * .1f));
                }

                await UniTask.WaitForEndOfFrame(_cts.Token);
            }
        }

        private void StopPointerTrack()
        {
            if (_cts.Token.CanBeCanceled)
            {
                PointerReleased.Invoke(GetOffsetFromTarget());
                _indexPoint = 0;
                _splineComputer.SetPoints(Array.Empty<SplinePoint>());
                _cts.Cancel();
                _cts.Dispose();
                _cts = new CancellationTokenSource();
            }
        }

        private Vector3[] GetOffsetFromTarget()
        {
            var points = NormalizePoints(_splineComputer.GetPoints(), 0.09f);
            var offsets = new Vector3[points.Length];
            var worldCenterPointDrawPanel = _rectTransform.TransformPoint(_rectTransform.rect.center);
            for (var i = 0; i < points.Length; ++i)
            {
                var vectorToPoint = worldCenterPointDrawPanel - points[i].position;
                offsets[i] = new Vector3(-vectorToPoint.x, 0, -vectorToPoint.y);
            }

            return offsets;
        }

        private SplinePoint[] NormalizePoints(SplinePoint[] targetCurve, float e)
        {
            SplinePoint[] resultCurve;
            
            float distanceMax = 0f;
            int indexMax = 0;
            for (var i = 1; i < targetCurve.Length; i++)
            {
                var distance = HandleUtility.DistancePointLine(targetCurve[i].position, targetCurve[0].position,
                    targetCurve[^1].position);
                if (distance > distanceMax)
                {
                    indexMax = i;
                    distanceMax = distance;
                }
            }
            
            if (distanceMax > e)
            {
                var recResult1 = NormalizePoints(targetCurve.Take(indexMax).ToArray(), e);
                var recResult2 = NormalizePoints(targetCurve.Skip(indexMax).ToArray(), e);

                resultCurve = recResult1.Take(recResult1.Length - 1).Concat(recResult2).ToArray();
            }
            else
            {
                var result = new SplinePoint[targetCurve.Length];
                Array.Copy(targetCurve, 0, result, 0, targetCurve.Length);
                resultCurve = result;
            }

            return resultCurve;
        }
    }
}