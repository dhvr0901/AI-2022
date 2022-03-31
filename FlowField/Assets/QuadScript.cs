using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadScript : MonoBehaviour
{
    Material mMaterial;
    MeshRenderer mMeshRenderer;

    float[] mPoints;
    int mHitCount;

    float mDelay;

    // Start is called before the first frame update
    void Start()
    {
        mDelay = 3;

        mMeshRenderer = GetComponent<MeshRenderer>();
        mMaterial = mMeshRenderer.material;

        mPoints = new float[32 * 3]; //32 point
    }

    // Update is called once per frame
    void Update()
    {
        mDelay -= Time.deltaTime;
        if(mDelay <=0)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Projectile"));
            go.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            mDelay = 0.5f;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint cp in collision.contacts)
        {
            Debug.Log("Contact with object " + cp.otherCollider.gameObject.name);

            Vector3 StartOfRay = cp.point - cp.normal;
            Vector3 RayDir = cp.normal * 1.5f;

            Ray ray = new Ray(StartOfRay, RayDir);
            RaycastHit hit;

            bool hitit = Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask("HeatMapLayer"));
            Debug.DrawRay(StartOfRay, RayDir, Color.white, 10);
            if (hitit)
            {
                Debug.Log("hit Object " + hit.collider.gameObject.name);
                Debug.Log("Hit Texture coordinates = " + hit.textureCoord.x + ", " + hit.textureCoord.y);
                addHitPoint(hit.textureCoord.x *4-2, hit.textureCoord.y *4-2);
            }

            Destroy(cp.otherCollider.gameObject);
        }
    }

    public void addHitPoint(float xp, float yp)
    {
        mPoints[mHitCount * 3] = xp;
        mPoints[mHitCount * 3 + 1] = yp;
        mPoints[mHitCount * 3 + 2] = Random.Range(1f, 3f);

        mHitCount++;
        mHitCount %= 32;

        mMaterial.SetFloatArray("_Hits", mPoints);
        mMaterial.SetInt("_HitCount", mHitCount);
    }
}
