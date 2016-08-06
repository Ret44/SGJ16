using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player : MonoBehaviour {

    public static Player instance;

    public GameObject playerModel;
    public Transform sphereTransform;
    public bool dead;
    public float range;
    public float HP;
    public float speed;
    public Vector3 velocity;

    public Vector3 targetScale;

    public SphereCollider sphereCollider;

    public void Awake()
    {
        instance = this;
        targetScale = playerModel.transform.localScale;
    }

    public void CallEnd()
    {
        sphereTransform.localScale = new Vector3(0f, 1f, 0f);
        sphereCollider.enabled = false;
    }

    public void ModelTweenEnd()
    {
        playerModel.transform.DOScale(targetScale, 0.25f);
    }

    public void SendCall()
    {
        if (!dead)
        {
            sphereTransform.DOScaleX(range, 0.5f);
            sphereTransform.DOScaleZ(range, 0.5f).OnComplete(CallEnd);

            sphereCollider.enabled = true;
            playerModel.transform.DOScale(0.5f, 0.25f).SetEase(Ease.InBounce).OnComplete(ModelTweenEnd);
        }
    }

    public void Die()
    {
        if(!dead) {
        dead = true;
        playerModel.transform.DOScaleX(0.4f, 0.1f);
        playerModel.transform.DOScaleZ(0.01f, 0.1f);
        playerModel.transform.DOLocalRotate(new Vector3(-90f, 0f, 0f), 0.1f);
        playerModel.transform.DOLocalMoveY(-0.25f, 0.1f);
        ParticleController.SpawnBlood(this.transform.position);
        }
    }

    public void Update()
    {
        if (!dead)
        {
            this.transform.Translate(velocity * speed * Time.deltaTime);
            playerModel.transform.rotation = Quaternion.LookRotation(velocity);
            playerModel.transform.localPosition = Vector3.zero;
            if (HP < 0)
                Die();

            if (HP < 100)
                HP += 0.5f;
       } 
    }
}
