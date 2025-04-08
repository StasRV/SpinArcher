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
        // Обработка начала и конца прицеливания
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

        // Ограничение угла поворота кости gun
        angle = Mathf.Clamp(angle, -maxGunAngle, maxGunAngle);

        // Поворот кости gun
        Bone gunBone = skeletonAnimation.Skeleton.FindBone("gun");
        if (gunBone != null)
        {
            gunBone.Rotation = angle - 90f; // Корректировка угла поворота
        }
    }
}