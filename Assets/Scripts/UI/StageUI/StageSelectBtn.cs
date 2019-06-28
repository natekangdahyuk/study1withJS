using UnityEngine;
using UnityEngine.UI;


public class StageSelectBtn : MonoBehaviour
{
    [SerializeField]
    RawImage Bg;

    [SerializeField]
    RawImage select;

    [SerializeField]
    RawImage Lock;

    [SerializeField]
    GameObject CompleteGroup;

    [SerializeField]
    RawImage boss;

    RawImage Monster;
    RawImage Line;
    Text Title;
        
    GameObject[] Complete = new GameObject[3];

    void Awake()
    {
        Bg = transform.Find( "Bg" ).GetComponent<RawImage>();
        select = transform.Find("Bg_Select").GetComponent<RawImage>();
        Monster = transform.Find("ImageMask").Find("Monster").GetComponent<RawImage>();
        Lock = transform.Find("Bg_Lock").GetComponent<RawImage>();
        boss = transform.Find("Boss").GetComponent<RawImage>();
        Line = transform.Find("Line").GetComponent<RawImage>();
        CompleteGroup = transform.Find("CompleteGroup").gameObject;
        Title = CompleteGroup.transform.Find("Title").GetComponent<Text>();
        for (int i = 0; i < Complete.Length; i++)
        {
            Complete[i] = CompleteGroup.transform.Find("CompleteStar" + (i + 1).ToString()).gameObject;
        }
    }

    public void Apply(StageReferenceData stageData, bool bLock, int star, int Difficulty)
    {
        MonsterDetailReferenceData detaildata = MonsterDetailTBL.GetData(stageData.MonsterIndex);
        MonsterReferenceData data = MonsterTBL.GetData(detaildata.MonsterIndex);

        if (data != null)
            Monster.texture = ResourceManager.LoadTexture(data.LobbyTexture);
        Lock.gameObject.SetActive(bLock);
        Monster.gameObject.SetActive(!bLock);
        CompleteGroup.gameObject.SetActive(!bLock);
        Title.text = StringTBL.GetData(stageData.StageName);
        for (int i = 0; i < Complete.Length; i++)
        {
            if (bLock)
            {
                Complete[i].gameObject.SetActive(false);
            }
            else
            {
                Complete[i].gameObject.SetActive(i < star);
            }
        }

        if (Difficulty == 1)
        {
            Bg.texture = Resources.Load("UIResource/btn_stage_normal") as Texture;
            Lock.texture = Resources.Load("UIResource/btn_stage_normal_lock") as Texture;
            select.texture = Resources.Load("UIResource/img_stage_focus_normal") as Texture;
            Line.texture = Resources.Load("UIResource/img_stage_line_normal") as Texture;
        }
        else if (Difficulty == 2)
        {
            Bg.texture = Resources.Load("UIResource/btn_stage_hard") as Texture;
            Lock.texture = Resources.Load("UIResource/btn_stage_hard_lock") as Texture;
            select.texture = Resources.Load("UIResource/img_stage_focus_hard") as Texture;
            Line.texture = Resources.Load("UIResource/img_stage_line_hard") as Texture;
        }
        else
        {
            Bg.texture = Resources.Load("UIResource/btn_stage_hell") as Texture;
            Lock.texture = Resources.Load("UIResource/btn_stage_hell_lock") as Texture;
            select.texture = Resources.Load("UIResource/img_stage_focus_hell") as Texture;
            Line.texture = Resources.Load("UIResource/img_stage_line_hell") as Texture;
        }

        if (stageData.StageIcon == 1)
        {
            boss.gameObject.SetActive(false);            
        }
        else if (stageData.StageIcon == 2)
        {
            boss.gameObject.SetActive(true);
            boss.texture = Resources.Load("UIResource/img_stage_boss_mid") as Texture;
        }
        else if (stageData.StageIcon == 3)
        {
            boss.gameObject.SetActive(true);
            boss.texture = Resources.Load("UIResource/img_stage_boss_big") as Texture;
        }
        else if (stageData.StageIcon == 4)
        {
            boss.gameObject.SetActive(true);
            boss.texture = Resources.Load("UIResource/img_stage_boss_named") as Texture;
        }
    }


    public bool IsLock()
    {
        return Lock.gameObject.activeSelf;
    }
    public void Select( bool bSelect )
    {
        select.gameObject.SetActive(bSelect);
    }

    public bool IsSelect()
    {
        return select.gameObject.activeSelf;
    }
}

