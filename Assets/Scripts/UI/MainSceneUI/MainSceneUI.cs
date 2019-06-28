using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Live2D.Cubism.Rendering;

public class MainSceneUI : baseUI
{
    [SerializeField]
    GameObject representParentPosition = null;

    [SerializeField]
    GameObject representParentBGPosition = null;


    GameObject Live2DModel;
    GameObject Live2DBG;
    GameObject Live2DFx;

    [SerializeField]
    GameObject BGEffectParent;

    [SerializeField]
    Text CharacterName;

    [SerializeField]
    Text CharacterLevel;

    [SerializeField]
    RawImage starImage;

    [SerializeField]
    Text CharacterDesc;

    [SerializeField]
    Animation TouchAnim;


    [SerializeField]
    GameObject[] NewList;

    string Live2DStr;
    public override void Init()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.OnEnter();
        topbar.ChangeScene(this);
        topbar.Set(this);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Apply();
        NetManager.GetMailList();
        NetManager.GetAchieveUser();

        SetNew(NewType.mission, MissionManager.I.IsNew());
    }


    public static bool bOnce = false;
    public void Apply()
    {
        gameObject.SetActive(true);
        CardData card = InventoryManager.I.representCharacter;

        if (card == null)
        {
            Debug.Log("## Inventory represent card value is null");
            return;
        }

        if (InventoryManager.I.representCharacter.Live2DModel != Live2DStr)
        {
            Live2DStr = InventoryManager.I.representCharacter.Live2DModel;
            if (Live2DModel != null)
            {
                GameObject.Destroy(Live2DModel.gameObject);
                GameObject.Destroy(Live2DBG.gameObject);
            }

            if (Live2DFx != null)
                GameObject.Destroy(Live2DFx.gameObject);

            Live2DModel = ResourceManager.Load(representParentPosition, InventoryManager.I.representCharacter.Live2DModel);
            Live2DBG = ResourceManager.Load(representParentBGPosition, InventoryManager.I.representCharacter.Live2DBG);

            if (Live2DModel)
            {
                CubismRenderController controller = Live2DModel.GetComponent<CubismRenderController>();

                if (controller)
                    controller.DepthOffset = 5;
            }

            //if( GameOption.LowMode == false )
            {
                string str = InventoryManager.I.representCharacter.Live2DBG;
                str = str.Replace("bg", "fx");
                Live2DFx = ResourceManager.Load(BGEffectParent, str);
            }


        }

        if (bOnce == false)
        {
            SoundManager.I.Play(SoundManager.SoundType.voice, card.Voice, GameOption.VoiceVoluem);
            bOnce = true;
        }


        CharacterDesc.text = card.referenceData.OneWord;
        //characterImage = ResourceManager.Load<RawImage>(representParentPosition, "Bundle/Textures/Girls/" + InventoryManager.I.representCharacter.texture);
        CharacterName.text = card.Name;
        //CharacterName.text = StringTBL.GetData(900034);
        CharacterLevel.text = card.Level.ToString();

        UIUtil.LoadStarEx(starImage, card.Star);

    }

    public void SetNew(NewType type, bool bShow)
    {
        NewList[(int)type].SetActive(bShow);
    }

    SoundObject touchSound = null;
    public void OnCharacterTouch()
    {
        CardData card = InventoryManager.I.representCharacter;

        if (card == null)
            return;

        if (TouchAnim.isPlaying)
        {
            TouchAnim.Stop();
            TouchAnim.Play();

        }
        else
        {
            TouchAnim.Play();
        }


        if (touchSound)
        {
            if (touchSound.IsPlay())
                return;
        }

        touchSound = SoundManager.I.Play(SoundManager.SoundType.voice, card.TouchSound, GameOption.VoiceVoluem);

    }
}