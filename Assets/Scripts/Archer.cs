using Spine;
using Spine.Unity;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    Bone gunBone;    // ость которую вращаем при прицеливании
    Bone bulletBone; // ость из которой идет выстрел
    Bone armR3Bone;  // ость правой руки (относительно ее рассчитываем угол выстрела)
    const float gravity = -9.81f; // ”скорение свободного падени€
    [HideInInspector]
    public Vector2 tensionPoint; //ѕозици€ нат€жени€ (точка где игрок удерживает курсор)
    bool inAim;

    public Arrow arrow;

    public GameObject trajectoryPointPrefab; // ѕрефаб точки траектории
    public int pointsCount = 5;
    private GameObject[] trajectoryPoints;

    public float maxPower, sensitivityPower;
    float power; // текуща€ сила выстрела
    private void Start()
    {
        gunBone = skeletonAnimation.Skeleton.FindBone("gun");
        bulletBone = skeletonAnimation.Skeleton.FindBone("bullet");
        armR3Bone = skeletonAnimation.Skeleton.FindBone("armR3");
        skeletonAnimation.AnimationState.Complete += HandleAnimationComplete;
        skeletonAnimation.AnimationState.Event += HandleAnimationEvent;

        CreateTrajectoryPoints();
    }

    void CreateTrajectoryPoints()
    {
        trajectoryPoints = new GameObject[pointsCount + 1];
        float startScale = 1.0f;
        float endScale = 0.1f;

        for (int i = 0; i <= pointsCount; i++)
        {
            float scale = Mathf.Lerp(startScale, endScale, (float)i / pointsCount);

            trajectoryPoints[i] = Instantiate(trajectoryPointPrefab, Vector3.zero, Quaternion.identity);
            trajectoryPoints[i].transform.parent = trajectoryPointPrefab.transform.parent;
            trajectoryPoints[i].transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    private void Update()
    {
        if (inAim)
        {
            Aim();
        }
    }

    public void StartAim()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "attack_start", false);
        inAim = true;
    }

    void Aim()
    {
        if (tensionPoint.x <= transform.position.x)
        {
            Vector2 objectPosition = gunBone.GetLocalPosition();
            Vector2 direction = tensionPoint - objectPosition;
            float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            float distance = Vector2.Distance(new Vector2(tensionPoint.x, tensionPoint.y), new Vector2(objectPosition.x, objectPosition.y));
            gunBone.Rotation = angle;
            power = Mathf.Min(distance * sensitivityPower, maxPower);
            DrawTrajectory(power);
        }
    }

    public void EndAim()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "attack_finish", false);
        inAim = false;
        for (int i = 0; i <= pointsCount; i++)
        {
            trajectoryPoints[i].gameObject.SetActive(false);
        }
    }

    void DrawTrajectory(float power)
    {
        for (int i = 1; i <= pointsCount; i++) //ћожно задать начальное i=0 чтобы отображалась точка с начала траектории
        {
            float time = i / (float)pointsCount;
            Vector2 position = CalculateTrajectoryPoint(time, power);
            trajectoryPoints[i].gameObject.SetActive(true);
            trajectoryPoints[i].transform.position = position;
        }
    }

    Vector2 CalculateTrajectoryPoint(float time, float power)
    {
        Vector2 objectPosition = bulletBone.GetWorldPosition(transform);
        Vector2 objectPosition2 = armR3Bone.GetWorldPosition(transform);
        Vector2 direction = objectPosition2 - objectPosition;
        Vector2 initialVelocity = -direction * power;
        Vector2 position = objectPosition + (initialVelocity * time) + (0.5f * gravity * time * time * Vector2.up);
        return position;
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
    void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "shoot")
        {
            Shoot();
        }
    }

    void Shoot()
    {
        arrow.gameObject.transform.position = bulletBone.GetWorldPosition(transform);
        arrow.gameObject.transform.rotation = bulletBone.GetQuaternion();
        arrow.gameObject.SetActive(true);
        arrow.Shoot(power);
    }
}
