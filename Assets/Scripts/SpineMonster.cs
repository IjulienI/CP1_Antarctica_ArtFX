using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Examples
{
    public class SpineMonster : MonoBehaviour
    {

        public float timeScale = 0.5f;
        [SpineBone] public string firstBoneName, secondBoneName, thirdBoneName, fourBoneName;

        [Header("Settings")]
        public Vector2 footSize;
        public float footRayRaise = 2f;
        public float comfyDistance = 1f;
        public float centerOfGravityXOffset = -0.25f;
        public float feetTooFarApartThreshold = 3f;
        public float offBalanceThreshold = 1.4f;
        public float minimumSpaceBetweenFeet = 0.5f;
        public float maxNewStepDisplacement = 2f;
        public float shuffleDistance = 1f;
        public float baseLerpSpeed = 3.5f;
        public FootMovement forward, backward;

        [Header("Debug")]
        [SerializeField] float balance;
        [SerializeField] float distanceBetweenFeet;
        [SerializeField] protected Foot firstFoot, secondFoot, thirdFoot, fourFoot;
        Foot stepFoot, nextFoot, otherFoot, lastFoot;

        Skeleton skeleton;
        Bone firstFootBone, secondFootBone, thirdFootBone, fourFootBone;

        [System.Serializable]
        public class FootMovement
        {
            public AnimationCurve xMoveCurve;
            public AnimationCurve raiseCurve;
            public float maxRaise;
            public float minDistanceCompensate;
            public float maxDistanceCompensate;
        }

        [System.Serializable]
        public class Foot
        {
            public Vector2 worldPos;
            public float displacementFromCenter;
            public float distanceFromCenter;

            [Space]
            public float lerp;
            public Vector2 worldPosPrev;
            public Vector2 worldPosNext;

            public bool IsStepInProgress { get { return lerp < 1f; } }
            public bool IsPrettyMuchDoneStepping { get { return lerp > 0.7f; } }

            public void UpdateDistance(float centerOfGravityX)
            {
                displacementFromCenter = worldPos.x - centerOfGravityX;
                distanceFromCenter = Mathf.Abs(displacementFromCenter);
            }

            public void StartNewStep(float newDistance, float centerOfGravityX, float tentativeY, float footRayRaise, RaycastHit2D[] hits, Vector2 footSize)
            {
                lerp = 0f;
                worldPosPrev = worldPos;
                float newX = centerOfGravityX - newDistance;
                Vector2 origin = new Vector2(newX, tentativeY + footRayRaise);
                int hitCount = Physics2D.BoxCast(origin, footSize, 0f, Vector2.down, new ContactFilter2D { useTriggers = false }, hits);
                worldPosNext = hitCount > 0 ? hits[0].point : new Vector2(newX, tentativeY);
            }

            public void UpdateStepProgress(float deltaTime, float stepSpeed, float shuffleDistance, FootMovement forwardMovement, FootMovement backwardMovement)
            {
                if (!this.IsStepInProgress)
                    return;

                lerp += deltaTime * stepSpeed;

                float strideSignedSize = worldPosNext.x - worldPosPrev.x;
                float strideSign = Mathf.Sign(strideSignedSize);
                float strideSize = (Mathf.Abs(strideSignedSize));

                FootMovement movement = strideSign > 0 ? forwardMovement : backwardMovement;

                worldPos.x = Mathf.Lerp(worldPosPrev.x, worldPosNext.x, movement.xMoveCurve.Evaluate(lerp));
                float groundLevel = Mathf.Lerp(worldPosPrev.y, worldPosNext.y, lerp);

                if (strideSize > shuffleDistance)
                {
                    float strideSizeFootRaise = Mathf.Clamp((strideSize * 0.5f), 1f, 2f);
                    worldPos.y = groundLevel + (movement.raiseCurve.Evaluate(lerp) * movement.maxRaise * strideSizeFootRaise);
                }
                else
                {
                    lerp += Time.deltaTime;
                    worldPos.y = groundLevel;
                }

                if (lerp > 1f)
                    lerp = 1f;
            }

            public static float GetNewDisplacement(float otherLegDisplacementFromCenter, float comfyDistance, float minimumFootDistanceX, float maxNewStepDisplacement, FootMovement forwardMovement, FootMovement backwardMovement)
            {
                FootMovement movement = Mathf.Sign(otherLegDisplacementFromCenter) < 0 ? forwardMovement : backwardMovement;
                float randomCompensate = Random.Range(movement.minDistanceCompensate, movement.maxDistanceCompensate);

                float newDisplacement = (otherLegDisplacementFromCenter * randomCompensate);
                if (Mathf.Abs(newDisplacement) > maxNewStepDisplacement || Mathf.Abs(otherLegDisplacementFromCenter) < minimumFootDistanceX)
                    newDisplacement = comfyDistance * Mathf.Sign(newDisplacement) * randomCompensate;

                return newDisplacement;
            }

        }

        public float Balance { get { return balance; } }

        void Start()
        {
            Time.timeScale = timeScale;
            Vector3 tpos = transform.position;

            // Default starting positions.
            firstFoot.worldPos = tpos;
            firstFoot.worldPos.x -= comfyDistance * 2;
            firstFoot.worldPosPrev = firstFoot.worldPosNext = firstFoot.worldPos;

            secondFoot.worldPos = tpos;
            secondFoot.worldPos.x -= comfyDistance;
            secondFoot.worldPosPrev = secondFoot.worldPosNext = secondFoot.worldPos;

            thirdFoot.worldPos = tpos;
            thirdFoot.worldPos.x += comfyDistance;
            thirdFoot.worldPosPrev = thirdFoot.worldPosNext = thirdFoot.worldPos;

            fourFoot.worldPos = tpos;
            fourFoot.worldPos.x += comfyDistance * 2;
            fourFoot.worldPosPrev = fourFoot.worldPosNext = fourFoot.worldPos;

            SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();
            skeleton = skeletonAnimation.Skeleton;

            skeletonAnimation.UpdateLocal += UpdateLocal;

            firstFootBone = skeleton.FindBone(firstBoneName);
            secondFootBone = skeleton.FindBone(secondBoneName);
            thirdFootBone = skeleton.FindBone(thirdBoneName);
            fourFootBone = skeleton.FindBone(fourBoneName);

            firstFoot.lerp = 1f;
            secondFoot.lerp = 1f;
            thirdFoot.lerp = 1f;
            fourFoot.lerp = 1f;

            stepFoot = fourFoot;
        }

        RaycastHit2D[] hits = new RaycastHit2D[1];

        private void UpdateLocal(ISkeletonAnimation animated)
        {
            Transform thisTransform = transform;

            Vector2 thisTransformPosition = thisTransform.position;
            float centerOfGravityX = thisTransformPosition.x + centerOfGravityXOffset;

            firstFoot.UpdateDistance(centerOfGravityX);
            secondFoot.UpdateDistance(centerOfGravityX);
            thirdFoot.UpdateDistance(centerOfGravityX);
            fourFoot.UpdateDistance(centerOfGravityX);
            balance = firstFoot.displacementFromCenter + secondFoot.displacementFromCenter + thirdFoot.displacementFromCenter + fourFoot.displacementFromCenter;
            distanceBetweenFeet = Mathf.Abs(firstFoot.worldPos.x - secondFoot.worldPos.x - thirdFoot.worldPos.x - fourFoot.worldPos.x);

            // Detect time to make a new step
            bool isTooOffBalance = Mathf.Abs(balance) > offBalanceThreshold;
            bool isFeetTooFarApart = distanceBetweenFeet > feetTooFarApartThreshold;
            bool timeForNewStep = isFeetTooFarApart || isTooOffBalance;
            if (timeForNewStep)
            {

                // Choose which foot to use for next step.


                if(stepFoot == secondFoot)
                {
                    stepFoot = thirdFoot;
                    nextFoot = fourFoot;
                    otherFoot = firstFoot;
                    lastFoot = secondFoot;
                }
                else if (stepFoot == thirdFoot)
                {
                    stepFoot = fourFoot;
                    nextFoot = firstFoot;
                    otherFoot = secondFoot;
                    lastFoot = thirdFoot;
                }
                else if(stepFoot == fourFoot)
                {
                    stepFoot = firstFoot;
                    nextFoot = secondFoot;
                    otherFoot = thirdFoot;
                    lastFoot = fourFoot;
                }
                else 
                { 
                    stepFoot = secondFoot;
                    nextFoot = thirdFoot;
                    otherFoot = fourFoot;
                    lastFoot = firstFoot;
                }

                // Start a new step.
                if (!stepFoot.IsStepInProgress && nextFoot.IsPrettyMuchDoneStepping)
                {
                    float newDisplacement = Foot.GetNewDisplacement(nextFoot.displacementFromCenter, comfyDistance, minimumSpaceBetweenFeet, maxNewStepDisplacement, forward, backward);
                    stepFoot.StartNewStep(newDisplacement, centerOfGravityX, thisTransformPosition.y, footRayRaise, hits, footSize);
                }

            }


            float deltaTime = Time.deltaTime;
            float stepSpeed = baseLerpSpeed;
            stepSpeed += (Mathf.Abs(balance) - 0.6f) * 2.5f;

            // Animate steps that are in progress.
            firstFoot.UpdateStepProgress(deltaTime, stepSpeed, shuffleDistance, forward, backward);
            secondFoot.UpdateStepProgress(deltaTime, stepSpeed, shuffleDistance, forward, backward);
            thirdFoot.UpdateStepProgress(deltaTime, stepSpeed, shuffleDistance, forward, backward);
            fourFoot.UpdateStepProgress(deltaTime, stepSpeed, shuffleDistance, forward, backward);

            firstFootBone.SetLocalPosition(thisTransform.InverseTransformPoint(firstFoot.worldPos));
            secondFootBone.SetLocalPosition(thisTransform.InverseTransformPoint(secondFoot.worldPos));
            thirdFootBone.SetLocalPosition(thisTransform.InverseTransformPoint(thirdFoot.worldPos));
            fourFootBone.SetLocalPosition(thisTransform.InverseTransformPoint(fourFoot.worldPos));
        }



        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                const float Radius = 0.15f;

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(firstFoot.worldPos, Radius);
                Gizmos.DrawWireSphere(firstFoot.worldPosNext, Radius);

                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(secondFoot.worldPos, Radius);
                Gizmos.DrawWireSphere(secondFoot.worldPosNext, Radius);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(thirdFoot.worldPos, Radius);
                Gizmos.DrawWireSphere(thirdFoot.worldPosNext, Radius);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(fourFoot.worldPos, Radius);
                Gizmos.DrawWireSphere(fourFoot.worldPosNext, Radius);
            }
        }

    }
}