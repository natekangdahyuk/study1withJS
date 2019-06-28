using UnityEngine;


public static class GameOption
{
    public static float BGMVolume;
    public static float EffectVoluem;
    public static float VoiceVoluem;
    public static float Sensitivity;
    public static bool Alarm;
    public static bool LowMode = false;
    public static int BitSort = 0;
    public static int GradeSort = 0;
    public static bool FirstEnter = true;
    public static bool bAutoPlay = true;
    public static bool bContinuePlay = false;
    public static int Frame = 40;

    public static int[,] bitValue = new int[4, 10];
    public static int[] bitType = new int[4];

    static public void LoadOption()
    {
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        EffectVoluem = PlayerPrefs.GetFloat("EffectVoluem", 0.5f);
        VoiceVoluem = PlayerPrefs.GetFloat("VoiceVoluem", 0.5f);
        Sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        Alarm = PlayerPrefs.GetInt("Alarm", 1) > 0 ? true : false;
        FirstEnter = PlayerPrefs.GetInt("FirstEnter", 1) > 0 ? true : false;
        BitSort = PlayerPrefs.GetInt("BitSort", 1);
        GradeSort = PlayerPrefs.GetInt("GradeSort", 1);
        LowMode = PlayerPrefs.GetInt("LowMode", 0) > 0 ? true : false;
        bAutoPlay = PlayerPrefs.GetInt("bAutoPlay", 0) > 0 ? true : false;
        bContinuePlay = PlayerPrefs.GetInt("bContinuePlay", 0) > 0 ? true : false;
        Frame = PlayerPrefs.GetInt("Frame", 40);

        for (int z = 0; z < 4; z++)
        {
            for (int i = 0; i < 10; i++)
            {
                bitValue[z, i] = PlayerPrefs.GetInt("bitValue" + z.ToString() + i.ToString(), 0);
            }
        }

        for (int i = 0; i < 4; i++)
            bitType[i] = PlayerPrefs.GetInt("bitType" + i.ToString(), 0);



        Application.targetFrameRate = GameOption.Frame;
    }

    static public void SaveOption()
    {
        if (BGMVolume > 0.5f)
            BGMVolume = 0.5f;

        if (EffectVoluem > 0.5f)
            EffectVoluem = 0.5f;

        if (VoiceVoluem > 0.5f)
            VoiceVoluem = 0.5f;

        PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
        PlayerPrefs.SetFloat("EffectVoluem", EffectVoluem);
        PlayerPrefs.SetFloat("VoiceVoluem", VoiceVoluem);
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
        PlayerPrefs.SetInt("Alarm", Alarm == true ? 1 : 0);
        PlayerPrefs.SetInt("FirstEnter", FirstEnter == true ? 1 : 0);
        PlayerPrefs.SetInt("BitSort", BitSort);
        PlayerPrefs.SetInt("GradeSort", GradeSort);
        PlayerPrefs.SetInt("LowMode", LowMode == true ? 1 : 0);
        PlayerPrefs.SetInt("bAutoPlay", bAutoPlay == true ? 1 : 0);
        PlayerPrefs.SetInt("bContinuePlay", bContinuePlay == true ? 1 : 0);
        PlayerPrefs.SetInt("Frame", Frame);



        Application.targetFrameRate = GameOption.Frame;
    }

    static public void SaveBit(int[] bitvalue, int index, int bittype)
    {
        for (int i = 0; i < 10; i++)
        {
            bitValue[index, i] = bitvalue[i];
        }

        bitType[index] = bittype;


        for (int z = 0; z < 4; z++)
        {
            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.SetInt("bitValue" + z.ToString() + i.ToString(), bitValue[z, i]);
            }
        }

        for (int i = 0; i < 4; i++)
            PlayerPrefs.SetInt("bitType" + i.ToString(), bitType[i]);


    }


}


