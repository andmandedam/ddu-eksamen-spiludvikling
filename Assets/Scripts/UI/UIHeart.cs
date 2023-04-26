using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeart : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hearts;

    void Start()
    {
        
    }
    public void SetHeartState(int heartState)
    {
        switch (heartState)
        {
            case (int) UIHeartState.Disabled:
                {
                    hearts[0].SetActive(false);
                    hearts[1].SetActive(false);
                    hearts[2].SetActive(false);
                    break;
                }

            case (int) UIHeartState.Empty:
                {
                    hearts[0].SetActive(true);
                    hearts[1].SetActive(false);
                    hearts[2].SetActive(false);
                    break;
                }

            case (int) UIHeartState.Half:
                {
                    hearts[0].SetActive(true);
                    hearts[1].SetActive(true);
                    hearts[2].SetActive(false);
                    break;
                }

            case (int) UIHeartState.Full:
                {
                    hearts[0].SetActive(true);
                    hearts[1].SetActive(false);
                    hearts[2].SetActive(true);
                    break;
                }
        }
    }

    public enum UIHeartState
    {
        Disabled = -1,
        Empty = 0,
        Half = 1,
        Full = 2,
    }
    
}
