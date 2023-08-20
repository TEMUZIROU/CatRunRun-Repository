using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_PrefabCreate : MonoBehaviour {
    //
    private FloorCreate _FloorCreate;

    //各敵キャラのプレハブを格納
    public GameObject[] _enemyPrfabs;
    //スポーンポイント
    private Vector3[] _enemyPos;
    //スポーンポイントの数
    private int _floorCount;
    //敵種類の選択用乱数
    private int _enemyRan;

    void Awake() {
        //足場作成スクリプトを定義する
        _FloorCreate = GetComponent<FloorCreate>();
        //実際に足場を作成する
        _FloorCreate.NewFloor_Create();

        //ポップできる足場の数を取得する
        _floorCount = GameObject.FindGameObjectsWithTag("Floor").Length;
        //各オブジェクトの定義
        _enemyPos = new Vector3[_floorCount];

        for (int i = 0; i > _enemyPrfabs.Length; i++) {
            _enemyPrfabs[i] = new GameObject();

        }

    }

    void Start() {
        //配列に格納されているプレハブを各座標にランダム生成する
        for (int i = 0; i < _floorCount; i++) {
            _enemyRan = Random.Range(0, _enemyPrfabs.Length);
            //出現座標を定める
            _enemyPos[i] = GameObject.FindGameObjectsWithTag("Floor")[i].transform.position;
            _enemyPos[i].y += 1f;
            //インスタンス生成
            Instantiate(_enemyPrfabs[_enemyRan], _enemyPos[i], Quaternion.identity);

        }

    }

}