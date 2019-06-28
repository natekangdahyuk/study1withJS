using UnityEngine;
using UnityEngine.UI;

public class CutUI : MonoBehaviour
{
    public Animation anim;
    public RawImage chacrater;
    public RawImage text;

    public void Play( bool bCri , int comboCount , string Name , int bit )
    {
        if(bit != 4 )
        {
            if( bCri )
            {
                text.texture = ResourceManager.LoadTexture( "img_char_cut_text_critical" );
                gameObject.SetActive( true );
                anim.Play();
                chacrater.texture = ResourceManager.LoadTexture( "img_char_cut_" + Name );
                //SoundManager.I.Play( SoundManager.SoundType.voice , "char_snd_" + Name + "_cut_01" , GameOption.VoiceVoluem );
            }

        }

        switch( comboCount )
        {
            case 3:
            text.texture = ResourceManager.LoadTexture( "img_char_cut_text_combo_05" );
            gameObject.SetActive( true );
            anim.Play();
            chacrater.texture = ResourceManager.LoadTexture( "img_char_cut_" + Name );
            //SoundManager.I.Play( SoundManager.SoundType.voice , "char_snd_" + Name + "_cut_01" , GameOption.VoiceVoluem );
            break;

            case 6:
            text.texture = ResourceManager.LoadTexture( "img_char_cut_text_combo_10" );
            gameObject.SetActive( true );
            anim.Play();
            chacrater.texture = ResourceManager.LoadTexture( "img_char_cut_" + Name );
            //SoundManager.I.Play( SoundManager.SoundType.voice , "char_snd_" + Name + "_cut_01" , GameOption.VoiceVoluem );
            break;

            case 9:
            text.texture = ResourceManager.LoadTexture( "img_char_cut_text_combo_15" );
            gameObject.SetActive( true );
            anim.Play();
            chacrater.texture = ResourceManager.LoadTexture( "img_char_cut_" + Name );
            //SoundManager.I.Play( SoundManager.SoundType.voice , "char_snd_" + Name + "_cut_01" , GameOption.VoiceVoluem );
            break;

            case 12:
            text.texture = ResourceManager.LoadTexture( "img_char_cut_text_combo_20" );
            gameObject.SetActive( true );
            anim.Play();
            chacrater.texture = ResourceManager.LoadTexture( "img_char_cut_" + Name );
            //SoundManager.I.Play( SoundManager.SoundType.voice , "char_snd_" + Name + "_cut_01" , GameOption.VoiceVoluem );
            break;

            case 15:
            text.texture = ResourceManager.LoadTexture( "img_char_cut_text_combo_25" );
            gameObject.SetActive( true );
            anim.Play();
            chacrater.texture = ResourceManager.LoadTexture( "img_char_cut_" + Name );
            //SoundManager.I.Play( SoundManager.SoundType.voice , "char_snd_" + Name + "_cut_01" , GameOption.VoiceVoluem );
            break;

            default:
            {
                if( comboCount >= 18)
                {
                    if(comboCount % 3 == 0 )
                    {
                        text.texture = ResourceManager.LoadTexture( "img_char_cut_text_combo_30" );
                        gameObject.SetActive( true );
                        anim.Play();
                        chacrater.texture = ResourceManager.LoadTexture( "img_char_cut_" + Name );
                    }
                }                
                //SoundManager.I.Play( SoundManager.SoundType.voice , "char_snd_" + Name + "_cut_01" , GameOption.VoiceVoluem );
            }
            break;

        }

        
    }

}