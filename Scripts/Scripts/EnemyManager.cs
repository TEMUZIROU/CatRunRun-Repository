using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    //�A�j���[�^�[
    private Animator _animator;
    //�X�P�[��
    private Vector3 _scale;
    //���W�b�g�{�f�B
    private Rigidbody2D _rbody;
    //
    private GameObject _chowObject;
    //
    public GameObject _chowPrefab;
    //
    private Vector3 _chowPos;

    //�X�e�[�^�X���
    private int _st;
    //_st = 1�@��{�`
    //_st = 2�@�ړ�
    //_st = 3 �U�����
    //_st = 4 �W�����v

    //�ړ��X�s�[�h
    public float _walkSpeed;
    //�W�����v��
    public float _jumpPower;
    //
    public string _attackItemName;
    //x����
    private bool _x_dire;
    //�^�C�}�[
    private float _timer;
    //�W�����v��(�W�����v�͒[�܂ł̊�1��܂�)
    private int _jumpCount;
    //�ړ�����(1�̎��ړ�����)
    private int _walkRan;
    //�W�����v����(1�̎��W�����v����)
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
        //��`
        _animator = this.GetComponent<Animator>();
        _scale = this.transform.localScale;
        _rbody = GetComponent<Rigidbody2D>();

    }
    void Start() {
        //������
        _animator.Play("Base");
        _st = 1;
        _timer = 0;
        _jumpCount = 0;
        _x_dire = false;
        _attackCount = 0;
        _cooltimer = 0f;

    }

    void FixedUpdate() {
        //���s���ŁA�W�����v����x�����Ă��Ȃ��ꍇ
        if (_st == 2 && _jumpCount == 0) {
            //����Ƀ����_���ɒ��I������
            _jumpRun = Random.Range(0, 500);

            //�W�����v����
            if (_jumpRun == 1) {
                if (_x_dire == true) {
                    _animator.Play("BaseRight");

                } else {
                    _animator.Play("Base");

                }
                _st = 4;

            }

            //�W�����v����
        } else if (_st == 4 && _jumpCount == 0) {
            _rbody.AddForce(new Vector2(0, _jumpPower), ForceMode2D.Impulse);

        }

        if (_st == 1) {
            _timer += Time.deltaTime;

            //0.5�b�����f����
            if (_timer >= 0.5) {
                _walkRan = Random.Range(0, 20);

                //������
                if (_walkRan == 1 && _x_dire == false) {
                    _timer = 0;
                    _st = 2;
                    _animator.Play("Walk");

                    //�E����
                } else if (_walkRan == 1 && _x_dire == true) {
                    _timer = 0;
                    _st = 2;
                    _animator.Play("WalkRight");

                }

            }

        } else if (_st == 2) {
            //���ړ�
            if (_x_dire == true) {
                transform.Translate(_walkSpeed / 50, 0, 0);

                //�E�ړ�
            } else {
                transform.Translate(-_walkSpeed / 50, 0, 0);

            }

            //�^�[���I��
        } else if (_st == 3) {
            _timer += Time.deltaTime;
            if (_timer >= 0.1f) {
                _timer = 0;
                _st = 1;
                _jumpCount = 0;

                //�E����
                if (_x_dire == true) {
                    _x_dire = false;
                    _animator.Play("Base");

                    //�����E
                } else {
                    _x_dire = true;
                    _animator.Play("BaseRight");

                }

            }

        }

        //�`���[���̎ˏo�ɂ��U��
        if (_st != 3) {
            _chowRan = Random.Range(0,100);
            if (_chowRan == 1) {
                //5�b�ɂP�񂾂�Cat_chow���ˏo����
                if (_attackCount < 1) {
                    //�G�̌����ɍ��킹�āACat_chow�̌������ς���
                    if (_x_dire == true) {
                        _chowPos_X = this.transform.position.x + 1;

                    } else {
                        _chowPos_X = this.transform.position.x - 1;

                    }
                    //Cat_chow�̏o�����W���߂�
                    _chowPos = new Vector3(_chowPos_X, this.transform.position.y + 1, 0);
                    //�C���X�^���X����
                    _chowObject = Instantiate(_chowPrefab, _chowPos, Quaternion.identity);

                    //���������`���[����chow_Manager����
                    //PosInfo_Deliv���Ăяo�����g�̈ʒu����n��
                    _chowObject.GetComponent<chow_Manager>().PosInfo_Deliv(this.gameObject.transform.position);

                    ++_attackCount;

                }

            } else {
                _cooltimer += Time.deltaTime;

                //�N�[���^�C�����߂�����A�ˏo�񐔂ƃ^�C�}�[�����Z�b�g����
                if (_cooltimer >= 3) {
                    _cooltimer = 0;
                    _attackCount = 0;

                }

            }

        }

    }

    void OnTriggerExit2D(Collider2D collision) {
        //�[�ɂ��܂ł̃W�����v�̉񐔂𐔂���
        if (_st == 4 && (collision.tag == "Floor" || collision.tag == "NP_Floor")) {
            _jumpCount += 1;

        //����̒[�ɕt�����甽�]����
        } else if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "NP_Floor" && _st == 2) {
            _st = 3;
            _timer = 0;
            _animator.Play("Turn");
            this.transform.localScale = _scale;

        }

    }

    void OnTriggerEnter2D(Collider2D collision) {
        //���n���A�A�j���[�V�����ƃX�e�[�^�X��߂�
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
