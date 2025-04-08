using Spine;
using Spine.Unity;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    Bone gunBone,bulletBone;

    public Camera mainCamera;

    bool inAim;

    public GameObject point;
    private void Start()
    {
        gunBone = skeletonAnimation.Skeleton.FindBone("gun");
        bulletBone = skeletonAnimation.Skeleton.FindBone("bullet");
        skeletonAnimation.AnimationState.Complete += HandleAnimationComplete;

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (MousePosition().x <= transform.position.x)
            {
                StartAim();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndAim();
        }

        if (inAim)
        {
            CalculationAim();
        }
    }

    Vector2 MousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void CalculationAim()
    {
        Vector2 mousePosition = MousePosition();
        if (mousePosition.x <= transform.position.x)
        {
            Vector2 objectPosition = gunBone.GetLocalPosition();
            Vector2 direction = mousePosition - objectPosition;
            float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            float distance = Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y), new Vector2(objectPosition.x, objectPosition.y));

            Aim(angle, distance);
        }
    }
    void StartAim()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "attack_start", false);
        inAim = true;
    }

    void Aim(float angle, float power)
    {
        gunBone.Rotation = angle;
    }

    void EndAim()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "attack_finish", false);
        inAim = false;
    }

    void HandleAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "attack_finish")
        {
            if (!inAim)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
            }
        }
    }
}
