using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachPoint;
    public Transform rightAttachPoint;

    #region 구버전
    //이 코드는 객체를 잡을 떄 어태치포인트를 변경하지만 XR Interaction Toolkit은 GetAttachTransform을 먼저 호출해서 미리 어태치포인트를 설정함
    //즉 잡기 전에 어태치포인트가 초기화 되어 있어야 정상적으로 변경됨

    //protected override void OnSelectEntered(SelectEnterEventArgs arg)
    //{
    //    if (arg.interactorObject.transform.CompareTag("Left Hand"))
    //    {
    //        attachTransform = leftAttachPoint;
    //    }
    //    else if (arg.interactorObject.transform.CompareTag("Right Hand"))
    //    {
    //        attachTransform = rightAttachPoint;
    //    }

    //    base.OnSelectEntered(arg);
    //}
    #endregion

    public override Transform GetAttachTransform(IXRInteractor interactor)
    {
        //이 메서드는 XR 시스템이 OnSelectEntered보다 먼저 호출하는 메서드이므로 잡기 전에 어태치포인트가 올바르게 설정됨
        //어태치 포인트를 동적으로 변경한 뒤 base메서드도 실행하여 기본동작도 유지함

        //어태치 포인트를 양 손마다 지정하여 어느 손으로 잡던 어태치 포인트가 동일하도록 함
        //사용하려면 한쪽 손 어태치 포인트 설정 후 반대쪽 손은 값을 반대로 해줘야 함

        Transform i_attachTransform = null;

        if (interactor.transform.CompareTag("Left Hand"))
        {
            i_attachTransform = leftAttachPoint;
        }
        if (interactor.transform.CompareTag("Right Hand"))
        {
            i_attachTransform = rightAttachPoint;
        }

        attachTransform = i_attachTransform;

        return i_attachTransform != null ? i_attachTransform : base.GetAttachTransform(interactor);
    }
}
