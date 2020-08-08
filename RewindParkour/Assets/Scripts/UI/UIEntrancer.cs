using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*
 * A class that handles enter/exit animations for a UI panel.
 */
[RequireComponent(typeof(CanvasGroup))]
public class UIEntrancer : MonoBehaviour
{
    /*
     * Three preset entrance animations.
     * SLIDE: Panel will move from offscreen point A to onscreen point B on a straight path.
     * ALPHA: Panel will fade in from fully transparent to fully opaque. When not visible, the panel can be stored in an offscreen location.
     * SCALE: Panel will start at (0, 0, 0) scale and animate towards a non-zero scale. When not visible, the panel can be stored in an offscreen location.
     */
    public enum UIEntranceType {
        SLIDE,
        ALPHA,
        SCALE
    }

    public CanvasGroup CanvasGroup { get; private set; }
    public RectTransform RectTransform { get; private set; }

    public bool IsOnScreen { get; private set; }

#pragma warning disable 649
    [SerializeField] private UIEntranceType type;
    [SerializeField] private float enterTime;
    [SerializeField] private Ease enterEase;
    [SerializeField] private float exitTime;
    [SerializeField] private Ease exitEase;
    

    [Header("Animation Info")]
    [SerializeField] private bool isOnInEditor;
    [SerializeField] private bool startsOnAtRuntime;
    [SerializeField] private bool animatesOnStart;
    [SerializeField] private bool usesRealtime;
    [SerializeField] private Vector2 moveOffset;
#pragma warning restore 649

    private Tween currentTween;

    public Vector2 onScreenPosition { get; private set; }
    public Vector2 offScreenPosition { get; private set; }
    private float baseAlpha;
    private Vector3 baseScale;

    private bool inited;

    // prepares the UIEntrancer for use.
    private void Init() {
        inited = true;
        
        RectTransform = transform as RectTransform;
        CanvasGroup = GetComponent<CanvasGroup>();

        onScreenPosition  = isOnInEditor ? RectTransform.anchoredPosition : RectTransform.anchoredPosition + moveOffset;
        offScreenPosition = isOnInEditor ? RectTransform.anchoredPosition - moveOffset : RectTransform.anchoredPosition;

        baseAlpha = CanvasGroup.alpha;
        baseScale = RectTransform.localScale;
        
        SetState(animatesOnStart ? !startsOnAtRuntime : startsOnAtRuntime);
    }


    private void Awake() {
        if (!inited) {
            Init();
        }
    }

    private void Start() {
        if (animatesOnStart) {
            if (startsOnAtRuntime) {
                Enter();
            } else {
                Exit();
            }
        }
    }


    /*
     * Animates the panel on screen.
     */
    public Tween Enter() {
        if (!inited) {
            Init();
        }

        currentTween?.Kill();

        switch (type) {
            case UIEntranceType.SLIDE:
                currentTween = RectTransform.DOAnchorPos(onScreenPosition, enterTime).SetEase(enterEase);
                break;
            case UIEntranceType.ALPHA:
                currentTween = DOTween.Sequence().Insert(
                    0, RectTransform.DOAnchorPos(onScreenPosition, 0).SetEase(enterEase)
                ).Insert(
                    0, CanvasGroup.DOFade(baseAlpha, enterTime).SetEase(enterEase)
                );
                break;
            case UIEntranceType.SCALE:
                currentTween = DOTween.Sequence().Insert(
                    0, RectTransform.DOAnchorPos(onScreenPosition, 0).SetEase(enterEase)
                ).Insert(
                    0, RectTransform.DOScale(baseScale, enterTime).SetEase(enterEase)
                );
                break;
        }

        if (usesRealtime) {
            currentTween = currentTween.SetUpdate(true);
        }

        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;

        IsOnScreen = true;

        return currentTween;
    }

    /*
     * Animates the panel off screen.
     */
    public Tween Exit() {
        if (!inited) {
            Init();
        }

        currentTween?.Kill();

        switch (type) {
            case UIEntranceType.SLIDE:
                currentTween = RectTransform.DOAnchorPos(offScreenPosition, exitTime).SetEase(exitEase);
                break;
            case UIEntranceType.ALPHA:
                currentTween = DOTween.Sequence().Append(
                    CanvasGroup.DOFade(0, exitTime).SetEase(exitEase)
                ).Append(
                    RectTransform.DOAnchorPos(offScreenPosition, 0)
                );
                break;
            case UIEntranceType.SCALE:
                currentTween = DOTween.Sequence().Append(
                    RectTransform.DOScale(Vector3.zero, exitTime).SetEase(exitEase)
                ).Append(
                    RectTransform.DOAnchorPos(offScreenPosition, 0)
                );
                break;
        }

        if (usesRealtime) {
            currentTween = currentTween.SetUpdate(true);
        }

        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;

        IsOnScreen = false;

        return currentTween;
    }

    /*
     * Animates the panel on screen, keeps it there for a given amount of time, and then removes the panel.
     */
    public Tween EnterThenExit(float duration) {
        if (!inited) {
            Init();
        }
        
        currentTween?.Kill(true);

        // grab enter animation
        Enter();
        Tween enterAnim = currentTween;

        // grab exit animation
        currentTween = null;
        Exit();
        Tween exitAnim = currentTween;

        // link animations together
        if (enterAnim is Sequence enterSeq) {
            currentTween = enterSeq.Insert(duration, exitAnim);
        } else {
            currentTween = DOTween.Sequence().Append(enterAnim).Insert(duration, exitAnim);
        }

        if (usesRealtime) {
            currentTween = currentTween.SetUpdate(true);
        }

        return currentTween;
    }


    /*
     * Immediately sets the panel's state to on screen without animating.
     */
    public void SetOn() => SetState(true);

    /*
     * Immediately sets the panel's state to off screen without animating.
     */
    public void SetOff() => SetState(false);

    // sets the panel to the state given by isOn
    private void SetState(bool isOn) {
        IsOnScreen = isOn;

        RectTransform.anchoredPosition = isOn ? onScreenPosition : offScreenPosition;

        switch (type) {
            case UIEntranceType.ALPHA:
                CanvasGroup.alpha = isOn ? baseAlpha : 0;
                break;
            case UIEntranceType.SCALE:
                RectTransform.localScale = isOn ? baseScale : Vector3.zero;
                break;
        }
    }


    public void Kill() {
        CanvasGroup.DOKill();
        RectTransform.DOKill();
    }

}
