using System;
using UnityEngine;




public class GameManager : MonoBehaviour
{
    //DOMANDE:
    /*
     - varibaili: devo per forza utilizzare il campo nell editor e assegnare lo script da li?
     - devo per forza usare la proprietà => per modificare dalle classe degli stati gli elementi? altrimenti dovrei mettere public
     - se raccolgo 3 libri -> cambio stato, uso un azione come implementata sotto?
    
    1) se ci sono piu istanze dentro uno stato es. N books ->  prendi tutte le istanze (LISTA) e iscrivine ognuna all'azione
    2) variabili che non sono usate in ADD TRANSITION / in GAME MANAGER -> messe direttamente in CLASSI
    3) USARE LE AZIONI DA OGGETTI -> A FSM (come libro): usare funzione in FSM per triggerare
    4) USARE AZIONI/CHIMATE A FUNZIONI DA FSM (npc) -> A OGGETTO : usare funzioni dentro gli oggetti; 
       es) FSM invoca funzione StartVoiceAnimation() in NPC
     */

    //GAME MANAGER
    public static GameManager Instance;
    //DICHIARAZIONE MACCHINA A STATI DEL SYSTEM
    /*per notificare a quegli Script (game obj) che lo stato è cambiato -> azione dentro FSM*/
    public static FiniteStateMachine<GameManager> stateMachine;

    //XR ORIGIN - MESH
    [SerializeField] private Transform XR_Origin;
    [SerializeField] private Transform NestedParent;
    [SerializeField] private Transform corridoio;
    public Transform XROrigin => XR_Origin;
    public GameObject mouth;
    public Transform corrOrigin => corridoio;
    public Transform FirstController => NestedParent;

    //NPC
    private NpcBehaviour _npc;
    //public NpcBehaviour npc => _npc;
    private float Delay = 10;
    public float voiceDelay => Delay;

    private Renderer _renderer;
    private Color _originalColor;
    public Renderer Renderer => _renderer; //chiama GET della proprietà 
    public Color OriginalColor => _originalColor;

    //DOOR
    private bool _isClosed = false;

    //BOOK
    private int counter = 0;

    //CD -MUSIC
    private bool _musicPlayed = false;

    //MUSHROOMS
    public int eatenMushrooms = 0;

    //EFFECTS
    public float effectTime = 5f;
    public EffectManager effectManager;
    public float initMusicVolume = 0.6f;
    
    //FINAL
    private bool _finalSwitch = false;

   

    private void Awake()
    {
        Instance = this; //CLASSE GameManager resa singleton
    }

    void Start()
    {
        stateMachine = new FiniteStateMachine<GameManager>(this);
        //npc
        _npc = FindObjectOfType<NpcBehaviour>();
        //XR origin
        mouth = FindObjectOfType<Mouth>().gameObject;
        //book : recuperare tutti i libri (findObjectsoftype -> array book -> for each iscrivo a azione)
        //utilizzabili
        _renderer = GetComponent<Renderer>();
        //_originalColor = _renderer.material.color;

        //STATES
        //door
        State startDoorState = new StartDoorState("StartDoorState", this, FindObjectOfType<Door>());
        //music
        State musicState = new MusicState("MusicState", this ,_npc ,FindObjectsOfType<Disco>());
        //book
        State searchBookState = new SearchBookState("SearchBookState", this,_npc,  FindObjectsOfType<Book>());
        //funghi
        State mushroomsState = new MushroomsState("MushroomsState", this,_npc, FindObjectsOfType<Mushroom>());
        //effects
        State effectState = new EffectState("EffectSate", this, _npc);
        //final
        State finalState = new FinalState("FinalState", this, _npc, FindObjectOfType<SceneTransitionManager>());

        //TRANSITIONS
        //door : apertura
        stateMachine.AddTransition(startDoorState, musicState, () => _isClosed);
        //musica : riproduzione
        stateMachine.AddTransition(musicState, searchBookState, () => _musicPlayed);
        //book : n=3
        stateMachine.AddTransition(searchBookState, mushroomsState, () => counter==3);
        //funghi : n
        stateMachine.AddTransition(mushroomsState, effectState, () => eatenMushrooms==1);
        //effect : dissolvenza
        stateMachine.AddTransition(effectState, finalState, () => _finalSwitch);


        //START STATE
        stateMachine.SetState(startDoorState);
    }

