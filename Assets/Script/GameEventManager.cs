using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameObject whiteBoardUI;
    public AnimationCurve openWhiteBoardMoveCurve;
    public AnimationCurve closeWhiteBoardMoveCurve;
    public GameObject TextAreaUI;
    public AnimationCurve openTextAreaMoveCurve;
    public AnimationCurve closeTextAreaMoveCurve;
    public GameObject leftChara;
    public GameObject rightChara;
    public GameObject backgroung;
    public GameObject eventPicture;
    // Start is called before the first frame update
    [Button("打开")]
    public async void Show()
    {
        await CustomThread.TimerAsync(1, time =>
        {
            whiteBoardUI.GetComponent<RectTransform>().localPosition = Vector3.up * openWhiteBoardMoveCurve.Evaluate(time);
            TextAreaUI.GetComponent<RectTransform>().localPosition = Vector3.up * openTextAreaMoveCurve.Evaluate(time);
            backgroung.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, time);
            eventPicture.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, time);
        });
        ShowLeftChara();
        ShowRightChara();
        //白板降下
        //文字框降下
        //事件显示
        //小人移入
    }
    [Button("关闭")]
    public async void Close()
    {
        CloseLeftChara();
        CloseRightChara();
        await CustomThread.TimerAsync(1, time =>
        {
            whiteBoardUI.GetComponent<RectTransform>().localPosition = Vector3.up * closeWhiteBoardMoveCurve.Evaluate(time);
            TextAreaUI.GetComponent<RectTransform>().localPosition = Vector3.up * closeTextAreaMoveCurve.Evaluate(time);
            backgroung.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, time);
            eventPicture.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, time);
        });
    }
    //设置图片
    public async void ShowLeftChara()
    {
        await CustomThread.TimerAsync(0.3f, time =>
        {
            leftChara.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(-800, -700, time), -200);
            leftChara.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, time);
        });
    }
    public async void ShowRightChara()
    {
        await CustomThread.TimerAsync(0.3f, time =>
        {
            rightChara.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(800, 700, time), -200);
            rightChara.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, time);
        });
    }
    public async void CloseLeftChara()
    {
        await CustomThread.TimerAsync(0.3f, time =>
        {
            leftChara.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(-700, -800, time), -200);
            leftChara.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, time);
        });
    }
    public async void CloseRightChara()
    {
        await CustomThread.TimerAsync(0.3f, time =>
        {
            rightChara.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Lerp(700, 800, time), -200);
            rightChara.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, time);
        });
    }
}
