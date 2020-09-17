using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTesseractDemo : MonoBehaviour
{
    [SerializeField] private Texture2D imageToRecognize;
    [SerializeField] private Text displayText;
    [SerializeField] private RawImage outputImage;
    private MyTesseractDriver _tesseractDriver;
    private string _text = "";

    //My Variables
    public MyTests cameraSyst;
    

    private void Start()
    {
        
        _tesseractDriver = new MyTesseractDriver();
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
        //Recoginze(texture);
        //SetImageDisplay();

    }

    private void Update()
    {
        

    }

    public void GetTextRecognition()
    {
        _tesseractDriver = new MyTesseractDriver();
        imageToRecognize = cameraSyst.GetImage();
        Recoginze(imageToRecognize);
    }

    private void Recoginze(Texture2D outputTexture)
    {
        ClearTextDisplay();
        AddToTextDisplay(_tesseractDriver.CheckTessVersion());
        //_tesseractDriver.Setup();
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
        ClearTextDisplay();
        AddToTextDisplay(_tesseractDriver.Recognize(outputTexture));
        AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
    }

    private void ClearTextDisplay()
    {
        _text = "";
    }

    private void AddToTextDisplay(string text, bool isError = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        _text += (string.IsNullOrWhiteSpace(displayText.text) ? "" :
                  "\n") + text;

        if (isError)
            Debug.LogError("El Error: "+ text);
        else
            Debug.Log(text);
    }

    private void LateUpdate()
    {
        displayText.text = _text;
    }

    private void SetImageDisplay()
    {
        RectTransform rectTransform =
             outputImage.GetComponent<RectTransform>();

        rectTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            rectTransform.rect.width *
            _tesseractDriver.GetHighlightedTexture().height /
            _tesseractDriver.GetHighlightedTexture().width);

        outputImage.texture =
            _tesseractDriver.GetHighlightedTexture();
    }

    private void OnSetupCompleteRecognize()
    {
        //AddToTextDisplay(_tesseractDriver.Recognize(imageToRecognize));
        AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
        //SetImageDisplay();
    }

}
