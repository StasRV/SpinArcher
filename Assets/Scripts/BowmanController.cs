using Spine;
using Spine.Unity;
using UnityEngine;

public class BowmanController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public float maxGunAngle = 60f;

    private bool isAiming = false;

    void Start()
    {
        if (skeletonAnimation == null)
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
        }
    }

    void Update()
    {
        // ��������� ������ � ����� ������������
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isAiming = false;
        }

        if (isAiming)
        {
            Aim();
        }
    }

    void Aim()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ����������� ���� �������� ����� gun
        angle = Mathf.Clamp(angle, -maxGunAngle, maxGunAngle);

        // ������� ����� gun
        Bone gunBone = skeletonAnimation.Skeleton.FindBone("gun");
        if (gunBone != null)
        {
            gunBone.Rotation = angle - 90f; // ������������� ���� ��������
        }
    }
}