using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDatabase : MonoBehaviour
{
    Message ph;
    public int nextIndex;
    public string currText;

    public class Message {
        public string body;
        public int next;

        public Message(string b, int n) {
            body = b;
            next = n;
        }
    }
    public List<Message> jukebox;
    // Start is called before the first frame update
    void Start()
    {
        //ph = new Message("Greetings! How is your evening going?", 1);
        jukebox = new List<Message>();
        jukebox.Add(new Message("Greetings! How is your evening going?", 1)); //0
        jukebox.Add(new Message("Welcome to the Alpha!", 2));
        jukebox.Add(new Message("Have a look around! Go talk to some people! See what you see!", -1));
        jukebox.Add(new Message("Wow, the space station seems so high tech!", 4)); //3
        jukebox.Add(new Message("Are you sure it's alright if we stay here?", -1));
        jukebox.Add(new Message("Hmm these lodgings are adequate.", 6)); //5
        jukebox.Add(new Message("Please direct me towards the laboratory. I'll need to begin my work.", -1));
        jukebox.Add(new Message("Making use of this abandoned space station was a good idea.", 8)); //7
        jukebox.Add(new Message("We'll need to do some maintenance and cleaning though...", -1));
    }

    public bool Next() {
        if (nextIndex < 0) {
            return false;
        }
        currText = jukebox[nextIndex].body;
        nextIndex = jukebox[nextIndex].next;
        return true;
    }

    public void Set(int i) {
        currText = jukebox[i].body;
        nextIndex = jukebox[i].next;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
