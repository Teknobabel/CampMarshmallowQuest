using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleManager : MonoBehaviour
{
    public enum Subtitles {
        Enabled,
        Disabled,
    }

    public static SubtitleManager m_subtitleManager;

    public Text m_text;

    private List<string> m_subtitles  = new List<string>();
    private Subtitles m_subtitleState = Subtitles.Disabled;

    void Awake () {
        m_subtitleManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_subtitles.Add("Hey Scoutmaster!"); //0
        m_subtitles.Add("Over here!"); //1
        m_subtitles.Add("The fire's roaring and our\nsticks are sharp, Scoutmaster."); //2
        m_subtitles.Add("The only thing missing\nare the marshmallows."); //3
        m_subtitles.Add("Here, consider this the\nSpear of Destiny."); //4
        m_subtitles.Add("It can transform an ordinary\nmarshmallow into a beautiful memory."); //5

        m_subtitles.Add("Hey there, my name's Jeff!"); //6
        m_subtitles.Add("The sky looks lovely tonight,\ndon't you think?"); //7
        m_subtitles.Add(""); //8
        m_subtitles.Add("So... what's that stick for?"); //9

        m_subtitles.Add("Mmmm, delicious."); //10
        m_subtitles.Add("Would this taste as sweet\nif I were alone?"); //11
        m_subtitles.Add("I could get a bag of marshmallows\nfrom the store any time,"); //12
        m_subtitles.Add("but it's only here with\nthem that it seems to make sense."); //13

        m_subtitles.Add("Have you ever frowned\nwhile eating a marshmallow?"); //14
        m_subtitles.Add("Of course not,\nit's impossible!"); //15
        m_subtitles.Add("They're sugar-packed\nbundles of pure joy!"); //16

        m_subtitles.Add("I saw what you\ndid to Jeff"); //17
        m_subtitles.Add("Why is this happening?!"); //18
        m_subtitles.Add("Don't tell anyone\nI cried so much, okay?"); //19
        m_subtitles.Add("Ahhh!"); //20

        m_subtitles.Add("Mmm, so tasty!"); //21
        m_subtitles.Add("Being a marshmallow must be great,"); //22
        m_subtitles.Add("your only purpose\nis to make people happy!"); //23
        m_subtitles.Add(""); //24

        m_subtitles.Add("A marshmallow may last\nlonger in the bag,"); //25
        m_subtitles.Add("but what kind of\nexistence is that?"); //26
        m_subtitles.Add("They were made\nto be eaten."); //27
        m_subtitles.Add("And what could be better\nthan being shared"); //28
        m_subtitles.Add("among friends on a\nnight like this?"); //29

        m_subtitles.Add("Why are you doing this?"); //30
        m_subtitles.Add("This is a dream, right?"); //31
        m_subtitles.Add("Am I going to wake up soon?"); //32
        m_subtitles.Add("Sugar can't fill the\nhole in your heart!"); //33
        
        m_subtitles.Add("Mmm, each bite is a\nlittle taste of heaven."); //34 
        m_subtitles.Add("A tough exterior filled\nwith warm, gooey sweetness."); //35
        m_subtitles.Add("Not too different from\nyou and I, right?"); //36

        m_subtitles.Add("I tried giving up sugar once,\nbut it didn't last very long."); //37
        m_subtitles.Add("Did you know they\nput sugar in"); //38
        m_subtitles.Add("pretty much everything?"); //39
        m_subtitles.Add("A kid just can't survive on\ncarrots and celery alone!"); //40
        m_subtitles.Add(""); //41
        m_subtitles.Add(""); //42

        m_subtitles.Add("What the fuck are you doing?!"); //43
        m_subtitles.Add("Fuck you asshole!"); //44
        m_subtitles.Add("I'll see you in hell!"); //45

        m_subtitles.Add("Mmmm, if only the\nmarshmallow knew"); //46
        m_subtitles.Add("that its the fire\nthat gives it purpose."); //47

        m_subtitles.Add("We're fated to\neventually reach into"); //48
        m_subtitles.Add("that bag and pull out\nthe final marshmallow."); //49
        m_subtitles.Add("The fantasy ends\nand the real world"); //50
        m_subtitles.Add("creeps back in."); //51
        m_subtitles.Add("Transition often\nrequires sacrifice,"); //52
        m_subtitles.Add("but it's up to\nyou to decide"); //53
        m_subtitles.Add("which end of the stick\nyou're going to be on."); //54

        m_subtitles.Add("Is this gonna hurt?"); //55
        m_subtitles.Add("Mother always said\nI'd meet that special"); //56
        m_subtitles.Add("someone who makes me\ntingle all over..."); //57
        
        m_subtitles.Add("It's so quiet..."); //58
        m_subtitles.Add("Do you feel like\nsomething's watching us?"); //59
        m_subtitles.Add("Is it already time\nfor a transition?"); //60

        m_subtitles.Add("What was that?"); //61
        m_subtitles.Add("It's coming from over there!"); //62
        m_subtitles.Add("What is that thing?"); //63

        m_subtitles.Add("Over here!"); //64
        m_subtitles.Add("Come here!"); //65
        m_subtitles.Add("Hey!"); //66
        m_subtitles.Add("Hello!"); //67
        m_subtitles.Add("Come here Scoutmaster!"); //68
        m_subtitles.Add("Come!"); //69
        m_subtitles.Add("That's perfect Scoutmaster,\ngive it here!"); //70

        m_subtitles.Add("Why?!"); //71
        m_subtitles.Add("Where's my mom? She said\nshe'd be back soon..."); //72
        m_subtitles.Add("Is that all I am,\njust a quick snack?"); //73
        m_subtitles.Add("I thought I'd get to do\nso much more..."); //74

        m_subtitles.Add("Ohmigod, ohmigod, ohmigod,\nplease don't eat me!!!"); //75
        m_subtitles.Add("I just wanted to look\nat the stars with my friends!!!"); //76
        m_subtitles.Add("*crying*"); //77
        m_subtitles.Add("Do I get a last\nrequest or something?"); //78
        m_subtitles.Add("If so, my request\nis to NOT DIE!"); //79

        m_subtitles.Add("At least let me clear\nmy browser history first???"); //80
        m_subtitles.Add("What about cupcakes?\nDo you like cupcakes?"); //81
        m_subtitles.Add("I saw the most delicious\nlooking cupcake ever"); //82
        m_subtitles.Add("wandering around here yesterday."); //83
        m_subtitles.Add("I'm sure I could find\nhim for you!"); //84
        m_subtitles.Add("I bet you'd rather have\nsomething salty, right?"); //85
        

        m_subtitles.Add("Fuck you asshole!"); //86
        m_subtitles.Add("I hope you choke on me!"); //87
        m_subtitles.Add("Does this make\nyou feel tough???"); //88
        m_subtitles.Add("My mom is gonna fuck you up!"); //89
        m_subtitles.Add("I hope I burn your mouth!"); //90

        m_subtitles.Add("Could you tell it was my first time?"); //91
        m_subtitles.Add("I hope I taste good..."); //92
        m_subtitles.Add("Don't burn me, okay?"); //93
        m_subtitles.Add("I've heard that I pair well\nwith chocolate and graham crackers."); //94
        m_subtitles.Add("Try not to fill up on sweets\nand spoil your dinner, alright?"); //95

        m_subtitles.Add("How does it taste Scoutmaster?"); //96
        m_subtitles.Add("I don't feel so well..."); //97

        m_subtitles.Add("There's a soft pretzel that\nbullies me all the time,"); //98
        m_subtitles.Add("he deserves this more\nthan I do!"); //99
        m_subtitles.Add("I stepped in some\nmud earlier,"); //100
        m_subtitles.Add("you should probably just\nthrow me out okay?"); //101
        m_subtitles.Add("If so, my request\nis to NOT DIE!"); //102
        m_subtitles.Add("Too late..."); //103

        m_subtitles.Add("I know a family of cookies\nright down the road,"); //104
        m_subtitles.Add("if you let me go\nI can take you to them!"); //105

        m_subtitles.Add("I'll take that."); //106
        m_subtitles.Add("Can I have it?"); //107
        m_subtitles.Add("Hand it over Scoutmaster!"); //108

        m_subtitles.Add("Hey, pick me up!"); //109
        m_subtitles.Add("Over here!"); //110
        m_subtitles.Add("Hi!"); //111

        m_subtitles.Add("Can you roast that first?"); //112
        m_subtitles.Add("It's not roasted!"); //113
        m_subtitles.Add("I want it roasted!"); //114
        m_subtitles.Add("Can you roast it please?"); //115

    }

    public void SetSubtitle (int textNum) {

        if (textNum < m_subtitles.Count && m_subtitleState == Subtitles.Enabled) {
            m_text.text = m_subtitles[textNum];
            m_text.gameObject.SetActive(true);
        }
    }

    public string GetSubtitle (int textNum) {

        if (textNum < m_subtitles.Count) {
            return m_subtitles[textNum];
        }
        
        return "Null";
    }

    public void DisableSubtitles () {

            m_text.gameObject.SetActive(false);
        
    }

    void Update () {
       // //Debug.Log(m_text.transform.eulerAngles);
       Vector3 rot = m_text.transform.eulerAngles;
       rot.z = 0;
       m_text.transform.eulerAngles = rot;
    }

    public Subtitles subtitleState {get{return m_subtitleState;} set{m_subtitleState = value;}}
}
