using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour {
    private GameObject _fishPrefab;
    private Animator _catAni;
    private Rigidbody2D _rbody;
    private Vector3 _fishPos;

    //�J�������W
    private Vector3 _cameraPos;
    //�ړ���
    private float _vx;
    //�ړ��X�s�[�h
    public float _catSpeed;
    //�W�����v��
    public float _catJump;
    //�W�����v�{�^����������
    private bool _pushFlag;
    //(�X�y�[�X�L�[�������ςȂ����ǂ���)�W�����v�������
    private bool _jumpFlag;
    //�U���I�u�W�F�N�g��
    public string _attackItemName;
    //���̎ˏo��
    private int _attackCount;
    //�N�[���^�C�}�[
    private float _cooltimer;
    //�ł��o��Fish��x���W
    private float _fishPos_x;
    //�ڒn��
    private int _enterCount;
    //�ڒn����
    private bool _groundFlag;
    //����(�E��)
    //true = �E�Afalse = ��
    private bool _dire;
    //�d�������^�C�}�[
    private float _friezeTimer;
    //�^�[���^�C�}�[
    private float _turnTimer;
    //�X�e�[�^�X���
    private int _catSt;
    //1 = ��{(�_����)
    //2 = ����
    //3 = �^�[��
    //4 = �d��

    //�U����ԏ��
    private int _attackSt;
    //1 = ��{
    //2 = �U��(Fish�̎ˏo�U��)

    void Awake() {
        //��`
        _fishPrefab = GameObject.Find(_attackItemName);
        _catAni = this.GetComponent<Animator>();
        _rbody = this.GetComponent<Rigidbody2D>();

    }

    void Start() {
        //������
        _catSt = 1;
        _attackSt = 1;
        _vx = 0;
        _turnTimer = 0;
        _friezeTimer = 0;
        _enterCount = 0;
        _cooltimer = 0;
        _attackCount = 0;
        _dire = true;//�E����
        _pushFlag = false;
        _jumpFlag = false;
        _catAni.Play("Base");

    }

    void Update() {
        //����ړ��ʂ�0�ɖ߂�
        _vx = 0;

        if (_catSt != 4) {//�d����Ԃ̎��̓L�[���͂��󂯕t���Ȃ�
            //�L�[���͎󂯕t��
            //�E�ړ�
            if (Input.GetKey("right") || Input.GetKey("d")) {
                //�^�[�����͈ړ�������󂯕t���Ȃ�
                if (_catSt != 3) {
                    _vx = _catSpeed;

                    if (_dire == false) {
                        CatTurn();

                    }

                }

            //���ړ�
            } else if (Input.GetKey("left") || Input.GetKey("a")) {
                //�^�[�����͈ړ�������󂯕t���Ȃ�
                if (_catSt != 3) {
                    _vx = -_catSpeed;

                    if (_dire == true) {
                        CatTurn();

                    }

                }

            }

            //�W�����v
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey("up") || Input.GetKey("w")) && _groundFlag == true) {
                if (_pushFlag == false) {
                    _jumpFlag = true;
                    _pushFlag = true;

                } else {
                    _pushFlag = false;

                }

            }

            //�U��(Fish�̎ˏo�U��)
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
                _attackSt = 2;

            }

        //0.2�b�ԍd��
        } else {
            _friezeTimer += Time.deltaTime;

            if (_friezeTimer >= 0.2) {
                _friezeTimer = 0;
                _catSt = 1;

            }

        }

    }


    void FixedUpdate() {
        //�ړ��ʂɍ��킹�āA�X�e���ƍĐ��A�j���[�V������ύX����
        //��{ �� ���s
        if (_catSt == 1) {
            //�E
            if (_vx > 0) {
                _catSt = 2;
                _catAni.Play("Walk");

            //��
            } else if (_vx < 0) {
                _catSt = 2;
                _catAni.Play("WalkLeft");

            }

        //���s �� ��{
        } else if (_catSt == 2) {
            if (_vx == 0) {
                _catSt = 1;

                //�E
                if (_dire == true) {
                    _catAni.Play("Base");

                //��
                } else {
                    _catAni.Play("BaseLeft");

                }

            }

        //�^�[��
        } else if (_catSt == 3) {
            //������^�C�}�[
            _turnTimer += Time.deltaTime;

            //0.1�b�ŐU��Ԃ�
            if (_turnTimer >= 0.1f) {
                _turnTimer = 0;

                if (_dire == true) {
                    _dire = false;
                    _catAni.Play("BaseLeft");

                } else {
                    _dire = true;
                    _catAni.Play("Base");

                }
                //��{�`�ɖ߂�
                _catSt = 1;

            }

        }

        //���ۂɃA�N�V�������N����
        //���E�ړ�
        if (_catSt == 2) {
            this.transform.Translate(_vx / 50f, 0, 0);

        }

        //�W�����v
        if (_jumpFlag == true) {
            _jumpFlag = false;
            _rbody.AddForce(new Vector2(0, _catJump), ForceMode2D.Impulse);

        }


        //�U��(Fish�̎ˏo�U��)
        if (_attackSt == 2) {
            ++_attackCount;

            //���𒴂�����Fish�̎ˏo���~�߂�
            if (_attackCount <= 1) {
                //�L�̌����ɍ��킹�āAFish�̌������ς���
                if (_dire == true) {
                    _fishPos_x = this.transform.position.x + 1;

                } else {
                    _fishPos_x = this.transform.position.x - 1;

                }
                //Fish�̏o�����W���߂�
                _fishPos = new Vector3(_fishPos_x, this.transform.position.y, 0);

                //�C���X�^���X����
                _fishPrefab = (GameObject)Resources.Load("Prefabs/" + _attackItemName);
                Instantiate(_fishPrefab, _fishPos, Quaternion.identity);

                //��2�X�e�[�^�X����{�`�ɖ߂�
                _attackSt = 1;

            //Fish�̎ˏo���~�܂�����A�N�[���^�C����0.2�b����
            } else {
                _cooltimer += Time.deltaTime;

                //�N�[���^�C�����߂�����A�ˏo�񐔂ƃ^�C�}�[�����Z�b�g����
                if (_cooltimer >= 0.5) {
                    _cooltimer = 0;
                    _attackCount = 0;

                }

            }

        }

        //���ꂩ�痎��������Game Over����
        if (this.transform.position.y < -5) {
            GameOver();

        }

    }

    //���C���J������Cat��Ǐ]������
    void LateUpdate() {
        _cameraPos = this.transform.position;

        if (_cameraPos.y < 0) {
            _cameraPos.y = 0;

        }

        _cameraPos.z = -10;
        Camera.main.gameObject.transform.position = _cameraPos;

    }

    //�ڒn���Ă����͍d����ԃI��(�ŏ��̈��̓m�[�J��)
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "NP_Floor") {
            if (_enterCount >= 1) {
                //�X�e���d���ɂ���
                _catSt = 4;

                //Cat�̃A�j���[�V��������{�`�ɖ߂�
                if (_dire == true) {
                    _catAni.Play("Base");

                } else {
                    _catAni.Play("BaseLeft");

                }

            }
            _enterCount++;

            //�ڒn����I��
            _groundFlag = true;

        }

        //�ڐG����Game Over����
        if (collision.gameObject.tag == "Enemy") {
            GameOver();

        }

    }


    //�ڒn����I�t
    void OnTriggerExit2D(Collider2D collision) {
        _groundFlag = false;

    }

    //�^�[�����\�b�h
    void CatTurn() {
        _catAni.Play("Turn");
        _catSt = 3;
        _turnTimer = 0;

    }

    //GameOver���\�b�h
    void GameOver() {
        _catAni.enabled = false;
        this.gameObject.SetActive(false);
        Debug.Log("Game Over");

    }

}