    void Update() => stateMachine.Tik(); //attensione: da errore se lo stato non viene aggiornato


    //FUNZIONI DI UPDATE STATE/ENTER STATE : da iserire in TIK() di ogni stato
    //Risposte ad AZIONI
    public void OnBookExplain(Book book, bool isGrabbed)
    {
        if (counter == 4)
        {   
            counter = 0;
            Debug.LogError("Hai preso più di 3 libri");
            return;
        }
        if(isGrabbed == false && !_npc._isTalkingNpc)
        {
            counter++;
            Debug.Log("Libro valido grabbato");
            switch (book.getTitle())
            {
                case "Leary":
                    _npc.StartAudioLeary();
                    break;
                case "Huxley":
                    _npc.StartAudioHuxley();
                    break;
                case "Devereux":
                    _npc.StartAudioDevereux();
                    break;
                default: throw new ArgumentOutOfRangeException(); 

            }
        }
    }
    public void ClosingDoor() 
    {
        _isClosed = true;
    }
    public void OnRiproductionCD() //musicPlayed bool tolto da Disco (azione)
    {
        _musicPlayed = true;
    }
    public void OnMushroomEaten(Mushroom mushroom) 
    {
        eatenMushrooms++;
        //gestione migliore : switch case per effettiv
    }
    
    //TRANSITION FUNCTIONS : da inserire in AddTransition( ... , () => false)
    //la check transition sostituisce l'azione di notifica del cambio stato ai vari oggetti
    public void SwitchToFinalState()
    {
        _finalSwitch = true;
    }

    //FUNZIONI UTILI
    public bool IsNpcTalking()
    {
        return _npc._isTalkingNpc;
    }
}


//CLASSI DEGLI STATI
public class StartDoorState : State
{
    private GameManager gameManager;
    private Door _door;
    public StartDoorState(string name, GameManager gameManager, Door _door) : base(name)
    {
        this.gameManager = gameManager;
        this._door = _door;
    }

    public override void Enter()
    {
        //quando torna nello stato di START : la posizione del personaggio deve posizionarsi davanti alla porta
        _door.DoorClosed += gameManager.ClosingDoor;

        //per debug first person controller
        gameManager.FirstController.position = gameManager.corrOrigin.position;
    }

    public override void Tik()
    {
    }

    public override void Exit()
    {
        _door.DoorClosed -= gameManager.ClosingDoor;
    }
}

public class MusicState : State
{
    private GameManager gameManager;
    private Disco[] _disco;
    private NpcBehaviour _npc;
    public MusicState(string name, GameManager guard, NpcBehaviour npc, Disco[] disco) : base(name)
    {
        gameManager = guard;
        this._disco = disco;
        this._npc = npc;
    }

    public override void Enter()
    {
        if (!_npc._isTalkingNpc)
        {
            _npc.StartWelcomeVoice(); //_npc.StartWelcomeAnimation(); --> dopo 1 sec
        }

        foreach (var d in _disco)
        {
            d.CDStart += gameManager.OnRiproductionCD;
        }
    }

    public override void Tik()
    {
        // _npc.TalkingToIdle(); se Invoke è pesante nell'Enter, allora scommenta IF e metti in TiK()
        _npc.TalkingToIdle();
    }

    public override void Exit()
    {
        
        foreach (var d in _disco)
        {
            d.CDStart -= gameManager.OnRiproductionCD;
        }
    }
}
public class SearchBookState : State
{
    private GameManager gameManager;
    private Book[] books;
    private NpcBehaviour _npc;
    public SearchBookState(string name, GameManager gameManager,NpcBehaviour _npc, Book[] books) : base(name) //passare book al costruttore
    {
        this.gameManager = gameManager;
        this._npc = _npc;
        this.books = books;
        
    }

