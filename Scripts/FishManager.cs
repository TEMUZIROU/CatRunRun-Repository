using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour{
    private GameObject _playerObject;
    private Vector3 _scale;

    //射出スピード
    public float _speed;
    //射出量・方向
    private float _vx;
    //射出継続時間
    public float _delay;

    void Awake() {
        //定義
        _playerObject = GameObject.Find("Cat");

    }

    void Start() {
        _scale = this.transform.localScale;

        //Fishの出現場所がCatより右の場合は右向きで右に
        //そうでなければ左向きにして、左に飛ばす
        if (this.transform.position.x >= _playerObject.transform.position.x) {
            _vx = _speed;

        } else {
            _vx = -_speed;
            _scale.x *= -1;
            this.transform.localScale = _scale;

        }

        //射出したFishを_delay後、削除する
        Destroy(this.gameObject, _delay);

    }

    void FixedUpdate() {
        //1秒間に1回進む
        this.transform.Translate(_vx / 50f, 0, 0);

    }

    //敵キャラクターに当たった瞬間、敵キャラと当たったFishを削除する
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
