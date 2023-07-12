using System;
using System.Reflection;
using Doublsb.Dialog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

    public class MyDialogueManager : MonoBehaviour
    {
        public static MyDialogueManager Instance;

        [Tooltip("Activar si el player tiene el control de la escena.")]
        public bool PlayerControl;

        private int Step { get; set; } = 0;

        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private GameObject nextBtn;
        [SerializeField] private GameObject characterName;
        [SerializeField] private Text characterText;

        private DOTDialogAnimator dialogAnimator;

        private MethodInfo storyMethod;
        private MethodInfo storyMaxStepMethod;
        private string currentText;
        private int maxStep = 0;
        private string currentScene;
        private bool isSubmitBtn;
        private bool canStart;
        //private bool canCheckVisibility;
        private const string TEXT_STORY = "Text_";
        private const string TEXT = "Text";
        private const string MAX_STEPS = "GetMaxStep";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            dialogAnimator = GetComponent<DOTDialogAnimator>();
        }

        private void LateUpdate()
        {
            if (canStart && PlayerControl)
            {
                CheckShowButton();   
            } else if (canStart && !PlayerControl)
            {
                //CheckDialogVisibility();
            }
        }

        public void Init()
        {
            currentScene = GetActiveStoryScene();
            var storyType = Type.GetType(currentScene);
            if (storyType == null) return;
            storyMethod = storyType.GetMethod(TEXT);
            storyMaxStepMethod = storyType.GetMethod(MAX_STEPS);

            if (storyMaxStepMethod != null)
            {
                maxStep = (int)storyMaxStepMethod.Invoke(null, null);
            }


            if (storyMethod == null) return;
            dialogAnimator.ShowDialogBox();
            canStart = true;
            NewDialogText();
        }
        
        // Recojo la escena actual y le añado el prefijo del script que contiene el dialogo
        private static string GetActiveStoryScene()
        {
            var scene = SceneManager.GetActiveScene().name;
            return TEXT_STORY + scene;
        }

        // Muestro nuevo texto
        //Cada vez que llamemos a este metodo mostrara una nueva linea de dialogo si el fichero aun tiene
        private void NewDialogText()
        {
            if (Step < maxStep)
            {
                Step++;

                currentText = (string)storyMethod.Invoke(null, new object[] { Step });

                var asteriskIndex = currentText.IndexOf("*");
                
                var characterName = currentText[..asteriskIndex];

                var dialogData = new DialogData(currentText[(asteriskIndex + 1)..]);

                characterText.text = characterName;

                //canCheckVisibility = true;

                dialogAnimator.ShowDialogBox();
                
                dialogManager.Show(dialogData);
            }
            else
            {
                StoryEnds();
            }
        }
        
        // Dialogos con opciones
        public void NewOptionText(string text, string character)
        {
            currentText = text;

            var dialogData = new DialogData(currentText);

            characterText.text = character;

            //canCheckVisibility = true;
            
            dialogAnimator.ShowDialogBox();

            dialogManager.Show(dialogData);
        }
        
        // Para cuando el Player controla el botón
        private void CheckShowButton()
        {
            var isActivated = CanContinue();
            nextBtn.SetActive(isActivated);   
            isSubmitBtn = !isActivated;
        }

        // Para cuando la cinemática controla los cambios de texto
        private bool CanContinue()
        {
            var asteriskIndex = currentText.IndexOf("*");
            var text = currentText[(asteriskIndex + 1)..];

            return dialogManager.Printer_Text.text.Length >= text.Length;
        }
        
        // No hay más texto que mostrar
        private void StoryEnds()
        {
            dialogAnimator.HideDialogBox();
        }
        
        // Player Input Action Submit
        public void OnBtnSubmit()
        {
            if (!PlayerControl) return;
            if (!nextBtn.activeSelf || isSubmitBtn) return;
            isSubmitBtn = true;
            NewDialogText();
        }
        
        // Siguiente texto controlado por la cinemática
        public void NextText()
        {
            if (PlayerControl) return;
            dialogAnimator.ShowDialogBox();
            NewDialogText();
        }
        
        // Carga de textos en Niveles por Script
        public void TextLevel(string level)
        {
            Step = 0;
            
            var storyType = Type.GetType(TEXT + "_" + level);

            if (storyType == null) return;
            storyMethod = storyType.GetMethod(TEXT);
            storyMaxStepMethod = storyType.GetMethod(MAX_STEPS);

            if (storyMaxStepMethod != null)
            {
                maxStep = (int)storyMaxStepMethod.Invoke(null, null);
            }


            if (storyMethod == null) return;
            dialogAnimator.ShowDialogBox();
            canStart = true;
            NewDialogText();
        }
        
        //Carga de Texto normal
        public void Text(string text, bool showCharacter = false)
        {
            ServiceLocator.GetService<GameInputs>().ActivateUIMode();
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed += HideDialogBox;
            dialogAnimator.ShowDialogBox();
            canStart = false;
            characterName.SetActive(showCharacter);
            dialogManager.Show(new DialogData(text));
        }
        
        public void HideDialogBox()
        {
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= HideDialogBox;
            ServiceLocator.GetService<GameInputs>().ActivatePlayerGroundMode();
            dialogAnimator.HideDialogBox();
        }

        public void StopStory()
        {
            Step = maxStep;
            StoryEnds();
        }
    }
