using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCreate : MonoBehaviour{
    //開始時の足場オブジェクト
    private GameObject _Start_Floor;
    //各足場のプレハブを入れる
    public GameObject[] _floorPrfabs;
    private Vector2 _nextFloor_pos;

    //生成する足場の数
    public int _floorQuantity;
    //足場種類の選択用乱数
    private int _floorRan;
    //足場縦位置用乱数
    private float _floorPos_X_Ran;
    //足場横位置用乱数
    private float _floorPos_Y_Ran;
    //
    private string _floorType;

    public void NewFloor_Create(){
        //開始時の足場オブジェクトを取得する
        _Start_Floor = GameObject.Find("Start_Floor");

        //各種類の足場プレハブを定義する
        for (int i = 0; i > _floorPrfabs.Length; i++) {
            _floorPrfabs[i] = new GameObject();

        }

        //Start_Floorを最初の比較対象にする
        _nextFloor_pos = _Start_Floor.transform.position;

        //足場を生成する
        for (int i = 0; i < _floorQuantity; i++) {
            //足場の種類を確定する
            _floorRan = Random.Range(0, _floorPrfabs.Length);

            if (i == 0) {//最初の足場を起点にする場合(最初のみ)
                _floorPos_X_Ran = Random.Range(10f, 10.5f);
                _floorPos_Y_Ran = Random.Range(-1.5f, 1.5f);

            } else {

                //足場の種類名を抜き出す
                _floorType = _floorPrfabs[Random.Range(0, _floorPrfabs.Length)].name;

                //次の足場の種類によって座標幅を決める※ゲーム進行不能を防ぐため
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

            //足場の座標を確定する
            _nextFloor_pos.x += _floorPos_X_Ran;
            _nextFloor_pos.y += _floorPos_Y_Ran;

            //最低ラインを定める
            if (_nextFloor_pos.y <= -4) {
                _nextFloor_pos.y = -3;

            }

            //インスタンス生成
            Instantiate(_floorPrfabs[_floorRan], _nextFloor_pos, Quaternion.identity);

        }
        
    }

}
