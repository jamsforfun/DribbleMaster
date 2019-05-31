using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class DataEventManager
{
    public int value1;
    public int value2;

    public DataEventManager(int a, int b)
    {
        value1 = a;
        value2 = b;
    }
}

/// <summary>
/// EventManager Description
/// 
/// use:
/// OnEnable: EventManager.Instance.StartListening(GameData.Event.GameOver, <myFunction>);
/// OnDisable: EventManager.Instance.StopListening(GameData.Event.GameOver, <myFunction>);
/// Trigger the event: EventManager.Instance.TriggerEvent(GameData.Event.GameOver);
/// </summary>
[TypeInfoBox("Global Event manager: StartListening, StopListening, TriggerEvent")]
public class EventManager : SingletonMono<EventManager>
{
    protected EventManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    #region Attributes

    private class UnityEventInt : UnityEvent<int>  {    }
    private class UnityEvent2Int : UnityEvent<int, int>  {    }
    private class UnityEventGameObject2Int : UnityEvent<GameObject, int, int> { }
    private class UnityEventBool : UnityEvent<bool>  {    }
    private class UnityEventBoolInt : UnityEvent<bool, int> { }
    private class UnityEventData : UnityEvent<DataEventManager>  {    }

    private Dictionary<GameData.Event, UnityEvent> eventDictionary;
    private Dictionary<GameData.Event, UnityEventInt> eventDictionaryInt;
    private Dictionary<GameData.Event, UnityEvent2Int> eventDictionary2Int;
    private Dictionary<GameData.Event, UnityEventGameObject2Int> eventDictionaryGameObject2Int;
    private Dictionary<GameData.Event, UnityEventBool> eventDictionaryBool;
    private Dictionary<GameData.Event, UnityEventBoolInt> eventDictionaryBoolInt;
    private Dictionary<GameData.Event, UnityEventData> eventDictionaryData;

    #endregion

    #region Initialization
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// test si on met le script en UNIQUE
    /// </summary>
    void Init()
    {
        if (eventDictionary == null)
            eventDictionary = new Dictionary<GameData.Event, UnityEvent>();
        if (eventDictionaryInt == null)
            eventDictionaryInt = new Dictionary<GameData.Event, UnityEventInt>();
        if (eventDictionary2Int == null)
            eventDictionary2Int = new Dictionary<GameData.Event, UnityEvent2Int>();
        if (eventDictionaryGameObject2Int == null)
            eventDictionaryGameObject2Int = new Dictionary<GameData.Event, UnityEventGameObject2Int>();
        if (eventDictionaryBool == null)
            eventDictionaryBool = new Dictionary<GameData.Event, UnityEventBool>();
        if (eventDictionaryBoolInt == null)
            eventDictionaryBoolInt = new Dictionary<GameData.Event, UnityEventBoolInt>();
        if (eventDictionaryData == null)
            eventDictionaryData = new Dictionary<GameData.Event, UnityEventData>();
    }
    #endregion

