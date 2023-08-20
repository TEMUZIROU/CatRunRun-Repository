using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chow_Manager : MonoBehaviour {
    //
    private Vector3 _enemyPosition;
    //
    private Vector3 _scale;
    //
    private Rigidbody2D _rbody;
    //;
    private BoxCollider2D _collider;

    //�ˏo�X�s�[�h
    public float _speed;
    //�ˏo�ʁE����(X��)
    private float _vx;
    //�ˏo�̌X��(�Œ�)
    public float _pinned_vy;
    //�ˏo�p������
    public float _delay;
    //
    private bool _moveFlag;

    void Awake() {
        _rbody = this.GetComponent<Rigidbody2D>();
        _collider = this.GetComponent<BoxCollider2D>();
        _scale = this.transform.localScale;

    }

    void Start(){
        //�ˏo
        _pinned_vy += 1.5f;
        //
        _moveFlag = true;

        //Cat_chow�̏o���ꏊ����������Enemy���E�̏ꍇ�͉E�����ŉE��
        //�����łȂ���΍������ɂ��āA���ɔ�΂�
        if (this.transform.position.x >= _enemyPosition.x) {
            _vx = _speed;

        } else {
            _vx = -_speed;
            _scale.x *= -1;
            this.transform.localScale = _scale;

        }

        //�ˏo����Fish��_delay��A�폜����
        Destroy(this.gameObject, _delay);

    }

    void FixedUpdate(){
        if (_moveFlag == true) {
            _rbody.AddForce(new Vector2(_vx, _pinned_vy), ForceMode2D.Impulse);

        }else{
            Destroy(this.gameObject, 0);

        }
        
        _rbody.gravityScale += 3;

    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "NP_Floor") {
            _moveFlag = false;

        } else if (collision.gameObject.tag == "Enemy") {
            _collider.enabled = false;

        }

    }

    //EnemyManager�ŌĂяo��
    public void PosInfo_Deliv(Vector3 _ePos) {
        _enemyPosition = _ePos;
        
    }

}
