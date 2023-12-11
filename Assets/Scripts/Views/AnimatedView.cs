using System;

using DG.Tweening;

using DrawAndRun.Data;

using UnityEngine;

using TweenType = DrawAndRun.Enums.TweenType;

namespace DrawAndRun.Views
{
    public class AnimatedView : MonoBehaviour
    {
        [SerializeField] private TweenSequenceData _sequenceData;
        [SerializeField] private bool _isRectTransform;
        [SerializeField] private bool _looped;

        private Sequence _activeSequence;

        public void StartAnimation()
        {
            if (_activeSequence.IsActive())
            {
                _activeSequence.Kill();
            }

            _activeSequence = DOTween.Sequence();

            foreach (var tweenData in _sequenceData.SequenceData)
            {
                switch (tweenData.TweenType)
                {
                    case Enums.TweenType.Move:
                        _activeSequence.Append(_isRectTransform
                            ? transform.GetComponent<RectTransform>()
                                .DOAnchorPos(tweenData.TransformDirection, tweenData.TweenTime)
                            : transform.DOLocalMove(tweenData.TransformDirection, tweenData.TweenTime));
                        break;
                    case Enums.TweenType.Rotate:
                        _activeSequence.Append(transform.DOLocalRotate(tweenData.TransformDirection, tweenData.TweenTime));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _activeSequence.SetLoops(_looped ? -1 : 1, LoopType.Yoyo);

            _activeSequence.Play();
        }

        public void StopAnimation()
        {
            if (_activeSequence.IsActive())
            {
                _activeSequence.Kill();
            }
        }
    }
}