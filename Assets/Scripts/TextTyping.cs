using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyping : MonoBehaviour {

    public Text text;
    public float characterPerSecond = 10;
    public float messageDelay = 1;
    private float charactersPending = 0;
    private Queue<Message> queue = new Queue<Message>();

    [System.Serializable]
    public class Message
    {
        public string message;
        public bool keepOnScreen;
        public System.Action<string> callback;
    }

    public bool IsEmpty
    {
        get { return queue.Count == 0; }
    }

    public void Clear()
    {
        queue.Clear();
    }

    public void Enqueue(string message, bool keepOnScreen = false, System.Action<string> callback = null)
    {
        queue.Enqueue(new Message {
            message = message,
            keepOnScreen = keepOnScreen,
            callback = callback
        });
    }
    
	void Start() {
        StartCoroutine(TypeCharacters());
    }
	
	void OnDestroy () {
        StopAllCoroutines();
	}

    IEnumerator TypeCharacters()
    {
        var sb = new System.Text.StringBuilder();
        Message currentMessage = null;
        int charsWritten = 0;
        bool keepOnScreen = false;
        while (enabled)
        {
            charactersPending += characterPerSecond * Time.deltaTime;
            if (charactersPending > 0)
            {
                if (currentMessage != null)
                {
                    string line = currentMessage.message;
                    int charCount = Mathf.FloorToInt(charactersPending);
                    if (charCount <= 0)
                    {
                        charactersPending -= charCount;
                    }
                    else if ((charCount + charsWritten) > line.Length)
                    {
                        Message m = currentMessage;
                        charactersPending -= (line.Length - charsWritten);
                        charactersPending -= (characterPerSecond * messageDelay);
                        sb.Append(line.Substring(charsWritten));
                        currentMessage = null;
                        charsWritten = 0;

                        if (m.callback != null)
                        {
                            m.callback(m.message);
                        }
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
                    currentMessage = queue.Dequeue();
                    charsWritten = 0;
                    keepOnScreen = currentMessage.keepOnScreen;
                }
                else if (!keepOnScreen)
                {
                    charactersPending = 0;
                    sb.Remove(0, sb.Length);
                    currentMessage = null;
                    charsWritten = 0;
                }
            }
            
            text.text = sb.ToString();
            yield return new WaitForEndOfFrame();
        }
    }
}
