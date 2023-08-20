using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCreate : MonoBehaviour{
    //�J�n���̑���I�u�W�F�N�g
    private GameObject _Start_Floor;
    //�e����̃v���n�u������
    public GameObject[] _floorPrfabs;
    private Vector2 _nextFloor_pos;

    //�������鑫��̐�
    public int _floorQuantity;
    //�����ނ̑I��p����
    private int _floorRan;
    //����c�ʒu�p����
    private float _floorPos_X_Ran;
    //���ꉡ�ʒu�p����
    private float _floorPos_Y_Ran;
    //
    private string _floorType;

    public void NewFloor_Create(){
        //�J�n���̑���I�u�W�F�N�g���擾����
        _Start_Floor = GameObject.Find("Start_Floor");

        //�e��ނ̑���v���n�u���`����
        for (int i = 0; i > _floorPrfabs.Length; i++) {
            _floorPrfabs[i] = new GameObject();

        }

        //Start_Floor���ŏ��̔�r�Ώۂɂ���
        _nextFloor_pos = _Start_Floor.transform.position;

        //����𐶐�����
        for (int i = 0; i < _floorQuantity; i++) {
            //����̎�ނ��m�肷��
            _floorRan = Random.Range(0, _floorPrfabs.Length);

            if (i == 0) {//�ŏ��̑�����N�_�ɂ���ꍇ(�ŏ��̂�)
                _floorPos_X_Ran = Random.Range(10f, 10.5f);
                _floorPos_Y_Ran = Random.Range(-1.5f, 1.5f);

            } else {

                //����̎�ޖ��𔲂��o��
                _floorType = _floorPrfabs[Random.Range(0, _floorPrfabs.Length)].name;

                //���̑���̎�ނɂ���č��W�������߂遦�Q�[���i�s�s�\��h������
                switch (_floorType) {
                    case "BlockFloor_Box":
                        _floorPos_X_Ran = Random.Range(5f, 5.5f);
                        _floorPos_Y_Ran = Random.Range(-1.5f, 1.0f);
                        break;
                    case "BlockFloor_Long":
                        _floorPos_X_Ran = Random.Range(5f, 5.5f);
                        _floorPos_Y_Ran = Random.Range(-1.5f, 1.0f);
                        break;
                    case "BlockFloor_Standard":
                        _floorPos_X_Ran = Random.Range(5f, 5.5f);
                        _floorPos_Y_Ran = Random.Range(-1.5f, 1.0f);
                        break;
                    case "BlockFloor_Tall":
                        _floorPos_X_Ran = Random.Range(5f, 5.5f);
                        _floorPos_Y_Ran = Random.Range(-2f, -1.5f);
                        break;

                }

            }

            //����̍��W���m�肷��
            _nextFloor_pos.x += _floorPos_X_Ran;
            _nextFloor_pos.y += _floorPos_Y_Ran;

            //�Œ჉�C�����߂�
            if (_nextFloor_pos.y <= -4) {
                _nextFloor_pos.y = -3;

            }

            //�C���X�^���X����
            Instantiate(_floorPrfabs[_floorRan], _nextFloor_pos, Quaternion.identity);

        }
        
    }

}
