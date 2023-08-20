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

    //射出スピード
    public float _speed;
    //射出量・方向(X軸)
    private float _vx;
    //射出の傾き(固定)
    public float _pinned_vy;
    //射出継続時間
    public float _delay;
    //
    private bool _moveFlag;

    void Awake() {
        _rbody = this.GetComponent<Rigidbody2D>();
        _collider = this.GetComponent<BoxCollider2D>();
        _scale = this.transform.localScale;

    }

    void Start(){
        //射出
        _pinned_vy += 1.5f;
        //
        _moveFlag = true;

        //Cat_chowの出現場所が生成元のEnemyより右の場合は右向きで右に
        //そうでなければ左向きにして、左に飛ばす
        if (this.transform.position.x >= _enemyPosition.x) {
            _vx = _speed;

        } else {
            _vx = -_speed;
            _scale.x *= -1;
            this.transform.localScale = _scale;

        }

        //射出したFishを_delay後、削除する
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

    //EnemyManagerで呼び出す
    public void PosInfo_Deliv(Vector3 _ePos) {
        _enemyPosition = _ePos;
        
    }

}
