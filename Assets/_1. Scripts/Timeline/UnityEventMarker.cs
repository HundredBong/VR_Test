using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class UnityEventMarker : Marker, INotification
{
    //Marker : 타임라인에서 마커로 사용되기위해 상속받음, Marker를 상속받은 시점에서 유니티는 이 클래스가 타임라인에서 사용될 마커임을 인식하게 됨
    //INotification : 타임라인에서 이벤트를 보낼 수 있는 클래스가 되려면 있어야함

    //id는 타임라인의 Notification 시스템에서 식별자로 사용됨
    //필요하지 않으므로 기본값(new PropertyName()) 반환
    public PropertyName id => new PropertyName();

    //호출할 이벤트의 이름
    public string eventName;
}
