using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToShoot : MonoBehaviour
{
    public float power = 10f;
    Rigidbody2D rb;

    public float minPower, maxPower;

    public TrajectoryLine tl;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 currentPoint_;
    Vector3 endPoint;

    public GameObject arrow;
    Transform arrowSkin;
    Vector3 offset;

    public GameObject VFX_particle;
    float VFX_lifeTime = .4f;
    float VFX_lifeCount = .4f;
    [SerializeField] float multiply = 3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        offset = arrow.transform.position - transform.position;
        arrowSkin = arrow.transform.GetChild(0);
        arrow.SetActive(false);
        VFX_particle.SetActive(false);
    }
    void Update()
    {
        #region Drag & Shoot
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;

            //開啟箭頭
            arrow.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;
            currentPoint_ = currentPoint;
            tl.RenderLine(startPoint, currentPoint);
            if (transform.localScale.y > .8f)
                transform.localScale += new Vector3(.05f, -.05f, 0);
            float scaleRange = CalculatePowerPercent(startPoint, currentPoint);
            arrowSkin.transform.localScale = new Vector3( 1.2f, scaleRange, 1);
        }
        else
        {
            if (transform.localScale.y < 1f)
                transform.localScale += new Vector3(-.05f, .05f, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            force = CalculatePowerVectorV2(startPoint, endPoint);
            rb.AddForce(force, ForceMode2D.Impulse);
            tl.EndLine();
            rb.gravityScale = 1;

            //關閉箭頭
            arrow.SetActive(false);
            //開啟粒子
            VFX_particle.SetActive(true);
            VFX_lifeCount = VFX_lifeTime;
        }
        #endregion
        RotateArrow();
        VFXSystem();
    }

    private void VFXSystem()
    {
        if (VFX_lifeCount > 0)
        {
            VFX_lifeCount -= 1 * Time.deltaTime;
        }
        else
        {
            if (VFX_particle.activeInHierarchy) { 
                VFX_particle.SetActive(false);
            }
        }
    }

    private void RotateArrow()
    {
        arrow.transform.position = transform.position + offset;
        Vector2 direction = startPoint - currentPoint_;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        arrow.transform.rotation = rotation;
    }

    private Vector2 CalculatePowerVectorV2(Vector2 beginPoint, Vector2 endPoint)
    {
        Vector2 difference = beginPoint - endPoint;
        float vectorPower = difference.magnitude;
        vectorPower = Mathf.Clamp(vectorPower * multiply, minPower, maxPower);

        return difference.normalized * vectorPower;
    }
    private float CalculatePowerPercent(Vector2 beginPoint, Vector2 endPoint)
    {
        Vector2 difference = beginPoint - endPoint;
        float vectorPower = difference.magnitude;
        vectorPower = Mathf.Clamp(vectorPower, minPower, maxPower);
        float scaleRange = Mathf.Abs(vectorPower) * multiply / maxPower;
        scaleRange = Mathf.Clamp(scaleRange, .5f, 1f);

        return scaleRange;
    }
}
