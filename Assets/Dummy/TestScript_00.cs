using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // ��Ʈ�� ������ ����Ʈ �ؾ���

public class TestScript_00 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>().DOMoveX(transform.position.x + 4, 3); /// <<<< ��Ʈ�� �Լ� ��� ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
