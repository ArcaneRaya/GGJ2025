using UnityEngine;
using TMPro;

// The bubble entity
public class BubbleEntity : MonoBehaviour {

    public TextMeshPro              mTextMesh;
    public string                   mDefaultText;
    public BubbleTarget[]           mTargets;
    public FloatingEffect           mFloatingEffect;
    public float                    mMoveSmoothTime             = 0.3f;
    public float                    mMoveMaxSpeed               = 3;
    public float                    mLetterInterval             = 0.1f;

    private BubbleTarget            mNearbyTarget;
    private bool                    mFoundTarget                = false;
    private bool                    isDragging                  = false;
    private string                  mTargetText;
    private int                     mCurrentTextLength;
    private float                   mTimeSinceLastLetter        = 0;
    private Vector3                 mTargetPosition;
    private Vector3                 velocity                    = Vector3.zero;

    private void Start()
    {
        mFloatingEffect.StartFloating();
        mTargetPosition = transform.position;
        mTextMesh.text = mDefaultText;
        mTargetText = mDefaultText + " ...";
        mCurrentTextLength = mTargetText.Length;
    }

    private void OnMouseDown()
    {
        if (mFoundTarget)
            return;

        isDragging = true;
        mFloatingEffect.StopFloating();
    }

    private void OnMouseUp()
    {
        if (mFoundTarget)
            return;

        if (mNearbyTarget != null)
        {
            mTargetPosition = mNearbyTarget.transform.position;
            mFoundTarget = true;
        }

        isDragging = false;
        mFloatingEffect.StartFloating();
    }

    private void Update()
    {
        if (!mFoundTarget && isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mTargetPosition = new Vector3(mousePos.x, mousePos.y, transform.position.z);

            // Clamp target position within screen
            mTargetPosition = new Vector3(Mathf.Clamp(mTargetPosition.x, -17, 17), Mathf.Clamp(mTargetPosition.y, -14, 14), mTargetPosition.z);

            BubbleTarget new_target = null;
            foreach (BubbleTarget target in mTargets)
            { 
                if (Vector3.Distance(mTargetPosition, target.gameObject.transform.position) < 2.5f)
                {
                    new_target = target;
                    break;
                }
            }

            if (new_target != mNearbyTarget)
            {
                mNearbyTarget = new_target;
                mTargetText = mDefaultText + " " + (mNearbyTarget == null ? "..." : mNearbyTarget.mTargetText);
                mCurrentTextLength = mDefaultText.Length;
                mTimeSinceLastLetter = 0;
            }
        }

        // Update text
        if (mCurrentTextLength != mTargetText.Length)
        {
            mTimeSinceLastLetter += Time.deltaTime;

            if (mTimeSinceLastLetter >= mLetterInterval)
            {
                mCurrentTextLength++;
                mTimeSinceLastLetter -= mLetterInterval;
            }

            mTextMesh.text = mTargetText.Substring(0, mCurrentTextLength);
        }

        transform.position = Vector3.SmoothDamp(transform.position, mTargetPosition, ref velocity, mMoveSmoothTime, mMoveMaxSpeed);
    }
}
