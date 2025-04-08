using Spine;
using Spine.Unity;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public Rigidbody2D rb;
    float powerscale = 1.25f; // Костыль
    bool inFly = false;

    private void OnEnable()
    {
        inFly = false;
        skeletonAnimation.AnimationState.SetAnimation(0, "idle", false);
        ReserRb();
        skeletonAnimation.AnimationState.Complete += HandleAnimationComplete;
    }
    void ReserRb()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
    }
    public void Shoot(float power)
    {
        inFly = true;
        float angle = transform.rotation.eulerAngles.z;
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        rb.velocity = direction * power * powerscale;

    }
    void FixedUpdate()
    {
        if (inFly)  // Обновление поворота стрелы в зависимости от её скорости и направления
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inFly)
        {
            EndFly();
        }
    }

    void EndFly()
    {
        inFly = false;
        ReserRb();
        skeletonAnimation.AnimationState.SetAnimation(0, "attack", false);
    }

    void HandleAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "attack")
        {
            gameObject.SetActive(false);
        }
    }
}
