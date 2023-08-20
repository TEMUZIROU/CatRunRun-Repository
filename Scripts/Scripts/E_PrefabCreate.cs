using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_PrefabCreate : MonoBehaviour {
    //
    private FloorCreate _FloorCreate;

    //�e�G�L�����̃v���n�u���i�[
    public GameObject[] _enemyPrfabs;
    //�X�|�[���|�C���g
    private Vector3[] _enemyPos;
    //�X�|�[���|�C���g�̐�
    private int _floorCount;
    //�G��ނ̑I��p����
    private int _enemyRan;

    void Awake() {
        //����쐬�X�N���v�g���`����
        _FloorCreate = GetComponent<FloorCreate>();
        //���ۂɑ�����쐬����
        _FloorCreate.NewFloor_Create();

        //�|�b�v�ł��鑫��̐����擾����
        _floorCount = GameObject.FindGameObjectsWithTag("Floor").Length;
        //�e�I�u�W�F�N�g�̒�`
        _enemyPos = new Vector3[_floorCount];

        for (int i = 0; i > _enemyPrfabs.Length; i++) {
            _enemyPrfabs[i] = new GameObject();

        }

    }

    void Start() {
        //�z��Ɋi�[����Ă���v���n�u���e���W�Ƀ����_����������
        for (int i = 0; i < _floorCount; i++) {
            _enemyRan = Random.Range(0, _enemyPrfabs.Length);
            //�o�����W���߂�
            _enemyPos[i] = GameObject.FindGameObjectsWithTag("Floor")[i].transform.position;
            _enemyPos[i].y += 1f;
            //�C���X�^���X����
            Instantiate(_enemyPrfabs[_enemyRan], _enemyPos[i], Quaternion.identity);

        }

    }

}