    #region Core
    /// <summary>
    /// ajoute un listener ?
    /// we look at the dictionary,
    /// we see if we have already a key value pair for what we are trying to add
    /// if so, we add to it,
    /// if not, we create a new unity event, we add the listener to it, 
    /// and we push that into the dictionary as the very first entry
    /// </summary>
    public static void StartListening(GameData.Event eventName, UnityAction listener)
    {
        if (!Instance)
            return;

        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(GameData.Event eventName, UnityAction<int> listener)
    {
        if (!Instance)
            return;

        UnityEventInt thisEvent = null;
        if (Instance.eventDictionaryInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEventInt();
            thisEvent.AddListener(listener);
            Instance.eventDictionaryInt.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(GameData.Event eventName, UnityAction<int, int> listener)
    {
        if (!Instance)
            return;

        UnityEvent2Int thisEvent = null;
        if (Instance.eventDictionary2Int.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent2Int();
            thisEvent.AddListener(listener);
            Instance.eventDictionary2Int.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(GameData.Event eventName, UnityAction<GameObject, int, int> listener)
    {
        if (!Instance)
            return;

        UnityEventGameObject2Int thisEvent = null;
        if (Instance.eventDictionaryGameObject2Int.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEventGameObject2Int();
            thisEvent.AddListener(listener);
            Instance.eventDictionaryGameObject2Int.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(GameData.Event eventName, UnityAction<bool, int> listener)
    {
        if (!Instance)
            return;

        UnityEventBoolInt thisEvent = null;
        if (Instance.eventDictionaryBoolInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEventBoolInt();
            thisEvent.AddListener(listener);
            Instance.eventDictionaryBoolInt.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(GameData.Event eventName, UnityAction<bool> listener)
    {
        if (!Instance)
            return;

        UnityEventBool thisEvent = null;
        if (Instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEventBool();
            thisEvent.AddListener(listener);
            Instance.eventDictionaryBool.Add(eventName, thisEvent);
        }
    }
    public static void StartListening(GameData.Event eventName, UnityAction<DataEventManager> listener)
    {
        if (!Instance)
            return;

        UnityEventData thisEvent = null;
        if (Instance.eventDictionaryData.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEventData();
            thisEvent.AddListener(listener);
            Instance.eventDictionaryData.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// unregister
    /// </summary>
    public static void StopListening(GameData.Event eventName, UnityAction listener)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEvent thisEvent = null;
        //si on veut unregister et que la clé existe dans le dico..
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(GameData.Event eventName, UnityAction<int> listener)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEventInt thisEvent = null;
        //si on veut unregister et que la clé existe dans le dico..
        if (Instance.eventDictionaryInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(GameData.Event eventName, UnityAction<int, int> listener)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEvent2Int thisEvent = null;
        //si on veut unregister et que la clé existe dans le dico..
        if (Instance.eventDictionary2Int.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(GameData.Event eventName, UnityAction<GameObject, int, int> listener)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEventGameObject2Int thisEvent = null;
        //si on veut unregister et que la clé existe dans le dico..
        if (Instance.eventDictionaryGameObject2Int.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(GameData.Event eventName, UnityAction<bool, int> listener)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEventBoolInt thisEvent = null;
        //si on veut unregister et que la clé existe dans le dico..
        if (Instance.eventDictionaryBoolInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(GameData.Event eventName, UnityAction<bool> listener)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEventBool thisEvent = null;
        //si on veut unregister et que la clé existe dans le dico..
        if (Instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void StopListening(GameData.Event eventName, UnityAction<DataEventManager> listener)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEventData thisEvent = null;
        //si on veut unregister et que la clé existe dans le dico..
        if (Instance.eventDictionaryData.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    /// trigger un event
    /// </summary>
    public static void TriggerEvent(GameData.Event eventName)
    {
        if (EventManager.Instance == null)   //au cas ou on a déja supprimé l'eventManager
            return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
    public static void TriggerEvent(GameData.Event eventName, int value)
    {
        UnityEventInt thisEvent = null;
        if (Instance.eventDictionaryInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }
    public static void TriggerEvent(GameData.Event eventName, int firstValue, int secondValue)
    {
        UnityEvent2Int thisEvent = null;
        if (Instance.eventDictionary2Int.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(firstValue, secondValue);
        }
    }
    public static void TriggerEvent(GameData.Event eventName, GameObject obj, int firstValue, int secondValue)
    {
        UnityEventGameObject2Int thisEvent = null;
        if (Instance.eventDictionaryGameObject2Int.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(obj, firstValue, secondValue);
        }
    }
    public static void TriggerEvent(GameData.Event eventName, bool active, int secondValue)
    {
        UnityEventBoolInt thisEvent = null;
        if (Instance.eventDictionaryBoolInt.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(active, secondValue);
        }
    }
    public static void TriggerEvent(GameData.Event eventName, bool value)
    {
        UnityEventBool thisEvent = null;
        if (Instance.eventDictionaryBool.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }
    public static void TriggerEvent(GameData.Event eventName, DataEventManager data)
    {
        UnityEventData thisEvent = null;
        if (Instance.eventDictionaryData.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(data);
        }
    }
    #endregion

    #region Unity ending functions

    #endregion
}
