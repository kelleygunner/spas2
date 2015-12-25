using UnityEngine;
using System.Collections;

public class AvatarPanel : MonoBehaviour 
{
    public Animator LenaSad;

    public Animator StepaNormal;
    public Animator LenaNormal;
    public Animator ArchiNormal;


    public void ShowAvatar(AvatarPersonage personage, AvatarEmotion emotion)
    {
        HideAll();

        switch (personage)
        {
            case AvatarPersonage.Stepa:
                {
                    switch (emotion)
                    { 
                        case AvatarEmotion.Sad :
                            {
                                break;
                            }
                        case AvatarEmotion.Normal:
                            {
                                StepaNormal.SetBool("SHOWN",true);
                                break;
                            }
                        case AvatarEmotion.Happy:
                            {
                                break;
                            }
                    }
                    break;
                }
            case AvatarPersonage.Lena:
                {
                    switch (emotion)
                    {
                        case AvatarEmotion.Sad:
                            {
                                LenaSad.SetBool("SHOWN", true);
                                break;
                            }
                        case AvatarEmotion.Normal:
                            {
                                LenaNormal.SetBool("SHOWN", true);
                                break;
                            }
                        case AvatarEmotion.Happy:
                            {
                                break;
                            }
                    }
                    break;
                }
            case AvatarPersonage.Archi:
                {
                    switch (emotion)
                    {
                        case AvatarEmotion.Sad:
                            {
                                break;
                            }
                        case AvatarEmotion.Normal:
                            {
                                ArchiNormal.SetBool("SHOWN", true);
                                break;
                            }
                        case AvatarEmotion.Happy:
                            {
                                break;
                            }
                    }
                    break;
                }
        
        }
    }

    public void HideAll()
    {
        if (LenaSad!=null) LenaSad.SetBool("SHOWN", false);

        if (StepaNormal != null) StepaNormal.SetBool("SHOWN", false);
        if (LenaNormal != null) LenaNormal.SetBool("SHOWN", false);
        if (ArchiNormal != null) ArchiNormal.SetBool("SHOWN", false);
    }

    public enum AvatarPersonage
    {
        Stepa,
        Lena,
        Archi
    }

    public enum AvatarEmotion
    { 
        Sad,
        Normal,
        Happy
    }
    
}
