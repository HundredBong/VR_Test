using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabHandPose : MonoBehaviour
{
    public float poseTransitionDuration = 0.2f;
    public HandData rightHandPose;
    public HandData leftHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;



    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        //��ü�� ���� �� ȣ���

        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.anim.enabled = false;

            if (handData.handType == HandData.HandModelType.Right)
            {
                //���� ���� ���� �����Ϳ� �̸� ���ص� �������� ������
                SetHandDataValues(handData, rightHandPose);
            }
            else if (handData.handType == HandData.HandModelType.left)
            {
                //���� ���� ���� �����Ϳ� �̸� ���ص� �޼��� ������
                SetHandDataValues(handData, leftHandPose);
            }

            StartCoroutine(SetHandDataCoroutine(handData, finalHandPosition, finalHandRotation, finalFingerRotations, startingHandPosition, startingHandRotation, startingFingerRotations));
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        //�̹� ���� ���� �����͸� �˰��ְ�, �������·� �ǵ������� starting ��ġ, ȸ������ ����ϸ� �ǹǷ� �߰��� HandData�� ���� ����

        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.anim.enabled = true;

            StartCoroutine(SetHandDataCoroutine(handData, startingHandPosition, startingHandRotation, startingFingerRotations, finalHandPosition, finalHandRotation, finalFingerRotations));
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        //���� ���� �����Ϳ� ��ǥ ���� �����͸� ���ؼ� ���� ��ġ, ȸ������ ���� ��ġ, ȸ������ ���� ������ ������
        //���� �� ��� ��ȭ��Ű�µ� ����


        //���� ���� ���� ��ġ�� �� ���� �������� ����Ͽ� h1.root.localPosition.x / h1.root.localScale.x ���� ������� �����ϸ��� �ݿ���
        startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
            h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
            h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        //���� ���� ���� ȸ������ ������
        startingHandRotation = h1.root.localRotation;

        //��ǥ�� �ϴ� ���� ȸ������ ������
        finalHandRotation = h2.root.localRotation;

        //���� ���� �հ��� ���� ȸ�� ���� ������
        startingFingerRotations = new Quaternion[h1.fingerBones.Length];

        //��ǥ�� �ϴ� �� �հ��� ���� ȸ�� ���� ������
        finalFingerRotations = new Quaternion[h1.fingerBones.Length];

        //for���� ���� �� �հ��� ���� ȸ�� ���� ������
        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }

    }

    public void SendHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    private IEnumerator SetHandDataCoroutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation,
        Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        //��ȭ��ų ���� ������, ��ǥ ������ �� ��ġ, ��ǥ ������ �� ȸ��, ��ǥ ������ �հ��� ���� �迭,
        //���� ������ �� ��ġ, ���� ������ �� ȸ��, ���� ������ �հ��� ���� �迭�� ���ڷ� ����
        //���� ���� �����, ��ǥ�� �ϰ� ���� ���� ����� �ε巴�� ���ϰ� ����

        float timer = 0f;
        while (timer < poseTransitionDuration)
        {
            //���� ��ġ���� ��ǥ ��ġ�� �̵� �� ȸ�� ����
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            //��ȭ��ų ���� �����Ϳ� ��ġ �� ȸ���� ����
            h.root.localPosition = p;
            h.root.localRotation = r;

            //�ݺ����� ���� �հ��� ������ ���� ������
            for (int i = 0; i < newBonesRotation.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;

            yield return null;
        }
    }


#if UNITY_EDITOR
    //MenuItem : ����Ƽ ������ ��� �޴��ٿ� Ŀ���� �޴��� �߰�����
    //�����ϸ� ���� ���õ� ������Ʈ���� GrabHandPose ������Ʈ�� ã�Ƽ� ������ ��� �������� �޼� ��� ��Ī���� �������
    [MenuItem("Tools/Mirror Selected Right Grab Pos")]
    public static void MirrorRightPose()
    {
        Debug.Log("Mirror Right Pose");
        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
    }
#endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        //PoseUsedToMirror : ��Ī�� ������ �Ǵ� �� ���� ������ (������)
        //PoseToMirror : ��Ī�� ����� ������ �� ���� ������ (�޼�)

        //���� ��ġ�� x�� �����ؼ� ��Ī ��ǥ�� �������
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        //���� ȸ������ y��� z���� �����Ͽ� ��Ī�� ȸ������ �������
        //���ʹϾ��� ������ ���� ������ �ݴ밡 �ǵ��� ������
        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
        mirroredQuaternion.y *= -1;
        mirroredQuaternion.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredQuaternion;

        //�հ��� ���� ȸ������ �״�� ������
        for (int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
        {
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }
    }
}