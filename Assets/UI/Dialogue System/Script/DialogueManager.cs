using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public bool PlayOnAwake = false;
    public bool isDialogue;

    public DialogueTrigger dialogueTrigger;
    public Text dialogueText;

	private Queue<string> sentences;

    [Header("Diaplay Time")]
    [Range(0, 100)]
    public int LetterSpeed = 5;
    public float printSentences = 5f;
    public float delay = 1f;

    public Text continueButton;
    public GameObject DialogueBox;
    public List<GameObject> buttons;

    [Header("Character")]
    public float distance = 50f;
    public Transform target;
    public List<Transform> npcs;

    public Cinemachine.CinemachineFreeLook follow;
    private int priority = 11;

    void Start () {
        dialogueTrigger = this.transform.GetComponent<DialogueTrigger>();
        sentences = new Queue<string>();
        if (PlayOnAwake)
        {
            continueButton.text = "TUTORIAL";
            OnStart();
        }
    }

    public GameObject end;
    public Text t;
    private bool ending = false;
    bool play = false;
    public UnityEngine.Playables.PlayableDirector playdir;
    public UnityEngine.Timeline.TimelineAsset timeline;
    void LateUpdate()
    {
        if (ending)
        {
            Cursor.visible = true;
            end.SetActive(true);
            t.gameObject.SetActive(true);
            t.text = "GameClear";
        }
        if (!play && !buttons[2] && !buttons[3] && !buttons[4])
        {
            Cursor.visible = false;
            npcs[1].gameObject.SetActive(true);
            playdir.Play(timeline);
            play = true;
        }
        for (int i = 0; i < npcs.Count; i++)
        { 
            if(Vector3.Distance(target.position, npcs[i].position) < distance)
            {
                if (buttons[i] == null) continue;
                Cursor.visible = true;
                buttons[i].SetActive(npcs[i].gameObject.activeSelf);
            }
        }
    }

    public void OnStart(bool clickedNextSentence = false)
    {
        isDialogue = true;
        StartDialogue(dialogueTrigger.dialogue, clickedNextSentence);
    }
    public void OnEnd()
    {
        Cursor.visible = false;
        isDialogue = false;
        DialogueBox.SetActive(false);
    }
    public void GetDialogueTrigger(DialogueTrigger trigger)
    {
        dialogueTrigger = trigger;
        if (!DialogueBox.activeSelf) DialogueBox.SetActive(true);
    }
    public void CMPriority(Cinemachine.CinemachineClearShot _cm)
    {
        _cm.Priority = (++priority);
    }

    private void StartDialogue (Dialogue dialogue, bool clickedNextSentence = false)
	{
		sentences.Clear();

        foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

        for (int i = 0; i < npcs.Count; i++)
        {
            if (buttons[i] == null || !npcs[i].gameObject) continue;
            if (buttons[i].activeSelf)
            {
                buttons[i].SetActive(false);
                if(i > 1) npcs[i].gameObject.SetActive(false);
                buttons[i] = null;
            }
        }

        continueButton.text = "CONTINUE";

        if (clickedNextSentence) DisplayNextSentence();
        else StartCoroutine(DisplaySentences());
    }
	public void DisplayNextSentence ()
	{
        if (sentences.Count == 0)
        {
            Cursor.visible = false;
            ending = play;
            dialogueText.text = "";
            continueButton.text = "";
            isDialogue = false;
            DialogueBox.SetActive(false);
            follow.Priority = (++priority);
            return;
        }

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}
	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
            yield return new WaitForSeconds(((float)LetterSpeed) /100f);
		}
	}
    IEnumerator DisplaySentences()
    {
        if (sentences.Count == 0)
        {
            Cursor.visible = false;
            ending = play;
            dialogueText.text = "";
            continueButton.text = "";
            PlayOnAwake = false;
            isDialogue = false;
            DialogueBox.SetActive(false);
            follow.Priority = (++priority);
            yield return null;
        }

        if (delay > 0)
        {
            dialogueText.text = "";
            yield return new WaitForSeconds(delay);
        }

        string sentence = sentences.Dequeue();
        if (LetterSpeed == 0)
        {
            dialogueText.text = sentence;
        }
        else
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(((float)LetterSpeed) / 100f);
            }
        }
        yield return new WaitForSeconds(printSentences);

        StartCoroutine(DisplaySentences());
        yield return null;
    }
}
