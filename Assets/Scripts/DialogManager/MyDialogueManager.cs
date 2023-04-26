using UnityEngine;
using Doublsb.Dialog;
using Honeti;

public class MyDialogueManager : MonoBehaviour
{
    public static MyDialogueManager Instance;
    
    private DialogManager dialogManager;
    private string text = "";
    private string textFromi18n = "";
    private string characterName = "Link";

    private void Awake()
    {
        dialogManager = FindObjectOfType<DialogManager>();
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);   
        }
    }

    void Start()
    {
        //Ejemplo para demo
        I18N.instance.setLanguage(LanguageCode.EN);
        text = I18N.instance.getValue("^text_init");
        textFromi18n = I18N.instance.getValue("^text_play");
        DialogData dialogData = new DialogData(text, characterName);
        
        dialogManager.Show(dialogData);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            dialogManager.Hide();
    }
}
