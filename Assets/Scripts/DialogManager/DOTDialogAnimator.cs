using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DOTDialogAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogImageObj;
    [SerializeField]
    private Image dialogImage;
    [SerializeField]
    private Image characterImage;
    
    private const float fadeInTime = 1f;


    private void Awake()
    {
        dialogImage = dialogImageObj.GetComponent<Image>();
    }

    public void ShowDialogBox()
    {
        dialogImageObj.SetActive(true);
        dialogImage.color = new Color(dialogImage.color.r, dialogImage.color.g, dialogImage.color.b, 0f);
        characterImage.color = new Color(characterImage.color.r, characterImage.color.g, characterImage.color.b, 0f);
        dialogImage.DOFade(1f, fadeInTime).Play();
        characterImage.DOFade(1f, fadeInTime).Play();
    }

    public void HideDialogBox()
    {
        dialogImage.DOFade(0f, fadeInTime).OnComplete(() => {
            dialogImageObj.SetActive(false);
        }).Play();
        characterImage.DOFade(0f, fadeInTime).Play();
    }

}
