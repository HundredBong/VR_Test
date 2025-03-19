using UnityEditor; //유니티 기본 에디터 기능을 사용하려면 필요함


//Editor 폴더 안에 안넣으면 #if UNITY_EDITOR, #endif 써야함


//UnietEventMarker라는 특정 클래스의 인스펙터를 커스텀하겠다는 뜻
//[CustomEditor] 어트리뷰트를 통해 어떤 클래스에 적용할지 명시
[CustomEditor(typeof(UnityEventMarker))]
public class UnityEventMarkerDrawer : Editor
{
    //이렇게 해도 인스펙터에 표시 안됨
    //public string eventName;
    //public float time;

    //SerializedProperty로 해야 인스펙터에 표시되고 값 자동 저장 가능함
    //값 자체가 아니라 값에 접근할 수 있는 핸들같은 역할을 함
    SerializedProperty eventNameProperty;
    SerializedProperty timeProperty;

    private void OnEnable()
    {
        //인스펙터에 표시할 프로퍼티 설정
        //eventNameProperty와 timeProperty에 해당 속성을 연결함
        //이름은 필드 이름과 정확히 일치해야함
        //UnityEventMarker에서 eventName이라는 필드를 찾아서 인스펙터에 표시함
        //m_Time은 UnityEventMarker의 부모 클래스인 Marker에 있음
        eventNameProperty = serializedObject.FindProperty("eventName");
        timeProperty = serializedObject.FindProperty("m_Time");
        
    }

    //인스펙터 창이 열려있으면 프레임마다 호출됨
    //값이 수정되었을 때 즉시 호출됨
    //Undo/Redo 때 즉시 호출됨
    public override void OnInspectorGUI()
    {
        //인스펙터에서 값이 수정될 수 있으니 현재 값 상태를 불러옴
        //현재 인스펙터 값과 실제 오브젝트 값을 동기화함
        //인스펙터에서 수정된 값을 serializedObject에 반영
        serializedObject.Update();

        //유니티가 자동으로 필드 타입을 인식해서 적절한 UI를 생성해줌
        //m_Time은 Marker에서 상속받은 시간 필드임
        //타임라인이 초와 프레임 단위로 처리하기 때문에 필드가 두 개 나옴
        EditorGUILayout.PropertyField(timeProperty);

        //어트리뷰트의 [Sapce]랑 거의 같음
        EditorGUILayout.Space();

        //인스펙터에 제목이나 설명 표시용, [Header]랑 비슷함
        //EditorStyles.boldLabel : 굵은 텍스트 적용
        EditorGUILayout.LabelField("호출할 이벤트 이름", EditorStyles.boldLabel);

        //string타입이니까 문자열 입력 가능한 필드 나옴
        EditorGUILayout.PropertyField(eventNameProperty);

        //인스펙터에서 값 수정 발생 -> 수정된 값은 seeializeObject에 저장됨, 즉시 반영되지 않음
        //ApplyModifiedProperties를 호출하면 serializedObject의 값이 실제 오브젝트 값으로 반영됨
        //변경된 값을 실제 오브젝트에 반영
        serializedObject.ApplyModifiedProperties();
    }
}
