using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyping : MonoBehaviour {

    public Text text;
    public float characterPerSecond = 10;
    public float messageDelay = 1;
    private float charactersPending = 0;
    private Queue<string> queue = new Queue<string>();

	// Use this for initialization
	void Start() {
        queue.Clear();
        queue.Enqueue("Systems Online");
        queue.Enqueue("Mission Objective: Extermination");
        queue.Enqueue("Commence");
        StartCoroutine(TypeCharacters());
    }
	
	void OnDestroy () {
        StopAllCoroutines();
	}

    IEnumerator TypeCharacters()
    {
        var sb = new System.Text.StringBuilder();
        string line = null;
        int charsWritten = 0;
        while (enabled)
        {
            charactersPending += characterPerSecond * Time.deltaTime;
            if (charactersPending > 0)
            {
                if (line != null)
                {
                    int charCount = Mathf.FloorToInt(charactersPending);
                    if (charCount <= 0)
                    {
                        charactersPending -= charCount;
                    }
                    else if ((charCount + charsWritten) > line.Length)
                    {
                        charactersPending -= (line.Length - charsWritten);
                        charactersPending -= (characterPerSecond * messageDelay);
                        sb.Append(line.Substring(charsWritten));
                        line = null;
                        charsWritten = 0;
                    }
                    else
                    {
                        charactersPending -= charCount;
                        sb.Append(line.Substring(charsWritten, charCount));
                        charsWritten += charCount;
                    }
                }
                else if (queue.Count > 0)
                {
                    charactersPending = 0;
                    sb.Remove(0, sb.Length);
                    line = queue.Dequeue();
                    charsWritten = 0;
                }
                else
                {
                    charactersPending = 0;
                    sb.Remove(0, sb.Length);
                    line = null;
                    charsWritten = 0;
                }
            }
            
            text.text = sb.ToString();
            yield return new WaitForEndOfFrame();
        }
    }
}
