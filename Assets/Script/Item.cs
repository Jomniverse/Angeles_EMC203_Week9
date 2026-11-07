
using UnityEngine;


public class Item : MonoBehaviour
{
    [Header("Simulated 3D Position")]
    public Vector3 itemPosition;

    [Header("Manual Hitbox")]
    public Vector3 halfSize =
        new Vector3(0.5f, 0.5f, 0f);

    [Header("Perspective Position")]
    public float farY = 11f;

    public Vector3 centerOffset = Vector3.zero;

    [Header("Collision State")]
    public bool hasHitPlayer = false;

    [Header("Sprite Scale")]
    public Vector3 baseScale = Vector3.one;

    private void Update()
    {

        float perspective =
            CameraComponent.focalLenth /
            (CameraComponent.focalLenth + itemPosition.z);

        transform.localScale = baseScale * perspective;

        float screenY =
            itemPosition.y * perspective +
            farY * (1f - perspective);

        transform.position = new Vector3(
            itemPosition.x * perspective,
            screenY,
            0f
        );
    }

    private void LateUpdate()
    {
        
    }

    public Vector3 GetCollisionCenter()
    {
        return new Vector3(
            itemPosition.x + centerOffset.x,
            itemPosition.y + centerOffset.y,
            0f
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        float perspective =
            CameraComponent.focalLenth /
            (CameraComponent.focalLenth + itemPosition.z);

        Vector3 collisionCenter = GetCollisionCenter();

        float screenY =
            collisionCenter.y * perspective +
            farY * (1f - perspective);

        Vector3 displayCenter = new Vector3(
            collisionCenter.x * perspective,
            screenY,
            0f
        );

        Gizmos.DrawWireCube(
            displayCenter,
            halfSize * 2f * perspective
        );
    }
}