    public override void Enter()
    {
        if (!_npc._isTalkingNpc)
        {
            _npc.StartSearchVoice(gameManager.voiceDelay); //appena musica parte, dopo 10 secondi NPC dice di cercare libri
        }
       
        foreach (Book b in books)
        {
            b.OnBookGrabbed += gameManager.OnBookExplain; 
            //da mettere qua: AZIONE = iscriversi una sola volta, run time se si verifica azione-> chiamata funzione
        }
    }

    public override void Tik()
    {
        // _npc.TalkingToIdle(); se Invoke è pesante nell'Enter, allora scommenta IF e metti in TiK()
        _npc.TalkingToIdle();
    }

    public override void Exit()
    {
        foreach (Book b in books)
        {
            b.OnBookGrabbed -= gameManager.OnBookExplain;

        }
    }
}
public class MushroomsState : State
{
    private GameManager gameManager;
    private Mushroom[] mushrooms;
    private NpcBehaviour _npc;
    private float pointingDelay =0;

    public MushroomsState(string name, GameManager guard,NpcBehaviour npc, Mushroom[] mushrooms) : base(name)
    {
        gameManager = guard;
        this.mushrooms = mushrooms;
        this._npc = npc;
    }

    public override void Enter()
    {
        gameManager.mouth.GetComponent<Collider>().enabled = true;
        _npc.StartAudioMushroomsAfterTime(); //discorso introduttivo ai funghi
        foreach (Mushroom m in mushrooms)
        {
            m.OnMushroomEaten += gameManager.OnMushroomEaten;
            //da mettere qua: AZIONE = iscriversi una sola volta, run time se si verifica azione-> chiamata funzione
        }
    }

    public override void Tik()
    {
        pointingDelay = pointingDelay + Time.deltaTime;
        _npc.TalkingToPointing(pointingDelay); 
        /*TalkinToPointing: contemporaneament al fatto che parla, calcoliamo quanto tempo dura l'audio, quando dice "prendi lì i funghetti" = delay
        parte pointing*/
    }

    public override void Exit()
    {

    }
}

public class EffectState : State
{
    private GameManager gameManager;
    private NpcBehaviour _npc;
    private float stateChangeTime = 0.0f;
    public EffectState(string name, GameManager guard, NpcBehaviour npc) : base(name)
    {
        gameManager = guard;
        this._npc = npc;
    }

    public override void Enter()
    {
        if (!_npc._isTalkingNpc)
        {
            _npc.StartAudioEffects(gameManager.voiceDelay); //dialogo post funghetti, appena prima inizio effetti
            stateChangeTime = Time.time;
        }
        gameManager.effectManager.BeginEffectsAfter(35);
        gameManager.Invoke("SwitchToFinalState", gameManager.effectTime*60+35);
    }

    public override void Tik()
    {
        _npc.TalkingToIdle();
        // if (Time.time - stateChangeTime > gameManager.voiceDelay)
        // {
        //     gameManager.effectManager.BeginEffects();
        //     stateChangeTime = 0.0f;
        // }
    }

    public override void Exit()
    {
    }
    
}
public class FinalState : State
{
    private GameManager gameManager;
    private NpcBehaviour _npc;
    private SceneTransitionManager _sceneTransitionManager;
    public FinalState(string name, GameManager guard, NpcBehaviour npc, SceneTransitionManager stm) : base(name)
    {
        gameManager = guard;
        this._npc = npc;
        this._sceneTransitionManager = stm;
    }

    public override void Enter()
    {
        _npc.getAudioSource().spatialBlend = 0;
        _npc.FinalSpeech();
        _sceneTransitionManager.GoToAsyncDelay(0, 60);
    }

    public override void Tik()
    {
    }

    public override void Exit()
    {
       //dissolvenza

    }
}
