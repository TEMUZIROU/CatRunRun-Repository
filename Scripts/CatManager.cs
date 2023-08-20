using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour {
    private GameObject _fishPrefab;
    private Animator _catAni;
    private Rigidbody2D _rbody;
    private Vector3 _fishPos;

    //カメラ座標
    private Vector3 _cameraPos;
    //移動量
    private float _vx;
    //移動スピード
    public float _catSpeed;
    //ジャンプ力
    public float _catJump;
    //ジャンプボタン押下判定
    private bool _pushFlag;
    //(スペースキー押しっぱなしかどうか)ジャンプ準備状態
    private bool _jumpFlag;
    //攻撃オブジェクト名
    public string _attackItemName;
    //魚の射出回数
    private int _attackCount;
    //クールタイマー
    private float _cooltimer;
    //打ち出すFishのx座標
    private float _fishPos_x;
    //接地回数
    private int _enterCount;
    //接地判定
    private bool _groundFlag;
    //方向(右左)
    //true = 右、false = 左
    private bool _dire;
    //硬直解除タイマー
    private float _friezeTimer;
    //ターンタイマー
    private float _turnTimer;
    //ステータス情報
    private int _catSt;
    //1 = 基本(棒立ち)
    //2 = 歩き
    //3 = ターン
    //4 = 硬直

    //攻撃状態情報
    private int _attackSt;
    //1 = 基本
    //2 = 攻撃(Fishの射出攻撃)

    void Awake() {
        //定義
        _fishPrefab = GameObject.Find(_attackItemName);
        _catAni = this.GetComponent<Animator>();
        _rbody = this.GetComponent<Rigidbody2D>();

    }

    void Start() {
        //初期化
        _catSt = 1;
        _attackSt = 1;
        _vx = 0;
        _turnTimer = 0;
        _friezeTimer = 0;
        _enterCount = 0;
        _cooltimer = 0;
        _attackCount = 0;
        _dire = true;//右から
        _pushFlag = false;
        _jumpFlag = false;
        _catAni.Play("Base");

    }

    void Update() {
        //毎回移動量を0に戻す
        _vx = 0;

        if (_catSt != 4) {//硬直状態の時はキー入力を受け付けない
            //キー入力受け付け
            //右移動
            if (Input.GetKey("right") || Input.GetKey("d")) {
                //ターン中は移動操作を受け付けない
                if (_catSt != 3) {
                    _vx = _catSpeed;

                    if (_dire == false) {
                        CatTurn();

                    }

                }

            //左移動
            } else if (Input.GetKey("left") || Input.GetKey("a")) {
                //ターン中は移動操作を受け付けない
                if (_catSt != 3) {
                    _vx = -_catSpeed;

                    if (_dire == true) {
                        CatTurn();

                    }

                }

            }

            //ジャンプ
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey("up") || Input.GetKey("w")) && _groundFlag == true) {
                if (_pushFlag == false) {
                    _jumpFlag = true;
                    _pushFlag = true;

                } else {
                    _pushFlag = false;

                }

            }

            //攻撃(Fishの射出攻撃)
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
                _attackSt = 2;

            }

        //0.2秒間硬直
        } else {
            _friezeTimer += Time.deltaTime;

            if (_friezeTimer >= 0.2) {
                _friezeTimer = 0;
                _catSt = 1;

            }

        }

    }


    void FixedUpdate() {
        //移動量に合わせて、ステ情報と再生アニメーションを変更する
        //基本 → 歩行
        if (_catSt == 1) {
            //右
            if (_vx > 0) {
                _catSt = 2;
                _catAni.Play("Walk");

            //左
            } else if (_vx < 0) {
                _catSt = 2;
                _catAni.Play("WalkLeft");

            }

        //歩行 → 基本
        } else if (_catSt == 2) {
            if (_vx == 0) {
                _catSt = 1;

                //右
                if (_dire == true) {
                    _catAni.Play("Base");

                //左
                } else {
                    _catAni.Play("BaseLeft");

                }

            }

        //ターン
        } else if (_catSt == 3) {
            //中割りタイマー
            _turnTimer += Time.deltaTime;

            //0.1秒で振り返る
            if (_turnTimer >= 0.1f) {
                _turnTimer = 0;

                if (_dire == true) {
                    _dire = false;
                    _catAni.Play("BaseLeft");

                } else {
                    _dire = true;
                    _catAni.Play("Base");

                }
                //基本形に戻す
                _catSt = 1;

            }

        }

        //実際にアクションを起こす
        //左右移動
        if (_catSt == 2) {
            this.transform.Translate(_vx / 50f, 0, 0);

        }

        //ジャンプ
        if (_jumpFlag == true) {
            _jumpFlag = false;
            _rbody.AddForce(new Vector2(0, _catJump), ForceMode2D.Impulse);

        }


        //攻撃(Fishの射出攻撃)
        if (_attackSt == 2) {
            ++_attackCount;

            //一回を超えたらFishの射出を止める
            if (_attackCount <= 1) {
                //猫の向きに合わせて、Fishの向きも変える
                if (_dire == true) {
                    _fishPos_x = this.transform.position.x + 1;

                } else {
                    _fishPos_x = this.transform.position.x - 1;

                }
                //Fishの出現座標を定める
                _fishPos = new Vector3(_fishPos_x, this.transform.position.y, 0);

                //インスタンス生成
                _fishPrefab = (GameObject)Resources.Load("Prefabs/" + _attackItemName);
                Instantiate(_fishPrefab, _fishPos, Quaternion.identity);

                //第2ステータスを基本形に戻す
                _attackSt = 1;

            //Fishの射出が止まったら、クールタイムが0.2秒入る
            } else {
                _cooltimer += Time.deltaTime;

                //クールタイムが過ぎたら、射出回数とタイマーをリセットする
                if (_cooltimer >= 0.5) {
                    _cooltimer = 0;
                    _attackCount = 0;

                }

            }

        }

        //足場から落ちた時のGame Over処理
        if (this.transform.position.y < -5) {
            GameOver();

        }

    }

    //メインカメラにCatを追従させる
    void LateUpdate() {
        _cameraPos = this.transform.position;

        if (_cameraPos.y < 0) {
            _cameraPos.y = 0;

        }

        _cameraPos.z = -10;
        Camera.main.gameObject.transform.position = _cameraPos;

    }

    //接地してすぐは硬直状態オン(最初の一回はノーカン)
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "NP_Floor") {
            if (_enterCount >= 1) {
                //ステを硬直にする
                _catSt = 4;

                //Catのアニメーションを基本形に戻す
                if (_dire == true) {
                    _catAni.Play("Base");

                } else {
                    _catAni.Play("BaseLeft");

                }

            }
            _enterCount++;

            //接地判定オン
            _groundFlag = true;

        }

        //接触時のGame Over処理
        if (collision.gameObject.tag == "Enemy") {
            GameOver();

        }

    }


    //接地判定オフ
    void OnTriggerExit2D(Collider2D collision) {
        _groundFlag = false;

    }

    //ターンメソッド
    void CatTurn() {
        _catAni.Play("Turn");
        _catSt = 3;
        _turnTimer = 0;

    }

    //GameOverメソッド
    void GameOver() {
        _catAni.enabled = false;
        this.gameObject.SetActive(false);
        Debug.Log("Game Over");

    }

}