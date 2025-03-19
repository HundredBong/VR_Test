using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

[System.Serializable]
public class NamedEvent
{
    //이벤트 이름
    public string eventName;

    //실제 발생할 이벤트
    public UnityEvent onTriggered;
}

public class UnityEventReceiver : MonoBehaviour, INotificationReceiver
{
    //INotification : 타임라인에 이벤트 송신용
    //INotificationReceiver : 타임라인에서 발생한 마커 이벤트 수신용
    public NamedEvent[] events;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        //전달된 INotification이 UnityEventMarker타입인지 확인함
        //맞으면 캐스팅해서 사용함
        if (notification is UnityEventMarker marker)
        {
            foreach (NamedEvent e in events)
            {
                //NamedEvent의 eventName과 UnityEventMarker의 eventName이 같은지 체크함
                if (e.eventName == marker.eventName)
                {                  
                    e.onTriggered?.Invoke();
                    Debug.Log($"이벤트 호출됨, {e.eventName}");
                }
            }
        }
    }
}
