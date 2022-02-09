using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneDirector : MonoBehaviour
{
    public GameObject PrefabCircle;
    public GameObject PrefabCross;
    public GameObject Result;
    public GameObject TextResult;

    int nowPlayer;
    int[] board =
    {
        -1,-1,-1,
        -1,-1,-1,
        -1,-1,-1
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���̃��[�h�֍s�����ǂ���
        bool next = false;

        //�u������
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(null != hit.collider)
            {
                Vector3 pos = hit.collider.gameObject.transform.position;

                //�擾�������W����z��ԍ���߂�
                int x = (int)pos.x + 1;
                int y = (int)pos.y + 1;

                int idx = x + y * 3;

                //�����u����Ă��Ȃ����
                if( -1 == board[idx])
                {
                    GameObject prefab = PrefabCircle;
                    if (1 == nowPlayer) prefab = PrefabCross;

                    Instantiate(prefab, pos, Quaternion.identity);

                    board[idx] = nowPlayer;
                    next = true;
                }
            }
        }

        // ���s�`�F�b�N
        if (next)
        {
            bool win = false;

            List<int[]> lines = new List<int[]>();
            lines.Add(new int[] { 0, 1, 2 });
            lines.Add(new int[] { 3, 4, 5 });
            lines.Add(new int[] { 6, 7, 8 });
            lines.Add(new int[] { 0, 3, 6 });
            lines.Add(new int[] { 1, 4, 7 });
            lines.Add(new int[] { 2, 5, 8 });
            lines.Add(new int[] { 0, 4, 8 });
            lines.Add(new int[] { 2, 4, 6 });

            foreach (var v in lines)
            {
                bool issame = true;
                for (int i = 0; i < v.Length; i++)
                {
                    int idx0 = v[0];
                    int idx1 = v[i];

                    //������Ă��Ȃ��p�^�[��
                    if (0 > board[idx1] || board[idx0] != board[idx1]) issame = false;
                }

                if (issame)
                {
                    win = true;
                }
            }

            //���s�`�F�b�N
            if(win)
            {
                Result.SetActive(true);
                TextResult.GetComponent<Text>().text = (nowPlayer + 1) + "P�̏����I";
            }
            else
            {
                //��������
                int cnt = 0;
                for(int i = 0; i < board.Length; i++)
                {
                    if (-1 == board[i]) cnt++;
                }

                if(0 == cnt)
                {
                    Result.SetActive(true);
                    TextResult.GetComponent<Text>().text = "��������!";
                }

                //���̃v���C���[��
                nowPlayer++;
                if (2 <= nowPlayer) nowPlayer = 0;
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
