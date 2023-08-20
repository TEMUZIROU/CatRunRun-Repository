using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour{
    private GameObject _playerObject;
    private Vector3 _scale;

    //�ˏo�X�s�[�h
    public float _speed;
    //�ˏo�ʁE����
    private float _vx;
    //�ˏo�p������
    public float _delay;

    void Awake() {
        //��`
        _playerObject = GameObject.Find("Cat");

    }

    void Start() {
        _scale = this.transform.localScale;

        //Fish�̏o���ꏊ��Cat���E�̏ꍇ�͉E�����ŉE��
        //�����łȂ���΍������ɂ��āA���ɔ�΂�
        if (this.transform.position.x >= _playerObject.transform.position.x) {
            _vx = _speed;

        } else {
            _vx = -_speed;
            _scale.x *= -1;
            this.transform.localScale = _scale;

        }

        //�ˏo����Fish��_delay��A�폜����
        Destroy(this.gameObject, _delay);

    }

    void FixedUpdate() {
        //1�b�Ԃ�1��i��
        this.transform.Translate(_vx / 50f, 0, 0);

    }

    //�G�L�����N�^�[�ɓ��������u�ԁA�G�L�����Ɠ�������Fish���폜����
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            Destroy(collision.gameObject);
            //collision.gameObject.SetActive(false);

        }

        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "NP_Floor") {
            Destroy(this.gameObject);

        }

    }

}
