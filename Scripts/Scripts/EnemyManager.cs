using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    //アニメーター
    private Animator _animator;
    //スケール
    private Vector3 _scale;
    //リジットボディ
    private Rigidbody2D _rbody;
    //
    private GameObject _chowObject;
    //
    public GameObject _chowPrefab;
    //
    private Vector3 _chowPos;

    //ステータス情報
    private int _st;
    //_st = 1　基本形
    //_st = 2　移動
    //_st = 3 振り向き
    //_st = 4 ジャンプ

    //移動スピード
    public float _walkSpeed;
    //ジャンプ力
    public float _jumpPower;
    //
    public string _attackItemName;
    //x方向
    private bool _x_dire;
    //タイマー
    private float _timer;
    //ジャンプ数(ジャンプは端までの間1回まで)
    private int _jumpCount;
    //移動乱数(1の時移動する)
    private int _walkRan;
    //ジャンプ乱数(1の時ジャンプする)
    private int _jumpRun;
    //
    private float _chowPos_X;
    //
    private int _attackCount;
    //
    private float _cooltimer;
    //
    private int _chowRan;


    void Awake() {
        //定義
        _animator = this.GetComponent<Animator>();
        _scale = this.transform.localScale;
        _rbody = GetComponent<Rigidbody2D>();

    }
    void Start() {
        //初期化
        _animator.Play("Base");
        _st = 1;
        _timer = 0;
        _jumpCount = 0;
        _x_dire = false;
        _attackCount = 0;
        _cooltimer = 0f;

    }

    void FixedUpdate() {
        //歩行中で、ジャンプを一度もしていない場合
        if (_st == 2 && _jumpCount == 0) {
            //さらにランダムに抽選をする
            _jumpRun = Random.Range(0, 500);

            //ジャンプ準備
            if (_jumpRun == 1) {
                if (_x_dire == true) {
                    _animator.Play("BaseRight");

                } else {
                    _animator.Play("Base");

                }
                _st = 4;

            }

            //ジャンプ処理
        } else if (_st == 4 && _jumpCount == 0) {
            _rbody.AddForce(new Vector2(0, _jumpPower), ForceMode2D.Impulse);

        }

        if (_st == 1) {
            _timer += Time.deltaTime;

            //0.5秒ずつ判断する
            if (_timer >= 0.5) {
                _walkRan = Random.Range(0, 20);

                //左準備
                if (_walkRan == 1 && _x_dire == false) {
                    _timer = 0;
                    _st = 2;
                    _animator.Play("Walk");

                    //右準備
                } else if (_walkRan == 1 && _x_dire == true) {
                    _timer = 0;
                    _st = 2;
                    _animator.Play("WalkRight");

                }

            }

        } else if (_st == 2) {
            //左移動
            if (_x_dire == true) {
                transform.Translate(_walkSpeed / 50, 0, 0);

                //右移動
            } else {
                transform.Translate(-_walkSpeed / 50, 0, 0);

            }

            //ターン終了
        } else if (_st == 3) {
            _timer += Time.deltaTime;
            if (_timer >= 0.1f) {
                _timer = 0;
                _st = 1;
                _jumpCount = 0;

                //右→左
                if (_x_dire == true) {
                    _x_dire = false;
                    _animator.Play("Base");

                    //左→右
                } else {
                    _x_dire = true;
                    _animator.Play("BaseRight");

                }

            }

        }

        //チュールの射出による攻撃
        if (_st != 3) {
            _chowRan = Random.Range(0,100);
            if (_chowRan == 1) {
                //5秒に１回だけCat_chowを射出する
                if (_attackCount < 1) {
                    //敵の向きに合わせて、Cat_chowの向きも変える
                    if (_x_dire == true) {
                        _chowPos_X = this.transform.position.x + 1;

                    } else {
                        _chowPos_X = this.transform.position.x - 1;

                    }
                    //Cat_chowの出現座標を定める
                    _chowPos = new Vector3(_chowPos_X, this.transform.position.y + 1, 0);
                    //インスタンス生成
                    _chowObject = Instantiate(_chowPrefab, _chowPos, Quaternion.identity);

                    //生成したチュールのchow_Managerから
                    //PosInfo_Delivを呼び出し自身の位置情報を渡す
                    _chowObject.GetComponent<chow_Manager>().PosInfo_Deliv(this.gameObject.transform.position);

                    ++_attackCount;

                }

            } else {
                _cooltimer += Time.deltaTime;

                //クールタイムが過ぎたら、射出回数とタイマーをリセットする
                if (_cooltimer >= 3) {
                    _cooltimer = 0;
                    _attackCount = 0;

                }

            }

        }

    }

    void OnTriggerExit2D(Collider2D collision) {
        //端につくまでのジャンプの回数を数える
        if (_st == 4 && (collision.tag == "Floor" || collision.tag == "NP_Floor")) {
            _jumpCount += 1;

        //足場の端に付いたら反転する
        } else if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "NP_Floor" && _st == 2) {
            _st = 3;
            _timer = 0;
            _animator.Play("Turn");
            this.transform.localScale = _scale;

        }

    }

    void OnTriggerEnter2D(Collider2D collision) {
        //着地時、アニメーションとステータスを戻す
        if (_st == 4 && (collision.tag == "Floor" || collision.tag == "NP_Floor")) {
            if (_x_dire == true) {
                _animator.Play("WalkRight");

            } else {
                _animator.Play("Walk");

            }
            _st = 2;

        }

        if (collision.name == "Cat_chow") {


        }

    }

}
