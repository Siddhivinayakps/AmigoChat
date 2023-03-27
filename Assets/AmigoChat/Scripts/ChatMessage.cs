using UnityEngine;
using TMPro;
public class ChatMessage : MonoBehaviour
{

    [SerializeField] 
    private TextMeshProUGUI authorName, textMessage, timestamp;

    [SerializeField]
    Transform chatMessageContent;

    private string message;

    public string Message
    {
        get
        {
            return message;
        }

        set
        {
            message = value;
            CreateMessage();
        }
    }

    private string author;
    public string Author
    {
        get
        {
            return author;
        }
        set
        {
            author = value;
            authorName.text = author;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMessage()
    {
        textMessage.text = message;
        timestamp.text = System.DateTime.Now.ToShortTimeString();
        Vector2 currentPos = chatMessageContent.localPosition;
        if (author.Equals("You"))
        {
            textMessage.alignment = TextAlignmentOptions.TopRight;
            currentPos.x += 20f;
            
        } else
        {
            textMessage.alignment = TextAlignmentOptions.TopLeft;
            currentPos.x -= 20f;
        }
        chatMessageContent.localPosition = currentPos;
    }
}
