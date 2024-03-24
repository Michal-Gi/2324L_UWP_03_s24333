using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathDialogue : MonoBehaviour
{
    [field: SerializeField]
    private TextMeshProUGUI textComponent;
    [field: SerializeField]
    private string[] text;
    [field: SerializeField]
    private float textSpeed;

    private int index;
    [field: SerializeField]
    private UnityEngine.UI.Image image;
    [field: SerializeField]
    private bool IsDeathDialogue;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player").GetComponent<PlayerHP>().currentHp > 0)
        {
            if (Input.GetKeyDown(KeyCode.E)) {
                Debug.Log("DeathDialogue started");
                image.enabled = true;
                index = 0;
                StartCoroutine(TypeText());
            }
            StartDialogue();
        if (image.enabled) { 
            if (Input.GetMouseButtonDown(0)) {
                if (textComponent.text == text[index])
                    NextLine();
                else { 
                    StopAllCoroutines();
                    textComponent.text = text[index];
                }
            }

        }}
    }

    IEnumerator TypeText() {
        foreach (char c in text[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() {
        if (index < text.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeText());
        }
        else { 
            gameObject.SetActive(false);
            if(IsDeathDialogue)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}