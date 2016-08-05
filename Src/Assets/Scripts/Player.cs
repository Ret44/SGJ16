using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour {

    public static Player instance;

    public GameObject playerModel;
    public Transform sphereTransform;

    public float range;

    public float speed;
    public Vector3 velocity;

    public Vector3 targetScale;
    
    public void Awake()
    {
        instance = this;
        targetScale = playerModel.transform.localScale;
    }

    public void CallEnd()
    {
        sphereTransform.localScale = new Vector3(0f, 1f, 0f);
    }

    public void ModelTweenEnd()
    {
        playerModel.transform.DOScale(targetScale, 0.25f);
    }

    public void SendCall()
    {
        sphereTransform.DOScaleX(range, 0.5f);
        sphereTransform.DOScaleZ(range, 0.5f).OnComplete(CallEnd);

        playerModel.transform.DOScale(0.5f, 0.25f).SetEase(Ease.InBounce).OnComplete(ModelTweenEnd);
    }

    public void Update()
    {
        this.transform.Translate(velocity * speed * Time.deltaTime);
        playerModel.transform.rotation = Quaternion.LookRotation(velocity);
        playerModel.transform.localPosition = Vector3.zero;
    }
}
