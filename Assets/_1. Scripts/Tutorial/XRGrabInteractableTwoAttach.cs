using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachPoint;
    public Transform rightAttachPoint;

    #region ������
    //�� �ڵ�� ��ü�� ���� �� ����ġ����Ʈ�� ���������� XR Interaction Toolkit�� GetAttachTransform�� ���� ȣ���ؼ� �̸� ����ġ����Ʈ�� ������
    //�� ��� ���� ����ġ����Ʈ�� �ʱ�ȭ �Ǿ� �־�� ���������� �����

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
        //�� �޼���� XR �ý����� OnSelectEntered���� ���� ȣ���ϴ� �޼����̹Ƿ� ��� ���� ����ġ����Ʈ�� �ùٸ��� ������
        //����ġ ����Ʈ�� �������� ������ �� base�޼��嵵 �����Ͽ� �⺻���۵� ������

        //����ġ ����Ʈ�� �� �ո��� �����Ͽ� ��� ������ ��� ����ġ ����Ʈ�� �����ϵ��� ��
        //����Ϸ��� ���� �� ����ġ ����Ʈ ���� �� �ݴ��� ���� ���� �ݴ�� ����� ��

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
